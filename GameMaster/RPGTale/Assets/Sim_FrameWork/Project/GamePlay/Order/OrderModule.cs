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

        /// <summary>
        /// Get Order Content
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Dictionary<Material,int> GetOrderContent(OrderData order)
        {
            Dictionary<Material, int> content = new Dictionary<Material, int>();

            var list = Utility.TryParseStringList(order.OrderContent, ',');
            for (int i = 0; i < list.Count; i++)
            {
                var l = Utility.TryParseIntList(list[i], ':');
                if (l.Count != 2)
                {
                    Debug.LogError("Parse Order Content Error ,order ID=" + order.OrderID);
                    return content;
                }
                var ma = MaterialModule.GetMaterialByMaterialID(l[0]);
                if (ma != null && !content.ContainsKey(ma))
                {
                    content.Add(ma, l[1]);
                }
            }

            return content;
        }

        public OrderContent.OrderReward GetOrderRewardData(OrderData data)
        {
            return config.FindOrderContent(data.OrderJsonConfig).Reward;
        }
        public Dictionary<Material,int> GetOrderRewardMaterial(OrderData data)
        {
            var reward = GetOrderRewardData(data);
            Dictionary<Material, int> result = new Dictionary<Material, int>();

            foreach(KeyValuePair<string,int> kvp in reward.Reward_Mateiral)
            {
                var ma = MaterialModule.GetMaterialByMaterialID(int.Parse(kvp.Key));
                if (ma != null)
                {
                    result.Add(ma, kvp.Value);
                }
            }
            return result;
        }


    }

    public class OrderConfig
    {
        public int order_max_count_default;

        public List<OrderContent> orderContent;

        public void ReadOrderConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            OrderConfig config = reader.LoadJsonDataConfig<OrderConfig>(Config.JsonConfigPath.OrderConfigJsonPath);
            order_max_count_default = config.order_max_count_default;
            orderContent = config.orderContent;
        }

        public OrderContent FindOrderContent(string id)
        {
            if (orderContent != null)
            {
                var content = orderContent.Find(x => x.ConfigID == id);
                if (content == null)
                    Debug.LogError("Find OrderConfigContent Error ,id=" + id);
                return content;
            }
            return null;      
        }



    }


    public class OrderContent
    {
        public string ConfigID;
        public OrderReward Reward;


        public class OrderReward
        {
            public float Reward_Currency;
            public float Reward_Reputation;
            public Dictionary<string, int> Reward_Mateiral;
            
        }

        public class OrderAppearConfig
        {

        }

        public class OrderDisAppearConfig
        {

        }

    }




    public class OrderInfo
    {
        public string GUID;
        public string Name;
        public string Desc;
        public Sprite BG;
        public float OrderTime;

        public Dictionary<Material, int> OrderContentDic;
        public OrderContent.OrderReward rewardData;


        ///Order actual
        public float remainTime;


        public OrderInfo(int orderID)
        {
            var data = OrderModule.GetOrderDataByID(orderID);
            Name = OrderModule.GetOrderName(data);
            Desc = OrderModule.GetOrderDesc(data);
            BG = OrderModule.GetOrderBGPath(data);
            OrderContentDic = OrderModule.GetOrderContent(data);
            OrderTime = data.TimeLimit;
            remainTime = OrderTime;

            rewardData = OrderModule.Instance.GetOrderRewardData(data);
        }



    }

}