using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class FactoryModule : MonoSingleton<FactoryModule> {

        public enum FactoryType
        {
            Manufacture,
            Raw,
            Science,
            Energy
        }
        //Slot类型
        public enum FactoryManuMaterialType
        {
            Input,
            Output,
            Byproduct
        }


        public List<Factory> FactoryList=new List<Factory> ();
        public Dictionary<int, Factory> FactoryDic=new Dictionary<int, Factory> ();

        public List<Factory_Raw> Factory_RawList=new List<Factory_Raw> ();
        public Dictionary<int, Factory_Raw> Factory_RawDic = new Dictionary<int, Factory_Raw>();
        public List<Factory_Manufacture> Factory_ManufactureList = new List<Factory_Manufacture>();
        public Dictionary<int, Factory_Manufacture> Factory_ManufactureDic = new Dictionary<int, Factory_Manufacture>();
        public List<Factory_Science> Factory_ScienceList = new List<Factory_Science>();
        public Dictionary<int, Factory_Science> Factory_ScienceDic = new Dictionary<int, Factory_Science>();
        public List<Factory_Energy> Factory_EnergyList = new List<Factory_Energy>();
        public Dictionary<int, Factory_Energy> Factory_EnergyDic = new Dictionary<int, Factory_Energy>();

        public List<FactoryTypeData> FactoryTypeDataList=new List<FactoryTypeData> ();
        public Dictionary<string, FactoryTypeData> FactoryTypeDataDic = new Dictionary<string, FactoryTypeData>();
        public List<TextData_Factory> TextData_FactoryList=new List<TextData_Factory> ();
        public Dictionary<string, TextData_Factory> TextData_FactoryDic=new Dictionary<string, TextData_Factory> ();

        private bool HasInit = false;

        public int FacotryGUID;
        public List<int> FactoryGUIDList = new List<int>();
        public Dictionary<int, FactoryHistory> FactoryHistoryDic = new Dictionary<int, FactoryHistory>();


        #region Data
        public void InitData()
        {
            if (HasInit)
                return;
            FactoryList = FactoryMetaDataReader.GetFactoryData();
            FactoryDic = FactoryMetaDataReader.GetFactoryDataDic();
            Factory_RawList = FactoryMetaDataReader.GetFactoryRowData();
            Factory_RawDic = FactoryMetaDataReader.GetFactory_RawDic();
            Factory_ManufactureList = FactoryMetaDataReader.GetFactory_ManufactureData();
            Factory_ManufactureDic = FactoryMetaDataReader.GetFactory_ManufactureDic();
            Factory_ScienceList = FactoryMetaDataReader.GetFactory_ScienceData();
            Factory_ScienceDic = FactoryMetaDataReader.GetFactory_ScienceDic();
            Factory_EnergyList = FactoryMetaDataReader.GetFactory_EnergyData();
            Factory_EnergyDic = FactoryMetaDataReader.GetFactory_EnergyDic();

            FactoryTypeDataList = FactoryMetaDataReader.GetFactoryTypeData();
            FactoryTypeDataDic = FactoryMetaDataReader.GetFactoryTypeDataDic();

            CheckTypeValid();
            HasInit = true;
        }



        private bool CheckTypeValid()
        {
            List<string> types = new List<string>();
            foreach(var type in FactoryTypeDataList)
            {
                string typestr = type.Type;
                if(Enum.IsDefined(typeof(FactoryType), typestr) == false)
                {
                    Debug.LogError("FacotyType InValid! Type=" + typestr);
                    return false;
                }
            }
            return true;
        }

        #endregion
        #region Method Data

        //Get Name
        public string GetFactoryName(int factoryID)
        {
            return GetFactoryTextByKey(GetFactoryByFacotryID(factoryID).FactoryDesc);
        }
        public string GetFactoryName(Factory factory)
        {
            return GetFactoryTextByKey(factory.FactoryName);
        }

        //Get Desc
        public string GetFactoryDesc(int factoryID)
        {
            return GetFactoryTextByKey(GetFactoryByFacotryID(factoryID).FactoryName);
        }
        public string GetFacotryDesc(Factory factory)
        {
            return GetFactoryTextByKey(factory.FactoryDesc);
        }

        //Get FactoryType
        public FactoryType GetFacotryType(int facotryID)
        {
            Factory factory = GetFactoryByFacotryID(facotryID);
            switch (factory.FactoryType)
            {
                case "Manufacture":
                    return FactoryType.Manufacture;
                case "Raw":
                    return FactoryType.Raw;
                case "Science":
                    return FactoryType.Science;
                case "Energy":
                    return FactoryType.Energy;
                default:
                    Debug.LogError("FactoryTypeError , Type=" + factory.FactoryType);
                    return FactoryType.Manufacture;
            }
        }

        //Get Type Data
        public FactoryTypeData GetFacotryTypeData(FactoryType type)
        {
            FactoryTypeData typeData = null;
            FactoryTypeDataDic.TryGetValue(type.ToString(), out typeData);
            if(typeData != null)
            {
                return typeData;
            }
            else
            {
                Debug.LogError("GetFactoryType Error Type= " + type);
                return null;
            }
        }
        public FactoryTypeData GetFacotryTypeData(int factoryID)
        {
            return GetFacotryTypeData(GetFacotryType(factoryID));
        }

        public T FetchFactoryTypeIndex<T>(int factoryID) where T:class
        {
            switch (GetFacotryType(factoryID))
            {
                case FactoryType.Manufacture:
                    return GetFactory_ManufactureData(GetFactoryByFacotryID(factoryID).FactoryTypeIndex) as T;
                case FactoryType.Raw:
                    return GetFacotryRawData(GetFactoryByFacotryID(factoryID).FactoryTypeIndex) as T;
                case FactoryType.Science:
                    return GetFactory_ScienceData(GetFactoryByFacotryID(factoryID).FactoryTypeIndex) as T;
                case FactoryType.Energy:
                    return GetFactory_EnergyData(GetFactoryByFacotryID(factoryID).FactoryTypeIndex) as T;
                default:
                    Debug.LogError("Fetch FacotryType Error facotryID=" + factoryID);
                    return null;
            }
        }

        public Factory_Manufacture GetFactory_ManufactureData(int manufactureID)
        {
            Factory_Manufacture factory_Manufacture = null;
            Factory_ManufactureDic.TryGetValue(manufactureID, out factory_Manufacture);
            if (factory_Manufacture == null)
            {
                Debug.LogError("Get factory_Manufacture Error , manufactureId=" + manufactureID);
                return null;
            }
            return factory_Manufacture;
        }

        public Factory_Raw GetFacotryRawData(int rawID)
        {
            Factory_Raw factory_Raw = null;
            Factory_RawDic.TryGetValue(rawID, out factory_Raw);
            if (factory_Raw == null)
            {
                Debug.LogError("Get Factory_Raw Error , rawID=" + rawID);
                return null;
            }
            return factory_Raw;
        }

        public Factory_Science GetFactory_ScienceData(int scienceID)
        {
            Factory_Science factory_Science = null;
            Factory_ScienceDic.TryGetValue(scienceID, out factory_Science);
            if (factory_Science == null)
            {
                Debug.LogError("Get Factory_Raw Error , scienceID=" + scienceID);
                return null;
            }
            return factory_Science;
        }

        public Factory_Energy GetFactory_EnergyData(int energyID)
        {
            Factory_Energy factory_Energy = null;
            Factory_EnergyDic.TryGetValue(energyID, out factory_Energy);
            if (factory_Energy == null)
            {
                Debug.LogError("Get Factory_Raw Error , energyID=" + energyID);
                return null;
            }
            return factory_Energy;
        }

        public Sprite GetFactoryIcon(int factoryID)
        {
            string path = GetFactoryByFacotryID(factoryID).FactoryIcon;
            return Utility.LoadSprite(path);
         
        }

        public ushort GetFactoryLevel(int factoryID)
        {
            return GetFactoryByFacotryID(factoryID).Level;
        }
        public ushort GetFacotryInputSlotNum(int factoryID)
        {
            return GetFactoryByFacotryID(factoryID).InputSlotNumBase;
        }
        public ushort GetFactoryOutputSlotNum(int factoryID)
        {
            return GetFactoryByFacotryID(factoryID).OutputSlotNumBase;
        }
        public ushort GetFacotryPlugSlotNum(int factoryID)
        {
            return GetFactoryByFacotryID(factoryID).PlugSlotNumBase;
        }
        public ushort GetFactoryStaffSlotNum(int factoryID)
        {
            return GetFactoryByFacotryID(factoryID).StaffSlotNumBase;
        }

        //Get Text By Key
        public string GetFactoryTextByKey(string key)
        {
            TextData_Factory text = null;
            TextData_FactoryDic.TryGetValue(key, out text);
            if (text != null)
            {
                return text.Value_CN;
            }
            else
            {
                Debug.LogError("GetFactoryText Error TextID=" + key);
                return string.Empty;
            }
        }

        public Factory GetFactoryByFacotryID(int factoryID)
        {
            Factory factory = null;
            FactoryDic.TryGetValue(factoryID, out factory);
            if (factory == null)
                Debug.LogError("Get FactoryDesc Error , ID=" + factoryID);
            return factory;
        }
        #endregion

        #region Main Function
        public void PlaceFactory(int factoryID)
        {
            Factory currentFactory = GetFactoryByFacotryID(factoryID);
            FactoryHistory history = new FactoryHistory(1, factoryID);

            FactoryHistoryDic.Add(factoryID, history);
        }

        //Manufacture



        public float GetManufactureSpeed(int factoryID)
        {
            return FetchFactoryTypeIndex<Factory_Manufacture>(factoryID).SpeedBase;
        }

        #endregion
    }

    public class FactoryHistory
    {
        public int FactoryGUID { get; set; }
        public int FacotoryID { get; set; }

        public FactoryHistory(int guid,int factoryID)
        {
            this.FacotoryID = factoryID;
            this.FactoryGUID = guid;
        }

    }
}