using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class FunctionBlockFormulaMetaDataReader 
    {
        public static List<FormulaData> FormulaDataList;
        public static Dictionary<int, FormulaData> FormulaDataDic;

        public static List<FormulaInfo> FormulaInfoList;
        public static Dictionary<int, FormulaInfo> FormulaInfoDic;

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<FunctionBlockFormulaMetaData>(ConfigPath.TABLE_FORMULA_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("MaterialMetaData Read Error");
                return;
            }

            FormulaDataList = config.AllFormulaDataList;
            FormulaDataDic = config.AllFormulaDataDic;
            FormulaInfoList = config.AllFormulaInfoList;
            FormulaInfoDic = config.AllFormulaInfoDic;
        }

        public static List<FormulaData> GetFormulaDataList()
        {
            LoadData();
            return FormulaDataList;
        }
        public static List<FormulaInfo> GetFormulaInfoList()
        {
            LoadData();
            return FormulaInfoList;
        }
        public static Dictionary<int,FormulaInfo> GetFormulaInfoDic()
        {
            LoadData();
            return FormulaInfoDic;
        }

        public static Dictionary<int,FormulaData> GetFormulaDataDic()
        {
            LoadData();
            return FormulaDataDic;
        }

    }
}