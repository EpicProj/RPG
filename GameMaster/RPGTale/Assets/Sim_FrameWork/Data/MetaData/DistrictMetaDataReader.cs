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
    }
}