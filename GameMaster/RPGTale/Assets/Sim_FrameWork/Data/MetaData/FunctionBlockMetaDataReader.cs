using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlockMetaDataReader
    {

        public static List<FunctionBlock> FunctionBlockDataList=new List<FunctionBlock> ();
        public static Dictionary<int, FunctionBlock> FunctionBlockDataDic=new Dictionary<int, FunctionBlock> ();

        public static List<FunctionBlock_Raw> FunctionBlock_RawList=new List<FunctionBlock_Raw> ();
        public static Dictionary<int, FunctionBlock_Raw> FunctionBlock_RawDic=new Dictionary<int, FunctionBlock_Raw> ();

        public static List<FunctionBlock_Manufacture> FunctionBlock_ManufactureList = new List<FunctionBlock_Manufacture>();
        public static Dictionary<int, FunctionBlock_Manufacture> FunctionBlock_ManufactureDic = new Dictionary<int, FunctionBlock_Manufacture>();

        public static List<FunctionBlock_Energy> FunctionBlock_EnergyList = new List<FunctionBlock_Energy>();
        public static Dictionary<int, FunctionBlock_Energy> FunctionBlock_EnergyDic = new Dictionary<int, FunctionBlock_Energy>();

        public static List<FunctionBlock_Science> FunctionBlock_ScienceList = new List<FunctionBlock_Science>();
        public static Dictionary<int, FunctionBlock_Science> FunctionBlock_ScienceDic = new Dictionary<int, FunctionBlock_Science>();

        public static List<FunctionBlockTypeData> FunctionBlockTypeDataList=new List<FunctionBlockTypeData> ();
        public static Dictionary<string,FunctionBlockTypeData> FunctionBlockTypeDataDic=new Dictionary<string, FunctionBlockTypeData> ();

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<FunctionBlockMetaData>(ConfigPath.TABLE_FUNCTIONBLOCK_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("FunctionBlockMetaData Read Error");
                return;
            }
            FunctionBlockDataList = config.AllFunctionBlockList;
            FunctionBlockDataDic = config.AllFunctionBlockDic;
            FunctionBlock_RawList = config.AllFunctionBlock_RawList;
            FunctionBlock_RawDic = config.AllFunctionBlock_RawDic;
            FunctionBlock_ManufactureList = config.AllFunctionBlock_ManufactureList;
            FunctionBlock_ManufactureDic = config.AllFunctionBlock_ManufactureDic;
            FunctionBlock_ScienceList = config.AllFunctionBlock_ScienceList;
            FunctionBlock_ScienceDic = config.AllFunctionBlock_ScienceDic;
            FunctionBlock_EnergyList = config.AllFunctionBlock_EnergyList;
            FunctionBlock_EnergyDic = config.AllFunctionBlock_EnergyDic;

            FunctionBlockTypeDataList = config.AllFunctionBlockTypeDataList;
            FunctionBlockTypeDataDic = config.AllFunctionBlockTypeDataDic;
        }


        public static List<FunctionBlock> GetFunctionBlockData()
        {
            LoadData();
            return FunctionBlockDataList;
        }
        public static List<FunctionBlock_Raw> GetFunctionBlockRowData()
        {
            LoadData();
            return FunctionBlock_RawList;
        }
        public static List<FunctionBlock_Manufacture> GetFunctionBlock_ManufactureData()
        {
            LoadData();
            return FunctionBlock_ManufactureList;
        }
        public static List<FunctionBlock_Science> GetFunctionBlock_ScienceData()
        {
            LoadData();
            return FunctionBlock_ScienceList;
        }
        public static List<FunctionBlock_Energy> GetFunctionBlock_EnergyData()
        {
            LoadData();
            return FunctionBlock_EnergyList;
        }
        public static List<FunctionBlockTypeData> GetFunctionBlockTypeData()
        {
            LoadData();
            return FunctionBlockTypeDataList;
        }

        public static Dictionary<int, FunctionBlock> GetFunctionBlockDataDic()
        {
            LoadData();
            return FunctionBlockDataDic;
        }
        public static Dictionary<string, FunctionBlockTypeData> GetFunctionBlockTypeDataDic()
        {
            LoadData();
            return FunctionBlockTypeDataDic;
        }
        public static Dictionary<int, FunctionBlock_Raw> GetFunctionBlock_RawDic()
        {
            LoadData();
            return FunctionBlock_RawDic;
        }
        public static Dictionary<int,FunctionBlock_Manufacture> GetFunctionBlock_ManufactureDic()
        {
            LoadData();
            return FunctionBlock_ManufactureDic;
        }
        public static Dictionary<int,FunctionBlock_Science> GetFunctionBlock_ScienceDic()
        {
            LoadData();
            return FunctionBlock_ScienceDic;
        }
        public static Dictionary<int,FunctionBlock_Energy> GetFunctionBlock_EnergyDic()
        {
            LoadData();
            return FunctionBlock_EnergyDic;
        }


        public static FunctionBlock GetFunctionBlockDataByKey(int key)
        {
            LoadData();
            FunctionBlock factory = null;
            if (FunctionBlockDataDic == null)
                return null;
            FunctionBlockDataDic.TryGetValue(key, out factory);
            if (factory == null)
            {
                Debug.LogError("Can not Find FunctionBlockData , Key = " + key);
                return null;
            }
            return factory;
        }

    }
}