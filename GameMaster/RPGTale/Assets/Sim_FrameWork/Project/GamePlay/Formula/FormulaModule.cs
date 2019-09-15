using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class FormulaModule : BaseModule<FormulaModule>
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

        public static List<FormulaData> FormulaDataList;
        public static Dictionary<int, FormulaData> FormulaDataDic;
        public static List<FormulaInfo> FormulaInfoList;
        public static Dictionary<int, FormulaInfo> FormulaInfoDic;


        public override void InitData()
        {
            FormulaDataList = FunctionBlockFormulaMetaDataReader.GetFormulaDataList();
            FormulaDataDic = FunctionBlockFormulaMetaDataReader.GetFormulaDataDic();
            FormulaInfoDic = FunctionBlockFormulaMetaDataReader.GetFormulaInfoDic();
            FormulaInfoList = FunctionBlockFormulaMetaDataReader.GetFormulaInfoList();
        }

        public override void Register()
        {
            
        }

        public FormulaModule()
        {
            InitData();
        }

        #region Data

        public static string GetFormulaName(FormulaData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.FormulaName);
        }
        public static string GetFormulaName(int formulaID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFormulaDataByID(formulaID).FormulaName);
        }
        public static string GetFormulaDesc(FormulaData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.FormulaDesc);
        }
        public static string GetFormulaDesc(int formulaID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFormulaDataByID(formulaID).FormulaDesc);
        }
        //Speed Base
        public static float GetProductSpeed(int formulaID)
        {
            return GetFormulaDataByID(formulaID).ProductSpeed;
        }
        


        //获取原料，产出或副产物列表
        public static Dictionary<Material,ushort> GetFormulaMaterialDic(int formulaID, MaterialProductType Gettype)
        {
            Dictionary<Material, ushort> result = new Dictionary<Material, ushort>();
            Dictionary<int, ushort> infoDic = GetFormulaMaterialList(formulaID, Gettype);
            if (infoDic != null)
            {
                foreach(KeyValuePair<int,ushort> kvp in infoDic)
                {
                    Material ma = MaterialModule.GetMaterialByMaterialID(kvp.Key);
                    result.Add(ma, kvp.Value);
                }
            }
            return result;
        }

        public static Dictionary<int, ushort> GetFormulaMaterialList(int formulaID, MaterialProductType Gettype)
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

        public static Dictionary<int, ushort> TryParseMaterialList(string s)
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


        public static FormulaData GetFormulaDataByID(int formulaID)
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
        public static FormulaInfo GetFormulaInfoByID(int infoID)
        {
            FormulaInfo info = null;
            FormulaInfoDic.TryGetValue(infoID, out info);
            if (info == null)
            {
                Debug.LogError("Can not Get FormulaInfoData  ID=" + infoID);
            }
            return info;
        }
        public static FormulaInfoType GetFormulaInfoType(int infoID)
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

        public static List<FormulaData> GetFormulaDataList(int infoID)
        {
            List<FormulaData> result = new List<FormulaData>();
            List<int> info = Utility.TryParseIntList(GetFormulaInfoByID(infoID).FormulaList,',');
            if (info.Count == 0)
            {
                Debug.LogError("Parse FormulaData List Error  infoID=" + infoID);
            }
            else
            {
                for(int i = 0; i < info.Count; i++)
                {
                    FormulaData data = GetFormulaDataByID(info[i]);
                    if (data != null)
                    {
                        result.Add(data);
                    }
                }
            }
            return result;
        }

        public static List<Material> GetFormulaTotalMaterialList(int formulaID, MaterialProductType Gettype)
        {
            List<Material> result = new List<Material>();
            foreach( var id in  GetFormulaMaterialList(formulaID, Gettype).Keys)
            {
                result.Add(MaterialModule.GetMaterialByMaterialID(id));
            }
            return result;
        }
        #endregion

        #region Method

        #endregion
    }



}