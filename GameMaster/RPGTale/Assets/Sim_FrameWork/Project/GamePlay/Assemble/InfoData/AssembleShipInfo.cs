using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
 * Assemble Ship Info
 * SOMA
 */
namespace Sim_FrameWork
{
    public class AssembleShipInfo
    {
        public int warShipID;
        public ushort UID;

        public Config.AssembleMainType mainTypeData;

        public string shipCustomName
        {
            get { return presetData.shipClassName + "·" + customData.customNameText; }
        }
        /// <summary>
        /// 建造时长
        /// </summary>
        public float TotalTimeCost
        {
            get
            {
                float value = 0;

                return value + presetData._metaData.BaseTimeCost;
            }
        }


        /// <summary>
        /// 船体耐久
        /// </summary>
        public int shipDurability
        {
            get
            {
                int value = 0;
                foreach(var partInfo in customData.customPartData.Values)
                {
                    var dic = partInfo.customDataInfo.propertyDic;
                    foreach(KeyValuePair<string,AssemblePartCustomDataInfo.CustomData> property in dic)
                    {
                        if (property.Key == Config.ConfigData.AssembleConfig.assembleShip_Durability_Property_Link)
                        {
                            value += (int)property.Value.propertyValueMax;
                        }
                    }
                }
                return value + presetData._metaData.HPBase;
            }
        }

        /// <summary>
        /// 速度
        /// </summary>
        private float _shipSpeed;
        public float shipSpeed { get { return (float)Math.Round(_shipSpeed, 2); } }
        public void AddShipSpeed(float value)
        {
            _shipSpeed += value;
            if (_shipSpeed <= 0)
                _shipSpeed = 0;
        }

        /// <summary>
        /// 火力
        /// </summary>
        private float _shipFirePower;
        public float shipFirePower { get { return (float)Math.Round(_shipFirePower, 2); } }
        public void AddShipFirePower(float value)
        {
            _shipFirePower += value;
            if (_shipFirePower <= 0)
                _shipFirePower = 0;
        }

        /// <summary>
        /// 探索
        /// </summary>
        private float _shipDetect;
        public float shipDetect { get { return (float)Math.Round(_shipDetect, 2); } }
        public void AddShipDetect(float value)
        {
            _shipDetect += value;
            if (_shipDetect <= 0)
                _shipDetect = 0;
        }

        /// <summary>
        /// 最大成员数
        /// </summary>
        public int shipCrewMax
        {
            get
            {
                int crew = 0;
                foreach (var partInfo in customData.customPartData.Values)
                {
                    var dic = partInfo.customDataInfo.propertyDic;
                    foreach (KeyValuePair<string, AssemblePartCustomDataInfo.CustomData> property in dic)
                    {
                        if (property.Key == Config.ConfigData.AssembleConfig.assembleShip_Member_Property_Link)
                        {
                            crew += (int)property.Value.propertyValueMax;
                        }
                    }
                }
                return crew + presetData._metaData.CrewMax;
            }
        }

        public float Explore
        {
            get
            {
                return presetData._metaData.DetectBase;
            }
        }

        /// <summary>
        /// 最大货仓储量
        /// </summary>
        public ushort shipStorage
        {
            get
            {
                ushort value = 0;
                foreach (var partInfo in customData.customPartData.Values)
                {
                    var dic = partInfo.customDataInfo.propertyDic;
                    foreach (KeyValuePair<string, AssemblePartCustomDataInfo.CustomData> property in dic)
                    {
                        if (property.Key == Config.ConfigData.AssembleConfig.assembleShip_Storage_Property_Link)
                        {
                            value += (ushort)property.Value.propertyValueMax;
                        }
                    }
                }
                return (ushort)(value + presetData._metaData.StorageBase);
            }
        }


        public AssembleShipTypePresetData presetData;
        public AssembleShipCustomData customData;

        public AssembleShipInfo() { }
        public void InitData(int shipID)
        {
            presetData = new AssembleShipTypePresetData(shipID);
            warShipID = presetData.WarshipID;
        }

