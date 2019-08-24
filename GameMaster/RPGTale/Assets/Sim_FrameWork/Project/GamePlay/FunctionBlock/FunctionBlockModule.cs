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
        public FunctionBlockType GetFunctionBlockType(int facotryID)
        {
            FunctionBlock functionBlock = GetFunctionBlockByBlockID(facotryID);
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
            string path = GetFunctionBlockByBlockID(functionBlockID).FunctionBlockIcon;
            return Utility.LoadSprite(path,Utility.SpriteType.png);
         
        }

        public ushort GetFunctionBlockLevel(int functionBlockID)
        {
            return GetFunctionBlockByBlockID(functionBlockID).Level;
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
            return MultiLanguage.Instance.GetTextValue(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockName);
        }
        public string GetFunctionBlockName(FunctionBlock block)
        {
            return MultiLanguage.Instance.GetTextValue(block.FunctionBlockName);
        }
        public string GetFunctionBlockDesc(int functionBlockID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockDesc);
        }
        public string GetFunctionBlockDesc(FunctionBlock block)
        {
            return MultiLanguage.Instance.GetTextValue(block.FunctionBlockDesc);
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
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        public List<List<DistrictData>> GetFuntionBlockAreaDetailDefaultData<T>(FunctionBlock block) where T : class
        {
            return GetFuntionBlockAreaDetailDefaultData(GetFuntionBlockAreaDetailDefault<T>(block));
        }

        private List<List<DistrictData>> GetFuntionBlockAreaDetailDefaultData(List<List<int>> list)
        {
          
            List<List<DistrictData>> result = new List<List<DistrictData>> ();
            if (list == null)
                return null;
            for (int i = 0; i < list.Count; i++)
            {
                List<DistrictData> dList = new List<DistrictData>();
                List<int> data = list[i];
                for (int j = 0; j < data.Count; j++)
                {
                    if (data[j] == -1)
                    {
                        //AddEmptySlot
                        dList.Add(new DistrictData { DistrictID=-1});
                        continue;
                    }else if (data[j] == -2)
                    {
                        //Add UnLockSlot
                        dList.Add(new DistrictData { DistrictID = -2 });
                        continue;
                    }
                    DistrictData dd = DistrictModule.Instance.GetDistrictDataByKey(data[j]);
                    if (dd == null)
                        continue;
                    dList.Add(dd);
                }
                result.Add(dList);
            }
            return result;
        }

        private List<List<int>> GetFuntionBlockAreaDetailDefault<T>(FunctionBlock block) where T : class
        {
            int id = block.FunctionBlockID;
            switch (GetFunctionBlockType(id))
            {
                case FunctionBlockType.Manufacture:
                    return TryParseAreaDetailDefault(GetFunctionBlock_ManufactureData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaDetailDefault);
                case FunctionBlockType.Energy:
                    return TryParseAreaDetailDefault(GetFunctionBlock_EnergyData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaDetailDefault);
                default:
                    Debug.LogError("Fetch AreaDetailDefault Error   id="+id);
                    return null;
            }
        }

        private List<List<int>> TryParseAreaDetailDefault(string content)
        {
            List<List<int>> result = new List<List<int>> ();
            List<string> ls = Utility.TryParseStringList(content, ';');
            if(string.IsNullOrEmpty(content) || ls.Count == 0)
            {
                Debug.LogWarning("Parse AreaDetailFail!");
                return result;
            }
            for(int i = 0; i < ls.Count; i++)
            {
                List<int> li = Utility.TryParseIntList(ls[i], ',');
                if (li.Count == 0)
                {
                    Debug.LogWarning("Parse AreaDetail Fail, string=" + ls[i]);
                }
                result.Add(li);
            }
            return result;
        }


        #endregion

        #region Main Function

        //Place Block
        public void PlaceFunctionBlock(int functionBlockID,Vector3 checkpos)
        {
            FunctionBlock currentFunctionBlock = GetFunctionBlockByBlockID(functionBlockID);
            if (currentFunctionBlock == null)
                return;
            if (CheckBlockCanPlace(currentFunctionBlock,checkpos) )
            {
                string tempUID=GenerateGUID();
                AddFunctionBlock(tempUID, currentFunctionBlock);
            }
            else
            {
                Debug.Log("Can not place functionBlock ");
            }
           


            //FunctionBlockHistory history = new FunctionBlockHistory(1, functionBlockID);

            //FunctionBlockHistoryDic.Add(functionBlockID, history);
        }

        public void AddFunctionBlock(string functionBlockUID,FunctionBlock functionBlock)
        {
            if (FunctionBlockGUIDList.Contains(functionBlockUID))
            {
                GenerateGUID();
            }
            CurrentFunctionBlockDataDic.Add(functionBlockUID, functionBlock);
            FunctionBlockGUIDList.Add(functionBlockUID);
        }

        public string GenerateGUID()
        {
            return Guid.NewGuid().ToString(); 
        }


        public bool CheckBlockCanPlace(FunctionBlock block,Vector3 checkPos)
        {
            //TODO
            return true;
        }
        //Manufacture



        public float GetManufactureSpeed(int functionBlockID)
        {
            return FetchFunctionBlockTypeIndex<FunctionBlock_Manufacture>(functionBlockID).SpeedBase;
        }

        #endregion
    }

    public class FunctionBlockHistory
    {
        public int FunctionBlockGUID { get; set; }
        public int FacotoryID { get; set; }

        public FunctionBlockHistory(int guid,int functionBlockID)
        {
            this.FacotoryID = functionBlockID;
            this.FunctionBlockGUID = guid;
        }

    }

    //BlockLevelData
    public class BlockLevelData
    {
        public int LevelIndex;
        public int NeedEXP;
    }
}