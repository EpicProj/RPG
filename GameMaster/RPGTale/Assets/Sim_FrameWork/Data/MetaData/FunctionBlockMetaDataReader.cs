using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class FunctionBlockMetaDataReader
    {

        public static List<FunctionBlock> FunctionBlockDataList;
        public static Dictionary<int, FunctionBlock> FunctionBlockDataDic;

        public static List<FunctionBlock_Labor> FunctionBlock_LaborList;
        public static Dictionary<int, FunctionBlock_Labor> FunctionBlock_LaborDic;

        public static List<FunctionBlock_Industry> FunctionBlock_IndustryList;
        public static Dictionary<int, FunctionBlock_Industry> FunctionBlock_IndustryDic;

        public static List<FunctionBlock_Energy> FunctionBlock_EnergyList;
        public static Dictionary<int, FunctionBlock_Energy> FunctionBlock_EnergyDic;

        public static List<FunctionBlock_Science> FunctionBlock_ScienceList;
        public static Dictionary<int, FunctionBlock_Science> FunctionBlock_ScienceDic;

        public static List<FunctionBlockTypeData> FunctionBlockTypeDataList;
        public static Dictionary<string,FunctionBlockTypeData> FunctionBlockTypeDataDic;

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
            FunctionBlock_LaborList = config.AllFunctionBlock_LaborList;
            FunctionBlock_LaborDic = config.AllFunctionBlock_LaborDic;
            FunctionBlock_IndustryList = config.AllFunctionBlock_IndustryList;
            FunctionBlock_IndustryDic = config.AllFunctionBlock_IndustryDic;
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
        public static List<FunctionBlock_Labor> GetFunctionBlock_LaborData()
        {
            LoadData();
            return FunctionBlock_LaborList;
        }
        public static List<FunctionBlock_Industry> GetFunctionBlock_IndustryData()
        {
            LoadData();
            return FunctionBlock_IndustryList;
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
        public static Dictionary<int, FunctionBlock_Labor> GetFunctionBlock_LaborDic()
        {
            LoadData();
            return FunctionBlock_LaborDic;
        }
        public static Dictionary<string, FunctionBlockTypeData> GetFunctionBlockTypeDataDic()
        {
            LoadData();
            return FunctionBlockTypeDataDic;
        }
        public static Dictionary<int, FunctionBlock_Industry> GetFunctionBlock_IndustryDic()
        {
            LoadData();
            return FunctionBlock_IndustryDic;
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

    }
}