        public AssembleShipSingleSaveData CreateSaveData()
        {
            List<int> partIDList = new List<int>();
            foreach(var part in customData.customPartData.Keys)
            {
                partIDList.Add(part);
            }
            AssembleShipSingleSaveData save = new AssembleShipSingleSaveData(warShipID, UID, customData.customNameText, partIDList);
            return save;
        }

        public bool LoadSaveData(AssembleShipSingleSaveData saveData)
        {
            if (saveData != null)
            {
                InitData(saveData.shipID);
                customData = new AssembleShipCustomData(saveData);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 舰船预设模板数据
    /// </summary>
    public class AssembleShipTypePresetData
    {
        public int WarshipID;

        public string TypeID { get { return _metaData.MainType; } }
        public Sprite TypeIcon { get { return AssembleModule.GetShipPresetMainTypeIcon(WarshipID); } }
        public string TypeName { get { return AssembleModule.GetShipPresetMainTypeName(WarshipID); } }

        public string shipSizeText { get { return AssembleModule.GetShipSizeText(_metaData.ShipScale); } }

        /// <summary>
        /// 模块数量
        /// </summary>
        public ushort ModuleNum
        {
            get
            {
                if (partConfig != null)
                    return (ushort)partConfig.configData.Count;
                return 0;
            }
        }

        public Sprite ShipSprite
        {
            get { return Utility.LoadSprite(_metaData.ShipSpritePath, Utility.SpriteType.png); }
        }

        public string shipClassDesc
        {
            get
            {
                if (_metaClass != null)
                    return MultiLanguage.Instance.GetTextValue(_metaClass.ClassDesc);
                return string.Empty;
            }
        }
        public string shipClassName
        {
            get
            {
                if (_metaClass != null)
                    return MultiLanguage.Instance.GetTextValue(_metaClass.ClassName);
                return string.Empty;
            }
        }

        public List<MaterialCostItem> shipCostBase = new List<MaterialCostItem>();

        public AssembleWarship _metaData;
        public AssembleWarshipClass _metaClass;
        public Config.AssembleShipPartConfig partConfig;

        public AssembleShipTypePresetData(int warShipID)
        {
            _metaData = AssembleModule.GetWarshipDataByKey(warShipID);

            if (_metaData != null)
            {
                WarshipID = _metaData.WarShipID;
                shipCostBase = AssembleModule.GetShipMaterialCost(_metaData.WarShipID);
                _metaClass = AssembleModule.GetWarshipClassDataByKey(_metaData.Class);
                partConfig = AssembleModule.GetShipPartConfigData(warShipID);
            }
        }
    }

    /// <summary>
    /// 舰船自定义数据
    /// </summary>
    public class AssembleShipCustomData
    {
        public int WarshipID;

        public string customNameText;

        public Dictionary<int, AssemblePartInfo> customPartData = new Dictionary<int, AssemblePartInfo>();

        public AssembleShipCustomData(int warshipID, string customName, Dictionary<int, AssemblePartInfo> customPartData)
        {
            this.WarshipID = warshipID;
            this.customNameText = customName;
            this.customPartData = customPartData;
        }

        public AssembleShipCustomData(AssembleShipSingleSaveData saveData)
        {
            if (saveData != null)
            {
                this.WarshipID = saveData.shipID;
                this.customNameText = saveData.customName_Partial;

                Dictionary<int, AssemblePartInfo> infoDic = new Dictionary<int, AssemblePartInfo>();
                for(int i = 0; i < saveData.customPartData.Count; i++)
                {
                    var partInfo = PlayerManager.Instance.GetAssemblePartInfo((ushort)saveData.customPartData[i]);
                    if (partInfo != null)
                        infoDic.Add(partInfo.UID, partInfo);
                }
                customPartData = infoDic;
            }
        }
    }

    #region Save Data

    public class AssembleShipSingleSaveData
    {
        public int shipID;
        public ushort UID;
        //Only Save Custom Part
        public string customName_Partial;

        public List<int> customPartData;

        public AssembleShipSingleSaveData(int shipID, ushort uid,string customName,List<int> customPartData)
        {
            this.shipID = shipID;
            this.UID = uid;
            this.customName_Partial = customName;
            this.customPartData = customPartData;
        }
    }

    #endregion
}