using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class FunctionBlockModule : BaseModule<FunctionBlockModule> {

    

        /// <summary>
        /// 格子类型
        /// </summary>
        public enum FunctionBlockManuMaterialType
        {
            Input,
            Output,
            Byproduct
        }


        protected static List<FunctionBlock> FunctionBlockList;
        protected static Dictionary<int, FunctionBlock> FunctionBlockDic;

        protected static List<FunctionBlockTypeData> FunctionBlockTypeDataList;
        protected static Dictionary<string, FunctionBlockTypeData> FunctionBlockTypeDataDic;


        #region Data

        public override void InitData()
        {
            FunctionBlockList = FunctionBlockMetaDataReader.GetFunctionBlockData();
            FunctionBlockDic = FunctionBlockMetaDataReader.GetFunctionBlockDataDic();

            FunctionBlockTypeDataList = FunctionBlockMetaDataReader.GetFunctionBlockTypeData();
            FunctionBlockTypeDataDic = FunctionBlockMetaDataReader.GetFunctionBlockTypeDataDic();

        }

        public override void Register()
        {
            
        }

        public FunctionBlockModule()
        {
            InitData();
        }
        #endregion

        #region Type
        private static bool CheckTypeValid(string type)
        {
            if (Enum.IsDefined(typeof(FunctionBlockType), type) == false)
            {
                Debug.LogError("FactoryType InValid! Type=" + type);
                return false;
            }
            return true;
        }

        //Get FunctionBlockType
        public static FunctionBlockType GetFunctionBlockType(int blockID)
        {
            var block = GetFunctionBlockByBlockID(blockID);
            if (CheckTypeValid(block.FunctionBlockType) == false)
                return FunctionBlockType.None;
            return (FunctionBlockType)Enum.Parse(typeof(FunctionBlockType), block.FunctionBlockType);
        }


        //Get Type Data
        public static FunctionBlockTypeData GetFacotryTypeData(FunctionBlockType type)
        {
            FunctionBlockTypeData typeData = null;
            FunctionBlockTypeDataDic.TryGetValue(type.ToString(), out typeData);
            if (typeData != null)
            {
                return typeData;
            }
            else
            {
                Debug.LogError("GetFunctionBlockType Error Type= " + type);
                return null;
            }
        }

        public static FunctionBlockType GetBlockType(FunctionBlockTypeData data)
        {
            FunctionBlockType result = FunctionBlockType.None;
            Enum.TryParse<FunctionBlockType>(data.Type, out result);
            return result;
        }

        public static FunctionBlockTypeData GetFacotryTypeData(int functionBlockID)
        {
            return GetFacotryTypeData(GetFunctionBlockType(functionBlockID));
        }

        public static List<FunctionBlockTypeData> GetInitMainType()
        {
            List<FunctionBlockTypeData> result = new List<FunctionBlockTypeData>();
            for(int i = 0; i < FunctionBlockTypeDataList.Count; i++)
            {
                if (FunctionBlockTypeDataList[i].DefaultShow == true)
                {
                    result.Add(FunctionBlockTypeDataList[i]);
                }
            }
            return result;
        }

        public static Sprite GetMainTypeSprite(FunctionBlockTypeData data)
        {
            return Utility.LoadSprite(data.TypeIcon, Utility.SpriteType.png);
        }
        public static string GetMainTypeName(FunctionBlockTypeData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.TypeName);
        }


        #endregion

        #region Method Data

        public static Sprite GetFunctionBlockIcon(int functionBlockID)
        {
            string path = GetFunctionBlockByBlockID(functionBlockID).BlockIcon;
            return Utility.LoadSprite(path,Utility.SpriteType.png);
         
        }
        public static Sprite GetFunctionBlockBG(int functionBlockID)
        {
            string path = GetFunctionBlockByBlockID(functionBlockID).BlockBG;
            return Utility.LoadSprite(path, Utility.SpriteType.png);
        }

        public static Sprite GetFunctionBlockTypeIcon(int functionBlockID)
        {
            var type = GetFunctionBlockType(functionBlockID);
            if(type!= FunctionBlockType.None)
            {
                var typedata = GetFacotryTypeData(type);
                if (typedata != null)
                    return Utility.LoadSprite(typedata.TypeIcon, Utility.SpriteType.png);
            }
            return null;
        }

        public static FunctionBlock GetFunctionBlockByBlockID(int functionBlockID)
        {
            FunctionBlock functionBlock = null;
            FunctionBlockDic.TryGetValue(functionBlockID, out functionBlock);
            if (functionBlock == null)
                Debug.LogError("Get FunctionBlock Error , ID=" + functionBlockID);
            return functionBlock;
        }

        public static string GetFunctionBlockName(int functionBlockID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFunctionBlockByBlockID(functionBlockID).BlockName);
        }
        public static string GetFunctionBlockName(FunctionBlock block)
        {
            return MultiLanguage.Instance.GetTextValue(block.BlockName);
        }
        public static string GetFunctionBlockDesc(int functionBlockID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFunctionBlockByBlockID(functionBlockID).BlockDesc);
        }
        public static string GetFunctionBlockDesc(FunctionBlock block)
        {
            return MultiLanguage.Instance.GetTextValue(block.BlockDesc);
        }

        public static Vector2 GetFunctionBlockAreaMax(FunctionBlock block)
        {
            return Utility.TryParseIntVector2(block.AreaMax, ',');
        }

        /// <summary>
        /// ID=-1  Empty  ;  ID=-2   UnLock;
        /// 生成所有区划底面信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        public static Dictionary<Vector2, DistrictAreaBase> GetFuntionBlockAreaDetailDefaultDataInfo(FunctionBlock block) 
        {
            Dictionary<Vector2, DistrictAreaBase> result = new Dictionary<Vector2, DistrictAreaBase>();

            Func<FunctionBlock, Dictionary<Vector2, DistrictData>> getDefaultData = (b) =>
            {
                return GetFuntionBlockDistrictData(GetFuntionBlockAreaDetailDefault(b));
            };

            Dictionary<Vector2, DistrictData> totalDic = getDefaultData(block);

            foreach (KeyValuePair<Vector2, DistrictData> kvp in totalDic)
            {
                if ( kvp.Value.DistrictID == -1)
                {
                    //empty 
                    DistrictAreaBase info = new DistrictAreaBase
                    {
                        data = kvp.Value,
                        Locked = false,
                        slotType = UI.DistrictSlotType.Empty,
                        Coordinate = kvp.Key,
                        //sprite = DistrictModule.GetDistrictIconSpriteList(kvp.Value.DistrictID)[0]
                    };
                    result.Add(kvp.Key, info);
                }
                else if (kvp.Value.DistrictID == -2)
                {
                    //Locked
                    DistrictAreaBase info = new DistrictAreaBase
                    {
                        data = kvp.Value,
                        Locked = true,
                        slotType= UI.DistrictSlotType.UnLock,
                        Coordinate = kvp.Key
                    };
                    result.Add(kvp.Key, info);
                }
                else
                {
                    Debug.LogError("AreaDetail Default parse Error  not -1 or -2  block id=" + block.FunctionBlockID);
                    return result;
                }
            }

            return result;

        }

        /// <summary>
        /// 生成所有初始区划信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        public static Dictionary<Vector2, DistrictAreaInfo> GetFuntionBlockOriginAreaInfo(FunctionBlock block)
        {

            Dictionary<Vector2, DistrictAreaInfo> result = new Dictionary<Vector2, DistrictAreaInfo>();
            Dictionary<Vector2, DistrictData> dic = GetFuntionBlockDistrictData(GetFuntionBlockOriginArea(block));

            if (dic == null)
                return result;
            //Check Range
            if (CheckDistrictDataOutofRange(block, true) && CheckTargetDistrictNoLock(block, true))
            {
                foreach (KeyValuePair<Vector2, DistrictData> kvp in dic)
                {
                    var type = DistrictModule.GetDistrictTypeArea(kvp.Value);
                    if (type.Count == 1)
                    {
                        DistrictAreaInfo info = new DistrictAreaInfo(kvp.Value.DistrictID, kvp.Key,kvp.Key);
                        result.Add(kvp.Key, info);
                    }
                    else if (type.Count > 1)
                    {

                    }
                   
                }
            }
            return result;
        }

        /// <summary>
        /// 检测区划是否重叠  或超出范围
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <param name="initCheck"></param>
        /// <returns></returns>
        public static bool CheckDistrictDataOutofRange(FunctionBlock block,bool initCheck) 
        {
            List<Vector2> CheckContent = new List<Vector2>();
            var Checkdic = GetFuntionBlockDistrictData(GetFuntionBlockOriginArea(block));
            var areaMax = GetFunctionBlockAreaMax(block);
            foreach (KeyValuePair<Vector2,DistrictData> kvp in Checkdic)
            {
                //Check District
                var v2 = DistrictModule.GetDistrictTypeArea(kvp.Value);
                for(int i = 0; i < v2.Count; i++)
                {
                    Vector2 currentPos = new Vector2(v2[i].x + kvp.Key.x, v2[i].y + kvp.Key.y);
                    if (CheckContent.Contains(currentPos))
                    {
                        if (initCheck)
                            Debug.LogError("DistrictData  OverLap!,vector2=" + kvp.Key);
                        return false;
                    }
                    CheckContent.Add(currentPos);
                }
            }
            for(int j = 0; j < CheckContent.Count; j++)
            {
                if(CheckContent[j].x>areaMax.x || CheckContent[j].y > areaMax.y)
                {
                    if (initCheck)
                        Debug.LogError("DistrictData  Out of Range!,vector2=" + CheckContent[j]);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查是否区划处于未解锁的格子内
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <param name="initCheck"></param>
        /// <returns></returns>
        private static bool CheckTargetDistrictNoLock(FunctionBlock block,bool initCheck)
        {
            Dictionary<Vector2, DistrictData> Checkdic = GetFuntionBlockDistrictData(GetFuntionBlockOriginArea(block));

            Func<FunctionBlock, List<Vector2>> getDistrict = (b) =>
             {
                 List<Vector2> result = new List<Vector2>();
                 Dictionary<Vector2, DistrictData> totalDic = GetFuntionBlockDistrictData(GetFuntionBlockAreaDetailDefault(b));
                 foreach (KeyValuePair<Vector2, DistrictData> kvp in totalDic)
                 {
                     if (kvp.Value.DistrictID == -2)
                     {
                         result.Add(kvp.Key);
                     }
                 }
                 return result;
             };

            List<Vector2> lockedList = getDistrict(block);
            foreach (KeyValuePair<Vector2,DistrictData> kvp in Checkdic)
            {
                //Check District
                List<Vector2> v2 = DistrictModule.GetDistrictTypeArea(kvp.Value);
                for (int i = 0; i < v2.Count; i++)
                {
                    Vector2 currentPos = new Vector2(v2[i].x + kvp.Key.x, v2[i].y + kvp.Key.y);
                    if (lockedList.Contains(currentPos))
                    {
                        if (initCheck)
                            Debug.LogError("DistrictData is in Locked District !,vector2=" + kvp.Key);
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// Get District Data
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private static Dictionary<Vector2, DistrictData> GetFuntionBlockDistrictData(Dictionary<Vector2, int> dic)
        {
            Dictionary<Vector2, DistrictData> result = new Dictionary<Vector2, DistrictData>();
            if (dic == null)
                return result;
            foreach(KeyValuePair<Vector2, int> kvp in dic)
            {
                DistrictData dd = DistrictModule.GetDistrictDataByKey(kvp.Value);
                if (dd == null)
                    continue;
                result.Add(kvp.Key, dd);
            }
            return result;
        }

        private static Dictionary<Vector2, int> GetFuntionBlockAreaDetailDefault(FunctionBlock block)
        {
            Func<string, Vector2, Dictionary<Vector2, int>> parse = (s, v) =>
             {
                 Dictionary<Vector2, int> result = new Dictionary<Vector2, int>();
                 List<string> ls = Utility.TryParseStringList(s, ';');
                 if (string.IsNullOrEmpty(s) || ls.Count == 0)
                 {
                     Debug.LogWarning("Parse AreaDetailFail!  content=" + s);
                     return result;
                 }
                 if (ls.Count != v.y)
                 {
                     Debug.LogError("AreaY Not Match Area Max ,content=" + s);
                     return result;
                 }

                 for (int i = 0; i < ls.Count; i++)
                 {
                     List<int> li = Utility.TryParseIntList(ls[i], ',');
                     if (li.Count != v.x)
                     {
                         Debug.LogError("AreaX Not Match Area Max ,content=" + s);
                         return result;
                     }
                     for (int j = 0; j < li.Count; j++)
                     {
                         Vector2 pos = new Vector2(i, j);
                         result.Add(pos, li[2]);
                     }
                 }
                 return result;
             };
            int id = block.FunctionBlockID;
            Vector2 areaMax = GetFunctionBlockAreaMax(block);
            return parse(block.AreaDetailDefault, areaMax);
        }

        private static Dictionary<Vector2 ,int> GetFuntionBlockOriginArea(FunctionBlock block)
        {
            Func<string, Dictionary<Vector2, int>> parse = (s) =>
            {
                Dictionary<Vector2, int> result = new Dictionary<Vector2, int>();
                List<string> ls = Utility.TryParseStringList(s, ';');
                if (string.IsNullOrEmpty(s) || ls.Count == 0)
                {
                    Debug.Log("OriginArea is Empty!");
                    return result;
                }
                for (int i = 0; i < ls.Count; i++)
                {
                    List<int> li = Utility.TryParseIntList(ls[i], ',');
                    if (li.Count != 3)
                    {
                        Debug.LogError("Parse OriginArea Format Error , content=" + li);
                        return result;
                    }
                    Vector2 v = new Vector2(li[0], li[1]);
                    if (result.ContainsKey(v))
                    {
                        Debug.LogError("Find Same Origin Area ,vector2=" + v);
                        return result;
                    }
                    result.Add(v, li[2]);
                }
                return result;
            };

            return parse(block.OriginArea);
        }
        #endregion


        #region Main Function

        public static int GetDistrictAreaIndex(Vector2 areaMax,Vector2 currentVector)
        {
            return (int)(currentVector.x * areaMax.x + currentVector.y);
        }


 
        /// <summary>
        /// Init Block Box Collider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        public Vector3 InitFunctionBlockBoxCollider(FunctionBlock block,float height)
        {
            Vector2 area = GetFunctionBlockAreaMax(block);
            return new Vector3(area.x, height, area.y);
        }

        #region BlockInfoData

        public static List<FormulaData> GetBlockFormulaList(int blockID)
        {
            List<FormulaData> result = new List<FormulaData>();
            var block = GetFunctionBlockByBlockID(blockID);
            if (block != null)
            {
                var config = Config.ConfigData.BlockConfigData.configData.Find(x => x.configName == block.BlockName);
                if (config == null)
                    return null;
                if (config.manuConfig != null)
                {
                    for (int i = 0; i < config.manuConfig.formulaIDList.Count; i++)
                    {
                        var formulaData = FormulaModule.GetFormulaDataByID(config.manuConfig.formulaIDList[i]);
                        if (formulaData != null)
                            result.Add(formulaData);                  
                    }
                    return result;
                }
            }
            return null;
        }

        public static List<int> GetBlockEXPMapData(int blockid)
        {
            var block = GetFunctionBlockByBlockID(blockid);
            if (block != null)
            {
                var config = Config.ConfigData.BlockConfigData.configData.Find(x => x.configName == block.BlockConfig);
                if (config != null)
                    return config.levelConfig.EXPMap;
            }
            return null;
        }

        public static int GetCurrentLevelEXP(List<int> expMap ,int currentLevel)
        {
            if (expMap == null)
            {
                Debug.LogError("EXPMAP IS NULL");
                return 0;
            }
               
            if (currentLevel > expMap.Count)
            {
                Debug.LogError("EXP data not found , index out of range, currentLevel=" + currentLevel + "expMap count=" + expMap.Count);
                return 0;
            }
            else
            {
                return expMap[currentLevel - 1];
            }
        }

        public static List<Config.BlockDistrictUnlockData.DistrictUnlockData> GetBlockDistrictUnlockData(int blockID)
        {
            var block = GetFunctionBlockByBlockID(blockID);
            if (block != null)
            {
                var config = Config.ConfigData.BlockConfigData.configData.Find(x => x.configName == block.BlockConfig);
                if (config != null)
                    return config.levelConfig.districtUnlock.UnlockData;
            }
            return null;
        }

        #endregion
        #endregion
    }

    public class FunctionBlockHistory
    {
        public string FunctionBlockGUID;
        public int FacotoryID;
        public string BuildDate;




        public FunctionBlockHistory(FunctionBlockInfoData blockInfo ,string buildData)
        {
            FacotoryID = blockInfo.BlockID;
            FunctionBlockGUID = blockInfo.dataModel.GUID;
            this.BuildDate = buildData;

        }

    }





}