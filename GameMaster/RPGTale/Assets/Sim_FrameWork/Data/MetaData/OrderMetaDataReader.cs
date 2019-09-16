using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class OrderMetaDataReader 
    {
        public static List<OrderData> OrderDataList;
        public static Dictionary<int, OrderData> OrderDataDic;

        public static List<OrderTypeData> OrderTypeDataList;
        public static Dictionary<string, OrderTypeData> OrderTypeDataDic;

        private static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<OrderMetaData>(ConfigPath.TABLE_ORDER_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("MaterialMetaData Read Error");
                return;
            }

            OrderDataList = config.AllOrderDataList;
            OrderDataDic = config.AllOrderDataDic;
            OrderTypeDataList = config.AllOrderTypeDataList;
            OrderTypeDataDic = config.AllOrderTypeDataDic;
        }

        public static List<OrderData> GetOrderData()
        {
            LoadData();
            return OrderDataList;
        }
        public static Dictionary<int, OrderData> GetOrderDataDic()
        {
            LoadData();
            return OrderDataDic;
        }
        public static List<OrderTypeData> GetOrderTypeData()
        {
            LoadData();
            return OrderTypeDataList;
        }
        public static Dictionary<string, OrderTypeData> GetOrderTypeDataDic()
        {
            LoadData();
            return OrderTypeDataDic;
        }


    }
}