using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Main Ship Info
 * SOMA
 * 
 */
namespace Sim_FrameWork
{
    #region Enums
    public enum MainShipAreaState
    {
        Working,
        LayOff,
        None,
    }
    public enum MainShipAreaEnergyCostType
    {
        EnergyLevel,

    }

    /// <summary>
    /// 护盾方向
    /// </summary>
    public enum MainShip_ShieldDirection
    {
        front,
        back,
        Left,
        Right
    }
    public enum MainShip_ShieldState
    {
        Disable,
        Open,
        Forbid
    }

    #endregion

    #region MainShip
    public class MainShipInfo
    {
        #region Shield
        /*
         * 护盾四个方向
         * 多层护盾
         */

        public Dictionary<MainShip_ShieldDirection, MainShipShieldInfo> shieldInfoDic;
        #endregion

        public MainShipPowerAreaInfo powerAreaInfo;
        public MainShipControlTowerInfo controlTowerInfo;
        public MainShipLivingAreaInfo livingAreaInfo;
        public MainShipHangarInfo hangarAreaInfo;
        public MainShipWorkingAreaInfo workingAreaInfo;

        

        public MainShipInfo() { }
        public bool InitInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.basePropertyConfig;
            if (config == null)
                return false;

            ///Init Shield Data
            shieldInfoDic = new Dictionary<MainShip_ShieldDirection, MainShipShieldInfo>();
            foreach(MainShip_ShieldDirection direction in Enum.GetValues(typeof(MainShip_ShieldDirection)))
            {
                MainShipShieldInfo info = new MainShipShieldInfo();
                if (info.InitData(direction))
                {
                    shieldInfoDic.Add(direction, info);
                }
            }

            powerAreaInfo = new MainShipPowerAreaInfo();
            livingAreaInfo = new MainShipLivingAreaInfo();
            controlTowerInfo = new MainShipControlTowerInfo();
            hangarAreaInfo = new MainShipHangarInfo();
            workingAreaInfo = new MainShipWorkingAreaInfo();

            if (powerAreaInfo.InitData() == false)
                return false;

            ///InitEnergyLoad
            powerAreaInfo.ChangeEnergyLoadValue((short)-livingAreaInfo.powerLevel_current);
            powerAreaInfo.ChangeEnergyLoadValue((short)-controlTowerInfo.powerLevel_current);
            powerAreaInfo.ChangeEnergyLoadValue((short)-hangarAreaInfo.powerLevel_current);
            powerAreaInfo.ChangeEnergyLoadValue((short)-workingAreaInfo.powerLevel_current);

            return true;
        }

