using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Up,
        Down,
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

        public Dictionary<MainShip_ShieldDirection, MainShipShieldInfo> shieldInfoDic = new Dictionary<MainShip_ShieldDirection, MainShipShieldInfo>();
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
        /// <summary>
        /// ShieldState
        /// </summary>
        public MainShip_ShieldState currentState = MainShip_ShieldState.Disable;


        public MainShip_ShieldDirection direction;

        // 护盾开启时初始值
        public int shield_open_init;
        

        public int shield_max;
        public int shield_current;

        // reality Value
        public int shieldCharge_current
        {
            get { return (int)(shieldChargeSpeed * shieldChargeRatio); }
        }
        public int shieldChargeSpeed;
        // 充能速度折损比例
        public float shieldChargeRatio;

        public byte shieldLayer_current;
        public byte shieldLayer_max;

        public bool InitData(MainShip_ShieldDirection direction)
        {
            return true;
        }

        public void LoadGameSave()
        {

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

        public ModifierDetailPackage_Block durabilityMaxDetailPac = new ModifierDetailPackage_Block();

        public void ChangeAreaDurability(int value)
        {
            durability_current += value;
            if (durability_current > durability_max)
                durability_current = durability_max;
            if (durability_current < 0)
                durability_current = 0;
        }

        public void ChangeAreaDurability_Max(ModifierDetailRootType_Block rootType, uint instanceID, int blockID, int value)
        {
            durabilityMaxDetailPac.ValueChange(rootType, instanceID, blockID, value);
            durability_max += value;
            if (durability_max < 0)
                durability_max = 0;
        }
        public void ChangeAreaDurability_Max(ModifierDetailRootType_Block rootType, int value)
        {
            durabilityMaxDetailPac.ValueChange(rootType, value);
            durability_max += value;
            if (durability_max < 0)
                durability_max = 0;
        }

        /// <summary>
        /// 能源等级最大值
        /// </summary>
        public byte powerLevel_max;
        /// <summary>
        /// 能源等级当前分配
        /// </summary>
        public short powerLevel_current;
        public ModifierDetailPackage_Block powerLevelMaxDetailPac = new ModifierDetailPackage_Block();
        public void ChangePowerLevelMax(ModifierDetailRootType_Block rootType, uint instanceID, int blockID, byte value)
        {
            powerLevelMaxDetailPac.ValueChange(rootType, instanceID, blockID, value);
            powerLevel_max += value;
        }
        public void ChangePowerLevelMax(ModifierDetailRootType_Block rootType,byte value)
        {
            powerLevelMaxDetailPac.ValueChange(rootType, value);
            powerLevel_max += value;
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
        public ModifierDetailPackage_Block DurabilityMaxDetailPac;

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
        public ModifierDetailPackage_Block durabilityMaxDetailPac = new ModifierDetailPackage_Block();
        
        public void ChangeAreaDurability(int value)
        {
            durability_current += value;
            if (durability_current > durability_max)
                durability_current = durability_max;
            if (durability_current < 0)
                durability_current = 0;
        }

        public void ChangeAreaDurability_Max(ModifierDetailRootType_Block rootType,uint instanceID,int blockID,int value)
        {
            durabilityMaxDetailPac.ValueChange(rootType, instanceID, blockID, value);
            durability_max += value;
            if (durability_max < 0)
                durability_max = 0;
        }
        public void ChangeAreaDurability_Max(ModifierDetailRootType_Block rootType,int value)
        {
            durabilityMaxDetailPac.ValueChange(rootType, value);
            durability_max += value;
            if (durability_max < 0)
                durability_max = 0;
        }

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public ushort PowerGenerateValue;
        public ModifierDetailPackage_Block powerGenerateDetailPac = new ModifierDetailPackage_Block();
        public void ChangePowerGenerateValue(ModifierDetailRootType_Block rootType, uint instanceID, int blockID, ushort value)
        {
            powerGenerateDetailPac.ValueChange(rootType, instanceID, blockID, value);
            PowerGenerateValue += value;
            if (PowerGenerateValue < 0)
                PowerGenerateValue = 0;
        }
        public void ChangePowerGenerateValue(ModifierDetailRootType_Block rootType, ushort value)
        {
            powerGenerateDetailPac.ValueChange(rootType, value);
            PowerGenerateValue += value;
            if (PowerGenerateValue < 0)
                PowerGenerateValue = 0;
        }

        /// <summary>
        /// 能源负载，用于其余舱室负载分配
        /// </summary>
        public short energyLoadValue_max;
        public short energyLoadValue_current;
        public ModifierDetailPackage_Block energyLoadMaxDetailPac = new ModifierDetailPackage_Block();
        public void ChangeEnergyLoadValue_Max(ModifierDetailRootType_Block rootType,uint instanceID,int blockID, short value)
        {
            energyLoadMaxDetailPac.ValueChange(rootType, instanceID, blockID, value);
            energyLoadValue_max += value;
            if (energyLoadValue_max < 0)
                energyLoadValue_max = 0;
        }
        public void ChangeEnergyLoadValue_Max(ModifierDetailRootType_Block rootType,short value)
        {
            energyLoadMaxDetailPac.ValueChange(rootType, value);
            energyLoadValue_max += value;
            if (energyLoadValue_max < 0)
                energyLoadValue_max = 0;
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
        public ModifierDetailPackage_Block storageMaxDetailPac = new ModifierDetailPackage_Block();

        public void AddMaxStoragePower(ModifierDetailRootType_Block rootType,uint instanceID,int blockID, int value)
        {
            storageMaxDetailPac.ValueChange(rootType, instanceID, blockID, value);
            storagePower_max += value;
            if (storagePower_max < 0)
                storagePower_max = 0;
        }
        public void AddMaxStoragePower(ModifierDetailRootType_Block rootType,int value)
        {
            storageMaxDetailPac.ValueChange(rootType, value);
            storagePower_max += value;
            if (storagePower_max < 0)
                storagePower_max = 0;
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
        public byte overLoadLevel_current = 0;
        public byte overLoadLevel_max;
        public ModifierDetailPackage_Block overLoadLevelMaxDetailPac = new ModifierDetailPackage_Block();
        public EnergyGenerateMode currentMode { get; protected set; }
        //是否解锁过载模式
        public bool unLockOverLoadMode = false;

        public void ChangeOverLoadLevelMax(ModifierDetailRootType_Block rootType, uint instanceID, int blockID, byte value)
        {
            overLoadLevelMaxDetailPac.ValueChange(rootType, instanceID, blockID, value);
            overLoadLevel_max += value;
            if (overLoadLevel_max < 0)
                overLoadLevel_max = 0;
        }

        public void ChangeOverLoadLevelMax(ModifierDetailRootType_Block rootType, byte value)
        {
            overLoadLevelMaxDetailPac.ValueChange(rootType, value);
            overLoadLevel_max += value;
            if (overLoadLevel_max < 0)
                overLoadLevel_max = 0;
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
                ChangeEnergyLoadValue_Max(ModifierDetailRootType_Block.OriginConfig,config.energyLoadBase);
                energyLoadValue_current = energyLoadValue_max;
                AddMaxStoragePower(ModifierDetailRootType_Block.OriginConfig, config.MaxStorageCountBase);
                ChangeEnergyMode(EnergyGenerateMode.Normal);
                unLockOverLoadMode = config.unlockOverLoad;
                ChangeOverLoadLevelMax(ModifierDetailRootType_Block.OriginConfig, config.overLoadLevelMax);
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
        public ModifierDetailPackage_Block DurabilityMaxDetailPac;

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public ushort PowerGenerateValue;
        public ModifierDetailPackage_Block PowerGenerateDetailPac;
        /// <summary>
        /// 能源负载，用于其余舱室负载分配
        /// </summary>
        public short EnergyLoadValueMax;
        public short EnergyLoadValueCurrent;
        public MainShipPowerAreaInfo.EnergyGenerateMode CurrentMode;
        public ModifierDetailPackage EnergyLoadDetailPac;
        public ModifierDetailPackage_Block EnergyLoadMaxDetailPac;

        public int StoragePower_max;
        public int StoragePower_current = 0;
        public ModifierDetailPackage_Block StorageMaxDetailPac;


        public byte OverLoadLevel_current = 0;
        public byte OverLoadLevel_max;
        public ModifierDetailPackage_Block OverLoadLevelMaxDetailPac;
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