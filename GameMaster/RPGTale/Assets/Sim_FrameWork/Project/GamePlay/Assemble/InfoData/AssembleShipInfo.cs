using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        private float _timeCost;
        public float timeCost { get { return _timeCost; } }
        public void AddTimeCost(float value)
        {
            _timeCost += value;
            if (_timeCost <= 0)
                _timeCost = 0;
        }

        /// <summary>
        /// 船体耐久
        /// </summary>
        private int _shipDurability;
        public int shipDurability { get { return _shipDurability; } }
        public void AddShipDurability(int value)
        {
            _shipDurability += value;
            if (_shipDurability <= 0)
                _shipDurability = 0;
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
        public float shipCrewMax;

        /// <summary>
        /// 最大货仓储量
        /// </summary>
        private ushort _shipStorage;
        public ushort shipStorage { get { return _shipStorage; } }
        public void AddShipStorage(ushort value)
        {
            _shipStorage += value;
            if (_shipStorage <= 0)
                _shipStorage = 0;
        }

        public AssembleShipTypePresetData presetData;
        public AssembleShipCustomData customData;


        public AssembleShipInfo(int shipID)
        {
            presetData = new AssembleShipTypePresetData(shipID);
            warShipID = presetData.WarshipID;

            AddShipDurability(presetData._metaData.HPBase);
            AddTimeCost(presetData._metaData.BaseTimeCost);
            AddShipFirePower(presetData._metaData.FirePowerBase);
            AddShipSpeed(presetData._metaData.SpeedBase);
            AddShipDetect(presetData._metaData.DetectBase);
            AddShipStorage(presetData._metaData.StorageBase);
            shipCrewMax = presetData._metaData.CrewMax;

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
    }

}