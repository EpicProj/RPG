using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class DistrictModule : BaseModule<DistrictModule> {

        #region Row Data
        public static List<DistrictData> DistrictDataList = new List<DistrictData>();
        public static Dictionary<int, DistrictData> DistrictDataDic = new Dictionary<int, DistrictData>();
        public static List<DistrictType> DistrictTypeList = new List<DistrictType>();
        public static Dictionary<int, DistrictType> DistrictTypeDic = new Dictionary<int, DistrictType>();

        private bool HasInit = false;
        public override void InitData()
        {
            if (HasInit)
                return;
            DistrictDataList = DistrictMetaDataReader.GetDistrictData();
            DistrictDataDic = DistrictMetaDataReader.GetDistrictDic();
            DistrictTypeList = DistrictMetaDataReader.GetDistrictType();
            DistrictTypeDic = DistrictMetaDataReader.GetDistrictTypeDic();
            HasInit = true;
        }

        public DistrictData GetDistrictDataByKey(int districtID)
        {
            DistrictData data = null;
            DistrictDataDic.TryGetValue(districtID, out data);
            if (data == null)
                Debug.LogError("Can not Find DistrictData ,ID=" + districtID);
            return data;
        }
        public string GetDistrictName(int districtID)
        {
            return MultiLanguage.Instance.GetTextValue(GetDistrictDataByKey(districtID).DistrictName);
        }
        public string GetDistrictName(DistrictData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.DistrictName);
        }
        public string GetDistrictDesc(int districtID)
        {
            return MultiLanguage.Instance.GetTextValue(GetDistrictDataByKey(districtID).DistrictDesc);
        }
        public string GetDistrictDesc(DistrictData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.DistrictDesc);
        }

        public Sprite GetDistrictIcon(DistrictData data)
        {
            string path = data.DistrictIcon;
            return Utility.LoadSprite(path, Utility.SpriteType.png);
        }

        public DistrictType GetDistrictType(int districtID)
        {
            return GetDistrictType(GetDistrictDataByKey(districtID));
        }
        public DistrictType GetDistrictType(DistrictData data)
        {
            DistrictType type = null;
            DistrictTypeDic.TryGetValue(data.Type, out type);
            if (type == null)
                Debug.LogError("Can not Get districtType ID=" + data.Type);
            return type;
        }

        public List<Vector2> GetDistrictTypeArea(DistrictData data)
        {
            return GetDistrictTypeArea(data.DistrictID);
        }
        public List<Vector2> GetDistrictTypeArea(int DistrictID)
        {
            List<Vector2> result = new List<Vector2>();
            DistrictType type = GetDistrictType(DistrictID);
            List<string> vectorList = Utility.TryParseStringList(type.TypeShape, ';');
            for (int i = 0; i < vectorList.Count; i++)
            {
                List<int> vector = Utility.TryParseIntList(vectorList[i], ',');
                if (vector.Count != 2)
                {
                    Debug.LogError("district type parse error ,vector=" + vector);
                    return result;
                }
                Vector2 v2 = new Vector2(vector[0], vector[1]);
                result.Add(v2);
            }
            return result;
        }

        public Dictionary<int, int> GetDistrictMaterialCostDic(DistrictData data)
        {
            Dictionary<int, int> result = null;
            List<string> materialList = Utility.TryParseStringList(data.MaterialCostList, ',');
            if (materialList == null)
                return result;
            for (int i = 0; i < materialList.Count; i++)
            {
                int materialID;
                int num;
                if(int.TryParse(materialList[i].Split(':')[0], out materialID))
                {
                    if (result.ContainsKey(materialID))
                    {
                        Debug.LogWarning("Find Same Material Cost ID ,ID=" + materialID);
                        continue;
                    }
                    else
                    {
                        if(int.TryParse(materialList[i].Split(':')[1], out num))
                        {
                            result.Add(materialID, num);
                        }
                        else
                        {
                            Debug.LogError("Parse MaterialCost Num Error string=" + materialList[i]);
                            continue;
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region Function

        public Dictionary<Material,int> GetMaterialCost(DistrictData data)
        {
            Dictionary<Material, int> MaterialCostDic = null;
            Dictionary<int, int> cost = GetDistrictMaterialCostDic(data);
            foreach(KeyValuePair<int,int> kvp in cost)
            {
                Material ma= MaterialModule.Instance.GetMaterialByMaterialID(kvp.Key);
                if (ma != null)
                {
                    MaterialCostDic.Add(ma, kvp.Value);
                }
            }
            return MaterialCostDic;
        }


        public List<string> GetDistrictEffectStrList(DistrictData data)
        {
            return Utility.TryParseStringList(data.EffectList, ',');

        }

  
        #endregion


    }


    public class DistrictAreaInfo
    {
        public DistrictData data;
        public bool isLargeDistrict;
        public int LargeDistrictIndex;
        public Vector2 OriginCoordinate;
    }
}