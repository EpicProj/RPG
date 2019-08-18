using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class FunctionBlockModule : MonoSingleton<FunctionBlockModule> {

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
        public List<TextData_FunctionBlock> TextData_FunctionBlockList=new List<TextData_FunctionBlock> ();
        public Dictionary<string, TextData_FunctionBlock> TextData_FunctionBlockDic=new Dictionary<string, TextData_FunctionBlock> ();

        private bool HasInit = false;

        public string FacotryGUID;
        public List<string> FunctionBlockGUIDList = new List<string>();
        public Dictionary<int, FunctionBlockHistory> FunctionBlockHistoryDic = new Dictionary<int, FunctionBlockHistory>();

        public Dictionary<string, FunctionBlock> CurrentFunctionBlockDataDic = new Dictionary<string, FunctionBlock>();


        #region Data
        public void InitData()
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

        //Get Name
        public string GetFunctionBlockName(int functionBlockID)
        {
            return GetFunctionBlockTextByKey(GetFunctionBlockByFacotryID(functionBlockID).FunctionBlockDesc);
        }
        public string GetFunctionBlockName(FunctionBlock functionBlock)
        {
            return GetFunctionBlockTextByKey(functionBlock.FunctionBlockName);
        }

        //Get Desc
        public string GetFunctionBlockDesc(int functionBlockID)
        {
            return GetFunctionBlockTextByKey(GetFunctionBlockByFacotryID(functionBlockID).FunctionBlockName);
        }
        public string GetFacotryDesc(FunctionBlock functionBlock)
        {
            return GetFunctionBlockTextByKey(functionBlock.FunctionBlockDesc);
        }

        //Get FunctionBlockType
        public FunctionBlockType GetFacotryType(int facotryID)
        {
            FunctionBlock functionBlock = GetFunctionBlockByFacotryID(facotryID);
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
            return GetFacotryTypeData(GetFacotryType(functionBlockID));
        }

        public T FetchFunctionBlockTypeIndex<T>(int functionBlockID) where T:class
        {
            switch (GetFacotryType(functionBlockID))
            {
                case FunctionBlockType.Manufacture:
                    return GetFunctionBlock_ManufactureData(GetFunctionBlockByFacotryID(functionBlockID).FunctionBlockTypeIndex) as T;
                case FunctionBlockType.Raw:
                    return GetFacotryRawData(GetFunctionBlockByFacotryID(functionBlockID).FunctionBlockTypeIndex) as T;
                case FunctionBlockType.Science:
                    return GetFunctionBlock_ScienceData(GetFunctionBlockByFacotryID(functionBlockID).FunctionBlockTypeIndex) as T;
                case FunctionBlockType.Energy:
                    return GetFunctionBlock_EnergyData(GetFunctionBlockByFacotryID(functionBlockID).FunctionBlockTypeIndex) as T;
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
            string path = GetFunctionBlockByFacotryID(functionBlockID).FunctionBlockIcon;
            return Utility.LoadSprite(path);
         
        }

        public ushort GetFunctionBlockLevel(int functionBlockID)
        {
            return GetFunctionBlockByFacotryID(functionBlockID).Level;
        }
        public ushort GetFacotryInputSlotNum(int functionBlockID)
        {
            return GetFunctionBlockByFacotryID(functionBlockID).InputSlotNumBase;
        }
        public ushort GetFunctionBlockOutputSlotNum(int functionBlockID)
        {
            return GetFunctionBlockByFacotryID(functionBlockID).OutputSlotNumBase;
        }
        public ushort GetFacotryPlugSlotNum(int functionBlockID)
        {
            return GetFunctionBlockByFacotryID(functionBlockID).PlugSlotNumBase;
        }
        public ushort GetFunctionBlockStaffSlotNum(int functionBlockID)
        {
            return GetFunctionBlockByFacotryID(functionBlockID).StaffSlotNumBase;
        }

        //Get Text By Key
        public string GetFunctionBlockTextByKey(string key)
        {
            TextData_FunctionBlock text = null;
            TextData_FunctionBlockDic.TryGetValue(key, out text);
            if (text != null)
            {
                return text.Value_CN;
            }
            else
            {
                Debug.LogError("GetFunctionBlockText Error TextID=" + key);
                return string.Empty;
            }
        }

        public FunctionBlock GetFunctionBlockByFacotryID(int functionBlockID)
        {
            FunctionBlock functionBlock = null;
            FunctionBlockDic.TryGetValue(functionBlockID, out functionBlock);
            if (functionBlock == null)
                Debug.LogError("Get FunctionBlockDesc Error , ID=" + functionBlockID);
            return functionBlock;
        }
        #endregion

        #region Main Function
        public void PlaceFunctionBlock(int functionBlockID)
        {
            FunctionBlock currentFunctionBlock = GetFunctionBlockByFacotryID(functionBlockID);
            FunctionBlockHistory history = new FunctionBlockHistory(1, functionBlockID);

            FunctionBlockHistoryDic.Add(functionBlockID, history);
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
}