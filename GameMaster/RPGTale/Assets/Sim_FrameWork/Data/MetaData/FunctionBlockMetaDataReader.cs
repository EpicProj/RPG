using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class FunctionBlockMetaDataReader
    {

        public static List<FunctionBlock> FunctionBlockDataList;
        public static Dictionary<int, FunctionBlock> FunctionBlockDataDic;

        public static List<FunctionBlock_Industry> FunctionBlock_IndustryList;
        public static Dictionary<int, FunctionBlock_Industry> FunctionBlock_IndustryDic;

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
            FunctionBlock_IndustryList = config.AllFunctionBlock_IndustryList;
            FunctionBlock_IndustryDic = config.AllFunctionBlock_IndustryDic;

            FunctionBlockTypeDataList = config.AllFunctionBlockTypeDataList;
            FunctionBlockTypeDataDic = config.AllFunctionBlockTypeDataDic;
        }


        public static List<FunctionBlock> GetFunctionBlockData()
        {
            LoadData();
            return FunctionBlockDataList;
        }
        public static List<FunctionBlock_Industry> GetFunctionBlock_IndustryData()
        {
            LoadData();
            return FunctionBlock_IndustryList;
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
        public static Dictionary<int, FunctionBlock_Industry> GetFunctionBlock_IndustryDic()
        {
            LoadData();
            return FunctionBlock_IndustryDic;
        }
    }
}