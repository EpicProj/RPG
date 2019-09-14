using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class CampMetaDataReader
    {
        public static List<CampData> CampDataList = new List<CampData>();
        public static Dictionary<int, CampData> CampDataDic = new Dictionary<int, CampData>();

        public static List<CampLevelData> CampLevelDataList = new List<CampLevelData>();
        public static Dictionary<int, CampLevelData> CampLevelDataDic = new Dictionary<int, CampLevelData>();

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<CampMetaData>(ConfigPath.TABLE_CAMP_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("CampMetaData Read Error");
                return;
            }
            CampDataList = config.AllCampDataList;
            CampDataDic = config.AllCampDataDic;
            CampLevelDataList = config.AllCampLevelDataList;
            CampLevelDataDic = config.AllCampLevelDataDic;
        }

        public static List<CampData> GetCampData()
        {
            LoadData();
            return CampDataList;
        }
        public static Dictionary<int,CampData> GetCampDataDic()
        {
            LoadData();
            return CampDataDic;
        }
        public static List<CampLevelData> GetCampLevelData()
        {
            LoadData();
            return CampLevelDataList;
        }
        public static Dictionary<int, CampLevelData> GetCampLevelDataDic()
        {
            LoadData();
            return CampLevelDataDic;
        }

    }
}