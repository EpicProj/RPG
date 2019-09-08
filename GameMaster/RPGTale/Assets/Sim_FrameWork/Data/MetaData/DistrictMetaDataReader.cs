using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class DistrictMetaDataReader
    {

        public static List<DistrictData> DistrictDataList;
        public static Dictionary<int, DistrictData> DistrictDataDic;
        public static List<DistrictType> DistrictTypeList;
        public static Dictionary<int, DistrictType> DistrictTypeDic;
        public static List<DistrictIcon> DistrictIconList;
        public static Dictionary<int, DistrictIcon> DistrictIconDic;
        public static List<DistrictModel> DistrictModelList;
        public static Dictionary<int, DistrictModel> DistrictModelDic;

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<DistrictMetaData>(ConfigPath.TABLE_DISTRICT_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("DistrictMetaData Read Error");
                return;
            }
            DistrictDataList = config.AllDistrictDataList;
            DistrictDataDic = config.AllDistrictDataDic;
            DistrictTypeList = config.AllDistrictTypeList;
            DistrictTypeDic = config.AllDistrictTypeDic;
            DistrictIconList = config.AllDistrictIconList;
            DistrictIconDic = config.AllDistrictIconDic;
            DistrictModelList = config.AllDistrictModelList;
            DistrictModelDic = config.AllDistrictModelDic;
        }



        public static List<DistrictData> GetDistrictData()
        {
            LoadData();
            return DistrictDataList;
        }

        public static Dictionary<int,DistrictData> GetDistrictDic()
        {
            LoadData();
            return DistrictDataDic;
        }
        public static List<DistrictType> GetDistrictType()
        {
            LoadData();
            return DistrictTypeList;
        }
        public static Dictionary<int, DistrictType> GetDistrictTypeDic()
        {
            LoadData();
            return DistrictTypeDic;
        }

        public static List<DistrictIcon> GetDistrictIcon()
        {
            LoadData();
            return DistrictIconList;
        }
        public static Dictionary<int, DistrictIcon> GetDistrictIconDic()
        {
            LoadData();
            return DistrictIconDic;
        }

        public static List<DistrictModel> GetDistrictModel()
        {
            LoadData();
            return DistrictModelList;
        }
        public static Dictionary<int,DistrictModel> GetDistrictModelDic()
        {
            LoadData();
            return DistrictModelDic;
        }
        public static DistrictData GetDistrictDataByKey(int key)
        {
            LoadData();
            DistrictData data = null;
            if (DistrictDataDic == null)
                return null;
            DistrictDataDic.TryGetValue(key, out data);
            if (data == null)
            {
                Debug.LogError("Can not Find DistrictData , Key = " + key);
                return null;
            }
            return data;
        }

        public static DistrictType GetDistrictTypeByKey(int key)
        {
            LoadData();
            DistrictType data = null;
            if (DistrictTypeDic == null)
                return null;
            DistrictTypeDic.TryGetValue(key, out data);
            if (data == null)
            {
                Debug.LogError("Can not Find DistrictType , Key = " + key);
                return null;
            }
            return data;
        }

        public static DistrictIcon GetDistrictIconByKey(int key)
        {
            LoadData();
            DistrictIcon icon = null;
            if (DistrictTypeDic == null)
                return null;
            DistrictIconDic.TryGetValue(key, out icon);
            if (icon == null)
            {
                Debug.LogError("Can not Find DistrictIcon , Key = " + key);
                return null;
            }
            return icon;
        }

    }
}