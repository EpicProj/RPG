using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FactoryMetaDataReader
    {

        public static List<Factory> FactoryDataList=new List<Factory> ();
        public static Dictionary<int, Factory> FactoryDataDic=new Dictionary<int, Factory> ();

        public static List<Factory_Raw> Factory_RawList=new List<Factory_Raw> ();
        public static Dictionary<int, Factory_Raw> Factory_RawDic=new Dictionary<int, Factory_Raw> ();

        public static List<Factory_Manufacture> Factory_ManufactureList = new List<Factory_Manufacture>();
        public static Dictionary<int, Factory_Manufacture> Factory_ManufactureDic = new Dictionary<int, Factory_Manufacture>();

        public static List<Factory_Energy> Factory_EnergyList = new List<Factory_Energy>();
        public static Dictionary<int, Factory_Energy> Factory_EnergyDic = new Dictionary<int, Factory_Energy>();

        public static List<Factory_Science> Factory_ScienceList = new List<Factory_Science>();
        public static Dictionary<int, Factory_Science> Factory_ScienceDic = new Dictionary<int, Factory_Science>();

        public static List<FactoryTypeData> FactoryTypeDataList=new List<FactoryTypeData> ();
        public static Dictionary<string,FactoryTypeData> FactoryTypeDataDic=new Dictionary<string, FactoryTypeData> ();

        public static List<TextData_Factory> TextData_FactoryList=new List<TextData_Factory> ();
        public static Dictionary<string, TextData_Factory> TextData_FactoryDic=new Dictionary<string, TextData_Factory> ();

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
            Factory_RawList = config.AllFactory_RawList;
            Factory_RawDic = config.AllFactory_RawDic;
            Factory_ManufactureList = config.AllFactory_ManufactureList;
            Factory_ManufactureDic = config.AllFactory_ManufactureDic;
            Factory_ScienceList = config.AllFactory_ScienceList;
            Factory_ScienceDic = config.AllFactory_ScienceDic;
            Factory_EnergyList = config.AllFactory_EnergyList;
            Factory_EnergyDic = config.AllFactory_EnergyDic;

            FactoryTypeDataList = config.AllFactoryTypeDataList;
            FactoryTypeDataDic = config.AllFactoryTypeDataDic;
            TextData_FactoryList = config.AllTextData_FactoryList;
            TextData_FactoryDic = config.AllTextData_FactoryDic;
        }


        public static List<Factory> GetFactoryData()
        {
            LoadData();
            return FactoryDataList;
        }
        public static List<Factory_Raw> GetFactoryRowData()
        {
            LoadData();
            return Factory_RawList;
        }
        public static List<Factory_Manufacture> GetFactory_ManufactureData()
        {
            LoadData();
            return Factory_ManufactureList;
        }
        public static List<Factory_Science> GetFactory_ScienceData()
        {
            LoadData();
            return Factory_ScienceList;
        }
        public static List<Factory_Energy> GetFactory_EnergyData()
        {
            LoadData();
            return Factory_EnergyList;
        }
        public static List<FactoryTypeData> GetFactoryTypeData()
        {
            LoadData();
            return FactoryTypeDataList;
        }
        public static List<TextData_Factory> GetTextData_FactoryData()
        {
            LoadData();
            return TextData_FactoryList;
        }
       

        public static Dictionary<int, Factory> GetFactoryDataDic()
        {
            LoadData();
            return FactoryDataDic;
        }
        public static Dictionary<string, FactoryTypeData> GetFactoryTypeDataDic()
        {
            LoadData();
            return FactoryTypeDataDic;
        }
        public static Dictionary<int, Factory_Raw> GetFactory_RawDic()
        {
            LoadData();
            return Factory_RawDic;
        }
        public static Dictionary<int,Factory_Manufacture> GetFactory_ManufactureDic()
        {
            LoadData();
            return Factory_ManufactureDic;
        }
        public static Dictionary<int,Factory_Science> GetFactory_ScienceDic()
        {
            LoadData();
            return Factory_ScienceDic;
        }
        public static Dictionary<int,Factory_Energy> GetFactory_EnergyDic()
        {
            LoadData();
            return Factory_EnergyDic;
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
        public static TextData_Factory GetTextData_FactoryByKey(string key)
        {
            LoadData();
            TextData_Factory text = null;
            if (FactoryDataDic == null)
                return null;
            TextData_FactoryDic.TryGetValue(key, out text);
            if (text == null)
            {
                Debug.LogError("Can not Find Factorytext , Key = " + key);
                return null;
            }
            return text;
        }

    }
}