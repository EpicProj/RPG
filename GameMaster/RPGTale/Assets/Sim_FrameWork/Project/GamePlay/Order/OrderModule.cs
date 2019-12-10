using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class OrderModule : BaseModule<OrderModule>
    {

        protected static List<OrderData> OrderDataList;
        protected static Dictionary<int, OrderData> OrderDataDic;
        protected static List<OrderTypeData> OrderTypeDataList;
        protected static Dictionary<string, OrderTypeData> OrderTypeDataDic;

        public OrderConfig config;

        public override void InitData()
        {
            OrderDataList = OrderMetaDataReader.GetOrderData();
            OrderDataDic = OrderMetaDataReader.GetOrderDataDic();
            OrderTypeDataList = OrderMetaDataReader.GetOrderTypeData();
            OrderTypeDataDic = OrderMetaDataReader.GetOrderTypeDataDic();

            config = new OrderConfig();
            config.ReadOrderConfigData();

        }

        public override void Register()
        {
           
        }

        public OrderModule()
        {
            InitData();
        }


        public static OrderData GetOrderDataByID(int orderID)
        {
            OrderData order = null;
            OrderDataDic.TryGetValue(orderID, out order);
            if (order == null)
            {
                Debug.LogError("OrderData is null ,ID=" + orderID);
            }
            return order;
        }

        public static string GetOrderName(int orderID)
        {
            return MultiLanguage.Instance.GetTextValue(GetOrderDataByID(orderID).Name);
        }
        public static string GetOrderName(OrderData order)
        {
            return MultiLanguage.Instance.GetTextValue(order.Name);
        }

        public static string GetOrderDesc(int orderID)
        {
            return MultiLanguage.Instance.GetTextValue(GetOrderDataByID(orderID).Desc);
        }
        public static string GetOrderDesc(OrderData order)
        {
            return MultiLanguage.Instance.GetTextValue(order.Desc);
        }

        public static Sprite GetOrderBGPath(OrderData order)
        {
            return Utility.LoadSprite(order.BGPath, Utility.SpriteType.png);
        }
        public static Sprite GetOrderBGPath(int orderID)
        {
            var order = GetOrderDataByID(orderID);
            return Utility.LoadSprite(order.BGPath, Utility.SpriteType.png);
        }

        public static OrganizationDataModel GetOrganizationBelong(int orderID)
        {
            OrganizationDataModel model = new OrganizationDataModel();
            model.Create(GetOrderDataByID(orderID).OrganizationBelong);
            return model;
        }

        /// <summary>
        /// Get Order Content
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Dictionary<MaterialDataModel,int> GetOrderContent(int orderID)
        {
            Dictionary<MaterialDataModel, int> content = new Dictionary<MaterialDataModel, int>();
            var data = GetOrderDataByID(orderID);
            if (data == null)
                return content;
            var list = Utility.TryParseStringList(data.OrderContent, ',');
            for (int i = 0; i < list.Count; i++)
            {
                var l = Utility.TryParseIntList(list[i], ':');
                if (l.Count != 2)
                {
                    Debug.LogError("Parse Order Content Error ,order ID=" + data.OrderID);
                    return content;
                }
                MaterialDataModel model = new MaterialDataModel();
                if (model.Create(l[0]) && !content.ContainsKey(model))
                {
                    content.Add(model, l[1]);
                }
            }

            return content;
        }

        public static OrganizationDataModel GetOrderBelongModel(int orderID)
        {
            var id = GetOrderDataByID(orderID).OrganizationBelong;
            OrganizationDataModel model = new OrganizationDataModel();
            if (!model.Create(id))
                Debug.LogError("Get OrganizationModel Fail OrderID=" + orderID);
            return model;
        }

        public OrderContent.OrderReward GetOrderRewardData(OrderData data)
        {
            return config.FindOrderContent(data.OrderJsonConfig).Reward;
        }
        public Dictionary<Material,ushort> GetOrderRewardMaterial(OrderContent.OrderReward reward)
        {
            Dictionary<Material, ushort> result = new Dictionary<Material, ushort>();

            foreach(KeyValuePair<string, ushort> kvp in reward.Reward_Mateiral)
            {
                var ma = MaterialModule.GetMaterialByMaterialID(Utility.TryParseInt(kvp.Key));
                if (ma != null)
                {
                    result.Add(ma, kvp.Value);
                }
            }
            return result;
        }

        #region Type

        public static string GetOrderTypeName(OrderTypeData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.Name);
        }
        public static string GetOrderTypeDesc(OrderTypeData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.Desc);
        }
        public static Sprite GetOrderIconPath(OrderTypeData data)
        {
            return Utility.LoadSprite(data.IconPath, Utility.SpriteType.png);
        }

        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static OrderTypeInfo GetOrderTypeInfo(OrderData order)
        {
            Func<OrderData, OrderTypeData> getData = (d) =>
            {
                OrderTypeData data = null;
                OrderTypeDataDic.TryGetValue(d.Type, out data);
                if (data == null)
                {
                    Debug.LogError("OrderType Error , Type Not Exist , Type=" + order.Type);
                }
                return data;
            };

            return new OrderTypeInfo(getData(order));

        }
        #endregion

        #region Refresh Data

        /// <summary>
        /// 获取订单刷新条件
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public OrderAppearData GetOrderAppearData(int orderID)
        {
            var order = GetOrderDataByID(orderID);
            var appearConfig = config.FindOrderContent(order.OrderJsonConfig).appearConfig;
            if (appearConfig != null)
            {
                OrderAppearData data = new OrderAppearData(appearConfig);
                return data;
            }
            return null;
        }
        /// <summary>
        /// 获取订单销毁条件
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public OrderDisAppearData GetOrderDisAppearData(int orderID)
        {
            var order = GetOrderDataByID(orderID);
            var disAppearConfig = config.FindOrderContent(order.OrderJsonConfig).disAppearConfig;
            if (disAppearConfig != null)
            {
                OrderDisAppearData data = new OrderDisAppearData(disAppearConfig);
                return data;
            }
            return null;
        }

        #endregion
    }

    public class OrderTypeInfo
    {
        public string TypeID;
        public string TypeName;
        public string TypeDesc;
        public Sprite Icon;

        public OrderTypeInfo(OrderTypeData data)
        {
            TypeID = data.TypeID;
            TypeName = OrderModule.GetOrderTypeName(data);
            TypeDesc = OrderModule.GetOrderTypeDesc(data);
            Icon = OrderModule.GetOrderIconPath(data);
        }
    }



    #region Order Config

    public class OrderConfig
    {
        public int order_max_count_default;

        public List<OrderContent> orderContent;

        public Dictionary<string, OrderContent> orderContentDic;

        public void ReadOrderConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            OrderConfig config = reader.LoadJsonDataConfig<OrderConfig>(Config.JsonConfigPath.OrderConfigJsonPath);
            order_max_count_default = config.order_max_count_default;
            orderContent = config.orderContent;
            ///Add Dic
            orderContentDic = new Dictionary<string, OrderContent>();
            for(int i = 0; i < orderContent.Count; i++)
            {
                if (orderContentDic.ContainsKey(orderContent[i].ConfigID))
                {
                    Debug.LogError("Find Same Order Config ID , ID=" + orderContent[i].ConfigID);
                    continue;
                }
                else
                {
                    orderContentDic.Add(orderContent[i].ConfigID, orderContent[i]);
                }
            }
        }

        public OrderContent FindOrderContent(string id)
        {
            OrderContent result = null;
            if (orderContentDic != null)
            { 
                orderContentDic.TryGetValue(id, out result);
                if (result == null)
                    Debug.LogError("Find OrderConfigContent Error ,id=" + id);
            }
            return result; 
        }

    }


    public class OrderContent
    {
        public string ConfigID;
        public OrderReward Reward;
        public OrderAppearConfig appearConfig;
        public OrderDisAppearConfig disAppearConfig;



        public class OrderReward
        {
            public int Reward_Currency;
            public int Reward_Reputation;
            public Dictionary<string, ushort> Reward_Mateiral;
            
        }

    }

    public class OrderAppearConfig
    {
        public Appear_Time_Config appear_Time;
        public Appear_PreOrder_Config appear_PreOrder;
        public Appear_Organization_Config appear_Organization;


        /// <summary>
        /// 按时间刷新
        /// </summary>
        public class Appear_Time_Config
        {
            public ushort Time_Year;
            public ushort Time_Month;
            public float Weight_Origin;
            public float Weight_Month_Add;
        }

        /// <summary>
        /// 订单前置
        /// </summary>
        public class Appear_PreOrder_Config
        {
            public Dictionary<string, int> OrderContent;
        }

        /// <summary>
        /// 订单组织前置
        /// </summary>
        public class Appear_Organization_Config
        {

        }

    }

    public class OrderDisAppearConfig
    {
        public DisAppear_Time_Config disAppear_Time;
        public DisAppear_PreOrder_Config disAppear_Preorder;

        public class DisAppear_Time_Config
        {
            public ushort Time_Year;
            public ushort Time_Month;
            public float Weight_Origin;
            public float Weight_Month_Add;
        }

        public class DisAppear_PreOrder_Config
        {
            public Dictionary<string, int> OrderContent;
        }



    }
    #endregion
}