        public bool LoadSaveData(MainShipSaveData saveData)
        {
            powerAreaInfo = new MainShipPowerAreaInfo();

            return powerAreaInfo.LoadSaveData(saveData.powerAreaSaveData);

        }
    }

    /// <summary>
    /// Shield Info
    /// </summary>
    public class MainShipShieldInfo
    {
        public string directionName;
        /// <summary>
        /// ShieldState
        /// </summary>
        public MainShip_ShieldState currentState = MainShip_ShieldState.Disable;

        public MainShip_ShieldDirection direction;

        private Config.MainShipShieldLevelMap _shieldConfig_current;
        public Config.MainShipShieldLevelMap ShieldConfig_current
        {
            get
            {
                if (_shieldConfig_current == null)
                {
                    _shieldConfig_current = MainShipModule.GetMainShipShieldLevelData(shieldLevel_current);
                }
                return _shieldConfig_current;
            }
        }

        //当前区域护盾分配能源等级
        public short shieldLevel_current =0;
        //可装备护盾槽数量
        public byte shieldEquip_slotNum_current
        {
            get
            {
                var config = Config.ConfigData.MainShipConfigData.basePropertyConfig.shield_slot_unlock_energycost_map;
                for(int i = 0; i < config.Length; i++)
                {
                    if (shieldLevel_current < config[i])
                        return (byte)i;
                }
                return 0;
            }
            protected set { }
        }
        /// <summary>
        /// 增加护盾等级
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddShieldLevel(short value)
        {
            shieldLevel_current += value;
            if (shieldLevel_current <= 0)
            {
                UpdateShieldLevel((short)(shieldLevel_current - value), 0);
                shieldLevel_current = 0;
                return false;
            }
            var max = Config.ConfigData.MainShipConfigData.basePropertyConfig.shield_energy_total_max_limit;
            if (shieldLevel_current > max)
            {
                UpdateShieldLevel((short)(shieldLevel_current - value), max);
                shieldLevel_current = max;
                return false;
            }

            UpdateShieldLevel((short)(shieldLevel_current - value), shieldLevel_current);
            return true;
        }

        public bool ChangeShieldLevel(byte value)
        {
            var max = Config.ConfigData.MainShipConfigData.basePropertyConfig.shield_energy_total_max_limit;
            short originLevel = shieldLevel_current;
            if(value> max)
            {
                shieldLevel_current = max;
                AddShieldLevel((short)(max - shieldLevel_current));
                return false;
            }
            else
            {
                shieldLevel_current = value;
                AddShieldLevel((short)(shieldLevel_current - originLevel));
                return true;
            }
        }

        /// <summary>
        /// 更新护盾等级
        /// </summary>
        /// <param name="preLevel"></param>
        /// <param name="currentLevel"></param>
        /// <returns></returns>
        bool UpdateShieldLevel(short preLevel,short currentLevel)
        {
            if (ShieldConfig_current == null)
                return false;
            var preConfig = MainShipModule.GetMainShipShieldLevelData(preLevel);
            var currentConfig = MainShipModule.GetMainShipShieldLevelData(currentLevel);
            if (preConfig == null || currentConfig == null)
                return false;
            //Update ShieldMax Data & ShieldValue
            AddShieldMax( ModifierDetailRootType_Mix.OriginConfig,currentConfig.shieldMax_base - preConfig.shieldMax_base);
            ChangeShieldCurrentValue(0);
            //Update ShieldOpenInit
            AddShieldOpenInit(ModifierDetailRootType_Mix.OriginConfig, currentConfig.shieldOpenInit_base - preConfig.shieldOpenInit_base);
            //Update ShieldChargeSpeed
            AddShieldChargeSpeed(ModifierDetailRootType_Mix.OriginConfig, currentConfig.shieldChargeSpeed_base - preConfig.shieldChargeSpeed_base);
            //Update EnergyCost
            AddEnergyCostValue(ModifierDetailRootType_Mix.OriginConfig,(short)(currentConfig.shieldEnergyCost_base - preConfig.shieldEnergyCost_base));

            DebugPlus.Log("Change Shield Level Success!");
            DebugPlus.LogObject<MainShipShieldInfo>(this);
            return true;
        }

        // 护盾开启时初始值
        public int shield_open_init;
        public ModifierDetailPackage_Mix shieldOpenInitDetailPac = new ModifierDetailPackage_Mix();

        public bool AddShieldOpenInit_Block(ModifierDetailRootType_Mix rootType, uint instanceID,int blockID,int value)
        {
            shield_open_init += value;
            if (shield_open_init < 0)
            {
                shield_open_init = 0;
                shieldOpenInitDetailPac.ValueChange_Block(rootType, instanceID, blockID, value- shield_open_init);
                return false;
            }
            else
            {
                shieldOpenInitDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool AddShieldOpenInit_Assemble(ModifierDetailRootType_Mix rootType, ushort UID, int partID, int value)
        {
            shield_open_init += value;
            if (shield_open_init < 0)
            {
                shield_open_init = 0;
                shieldOpenInitDetailPac.ValueChange_Assemble(rootType, UID, partID, value - shield_open_init);
                return false;
            }
            else
            {
                shieldOpenInitDetailPac.ValueChange_Assemble(rootType, UID, partID, value);
                return true;
            }
        }
        public bool AddShieldOpenInit(ModifierDetailRootType_Mix rootType,int value)
        {
            shield_open_init += value;
            if (shield_open_init < 0)
            {
                shield_open_init = 0;
                shieldOpenInitDetailPac.ValueChange(rootType, value - shield_open_init);
                return false;
            }
            else
            {
                shieldOpenInitDetailPac.ValueChange(rootType,value);
                return true;
            }
        }

        /// <summary>
        /// Shield Value 
        /// </summary>
        public int shield_max;
        public int shield_current;
        public ModifierDetailPackage_Mix shieldMaxDetailPac = new ModifierDetailPackage_Mix();
        public bool AddShieldMax_Block(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, int value)
        {
            shield_max += value;
            if (shield_max < 0)
            {
                shieldMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - shield_max);
                shield_max = 0;
                return false;
            }
            else
            {
                shieldMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool AddShieldMax_Assemble(ModifierDetailRootType_Mix rootType, ushort UID, int partID, int value)
        {
            shield_max += value;
            if (shield_max < 0)
            {
                shieldMaxDetailPac.ValueChange_Assemble(rootType, UID, partID, value - shield_max);
                shield_max = 0;
                return false;
            }
            else
            {
                shieldMaxDetailPac.ValueChange_Assemble(rootType, UID, partID, value);
                return true;
            }
        }
        public bool AddShieldMax(ModifierDetailRootType_Mix rootType, int value)
        {
            shield_max += value;
            if (shield_max < 0)
            {
                shieldMaxDetailPac.ValueChange(rootType,value - shield_max);
                shield_max = 0;
                return false;
            }
            else
            {
                shieldMaxDetailPac.ValueChange(rootType,value);
                return true;
            }
        }

        public bool ChangeShieldCurrentValue(int value)
        {
            shield_current += value;
            if (shield_current > shield_max)
            {
                shield_current = shield_max;
                return false;
            }
            if (shield_current < 0)
            {
                shield_current = 0;
                return false;
            }
            return true;
        }

        // reality Value
        public int shieldCharge_current
        {
            get { return (int)(shieldChargeSpeed * shieldChargeRatio); }
        }

        public int shieldChargeSpeed;
        // 充能速度折损比例
        public float shieldChargeRatio =1.0f;
        public ModifierDetailPackage_Mix shieldChargeSpeedDetailPac = new ModifierDetailPackage_Mix();
        public bool AddShieldChargeSpeed_Block(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, int value)
        {
            shieldChargeSpeed += value;
            if (shieldChargeSpeed < 0)
            {
                shieldChargeSpeedDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - shieldChargeSpeed);
                shieldChargeSpeed = 0;
                return false;
            }
            else
            {
                shieldChargeSpeedDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool AddShieldChargeSpeed_Assemble(ModifierDetailRootType_Mix rootType, ushort UID, int partID, int value)
        {
            shieldChargeSpeed += value;
            if (shieldChargeSpeed < 0)
            {
                shieldChargeSpeedDetailPac.ValueChange_Assemble(rootType, UID, partID, value - shieldChargeSpeed);
                shieldChargeSpeed = 0;
                return false;
            }
            else
            {
                shieldChargeSpeedDetailPac.ValueChange_Assemble(rootType, UID, partID, value);
                return true;
            }
        }
        public bool AddShieldChargeSpeed(ModifierDetailRootType_Mix rootType, int value)
        {
            shieldChargeSpeed += value;
            if (shieldChargeSpeed < 0)
            {
                shieldChargeSpeedDetailPac.ValueChange(rootType, value - shieldChargeSpeed);
                shieldChargeSpeed = 0;
                return false;
            }
            else
            {
                shieldChargeSpeedDetailPac.ValueChange(rootType, value);
                return true;
            }
        }

        /// <summary>
        /// 护盾层数
        /// </summary>
        public short shieldLayer_current;
        public short shieldLayer_max;

        /// <summary>
        /// 能源消耗
        /// </summary>
        private short energyCost_current;
        public short EnergyCost_current
        {
            get
            {
                if (currentState == MainShip_ShieldState.Open)
                    return energyCost_current;
                return 0;
            }
        }
        public ModifierDetailPackage_Mix energyCostDetailPac = new ModifierDetailPackage_Mix();
        public bool AddEnergyCostValue_Block(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, short value)
        {
            energyCost_current += value;
            if (energyCost_current < 0)
            {
                energyCostDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - energyCost_current);
                energyCost_current = 0;
                return false;
            }
            else
            {
                energyCostDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool AddEnergyCostValue_Assemble(ModifierDetailRootType_Mix rootType, ushort UID, int partID, short value)
        {
            energyCost_current += value;
            if (energyCost_current < 0)
            {
                energyCostDetailPac.ValueChange_Assemble(rootType, UID, partID, value - energyCost_current);
                energyCost_current = 0;
                return false;
            }
            else
            {
                energyCostDetailPac.ValueChange_Assemble(rootType, UID, partID, value);
                return true;
            }
        }
        public bool AddEnergyCostValue(ModifierDetailRootType_Mix rootType, short value)
        {
            energyCost_current += value;
            if (energyCost_current < 0)
            {
                energyCostDetailPac.ValueChange(rootType, value - energyCost_current);
                energyCost_current = 0;
                return false;
            }
            else
            {
                energyCostDetailPac.ValueChange(rootType, value);
                return true;
            }
        }

        public MainShipModifier shipShieldModifier;

        public MainShipShieldInfo() { }
        public bool InitData(MainShip_ShieldDirection direction)
        {
            this.direction = direction;
            directionName = MainShipModule.GetMainShipShieldDirectionName(direction);
            if (ShieldConfig_current == null)
                return false;
            shipShieldModifier = new MainShipModifier(ModifierTarget.MainShipShield);
            return true;
        }

        public void LoadGameSave(MainShipShieldSaveData saveData)
        {
            currentState = saveData.CurrentState;
            direction = saveData.Direction;
            directionName = MainShipModule.GetMainShipShieldDirectionName(direction);
            shieldLevel_current = saveData.ShieldLevel_current;

            shield_open_init = saveData.Shield_open_init;
            shieldOpenInitDetailPac = saveData.ShieldOpenInitDetailPac;

            shield_max = saveData.Shield_max;
            shield_current = saveData.Shield_current;
            shieldMaxDetailPac = saveData.ShieldMaxDetailPac;

            shieldChargeSpeed = saveData.ShieldChargeSpeed;
            shieldChargeSpeedDetailPac = saveData.ShieldChargeSpeedDetailPac;

            energyCost_current = saveData.EnergyCost_current;
            energyCostDetailPac = saveData.EnergyCostDetailPac;
        }
    }
    /// <summary>
    /// Shield Save Data
    /// </summary>
    public class MainShipShieldSaveData
    {
        public MainShip_ShieldState CurrentState;
        public MainShip_ShieldDirection Direction;

        public short ShieldLevel_current;

        public int Shield_open_init;
        public ModifierDetailPackage_Mix ShieldOpenInitDetailPac;

        public int Shield_max;
        public int Shield_current;
        public ModifierDetailPackage_Mix ShieldMaxDetailPac;

        public int ShieldChargeSpeed;
        public ModifierDetailPackage_Mix ShieldChargeSpeedDetailPac;

        public short EnergyCost_current;
        public ModifierDetailPackage_Mix EnergyCostDetailPac;

        public MainShipShieldSaveData(MainShipShieldInfo info)
        {
            CurrentState = info.currentState;
            Direction = info.direction;
            ShieldLevel_current = info.shieldLevel_current;

            Shield_open_init = info.shield_open_init;
            ShieldOpenInitDetailPac = info.shieldOpenInitDetailPac;

            Shield_max = info.shield_max;
            Shield_current = info.shield_current;
            ShieldMaxDetailPac = info.shieldMaxDetailPac;

            ShieldChargeSpeed = info.shieldChargeSpeed;
            ShieldChargeSpeedDetailPac = info.shieldChargeSpeedDetailPac;

            EnergyCost_current = info.EnergyCost_current;
            EnergyCostDetailPac = info.energyCostDetailPac;
        }

    }

    #endregion
    /*
     * Base Info
     */
    public class MainShipAreaBaseInfo
    {
        public MainShipAreaState areaState = MainShipAreaState.None;

        public string areaIconPath;

        /// <summary>
        /// area durability
        /// </summary>
        public int durability_max;
        public int durability_current;

        public ModifierDetailPackage_Mix durabilityMaxDetailPac = new ModifierDetailPackage_Mix();

        public bool ChangeAreaDurability(int value)
        {
            durability_current += value;
            if (durability_current > durability_max)
            {
                durability_current = durability_max;
                return false;
            }
            if (durability_current < 0)
            {
                durability_current = 0;
                return false;
            }
            return true;
        }

        public bool ChangeAreaDurability_Max(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, int value)
        {
            durability_max += value;
            if (durability_max < 0)
            {
                durabilityMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - durability_max);
                durability_max = 0;
                return false;
            }
            else
            {
                durabilityMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
                
        }
        public bool ChangeAreaDurability_Max(ModifierDetailRootType_Mix rootType, int value)
        {
            durability_max += value;
            if (durability_max < 0)
            {
                durabilityMaxDetailPac.ValueChange(rootType, value - durability_max);
                durability_max = 0;
                return false;
            }
            else
            {
                durabilityMaxDetailPac.ValueChange(rootType, value);
                return true;
            }
                
        }

        /// <summary>
        /// 能源等级最大值
        /// </summary>
        public byte powerLevel_max;
        /// <summary>
        /// 能源等级当前分配
        /// </summary>
        public short powerLevel_current;
        public ModifierDetailPackage_Mix powerLevelMaxDetailPac = new ModifierDetailPackage_Mix();
        public bool ChangePowerLevelMax(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, byte value)
        {
            var maxValue = Config.ConfigData.MainShipConfigData.areaEnergyLevelMax;
            powerLevel_max += value;
            if (powerLevel_max > maxValue)
            {
                powerLevelMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, powerLevel_max- maxValue);
                powerLevel_max = maxValue;
                return false;
            }
            else
            {
                powerLevelMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool ChangePowerLevelMax(ModifierDetailRootType_Mix rootType,byte value)
        {
            var maxValue = Config.ConfigData.MainShipConfigData.areaEnergyLevelMax;
            powerLevel_max += value;
            if (powerLevel_max > maxValue)
            {
                powerLevelMaxDetailPac.ValueChange(rootType, powerLevel_max- maxValue);
                powerLevel_max = maxValue;
                return false;
            }
            else
            {
                powerLevelMaxDetailPac.ValueChange(rootType, value);
                return true;
            }     
        }

        /// <summary>
        /// EnergyCost
        /// </summary>
        public ushort powerConsumeBase;
        public ushort powerConsumeExtra;
        /// Add Rate
        public float powerConsumeRate
        {
            get
            {
                float ValueInitial = 1.0f;
                foreach(float value in energyCostRateAddDetail.Values)
                {
                    ValueInitial += value;
                }
                return ValueInitial;
            }
        }

        /// <summary>
        /// 倍率加成详情
        /// </summary>
        public Dictionary<MainShipAreaEnergyCostType, float> energyCostRateAddDetail =new Dictionary<MainShipAreaEnergyCostType, float>();

        public ushort powerConsumeCurrent
        {
            get
            {
                if(areaState == MainShipAreaState.Working)
                {
                    return (ushort)(powerConsumeBase* powerConsumeRate + powerConsumeExtra);
                }
                else
                {
                    return 0;
                }
            }
        }

        public void UpdateAreaState()
        {
            if (powerLevel_current == 0)
                areaState = MainShipAreaState.LayOff;
            else if (powerLevel_current > 0)
                areaState = MainShipAreaState.Working;
        }

        /// <summary>
        ///  Same Type will Cover Value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="costType"></param>
        public void ChangePowerConsumeRate(float value,MainShipAreaEnergyCostType costType)
        {
            if (energyCostRateAddDetail.ContainsKey(costType))
            {
                energyCostRateAddDetail[costType] = value;
            }
            else
            {
                energyCostRateAddDetail.Add(costType, value);
            }
        }

        public bool LoadGameSaveData(MainShipAreaGeneralSaveData saveData)
        {
            if (saveData == null)
                return false;
            durability_max = saveData.Durability_max;
            durability_current = saveData.Durability_current;
            durabilityMaxDetailPac = saveData.DurabilityMaxDetailPac;

            return true;
        }
    }

    /// <summary>
    /// General Area SaveData
    /// </summary>
    public class MainShipAreaGeneralSaveData
    {
        public int Durability_max;
        public int Durability_current;
        public ModifierDetailPackage_Mix DurabilityMaxDetailPac;

        public MainShipAreaGeneralSaveData(MainShipAreaBaseInfo baseinfo)
        {
            Durability_max = baseinfo.durability_max;
            Durability_current = baseinfo.durability_current;
            DurabilityMaxDetailPac = baseinfo.durabilityMaxDetailPac;
        }
    }

    #region Power Area
    public class MainShipPowerAreaInfo
    {

        public enum EnergyGenerateMode
        {
            Normal,
            Overload
        }

        public string areaIconPath;

        /// <summary>
        /// Area duribility
        /// </summary>
        public int durability_max;
        public int durability_current;
        public ModifierDetailPackage_Mix durabilityMaxDetailPac = new ModifierDetailPackage_Mix();
        
        public void ChangeAreaDurability(int value)
        {
            durability_current += value;
            if (durability_current > durability_max)
                durability_current = durability_max;
            if (durability_current < 0)
                durability_current = 0;
        }

        public bool ChangeAreaDurability_Max(ModifierDetailRootType_Mix rootType,uint instanceID,int blockID,int value)
        {
            durability_max += value;
            if (durability_max < 0)
            {
                durabilityMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - durability_max);
                durability_max = 0;
                return false;
            }
            else
            {
                durabilityMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool ChangeAreaDurability_Max(ModifierDetailRootType_Mix rootType,int value)
        {
            durability_max += value;
            if (durability_max < 0)
            {
                durabilityMaxDetailPac.ValueChange(rootType, value - durability_max);
                durability_max = 0;
                return false;
            }
            else
            {
                durabilityMaxDetailPac.ValueChange(rootType, value);
                return true;
            }
        }

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public short PowerGenerateValue;
        public ModifierDetailPackage_Mix powerGenerateDetailPac = new ModifierDetailPackage_Mix();
        public bool ChangePowerGenerateValue(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, short value)
        {
            PowerGenerateValue += value;
            if (PowerGenerateValue < 0)
            {
                powerGenerateDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - PowerGenerateValue);
                PowerGenerateValue = 0;
                return false;
            }
            else
            {
                powerGenerateDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool ChangePowerGenerateValue(ModifierDetailRootType_Mix rootType, short value)
        {
            PowerGenerateValue += value;
            if (PowerGenerateValue < 0)
            {
                powerGenerateDetailPac.ValueChange(rootType, value - PowerGenerateValue);
                PowerGenerateValue = 0;
                return false;
            }
            else
            {
                powerGenerateDetailPac.ValueChange(rootType, value);
                return true;
            }                
        }

        /// <summary>
        /// 能源负载，用于其余舱室负载分配
        /// </summary>
        public short energyLoadValue_max;
        public short energyLoadValue_current;
        public ModifierDetailPackage_Mix energyLoadMaxDetailPac = new ModifierDetailPackage_Mix();
        public bool ChangeEnergyLoadValue_Max(ModifierDetailRootType_Mix rootType,uint instanceID,int blockID, short value)
        {
            energyLoadValue_max += value;
            if (energyLoadValue_max < 0)
            {
                energyLoadMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - energyLoadValue_max);
                energyLoadValue_max = 0;
                return false;
            }
            else
            {
                energyLoadMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool ChangeEnergyLoadValue_Max(ModifierDetailRootType_Mix rootType,short value)
        {
            energyLoadValue_max += value;
            if (energyLoadValue_max < 0)
            {
                energyLoadMaxDetailPac.ValueChange(rootType, value - energyLoadValue_max);
                energyLoadValue_max = 0;
                return false;
            }
            else
            {
                energyLoadMaxDetailPac.ValueChange(rootType, value);
                return true;
            }
        }

        /// <summary>
        /// 能源负载详细分配
        /// </summary>
        public ModifierDetailPackage energyLoadDetailPac = new ModifierDetailPackage();

        /// <summary>
        /// False = Change Faild
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool ChangeEnergyLoadValue(short count)
        {
            energyLoadValue_current += count;
            if (energyLoadValue_current > energyLoadValue_max)
            {
                energyLoadValue_current = energyLoadValue_max;
                return true;
            }
            else if (energyLoadValue_current < 0)
            {
                energyLoadValue_current = 0;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 各个区域使用的能源详情
        /// </summary>
        /// <param name="type"></param>
        /// <param name="changeValue"></param>
        public void RefreshEnergyLoadDetail(ModifierDetailRootType_Simple type, short changeValue)
        {
            energyLoadDetailPac.ValueChange(type, changeValue);
        }


        /// <summary>
        /// Power Storage
        /// </summary>
        public int storagePower_max;
        public int storagePower_current=0;
        public ModifierDetailPackage_Mix storageMaxDetailPac = new ModifierDetailPackage_Mix();

        public bool AddMaxStoragePower(ModifierDetailRootType_Mix rootType,uint instanceID,int blockID, int value)
        {
            storagePower_max += value;
            if (storagePower_max < 0)
            {
                storageMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value- storagePower_max);
                storagePower_max = 0;
                return false;
            }
            else
            {
                storageMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
                
        }
        public bool AddMaxStoragePower(ModifierDetailRootType_Mix rootType,int value)
        {
            storagePower_max += value;
            if (storagePower_max < 0)
            {
                storageMaxDetailPac.ValueChange(rootType, value -storagePower_max);
                storagePower_max = 0;
                return false;
            }
            else
            {
                storageMaxDetailPac.ValueChange(rootType, value);
                return true;
            }
        }

        public void ChangeCurrentStoragePower(int value)
        {
            storagePower_current += value;
            if (storagePower_current > storagePower_max)
                storagePower_current = storagePower_max;
            if (storagePower_current < 0)
                storagePower_current = 0;
        }

        /// <summary>
        /// Over Load
        /// </summary>
        public short overLoadLevel_current = 0;
        public short overLoadLevel_max;
        public ModifierDetailPackage_Mix overLoadLevelMaxDetailPac = new ModifierDetailPackage_Mix();
        public EnergyGenerateMode currentMode { get; protected set; }
        //是否解锁过载模式
        public bool unLockOverLoadMode = false;

        public bool ChangeOverLoadLevelMax(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, short value)
        {
            overLoadLevel_max += value;
            if (overLoadLevel_max < 0)
            {
                overLoadLevelMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - overLoadLevel_max);
                overLoadLevel_max = 0;
                return false;
            }
            else
            {
                overLoadLevelMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            } 
        }

        public bool ChangeOverLoadLevelMax(ModifierDetailRootType_Mix rootType, short value)
        {
            overLoadLevel_max += value;
            if (overLoadLevel_max < 0)
            {
                overLoadLevelMaxDetailPac.ValueChange(rootType, value - overLoadLevel_max);
                overLoadLevel_max = 0;
                return false;
            }
            else
            {
                overLoadLevelMaxDetailPac.ValueChange(rootType, value);
                return true;
            }
        }

        /// <summary>
        /// 更改过载等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool ChangeOverLoadLevel(byte level=1)
        {
            overLoadLevel_current += level;
            if (overLoadLevel_current > overLoadLevel_max)
                overLoadLevel_current = overLoadLevel_max;
            if (energyLoadValue_current < 0)
                energyLoadValue_current = 0;

            if (MainShipModule.GetPowerAreaOverLoadMap(overLoadLevel_current) == null)
            {
                DebugPlus.LogError("[PowerAreaOverLoadMap] : GetMap Error ! currentLevel=" + overLoadLevel_current);
                overLoadLevel_current -= level;
                return false;
            }
            return true;
        }
        public void ChangeEnergyMode(EnergyGenerateMode mode)
        {
            currentMode = mode;
        }

        /// <summary>
        /// Modifier
        /// </summary>
        public MainShipAreaModifier areaModifier;


        public MainShipPowerAreaInfo() { }
        public bool InitData()
        {
            var config = Config.ConfigData.MainShipConfigData.powerAreaConfig;
            if (config != null)
            {
                areaIconPath = config.areaIconPath;
                PowerGenerateValue = config.energyGenerateBase;
                ChangeEnergyLoadValue_Max(ModifierDetailRootType_Mix.OriginConfig,config.energyLoadBase);
                energyLoadValue_current = energyLoadValue_max;
                AddMaxStoragePower(ModifierDetailRootType_Mix.OriginConfig, config.MaxStorageCountBase);
                ChangeEnergyMode(EnergyGenerateMode.Normal);
                unLockOverLoadMode = config.unlockOverLoad;
                ChangeOverLoadLevelMax(ModifierDetailRootType_Mix.OriginConfig, config.overLoadLevelMax);
                areaModifier = new MainShipAreaModifier(ModifierTarget.MainShipPowerArea);
                return true;
            }
            DebugPlus.LogError("[MainShipPowerAreaInfo] : InitData Fail ! config is null!");
            return false;
        }


        public bool LoadSaveData(MainShipPowerAreaSaveData saveData)
        {
            var config = Config.ConfigData.MainShipConfigData.powerAreaConfig;
            if (config == null)
            {
                DebugPlus.LogError("[MainShipPowerAreaSaveData] : save Data Error!");
            }
            areaIconPath = config.areaIconPath;

            durability_current = saveData.Durability_current;
            durability_max = saveData.Durability_current;
            durabilityMaxDetailPac = saveData.DurabilityMaxDetailPac;

            PowerGenerateValue = saveData.PowerGenerateValue;
            powerGenerateDetailPac = saveData.PowerGenerateDetailPac;

            energyLoadValue_max = saveData.EnergyLoadValueMax;
            energyLoadValue_current = saveData.EnergyLoadValueCurrent;
            currentMode = saveData.CurrentMode;
            energyLoadDetailPac = saveData.EnergyLoadDetailPac;
            energyLoadMaxDetailPac = saveData.EnergyLoadMaxDetailPac;

            storagePower_max = saveData.StoragePower_max;
            storagePower_current = saveData.StoragePower_max;
            storageMaxDetailPac = saveData.StorageMaxDetailPac;

            overLoadLevel_current = saveData.OverLoadLevel_current;
            overLoadLevel_max = saveData.OverLoadLevel_max;
            overLoadLevelMaxDetailPac = saveData.OverLoadLevelMaxDetailPac;
            return true;
        }
    }

    /// <summary>
    /// Save Data
    /// </summary>
    public class MainShipPowerAreaSaveData
    {
        public int Durability_max;
        public int Durability_current;
        public ModifierDetailPackage_Mix DurabilityMaxDetailPac;

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public short PowerGenerateValue;
        public ModifierDetailPackage_Mix PowerGenerateDetailPac;
        /// <summary>
        /// 能源负载，用于其余舱室负载分配
        /// </summary>
        public short EnergyLoadValueMax;
        public short EnergyLoadValueCurrent;
        public MainShipPowerAreaInfo.EnergyGenerateMode CurrentMode;
        public ModifierDetailPackage EnergyLoadDetailPac;
        public ModifierDetailPackage_Mix EnergyLoadMaxDetailPac;

        public int StoragePower_max;
        public int StoragePower_current;
        public ModifierDetailPackage_Mix StorageMaxDetailPac;


        public short OverLoadLevel_current;
        public short OverLoadLevel_max;
        public ModifierDetailPackage_Mix OverLoadLevelMaxDetailPac;
        public bool UnLockOverLoadMode;

        public MainShipPowerAreaSaveData(MainShipPowerAreaInfo info)
        {
            Durability_max = info.durability_max;
            Durability_current = info.durability_current;
            DurabilityMaxDetailPac = info.durabilityMaxDetailPac;

            PowerGenerateValue = info.PowerGenerateValue;
            PowerGenerateDetailPac = info.powerGenerateDetailPac;

            EnergyLoadValueMax = info.energyLoadValue_max;
            EnergyLoadValueCurrent = info.energyLoadValue_current;
            EnergyLoadDetailPac = info.energyLoadDetailPac;
            EnergyLoadMaxDetailPac = info.energyLoadMaxDetailPac;
            CurrentMode = info.currentMode;

            StoragePower_max = info.storagePower_max;
            StoragePower_current = info.storagePower_current;
            StorageMaxDetailPac = info.storageMaxDetailPac;

            OverLoadLevel_current = info.overLoadLevel_current;
            OverLoadLevel_max = info.overLoadLevel_max;
            OverLoadLevelMaxDetailPac = info.overLoadLevelMaxDetailPac;
        }
    }

    #endregion
    public class MainShipControlTowerInfo: MainShipAreaBaseInfo
    {
        public MainShipControlTowerInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.controlTowerAreaConfig;
            if (config != null)
            {
                areaIconPath = config.baseConfig.areaIconPath;
                durability_max = config.baseConfig.Durability_Initial;
                durability_current = durability_max;
                powerLevel_max = config.baseConfig.PowerLevel_Max_Initial;
                powerLevel_current = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();
            }
        }

        public void ChangePowerLevel(short level = 1)
        {
            powerLevel_current += level;
            if (powerLevel_current > powerLevel_max)
                powerLevel_current = powerLevel_max;
            else if (powerLevel_current < 0)
                powerLevel_current = 0;

            if (MainShipModule.GetControlTowerAreaEnergyLevelMapData(powerLevel_current) == null)
            {
                DebugPlus.LogError("[ControlTowerArea] : EnergyLevelMap Error! currentLevel=" + powerLevel_current);
                powerLevel_current -= level;
            }
                
            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetControlTowerAreaEnergyLevelMapData(powerLevel_current);
                if (levelData != null)
                {
                    ChangePowerConsumeRate((float)levelData.energyCostRate, MainShipAreaEnergyCostType.EnergyLevel);
                }
            }
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.MainShip_Area_PowerLevel_Change, new List<object>() { MainShipAreaType.ControlTower }));
        }

    }

    public class MainShipLivingAreaInfo: MainShipAreaBaseInfo
    {

        public MainShipLivingAreaInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.livingAreaConfig;
            if (config != null)
            {
                areaIconPath = config.baseConfig.areaIconPath;
                durability_max = config.baseConfig.Durability_Initial;
                durability_current = durability_max;
                powerLevel_max = config.baseConfig.PowerLevel_Max_Initial;
                powerLevel_current = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();
            }
        }
        public void ChangePowerLevel(short level = 1)
        {
            powerLevel_current += level;
            if (powerLevel_current > powerLevel_max)
                powerLevel_current = powerLevel_max;
            else if (powerLevel_current < 0)
                powerLevel_current = 0;

            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetLivingAreaEnergyLevelMapData(powerLevel_current);
                if (levelData != null)
                {
                    ChangePowerConsumeRate((float)levelData.energyCostRate, MainShipAreaEnergyCostType.EnergyLevel);
                }
            }
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.MainShip_Area_PowerLevel_Change, new List<object>() { MainShipAreaType.LivingArea }));
        }

    }

    public class MainShipHangarInfo: MainShipAreaBaseInfo
    {

        public MainShipHangarInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.hangarAreaConfig;
            if (config != null)
            {
                areaIconPath = config.baseConfig.areaIconPath;
                durability_max = config.baseConfig.Durability_Initial;
                durability_current = durability_max;
                powerLevel_max = config.baseConfig.PowerLevel_Max_Initial;
                powerLevel_current = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();
            }
        }

        public void ChangePowerLevel(short level = 1)
        {
            powerLevel_current += level;
            if (powerLevel_current > powerLevel_max)
                powerLevel_current = powerLevel_max;
            else if (powerLevel_current < 0)
                powerLevel_current = 0;

            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetHangarAreaEnergyLevelMapData(powerLevel_current);
                if (levelData != null)
                {
                    ChangePowerConsumeRate((float)levelData.energyCostRate, MainShipAreaEnergyCostType.EnergyLevel);
                }
            }
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.MainShip_Area_PowerLevel_Change, new List<object>() { MainShipAreaType.hangar }));
        }
    }

    public class MainShipWorkingAreaInfo: MainShipAreaBaseInfo
    {
        public MainShipAreaModifier areaModifier;
        public MainShipWorkingAreaInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.workingAreaConfig;
            if (config != null)
            {
                areaIconPath = config.baseConfig.areaIconPath;
                durability_max = config.baseConfig.Durability_Initial;
                durability_current = durability_max;
                powerLevel_max = config.baseConfig.PowerLevel_Max_Initial;
                powerLevel_current = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();

                areaModifier = new MainShipAreaModifier(ModifierTarget.MainShipWorkingArea);
            }
        }

        public void ChangePowerLevel(short level = 1)
        {
            powerLevel_current += level;
            if (powerLevel_current > powerLevel_max)
                powerLevel_current = powerLevel_max;
            else if (powerLevel_current < 0)
                powerLevel_current = 0;

            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetWorkingAreaEnergyLevelMapData(powerLevel_current);
                if (levelData != null)
                {
                    ChangePowerConsumeRate((float)levelData.energyCostRate, MainShipAreaEnergyCostType.EnergyLevel);
                }
            }
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.MainShip_Area_PowerLevel_Change, new List<object>() { MainShipAreaType.WorkingArea }));
        }
    }

    #region Game SaveData

    public class MainShipSaveData
    {
        public MainShipPowerAreaSaveData powerAreaSaveData;

        public MainShipSaveData()
        {

        }
    }

    #endregion
}