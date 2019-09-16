using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class OrderModule : BaseModule<OrderModule>
    {

        protected static List<OrderData> OrderDataList;
        protected static Dictionary<int, OrderData> OrderDataDic;
        protected static List<OrderTypeData> OrderTypeDataList;
        protected static Dictionary<string, OrderTypeData> OrderTypeDataDic;


        public override void InitData()
        {
            OrderDataList = OrderMetaDataReader.GetOrderData();
            OrderDataDic = OrderMetaDataReader.GetOrderDataDic();
            OrderTypeDataList = OrderMetaDataReader.GetOrderTypeData();
            OrderTypeDataDic = OrderMetaDataReader.GetOrderTypeDataDic();


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


    }


    public class OrderInfo
    {
        public string GUID;
        public string Name;
        public string Desc;
        public Sprite BG;


        public OrderInfo(OrderData data)
        {
            Name = OrderModule.GetOrderName(data);
            Desc = OrderModule.GetOrderDesc(data);
            BG = OrderModule.GetOrderBGPath(data);
        }



    }

}