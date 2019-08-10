using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FactoryMetaDataReader
    {

        public static List<Factory> FactoryDataList;
        public static Dictionary<int, Factory> FactoryDataDic;

        public static List<Factory_Raw> Factory_RowList;
        public static Dictionary<int, Factory_Raw> Factory_RowDic;

        public static List<FactoryTypeData> FactoryTypeDataList;
        public static Dictionary<string,FactoryTypeData> FactoryTypeDataDic;

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
            Factory_RowList = config.AllFactory_RawList;
            Factory_RowDic = config.AllFactory_RawDic;
            FactoryTypeDataList = config.AllFactoryTypeDataList;
            FactoryTypeDataDic = config.AllFactoryTypeDataDic;
        }


        public static List<Factory> GetFactoryData()
        {
            LoadData();
            return FactoryDataList;
        }
        public static List<Factory_Raw> GetFactoryRowData()
        {
            LoadData();
            return Factory_RowList;
        }
        public static List<FactoryTypeData> GetFactoryTypeData()
        {
            LoadData();
            return FactoryTypeDataList;
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