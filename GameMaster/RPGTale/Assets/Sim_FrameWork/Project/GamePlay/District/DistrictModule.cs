using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class DistrictModule : BaseModule<DistrictModule>{

        #region Row Data
        public static List<DistrictData> DistrictDataList;
        public static Dictionary<int, DistrictData> DistrictDataDic;
        public static List<DistrictType> DistrictTypeList;
        public static Dictionary<int, DistrictType> DistrictTypeDic;



        public override void InitData()
        {
            DistrictDataList = DistrictMetaDataReader.GetDistrictData();
            DistrictDataDic = DistrictMetaDataReader.GetDistrictDic();
            DistrictTypeList = DistrictMetaDataReader.GetDistrictType();
            DistrictTypeDic = DistrictMetaDataReader.GetDistrictTypeDic();
        }

        public override void Register()
        {
            
        }

        public DistrictModule()
        {
            InitData();
        }

        public static DistrictData GetDistrictDataByKey(int districtID)
        {
            DistrictData data = null;
            DistrictDataDic.TryGetValue(districtID, out data);
            if (data == null)
                Debug.LogError("Can not Find DistrictData ,ID=" + districtID);
            return data;
        }

        public static string GetDistrictName(int districtID)
        {
            return MultiLanguage.Instance.GetTextValue(GetDistrictDataByKey(districtID).DistrictName);
        }
        public static string GetDistrictName(DistrictData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.DistrictName);
        }
        public static string GetDistrictDesc(int districtID)
        {
            return MultiLanguage.Instance.GetTextValue(GetDistrictDataByKey(districtID).DistrictDesc);
        }


        public static string GetDistrictDesc(DistrictData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.DistrictDesc);
        }


        public static DistrictType GetDistrictType(int districtID)
        {
            return GetDistrictType(GetDistrictDataByKey(districtID));
        }
        public static DistrictType GetDistrictType(DistrictData data)
        {
            DistrictType type = null;
            DistrictTypeDic.TryGetValue(data.Type, out type);
            if (type == null)
                Debug.LogError("Can not Get districtType ID=" + data.Type);
            return type;
        }

        /// <summary>
        /// Get Shape Area
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Vector2> GetDistrictTypeArea(DistrictData data)
        {
            return GetDistrictTypeArea(data.DistrictID);
        }
        public static List<Vector2> GetDistrictTypeArea(int DistrictID)
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

        public static List<Vector2> GetRealDistrictTypeArea(int DistrictID,Vector2 pos)
        {
            List<Vector2> result = new List<Vector2>();
            List<Vector2> input = GetDistrictTypeArea(DistrictID);
            for (int i=0;i< input.Count; i++)
            {
                result.Add(new Vector2(input[i].x + pos.x, input[i].y + pos.y));
            }
            return result;
        }
        public static List<Vector2> GetRealDistrictTypeArea(DistrictData data, Vector2 pos)
        {
            return GetRealDistrictTypeArea(data.DistrictID, pos);
        }
        public static Dictionary<int, int> GetDistrictMaterialCostDic(DistrictData data)
        {
            Dictionary<int, int> result = new Dictionary<int, int> ();
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


        /// <summary>
        /// 获取区划模型
        /// </summary>
        /// <param name="districtID"></param>
        /// <returns></returns>
        public static GameObject GetDistrictPrefab(int districtID)
        {
            var data = GetDistrictType(districtID);
            if (data != null)
                return ObjectManager.Instance.InstantiateObject(data.ModelPath);
            return
                null;
        }
        #endregion

        #region Function

        public Dictionary<Material,int> GetMaterialCost(DistrictData data)
        {
            Dictionary<Material, int> MaterialCostDic = new Dictionary<Material, int> ();
            Dictionary<int, int> cost = GetDistrictMaterialCostDic(data);
            foreach(KeyValuePair<int,int> kvp in cost)
            {
                Material ma= MaterialModule.GetMaterialByMaterialID(kvp.Key);
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
        public bool Locked;
        public bool isNone;
        public Vector2 Coordinate;

        public int districtID;

        /// <summary>
        /// 规划格类型
        /// </summary>
        public UI.DistrictSlotType slotType;
        public string prefabModelPath;

        public DistrictAreaInfo() { }
        public DistrictAreaInfo InitData(Config.Block_District_GridConfig config)
        {
            DistrictAreaInfo info = new DistrictAreaInfo();
            info.Locked = config.unlockDefault;
            info.isNone = config.isNone;
            info.Coordinate = new Vector2(config.coordinate[0], config.coordinate[1]);

            //var districtList= DistrictModule.GetDistrictTypeArea(districtID);
            //for(int i = 0; i < districtList.Count; i++)
            //{
            //    largeDistrictCoordinateList.Add(districtList[i] + originPos);
            //}
            //OriginCoordinate = originPos;
            //RealCoordinate = realPos;
            //slotType = isLargeDistrict ? UI.DistrictSlotType.LargeDistrict : UI.DistrictSlotType.NormalDistrict;
            //if (realPos == originPos)
            //{
            //    prefabModelPath = DistrictModule.GetDistrictType(districtID).ModelPath;
            //}
            return info;

        }

    }


}