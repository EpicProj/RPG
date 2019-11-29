using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class TechnologyMetaDataReader
    {
        public static List<Technology> TechnologyList;
        public static Dictionary<int, Technology> TechnologyDic;

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<TechnologyMetaData>(ConfigPath.TABLE_TECHNOLOGY_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("TechnologyMetaData Read Error");
                return;
            }
            TechnologyList = config.AllTechnologyList;
            TechnologyDic = config.AllTechnologyDic;

        }

        public static List<Technology> GetTechnologyList()
        {
            LoadData();
            return TechnologyList;
        }
        public static Dictionary<int,Technology> GetTechnologyDic()
        {
            LoadData();
            return TechnologyDic;
        }


    }
}