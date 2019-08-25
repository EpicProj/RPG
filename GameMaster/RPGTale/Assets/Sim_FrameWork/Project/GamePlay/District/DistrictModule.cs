using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class DistrictModule : BaseModule<DistrictModule> {

        #region Row Data
        public static List<DistrictData> DistrictDataList = new List<DistrictData>();
        public static Dictionary<int, DistrictData> DistrictDataDic = new Dictionary<int, DistrictData>();

        private bool HasInit = false;
        public override void InitData()
        {
            if (HasInit)
                return;
            DistrictDataList = DistrictMetaDataReader.GetDistrictData();
            DistrictDataDic = DistrictMetaDataReader.GetDistrictDic();
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

        public Vector2 GetDistrictArea(DistrictData data)
        {
            return Utility.TryParseIntVector2(data.Area, ',');
        }
        public Vector2 GetDistrictArea(int DistrictID)
        {
            return Utility.TryParseIntVector2(GetDistrictDataByKey(DistrictID).Area, ',');
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


        public Vector2 FindMaxDistrictBlockVector(DistrictAreaInfo info)
        {
            if (info == null || info.largeDistrictIndex==null)
            {
                Debug.LogWarning("Parse DistrictBlockVector Error  ");
            }
            List<Vector2> list = info.largeDistrictIndex;
            return list[list.Count-1];
        }

        //public List<Vector2> FindMaxDistrictAreaVectorList(DistrictData data,Vector2 first)
        //{
        //    List<Vector2> result = new List<Vector2>();
        //    Vector2 max = FindMaxDistrictBlockVector(data, first);
        //    float x = max.x - first.x + 1;
        //    float y = max.y - first.y + 1;
        //    for(int i=0;i<x; i++)
        //    {
        //        Vector2 v = new Vector2(first.x + i, first.y);
        //        result.Add(v);
        //        for(int j = 0; j < y; j++)
        //        {
        //            Vector2 v1 = new Vector2(first.x, first.y+j);
        //            if (result.Contains(v1))
        //                continue;
        //            result.Add(v1);
        //        }
        //    }
        //    return result;
        //}

        #endregion


    }


    public class DistrictAreaInfo
    {
        public DistrictData data;
        public bool isLargeDistrict;
        public List<Vector2> largeDistrictIndex;
    }
}