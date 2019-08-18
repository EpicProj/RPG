using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FactoryFormulaMetaDataReader 
    {
        public static List<FormulaData> FormulaDataList = new List<FormulaData>();
        public static Dictionary<int, FormulaData> FormulaDataDic = new Dictionary<int, FormulaData>();

        public static List<TextMap_Formula> TextMap_FormulaList = new List<TextMap_Formula>();
        public static Dictionary<string, TextMap_Formula> TextMap_FormulaDic = new Dictionary<string, TextMap_Formula>();

        public static List<FormulaInfo> FormulaInfoList = new List<FormulaInfo>();
        public static Dictionary<int, FormulaInfo> FormulaInfoDic = new Dictionary<int, FormulaInfo>();

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<FactoryFormulaMetaData>(ConfigPath.TABLE_FORMULA_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("MaterialMetaData Read Error");
                return;
            }

            FormulaDataList = config.AllFormulaDataList;
            FormulaDataDic = config.AllFormulaDataDic;
            FormulaInfoList = config.AllFormulaInfoList;
            FormulaInfoDic = config.AllFormulaInfoDic;
            TextMap_FormulaList = config.AllTextMap_FormulaList;
            TextMap_FormulaDic = config.AllTextMap_FormulaDic;
        }

        public static List<FormulaData> GetFormulaDataList()
        {
            LoadData();
            return FormulaDataList;
        }
        public static List<TextMap_Formula> GetTextMap_FormulaList()
        {
            LoadData();
            return TextMap_FormulaList;
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
        public static Dictionary<string ,TextMap_Formula> GetTextMap_FormulaDic()
        {
            LoadData();
            return TextMap_FormulaDic;
        }

    }
}