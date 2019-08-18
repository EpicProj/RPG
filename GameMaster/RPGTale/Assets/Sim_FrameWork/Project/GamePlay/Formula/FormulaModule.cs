using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class FormulaModule : MonoSingleton<FormulaModule>
    {
        public enum MaterialProductType
        {
            Input,
            Output,
            Byproduct
        }
        public enum FormulaInfoType
        {
            //可选的
            OPtional,
            //自动适配
            Automatic
        }

        public List<FormulaData> FormulaDataList = new List<FormulaData>();
        public Dictionary<int, FormulaData> FormulaDataDic = new Dictionary<int, FormulaData>();
        public List<FormulaInfo> FormulaInfoList = new List<FormulaInfo>();
        public Dictionary<int, FormulaInfo> FormulaInfoDic = new Dictionary<int, FormulaInfo>();

        public List<TextMap_Formula> TextMap_FormulaList = new List<TextMap_Formula>();
        public Dictionary<string, TextMap_Formula> TextMap_FormulaDic = new Dictionary<string, TextMap_Formula>();

        private bool HasInit = false;

        #region Data
        public void InitData()
        {
            if (HasInit)
                return;
            FormulaDataList = FunctionBlockFormulaMetaDataReader.GetFormulaDataList();
            FormulaDataDic = FunctionBlockFormulaMetaDataReader.GetFormulaDataDic();
            FormulaInfoDic = FunctionBlockFormulaMetaDataReader.GetFormulaInfoDic();
            FormulaInfoList = FunctionBlockFormulaMetaDataReader.GetFormulaInfoList();
            TextMap_FormulaList = FunctionBlockFormulaMetaDataReader.GetTextMap_FormulaList();
            TextMap_FormulaDic = FunctionBlockFormulaMetaDataReader.GetTextMap_FormulaDic();

            HasInit = true;
        }

        public string GetFormulaName(FormulaData data)
        {
            return GetFormulaTextByKey(data.FormulaName);
        }
        public string GetFormulaName(int formulaID)
        {
            return GetFormulaTextByKey(GetFormulaDataByID(formulaID).FormulaName);
        }
        public string GetFormulaDesc(FormulaData data)
        {
            return GetFormulaTextByKey(data.FormulaDesc);
        }
        public string GetFormulaDesc(int formulaID)
        {
            return GetFormulaTextByKey(GetFormulaDataByID(formulaID).FormulaDesc);
        }
        //Speed Base
        public float GetProductSpeed(int formulaID)
        {
            return GetFormulaDataByID(formulaID).ProductSpeed;
        }
        


        //获取原料，产出或副产物列表
        public Dictionary<int, ushort> GetFormulaMaterialList(int formulaID, MaterialProductType Gettype)
        {
            FormulaData fm = GetFormulaDataByID(formulaID);
            if (string.IsNullOrEmpty(fm.InputMaterialList) || string.IsNullOrEmpty(fm.OutputMaterialList) || string.IsNullOrEmpty(fm.ByProductList))
            {
                Debug.LogError("Manufacture List is null , formulaID  = " + formulaID);
            }
            switch (Gettype)
            {
                case MaterialProductType.Byproduct:
                    return TryParseMaterialList(fm.ByProductList);
                case MaterialProductType.Input:
                    return TryParseMaterialList(fm.InputMaterialList);
                case MaterialProductType.Output:
                    return TryParseMaterialList(fm.OutputMaterialList);
                default:
                    Debug.LogError("GetManufactureMaterialList Type Error !");
                    return null;
            }
        }

        public Dictionary<int, ushort> TryParseMaterialList(string s)
        {
            Dictionary<int, ushort> materialDic = new Dictionary<int, ushort>();
            try
            {
                string[] info = s.Split(',');
                for (int i = 0; i < info.Length; i++)
                {
                    materialDic.Add(Convert.ToInt32(info[i].Split(':')[0]), Convert.ToUInt16(info[i].Split(':')[1]));
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return materialDic;
        }

        public string GetFormulaTextByKey(string key)
        {
            TextMap_Formula text = null;
            TextMap_FormulaDic.TryGetValue(key, out text);
            if (text != null)
            {
                return text.Value_CN;
            }
            else
            {
                Debug.LogError("GetFormulaText Error TextID=" + key);
                return string.Empty;
            }
        }

        public FormulaData GetFormulaDataByID(int formulaID)
        {
            FormulaData data = null;
            FormulaDataDic.TryGetValue(formulaID, out data);
            if (data == null)
            {
                Debug.LogError("Can not Get FormulaData  ID=" + formulaID);
            }
            return data;
        }

        //Info
        public FormulaInfo GetFormulaInfoByID(int infoID)
        {
            FormulaInfo info = null;
            FormulaInfoDic.TryGetValue(infoID, out info);
            if (info == null)
            {
                Debug.LogError("Can not Get FormulaInfoData  ID=" + infoID);
            }
            return info;
        }
        public FormulaInfoType GetFormulaInfoType(int infoID)
        {
            switch (GetFormulaInfoByID(infoID).InfoType)
            {
                case 1:
                    return FormulaInfoType.Automatic;
                case 2:
                    return FormulaInfoType.OPtional;
                default:
                    Debug.LogError("FormulaInfo Type Error ,info ID=" + infoID);
                    return FormulaInfoType.Automatic;
            }
        }

        #endregion

        #region Method

        #endregion
    }



}