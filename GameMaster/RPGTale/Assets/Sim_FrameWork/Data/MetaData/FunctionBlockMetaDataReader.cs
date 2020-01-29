using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class FunctionBlockMetaDataReader
    {

        public static List<FunctionBlock> FunctionBlockDataList;
        public static Dictionary<int, FunctionBlock> FunctionBlockDataDic;

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

            FunctionBlockTypeDataList = config.AllFunctionBlockTypeDataList;
            FunctionBlockTypeDataDic = config.AllFunctionBlockTypeDataDic;
        }


        public static List<FunctionBlock> GetFunctionBlockData()
        {
            LoadData();
            return FunctionBlockDataList;
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
    }
}