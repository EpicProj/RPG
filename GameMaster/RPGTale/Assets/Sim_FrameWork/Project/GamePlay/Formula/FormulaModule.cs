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
            Enhance
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
        public static Sprite GetFormulaIcon(FormulaData data)
        {
            return Utility.LoadSprite(data.IconPath, Utility.SpriteType.png);
        }
        public static Sprite GetFormulaIcon(int formulaID)
        {
            return GetFormulaIcon(GetFormulaDataByID(formulaID));
        }

        //Speed Base
        public static float GetProductSpeed(int formulaID)
        {
            return GetFormulaDataByID(formulaID).ProductSpeed;
        }



        /// <summary>
        /// 获取原料列表
        /// </summary>
        /// <param name="formulaID"></param>
        /// <param name="Gettype"></param>
        /// <returns></returns>
        public static List<FormulaItem> GetFormulaItemList(int formulaID, MaterialProductType Gettype)
        {
            List<FormulaItem> result = new List<FormulaItem>();
            Dictionary<int, ushort> infoDic = GetFormulaMaterialList(formulaID, Gettype);
            if (infoDic != null)
            {
                foreach(KeyValuePair<int,ushort> kvp in infoDic)
                {
                    Material ma = MaterialModule.GetMaterialByMaterialID(kvp.Key);
                    FormulaItem item = new FormulaItem(ma, kvp.Value);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取增幅材料
        /// </summary>
        /// <param name="formulaID"></param>
        /// <returns></returns>
        public static FormulaItem GetFormulaEnhanceMaterial(int formulaID)
        {
            var list= GetFormulaItemList(formulaID, MaterialProductType.Enhance);
            if (list.Count == 1)
            {
                return list[0];
            }
            return new FormulaItem(new MaterialDataModel(), 0);
        }

        /// <summary>
        /// 获取 产出材料
        /// </summary>
        /// <param name="formulaID"></param>
        /// <returns></returns>
        public static FormulaItem GetFormulaOutputMaterial(int formulaID)
        {
            var list = GetFormulaItemList(formulaID, MaterialProductType.Output);
            if (list.Count == 1)
            {
                return list[0];
            }
            return new FormulaItem(new MaterialDataModel(), 0);
        }


        private static Dictionary<int, ushort> GetFormulaMaterialList(int formulaID, MaterialProductType Gettype)
        {
            FormulaData fm = GetFormulaDataByID(formulaID);
            if (string.IsNullOrEmpty(fm.InputMaterialList) || string.IsNullOrEmpty(fm.OutputMaterial))
            {
                Debug.LogError("Manufacture List is null , formulaID  = " + formulaID);
            }
            switch (Gettype)
            {
                case MaterialProductType.Enhance:
                    var dic= TryParseMaterialList(fm.EnhanceMaterial);
                    if (dic == null || dic.Count == 1)
                    {
                        return dic;
                    }
                    else
                    {
                        Debug.LogError("GetEnhanceMaterial Error ! formulaID="+formulaID);
                        return new Dictionary<int, ushort>();
                    }
                case MaterialProductType.Input:
                    var inputDic= TryParseMaterialList(fm.InputMaterialList);
                    if (inputDic.Count > 3)
                        Debug.LogError("FormulaLimit Error Input Max is 3! formulaID=" + formulaID);
                    return inputDic;
                case MaterialProductType.Output:
                    var outputDic= TryParseMaterialList(fm.OutputMaterial);
                    if (outputDic.Count == 1)
                    {
                        return outputDic;
                    }
                    else
                    {
                        Debug.LogError("Get OutPut Material Error ! formulaID=" + formulaID);
                        return new Dictionary<int, ushort>();
                    }
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

        #endregion

    }



}