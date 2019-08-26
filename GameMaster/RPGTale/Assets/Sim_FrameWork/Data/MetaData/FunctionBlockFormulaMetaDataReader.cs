using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlockFormulaMetaDataReader 
    {
        public static List<FormulaData> FormulaDataList = new List<FormulaData>();
        public static Dictionary<int, FormulaData> FormulaDataDic = new Dictionary<int, FormulaData>();

        public static List<FormulaInfo> FormulaInfoList = new List<FormulaInfo>();
        public static Dictionary<int, FormulaInfo> FormulaInfoDic = new Dictionary<int, FormulaInfo>();

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