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
            Energy
        }
        //Slot类型
        public enum FunctionBlockManuMaterialType
        {
            Input,
            Output,
            Byproduct
        }


        public List<FunctionBlock> FunctionBlockList=new List<FunctionBlock> ();
        public Dictionary<int, FunctionBlock> FunctionBlockDic=new Dictionary<int, FunctionBlock> ();

        public List<FunctionBlock_Raw> FunctionBlock_RawList=new List<FunctionBlock_Raw> ();
        public Dictionary<int, FunctionBlock_Raw> FunctionBlock_RawDic = new Dictionary<int, FunctionBlock_Raw>();
        public List<FunctionBlock_Manufacture> FunctionBlock_ManufactureList = new List<FunctionBlock_Manufacture>();
        public Dictionary<int, FunctionBlock_Manufacture> FunctionBlock_ManufactureDic = new Dictionary<int, FunctionBlock_Manufacture>();
        public List<FunctionBlock_Science> FunctionBlock_ScienceList = new List<FunctionBlock_Science>();
        public Dictionary<int, FunctionBlock_Science> FunctionBlock_ScienceDic = new Dictionary<int, FunctionBlock_Science>();
        public List<FunctionBlock_Energy> FunctionBlock_EnergyList = new List<FunctionBlock_Energy>();
        public Dictionary<int, FunctionBlock_Energy> FunctionBlock_EnergyDic = new Dictionary<int, FunctionBlock_Energy>();

        public List<FunctionBlockTypeData> FunctionBlockTypeDataList=new List<FunctionBlockTypeData> ();
        public Dictionary<string, FunctionBlockTypeData> FunctionBlockTypeDataDic = new Dictionary<string, FunctionBlockTypeData>();


        private bool HasInit = false;
        public string FacotryGUID;
        public List<string> FunctionBlockGUIDList = new List<string>();
        public Dictionary<int, FunctionBlockHistory> FunctionBlockHistoryDic = new Dictionary<int, FunctionBlockHistory>();

        public Dictionary<string, FunctionBlock> CurrentFunctionBlockDataDic = new Dictionary<string, FunctionBlock>();

        //info Data
        public BlockBaseInfoData infoData;

        #region Data
        public override void InitData()
        {
            if (HasInit)
                return;
            FunctionBlockList = FunctionBlockMetaDataReader.GetFunctionBlockData();
            FunctionBlockDic = FunctionBlockMetaDataReader.GetFunctionBlockDataDic();
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

            infoData = new BlockBaseInfoData();
            HasInit = true;
        }



        private bool CheckTypeValid(string type)
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
        public FunctionBlockType GetFunctionBlockType(int blockID)
        {
            FunctionBlock functionBlock = GetFunctionBlockByBlockID(blockID);
            if (CheckTypeValid(functionBlock.FunctionBlockType) == false)
                return FunctionBlockType.Energy;
            return (FunctionBlockType)Enum.Parse(typeof(FunctionBlockType), functionBlock.FunctionBlockType);
        }

        //Get Type Data
        public FunctionBlockTypeData GetFacotryTypeData(FunctionBlockType type)
        {
            FunctionBlockTypeData typeData = null;
            FunctionBlockTypeDataDic.TryGetValue(type.ToString(), out typeData);
            if(typeData != null)
            {
                return typeData;
            }
            else
            {
                Debug.LogError("GetFunctionBlockType Error Type= " + type);
                return null;
            }
        }
        public FunctionBlockTypeData GetFacotryTypeData(int functionBlockID)
        {
            return GetFacotryTypeData(GetFunctionBlockType(functionBlockID));
        }

        /// <summary>
        /// Get FunctionBlock Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="functionBlockID"></param>
        /// <returns></returns>
        public T FetchFunctionBlockTypeIndex<T>(int functionBlockID) where T:class
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
                default:
                    Debug.LogError("Fetch FacotryType Error facotryID=" + functionBlockID);
                    return null;
            }
        }

        public FunctionBlock_Manufacture GetFunctionBlock_ManufactureData(int manufactureID)
        {
            FunctionBlock_Manufacture functionBlock_Manufacture = null;
            FunctionBlock_ManufactureDic.TryGetValue(manufactureID, out functionBlock_Manufacture);
            if (functionBlock_Manufacture == null)
            {
                Debug.LogError("Get functionBlock_Manufacture Error , manufactureId=" + manufactureID);
                return null;
            }
            return functionBlock_Manufacture;
        }

        public FunctionBlock_Raw GetFacotryRawData(int rawID)
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

        public FunctionBlock_Science GetFunctionBlock_ScienceData(int scienceID)
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

        public FunctionBlock_Energy GetFunctionBlock_EnergyData(int energyID)
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

        public Sprite GetFunctionBlockIcon(int functionBlockID)
        {
            string path = GetFunctionBlockByBlockID(functionBlockID).BlockIcon;
            return Utility.LoadSprite(path,Utility.SpriteType.png);
         
        }
        public Sprite GetFunctionBlockBG(int functionBlockID)
        {
            string path = GetFunctionBlockByBlockID(functionBlockID).BlockBG;
            return Utility.LoadSprite(path, Utility.SpriteType.png);
        }

        public ushort GetFunctionBlockLevel(int functionBlockID)
        {
            return GetFunctionBlockByBlockID(functionBlockID).Level;
        }
        public float GetManufactureSpeed(int functionBlockID)
        {
            return FetchFunctionBlockTypeIndex<FunctionBlock_Manufacture>(functionBlockID).SpeedBase;
        }

        public FunctionBlock GetFunctionBlockByBlockID(int functionBlockID)
        {
            FunctionBlock functionBlock = null;
            FunctionBlockDic.TryGetValue(functionBlockID, out functionBlock);
            if (functionBlock == null)
                Debug.LogError("Get FunctionBlock Error , ID=" + functionBlockID);
            return functionBlock;
        }

        public string GetFunctionBlockName(int functionBlockID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFunctionBlockByBlockID(functionBlockID).BlockName);
        }
        public string GetFunctionBlockName(FunctionBlock block)
        {
            return MultiLanguage.Instance.GetTextValue(block.BlockName);
        }
        public string GetFunctionBlockDesc(int functionBlockID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFunctionBlockByBlockID(functionBlockID).BlockDesc);
        }
        public string GetFunctionBlockDesc(FunctionBlock block)
        {
            return MultiLanguage.Instance.GetTextValue(block.BlockDesc);
        }

        public Vector2 GetFunctionBlockAreaMax<T>(FunctionBlock block) where T:class
        {
            int id = block.FunctionBlockID;
            switch (GetFunctionBlockType(id))
            {
                case FunctionBlockType.Manufacture:
                    return Utility.TryParseIntVector2(GetFunctionBlock_ManufactureData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaMax,',');
                case FunctionBlockType.Raw:
                    return Utility.TryParseIntVector2(GetFacotryRawData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaMax,',');
                case FunctionBlockType.Science:
                    return Utility.TryParseIntVector2(GetFunctionBlock_ScienceData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaMax,',');
                case FunctionBlockType.Energy:
                    return Utility.TryParseIntVector2(GetFunctionBlock_EnergyData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaMax,',');
                default:
                    Debug.LogError("Fetch AreaMax Error BlockID=" + id);
                    return Vector2.zero;
            }
        }

        /// <summary>
        /// ID=-1  Empty  ;  ID=-2   UnLock;
        /// 生成所有区划底面信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        public Dictionary<Vector2, DistrictAreaBase> GetFuntionBlockAreaDetailDefaultDataInfo<T>(FunctionBlock block) where T : class
        {
            Dictionary<Vector2, DistrictAreaBase> result = new Dictionary<Vector2, DistrictAreaBase>();
            Dictionary<Vector2, DistrictData> totalDic = GetDistrictAreaDetailDefaultData<T>(block);

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
                        sprite = DistrictModule.Instance.GetDistrictIconSpriteList(kvp.Value.DistrictID)[0]
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
        public Dictionary<Vector2, DistrictAreaInfo> GetFuntionBlockOriginAreaInfo<T>(FunctionBlock block) where T : class
        {
            Dictionary<Vector2, DistrictAreaInfo> result = new Dictionary<Vector2, DistrictAreaInfo>();
            Dictionary<Vector2, DistrictData> dic = GetDistrictOriginData<T>(block);

            if (dic == null)
                return result;
            //Check Range
            if (CheckDistrictDataOutofRange<T>(block, true) && CheckTargetDistrictNoLock<T>(block, true))
            {
                int largeDistrictIndex = 1;
                foreach (KeyValuePair<Vector2, DistrictData> kvp in dic)
                {
                    //District Larger than 1X1
                    var largeArea = DistrictModule.Instance.GetDistrictTypeArea(kvp.Value);
                    if (largeArea.Count == 1)
                    {
                        //1x1 grid
                        DistrictAreaInfo info = new DistrictAreaInfo
                        {
                            data = kvp.Value,
                            isLargeDistrict = false,
                            slotType = DistrictSlotType.NormalDistrict,
                            OriginCoordinate = kvp.Key,
                            sprite = DistrictModule.Instance.GetDistrictIconSpriteList(kvp.Value.DistrictID)[0]
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
                                sprite = DistrictModule.Instance.GetDistrictIconSpriteList(kvp.Value.DistrictID)[i]
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


        /// <summary>
        /// 获取所有初始区划信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        private Dictionary<Vector2,DistrictData> GetDistrictOriginData<T>(FunctionBlock block) where T :class
        {
            return GetFuntionBlockDistrictData(GetFuntionBlockOriginArea<T>(block));
        }
        /// <summary>
        /// 获取所有基础区划信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        private Dictionary<Vector2,DistrictData> GetDistrictAreaDetailDefaultData<T>(FunctionBlock block) where T : class
        {
            return GetFuntionBlockDistrictData(GetFuntionBlockAreaDetailDefault<T>(block));
        }

        /// <summary>
        /// 检测区划是否重叠  或超出范围
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <param name="initCheck"></param>
        /// <returns></returns>
        public bool CheckDistrictDataOutofRange<T>(FunctionBlock block,bool initCheck) where T:class
        {
            List<Vector2> CheckContent = new List<Vector2>();
            Dictionary<Vector2, DistrictData> Checkdic = GetFuntionBlockDistrictData(GetFuntionBlockOriginArea<T>(block));
            Vector2 areaMax = GetFunctionBlockAreaMax<T>(block);
            foreach (KeyValuePair<Vector2,DistrictData> kvp in Checkdic)
            {
                //Check District
                List<Vector2> v2 = DistrictModule.Instance.GetDistrictTypeArea(kvp.Value);
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
        private bool CheckTargetDistrictNoLock<T>(FunctionBlock block,bool initCheck) where T:class
        {
            Dictionary<Vector2, DistrictData> Checkdic = GetFuntionBlockDistrictData(GetFuntionBlockOriginArea<T>(block));
            List<Vector2> lockedList = GetLockedDistrictList<T>(block);
            foreach (KeyValuePair<Vector2,DistrictData> kvp in Checkdic)
            {
                //Check District
                List<Vector2> v2 = DistrictModule.Instance.GetDistrictTypeArea(kvp.Value);
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


        private List<Vector2> GetLockedDistrictList<T>(FunctionBlock block) where T:class
        {
            List<Vector2> result = new List<Vector2>();
            Dictionary<Vector2, DistrictData> totalDic = GetFuntionBlockDistrictData(GetFuntionBlockAreaDetailDefault<T>(block));
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
        private Dictionary<Vector2, DistrictData> GetFuntionBlockDistrictData(Dictionary<Vector2, int> dic)
        {
            Dictionary<Vector2, DistrictData> result = new Dictionary<Vector2, DistrictData>();
            if (dic == null)
                return result;
            foreach(KeyValuePair<Vector2, int> kvp in dic)
            {
                DistrictData dd = DistrictModule.Instance.GetDistrictDataByKey(kvp.Value);
                if (dd == null)
                    continue;
                result.Add(kvp.Key, dd);
            }
            return result;
        }

        private Dictionary<Vector2, int> GetFuntionBlockAreaDetailDefault<T>(FunctionBlock block) where T : class
        {
            int id = block.FunctionBlockID;
            Vector2 areaMax = GetFunctionBlockAreaMax<T>(block);
            switch (GetFunctionBlockType(id))
            {
                case FunctionBlockType.Manufacture:
                    return TryParseAreaDetailDefault(GetFunctionBlock_ManufactureData(block.FunctionBlockTypeIndex).AreaDetailDefault ,areaMax);
                case FunctionBlockType.Energy:
                    return TryParseAreaDetailDefault(GetFunctionBlock_EnergyData(block.FunctionBlockTypeIndex).AreaDetailDefault,areaMax);
                default:
                    Debug.LogError("Fetch AreaDetailDefault Error   id="+id);
                    return null;
            }
        }

        private Dictionary<Vector2, int> TryParseAreaDetailDefault(string content, Vector2 areaMax)
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


        private Dictionary<Vector2 ,int > GetFuntionBlockOriginArea<T>(FunctionBlock block)
        {
            int id = block.FunctionBlockID;
            switch (GetFunctionBlockType(id))
            {
                case FunctionBlockType.Manufacture:
                    return TryParseOriginArea(GetFunctionBlock_ManufactureData(block.FunctionBlockTypeIndex).OriginArea);
                case FunctionBlockType.Energy:
                    return TryParseOriginArea(GetFunctionBlock_EnergyData(block.FunctionBlockTypeIndex).OriginArea);
                default:
                    Debug.LogError("Fetch OriginArea Error   id=" + id);
                    return null;

            }
        }
        private Dictionary<Vector2,int> TryParseOriginArea(string content)
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

        public int GetDistrictAreaIndex(Vector2 areaMax,Vector2 currentVector)
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
        public Vector3 InitFunctionBlockBoxCollider<T>(FunctionBlock block) where T:class
        {
            Vector2 area = GetFunctionBlockAreaMax<T>(block);
            return new Vector3(area.x, 3.0f, area.y);
        }

        #region BlockInfoData


        public List<int> GetBlockEXPMapData(string id ,FunctionBlockType type)
        {
            switch (type)
            {
                case FunctionBlockType.Manufacture:
                    List<BlockLevelData> manuData = infoData.ManufactoryBlockLevelDataList;
                    if (manuData == null)
                    {
                        Debug.LogError("Can not Find ManuBlockEXP Map  id=" + id);
                        return null;
                    }
                    return manuData.Find(x => x.ID == id).EXPMap;
                default:
                    return null;
            }
           
        }

        public List<int> GetBlockEXPMapData(int blockid)
        {
            FunctionBlockType type = GetFunctionBlockType(blockid);
            switch (type)
            {
                case FunctionBlockType.Manufacture:
                    return GetBlockEXPMapData(GetFunctionBlockByBlockID(blockid).EXPDataJsonIndex,type);
                default:
                    return null;
            }
        }

        public int GetCurrentLevelEXP(List<int> expMap ,int currentLevel)
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

        public List<DistrictUnlockData> GetManuBlockDistrictUnlockData(string id , FunctionBlockType type)
        {
            switch (type)
            {
                case FunctionBlockType.Manufacture:
                    for (int i = 0; i < infoData.ManuBlockDistrictUnlockDataList.Count; i++)
                    {
                        if (infoData.ManuBlockDistrictUnlockDataList[i].ID == id)
                        {
                            return infoData.ManuBlockDistrictUnlockDataList[i].UnlockData;
                        }
                    }
                    Debug.LogError("can not find unlockdata,id=" + id);
                    return null;
                default:
                    return null;
            }
          
          
           
        }
        /// <summary>
        /// get district unlockData
        /// </summary>
        /// <param name="blockid"></param>
        /// <returns></returns>
        public List<DistrictUnlockData> GetManuBlockDistrictUnlockData(int blockid)
        {
            FunctionBlockType type = GetFunctionBlockType(blockid);
            return GetManuBlockDistrictUnlockData(GetFunctionBlockByBlockID(blockid).DistrictData,type);
        }

        #endregion

        //Formula
        public List<Dictionary<Material,ushort>> GetFunctionBlockFormulaDataList(FunctionBlock block,FormulaModule.MaterialProductType GetType)
        {
            List<Dictionary<Material, ushort>> result = new List<Dictionary<Material, ushort>>();

            List<FormulaData> data = GetFormulaDataList(block);
            for (int i = 0; i < data.Count; i++)
            {
                Dictionary<Material, ushort> maDic = FormulaModule.Instance.GetFormulaMaterialDic(data[i].FormulaID, GetType);
                result.Add(maDic);
            }
            return result;
        }
        public List<FormulaData> GetFormulaDataList(FunctionBlock block)
        {
            return FormulaModule.Instance.GetFormulaDataList(FetchFunctionBlockTypeIndex<FunctionBlock_Manufacture>(block.FunctionBlockID).FormulaInfoID);
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