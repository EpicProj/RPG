using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class FunctionBlockModule : BaseModule<FunctionBlockModule> {

        public enum FunctionBlockType
        {
            Manufacture,
            Raw,
            Science,
            Energy,
            Labor,
        }
        //Slot类型
        public enum FunctionBlockManuMaterialType
        {
            Input,
            Output,
            Byproduct
        }


        public static List<FunctionBlock> FunctionBlockList;
        public static Dictionary<int, FunctionBlock> FunctionBlockDic;

        public static List<FunctionBlock_Labor> FunctionBlock_LaborList;
        public static Dictionary<int, FunctionBlock_Labor> FunctionBlock_LaborDic;
        public static List<FunctionBlock_Raw> FunctionBlock_RawList;
        public static Dictionary<int, FunctionBlock_Raw> FunctionBlock_RawDic;
        public static List<FunctionBlock_Manufacture> FunctionBlock_ManufactureList;
        public static Dictionary<int, FunctionBlock_Manufacture> FunctionBlock_ManufactureDic;
        public static List<FunctionBlock_Science> FunctionBlock_ScienceList;
        public static Dictionary<int, FunctionBlock_Science> FunctionBlock_ScienceDic;
        public static List<FunctionBlock_Energy> FunctionBlock_EnergyList;
        public static Dictionary<int, FunctionBlock_Energy> FunctionBlock_EnergyDic;

        public static List<FunctionBlockTypeData> FunctionBlockTypeDataList;
        public static Dictionary<string, FunctionBlockTypeData> FunctionBlockTypeDataDic;


        private bool HasInit = false;
        public List<string> FunctionBlockGUIDList = new List<string>();
        public Dictionary<int, FunctionBlockHistory> FunctionBlockHistoryDic = new Dictionary<int, FunctionBlockHistory>();

        public Dictionary<string, FunctionBlock> CurrentFunctionBlockDataDic = new Dictionary<string, FunctionBlock>();

        //info Data
        public static ManufactoryBaseInfoData manufactoryBaseInfoData;
        public static LaborBaseInfoData laborBaseInfoData;

        #region Data

        public override void InitData()
        {
            if (HasInit)
                return;
            FunctionBlockList = FunctionBlockMetaDataReader.GetFunctionBlockData();
            FunctionBlockDic = FunctionBlockMetaDataReader.GetFunctionBlockDataDic();
            FunctionBlock_LaborList = FunctionBlockMetaDataReader.GetFunctionBlock_LaborData();
            FunctionBlock_LaborDic = FunctionBlockMetaDataReader.GetFunctionBlock_LaborDic();
            FunctionBlock_RawList = FunctionBlockMetaDataReader.GetFunctionBlockRowData();
            FunctionBlock_RawDic = FunctionBlockMetaDataReader.GetFunctionBlock_RawDic();
            FunctionBlock_ManufactureList = FunctionBlockMetaDataReader.GetFunctionBlock_ManufactureData();
            FunctionBlock_ManufactureDic = FunctionBlockMetaDataReader.GetFunctionBlock_ManufactureDic();
            FunctionBlock_ScienceList = FunctionBlockMetaDataReader.GetFunctionBlock_ScienceData();
            FunctionBlock_ScienceDic = FunctionBlockMetaDataReader.GetFunctionBlock_ScienceDic();
            FunctionBlock_EnergyList = FunctionBlockMetaDataReader.GetFunctionBlock_EnergyData();
            FunctionBlock_EnergyDic = FunctionBlockMetaDataReader.GetFunctionBlock_EnergyDic();

            FunctionBlockTypeDataList = FunctionBlockMetaDataReader.GetFunctionBlockTypeData();
            FunctionBlockTypeDataDic = FunctionBlockMetaDataReader.GetFunctionBlockTypeDataDic();

            //Init Info Data
            manufactoryBaseInfoData = new ManufactoryBaseInfoData();
            manufactoryBaseInfoData.LoadData();
            laborBaseInfoData = new LaborBaseInfoData();
            laborBaseInfoData.LoadData();
            HasInit = true;
        }



        private static bool CheckTypeValid(string type)
        {
            if (Enum.IsDefined(typeof(FunctionBlockType), type) == false)
            {
                Debug.LogError("FacotyType InValid! Type=" + type);
                return false;
            }
            return true;
        }

        #endregion
        #region Method Data

        //Get FunctionBlockType
        public static FunctionBlockType GetFunctionBlockType(int blockID)
        {
            FunctionBlock functionBlock = GetFunctionBlockByBlockID(blockID);
            if (CheckTypeValid(functionBlock.FunctionBlockType) == false)
                return FunctionBlockType.Energy;
            return (FunctionBlockType)Enum.Parse(typeof(FunctionBlockType), functionBlock.FunctionBlockType);
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
        public static FunctionBlockTypeData GetFacotryTypeData(int functionBlockID)
        {
            return GetFacotryTypeData(GetFunctionBlockType(functionBlockID));
        }

        /// <summary>
        /// Get FunctionBlock Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="functionBlockID"></param>
        /// <returns></returns>
        public static T FetchFunctionBlockTypeIndex<T>(int functionBlockID) where T : class
        {
            switch (GetFunctionBlockType(functionBlockID))
            {
                case FunctionBlockType.Manufacture:
                    return GetFunctionBlock_ManufactureData(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockTypeIndex) as T;
                case FunctionBlockType.Raw:
                    return GetFacotryRawData(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockTypeIndex) as T;
                case FunctionBlockType.Science:
                    return GetFunctionBlock_ScienceData(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockTypeIndex) as T;
                case FunctionBlockType.Energy:
                    return GetFunctionBlock_EnergyData(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockTypeIndex) as T;
                case FunctionBlockType.Labor:
                    return GetFunctionBlock_LaborData(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockTypeIndex) as T;
                default:
                    Debug.LogError("Fetch FacotryType Error facotryID=" + functionBlockID);
                    return null;
            }

        }

        public static FunctionBlock_Labor GetFunctionBlock_LaborData(int laborID)
        {
            FunctionBlock_Labor labor = null;
            FunctionBlock_LaborDic.TryGetValue(laborID, out labor);
            if (labor == null)
            {
                Debug.LogError("Get FunctionBlock_Labor Data Error! LaborID=" + labor);
            }
            return labor;
        }
        public static FunctionBlock_Manufacture GetFunctionBlock_ManufactureData(int manufactureID)
        {
            FunctionBlock_Manufacture functionBlock_Manufacture = null;
            FunctionBlock_ManufactureDic.TryGetValue(manufactureID, out functionBlock_Manufacture);
            if (functionBlock_Manufacture == null)
            {
                Debug.LogError("Get functionBlock_Manufacture Error , manufactureId=" + manufactureID);
            }
            return functionBlock_Manufacture;
        }

        public static FunctionBlock_Raw GetFacotryRawData(int rawID)
        {
            FunctionBlock_Raw functionBlock_Raw = null;
            FunctionBlock_RawDic.TryGetValue(rawID, out functionBlock_Raw);
            if (functionBlock_Raw == null)
            {
                Debug.LogError("Get FunctionBlock_Raw Error , rawID=" + rawID);
                return null;
            }
            return functionBlock_Raw;
        }

        public static FunctionBlock_Science GetFunctionBlock_ScienceData(int scienceID)
        {
            FunctionBlock_Science functionBlock_Science = null;
            FunctionBlock_ScienceDic.TryGetValue(scienceID, out functionBlock_Science);
            if (functionBlock_Science == null)
            {
                Debug.LogError("Get FunctionBlock_Raw Error , scienceID=" + scienceID);
                return null;
            }
            return functionBlock_Science;
        }

        public static FunctionBlock_Energy GetFunctionBlock_EnergyData(int energyID)
        {
            FunctionBlock_Energy functionBlock_Energy = null;
            FunctionBlock_EnergyDic.TryGetValue(energyID, out functionBlock_Energy);
            if (functionBlock_Energy == null)
            {
                Debug.LogError("Get FunctionBlock_Raw Error , energyID=" + energyID);
                return null;
            }
            return functionBlock_Energy;
        }

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

        public static ushort GetFunctionBlockMaxLevel(int functionBlockID)
        {
            return GetFunctionBlockByBlockID(functionBlockID).MaxLevel;
        }
        public static float GetManufactureSpeed(int functionBlockID)
        {
            return FetchFunctionBlockTypeIndex<FunctionBlock_Manufacture>(functionBlockID).SpeedBase;
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
            Dictionary<Vector2, DistrictData> totalDic = GetDistrictAreaDetailDefaultData(block);

            foreach (KeyValuePair<Vector2, DistrictData> kvp in totalDic)
            {
                if ( kvp.Value.DistrictID == -1)
                {
                    //empty 
                    DistrictAreaBase info = new DistrictAreaBase
                    {
                        data = kvp.Value,
                        Locked = false,
                        slotType = DistrictSlotType.Empty,
                        Coordinate = kvp.Key,
                        sprite = DistrictModule.GetDistrictIconSpriteList(kvp.Value.DistrictID)[0]
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
                        slotType= DistrictSlotType.UnLock,
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
            Dictionary<Vector2, DistrictData> dic = GetDistrictOriginData(block);

            if (dic == null)
                return result;
            //Check Range
            if (CheckDistrictDataOutofRange(block, true) && CheckTargetDistrictNoLock(block, true))
            {
                int largeDistrictIndex = 1;
                foreach (KeyValuePair<Vector2, DistrictData> kvp in dic)
                {
                    //District Larger than 1X1
                    var largeArea = DistrictModule.GetDistrictTypeArea(kvp.Value);
                    if (largeArea.Count == 1)
                    {
                        //1x1 grid
                        DistrictAreaInfo info = new DistrictAreaInfo
                        {
                            data = kvp.Value,
                            isLargeDistrict = false,
                            slotType = DistrictSlotType.NormalDistrict,
                            OriginCoordinate = kvp.Key,
                            sprite = DistrictModule.GetDistrictIconSpriteList(kvp.Value.DistrictID)[0]
                        };
                        result.Add(kvp.Key, info);
                        continue;
                    }
                    else if (largeArea.Count > 1)
                    {
                        //Add Area List
                        for (int i = 0; i < largeArea.Count; i++)
                        {
                            Vector2 currentPos = new Vector2(largeArea[i].x + kvp.Key.x, largeArea[i].y + kvp.Key.y);
                            DistrictAreaInfo info = new DistrictAreaInfo
                            {
                                data = kvp.Value,
                                isLargeDistrict = true,
                                LargeDistrictIndex = largeDistrictIndex,
                                slotType = DistrictSlotType.LargeDistrict,
                                OriginCoordinate = new Vector2(largeArea[0].x + kvp.Key.x, largeArea[0].y + kvp.Key.y),
                                sprite = DistrictModule.GetDistrictIconSpriteList(kvp.Value.DistrictID)[i]
                            };
                            result.Add(currentPos, info);
                        }
                        largeDistrictIndex++;
                    }
                    else
                    {
                        Debug.LogError("DistrictData Area Error ,ID=" + kvp.Value.DistrictID);
                        continue;
                    }
                }

            }
            return result;
        }
        private static Dictionary<Vector2,DistrictData> GetDistrictOriginData(FunctionBlock block)
        {
            return GetFuntionBlockDistrictData(GetFuntionBlockOriginArea(block));
        }
        private static Dictionary<Vector2,DistrictData> GetDistrictAreaDetailDefaultData(FunctionBlock block)
        {
            return GetFuntionBlockDistrictData(GetFuntionBlockAreaDetailDefault(block));
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
            List<Vector2> lockedList = GetLockedDistrictList(block);
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


        private static List<Vector2> GetLockedDistrictList(FunctionBlock block)
        {
            List<Vector2> result = new List<Vector2>();
            Dictionary<Vector2, DistrictData> totalDic = GetFuntionBlockDistrictData(GetFuntionBlockAreaDetailDefault(block));
            foreach(KeyValuePair<Vector2,DistrictData> kvp in totalDic)
            {
                if (kvp.Value.DistrictID == -2)
                {
                    result.Add(kvp.Key);
                }
            }
            return result;
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
            int id = block.FunctionBlockID;
            Vector2 areaMax = GetFunctionBlockAreaMax(block);
            return TryParseAreaDetailDefault(block.AreaDetailDefault, areaMax);
        }

        private static Dictionary<Vector2, int> TryParseAreaDetailDefault(string content, Vector2 areaMax)
        {
            Dictionary<Vector2, int> result = new Dictionary<Vector2, int> ();
            List<string> ls = Utility.TryParseStringList(content, ';');
            if (string.IsNullOrEmpty(content) || ls.Count == 0)
            {
                Debug.LogWarning("Parse AreaDetailFail!  content="+content);
                return result;
            }
            if(ls.Count != areaMax.y)
            {
                Debug.LogError("AreaY Not Match Area Max ,content=" + content);
                return result;
            }

            for (int i = 0; i < ls.Count; i++)
            {
                List<int> li = Utility.TryParseIntList(ls[i], ',');
                if (li.Count != areaMax.x)
                {
                    Debug.LogError("AreaX Not Match Area Max ,content=" + content);
                    return result;
                }
                for (int j = 0; j < li.Count; j++)
                {
                    Vector2 pos = new Vector2(i,j);
                    result.Add(pos, li[2]);
                }
            }
            return result;
        }

        private static Dictionary<Vector2 ,int> GetFuntionBlockOriginArea(FunctionBlock block)
        {
            return TryParseOriginArea(block.OriginArea);
        }
        private static Dictionary<Vector2,int> TryParseOriginArea(string content)
        {
            Dictionary<Vector2, int> result = new Dictionary<Vector2, int>();
            List<string> ls = Utility.TryParseStringList(content, ';');
            if(string.IsNullOrEmpty(content) || ls.Count == 0)
            {
                Debug.Log("OriginArea is Empty!");
                return result;
            }
            for(int i = 0; i < ls.Count; i++)
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
        }
        #endregion

        #region Main Function

        public static int GetDistrictAreaIndex(Vector2 areaMax,Vector2 currentVector)
        {
            return (int)(currentVector.x * areaMax.x + currentVector.y);
        }


        //Place Block
        public void PlaceFunctionBlock(int functionBlockID,Vector3 checkpos)
        {
            FunctionBlock currentFunctionBlock = GetFunctionBlockByBlockID(functionBlockID);
            if (currentFunctionBlock == null)
                return ;
            if (CheckBlockCanPlace(currentFunctionBlock,checkpos) )
            {
                
            }
            else
            {
                Debug.Log("Can not place functionBlock ");
                return ;
            }
           


            //FunctionBlockHistory history = new FunctionBlockHistory(1, functionBlockID);

            //FunctionBlockHistoryDic.Add(functionBlockID, history);
        }


        public string GenerateGUID(FunctionBlock functionBlock)
        {
            string GUID= Guid.NewGuid().ToString();
            if (FunctionBlockGUIDList.Contains(GUID))
            {
                GenerateGUID(functionBlock);
            }
            CurrentFunctionBlockDataDic.Add(GUID, functionBlock);
            FunctionBlockGUIDList.Add(GUID);
            return GUID;
        }


        public bool CheckBlockCanPlace(FunctionBlock block,Vector3 checkPos)
        {
            //Todo
            return true;
        }


        public void InitDistrictBlockModel<T>(FunctionBlock block) where T:class
        {
            //Dictionary<Vector2, DistrictAreaInfo> districtDic = GetFuntionBlockAreaDetailDefaultData<T>(block);
            //if (districtDic == null)
            //    return;
            
            //foreach(KeyValuePair<Vector2, DistrictAreaInfo> kvp in districtDic)
            //{
            //    var data = kvp.Value.data;
            //    if(data==null || data.DistrictID == -1 || data.DistrictID == -2)
            //    {
            //        //empty Model
            //        continue;
            //    }

            //}

          
        }

        /// <summary>
        /// Init Block Box Collider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        public Vector3 InitFunctionBlockBoxCollider(FunctionBlock block)
        {
            Vector2 area = GetFunctionBlockAreaMax(block);
            return new Vector3(area.x, 3.0f, area.y);
        }

        #region BlockInfoData
        /// <summary>
        ///  区块固有等级
        /// </summary>
        /// <param name="blockID"></param>
        /// <returns></returns>
        public static ManufactoryBaseInfoData.ManufactureInherentLevelData GetManuInherentLevelData(FunctionBlock_Manufacture manuData)
        {
            List<ManufactoryBaseInfoData.ManufactureInherentLevelData> ManuLevel = manufactoryBaseInfoData.InherentLevelDatas;
            if (ManuLevel == null)
            {
                Debug.LogError("Can not Find Manu InherentLevelData!");
                return null;
            }
            return ManuLevel.Find(x => x.Name == manuData.InherentLevel);
        }

        public static string GetCurrentInherentLevelName(ManufactoryBaseInfoData.ManufactureInherentLevelData level)
        {
            if (level == null)
            {
                Debug.LogError("Manu InherentLevel is null");
                return string.Empty;
            }
            return MultiLanguage.Instance.GetTextValue(level.LevelName);
        }
        public static LaborBaseInfoData.LaborInherentLevelData GetLaborInherentLevelData(FunctionBlock_Labor laborData)
        {
            List<LaborBaseInfoData.LaborInherentLevelData> laborLevel = laborBaseInfoData.InherentLevelDatas;
            if (laborLevel == null)
            {
                Debug.LogError("Can not Find Labor InherentLevelData!");
                return null;
            }
            return laborLevel.Find(x => x.LevelName == laborData.InherentLevel);
        }
        private static List<string> GetBlockInherentLevelList()
        {
            List<string> result = new List<string>();
            foreach(var data in manufactoryBaseInfoData.InherentLevelDatas)
            {
                if (result.Contains(data.Name))
                {
                    Debug.LogError("Find Same BlockInhernet Level Data ID=" + data.Name);
                    continue;
                }
                result.Add(data.Name);
            }
            return result;
        }

        /// <summary>
        /// EXP
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static List<int> GetBlockEXPMapData(string id ,FunctionBlockType type)
        {
            switch (type)
            {
                case FunctionBlockType.Manufacture:
                    List<BlockLevelData> manuData = manufactoryBaseInfoData.BlockLevelDatas;
                    if (manuData == null)
                    {
                        Debug.LogError("Can not Find ManuBlockEXP Map  id=" + id);
                        return null;
                    }
                    return manuData.Find(x => x.ID == id).EXPMap;
                case FunctionBlockType.Labor:
                    List<BlockLevelData> laborData = laborBaseInfoData.BlockLevelDatas;
                    if (laborData == null)
                    {
                        Debug.LogError("Can not Find LaborEXP Map  id=" + id);
                        return null;
                    }
                    return laborData.Find(x => x.ID == id).EXPMap;
                default:
                    return null;
            }
           
        }

        public static List<int> GetBlockEXPMapData(int blockid)
        {
            FunctionBlockType type = GetFunctionBlockType(blockid);
            switch (type)
            {
                case FunctionBlockType.Manufacture:
                    return GetBlockEXPMapData(GetFunctionBlockByBlockID(blockid).EXPDataJsonIndex,type);
                case FunctionBlockType.Labor:
                    return GetBlockEXPMapData(GetFunctionBlockByBlockID(blockid).EXPDataJsonIndex,type);
                default:
                    return null;
            }
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

        private static List<BlockDistrictUnlockData.DistrictUnlockData> GetManuBlockDistrictUnlockData(string id , FunctionBlockType type)
        {
            switch (type)
            {
                case FunctionBlockType.Manufacture:
                    List < BlockDistrictUnlockData .DistrictUnlockData> manuData= manufactoryBaseInfoData.DistrictUnlockDatas.Find(x => x.ID == id).UnlockData;
                    if (manuData == null)
                    {
                        Debug.LogError("can not find unlockdata,id=" + id);
                        return null;
                    }
                    return manuData;

                case FunctionBlockType.Labor:
                    List<BlockDistrictUnlockData.DistrictUnlockData> laborData = laborBaseInfoData.DistrictUnlockDatas.Find(x => x.ID == id).UnlockData;
                    if (laborData == null)
                    {
                        Debug.LogError("can not find unlockdata,id=" + id);
                        return null;
                    }
                    return laborData;

                default:
                    return null;
            }
          
          
           
        }
        /// <summary>
        /// get district unlockData
        /// </summary>
        /// <param name="blockid"></param>
        /// <returns></returns>
        public static List<BlockDistrictUnlockData.DistrictUnlockData> GetManuBlockDistrictUnlockData(int blockid)
        {
            FunctionBlockType type = GetFunctionBlockType(blockid);
            return GetManuBlockDistrictUnlockData(GetFunctionBlockByBlockID(blockid).DistrictData,type);
        }

        #endregion

        //Formula
        public static List<Dictionary<Material,ushort>> GetFunctionBlockFormulaDataList(FunctionBlock block,FormulaModule.MaterialProductType GetType)
        {
            List<Dictionary<Material, ushort>> result = new List<Dictionary<Material, ushort>>();

            List<FormulaData> data = GetFormulaDataList(block);
            for (int i = 0; i < data.Count; i++)
            {
                Dictionary<Material, ushort> maDic = FormulaModule.GetFormulaMaterialDic(data[i].FormulaID, GetType);
                result.Add(maDic);
            }
            return result;
        }
        public static List<FormulaData> GetFormulaDataList(FunctionBlock block)
        {
            return FormulaModule.GetFormulaDataList(FetchFunctionBlockTypeIndex<FunctionBlock_Manufacture>(block.FunctionBlockID).FormulaInfoID);
        }


        #endregion
    }

    public class FunctionBlockHistory
    {
        public string FunctionBlockGUID;
        public int FacotoryID;
        public string BuildDate;




        public FunctionBlockHistory(FunctionBlock block ,string buildData)
        {
            FacotoryID = block.FunctionBlockID;
            FunctionBlockGUID = FunctionBlockModule.Instance.GenerateGUID(block);
            this.BuildDate = buildData;

        }

    }





}