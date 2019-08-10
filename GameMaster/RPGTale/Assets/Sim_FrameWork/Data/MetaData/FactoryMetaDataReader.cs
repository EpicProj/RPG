using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FactoryMetaDataReader
    {

        public static List<Factory> FactoryDataList;
        public static Dictionary<int, Factory> FactoryDataDic;

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<FactoryMetaData>(ConfigPath.TABLE_FACTORY_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("FactoryMetaData Read Error");
                return;
            }
            FactoryDataList = config.AllFactoryList;
            FactoryDataDic = config.AllFactoryDic;
        }


        public static List<Factory> GetFactoryData()
        {
            LoadData();
            return FactoryDataList;
        }
        public static Dictionary<int, Factory> GetFactoryDataDic()
        {
            LoadData();
            return FactoryDataDic;
        }


        public static Factory GetFactoryDataByKey(int key)
        {
            LoadData();
            Factory factory = null;
            if (FactoryDataDic == null)
                return null;
            FactoryDataDic.TryGetValue(key, out factory);
            if (factory == null)
            {
                Debug.LogError("Can not Find FactoryData , Key = " + key);
                return null;
            }
            return factory;
        }

    }
}