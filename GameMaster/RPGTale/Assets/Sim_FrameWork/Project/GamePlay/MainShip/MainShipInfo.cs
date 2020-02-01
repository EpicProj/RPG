using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
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

    public class MainShipInfo
    {
        #region Shield
        public int ShieldMax;
        public int ShieldInit;

        public int currentShieldValue;

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

            ShieldMax = config.ShieldBase_Max;
            ShieldInit = config.ShieldBase_Initial;

            powerAreaInfo = new MainShipPowerAreaInfo();
            livingAreaInfo = new MainShipLivingAreaInfo();
            controlTowerInfo = new MainShipControlTowerInfo();
            hangarAreaInfo = new MainShipHangarInfo();
            workingAreaInfo = new MainShipWorkingAreaInfo();

            if (powerAreaInfo.InitData() == false)
                return false;

            ///InitEnergyLoad
            powerAreaInfo.ChangeEnergyLoadValue((short)-livingAreaInfo.powerLevelCurrent);
            powerAreaInfo.ChangeEnergyLoadValue((short)-controlTowerInfo.powerLevelCurrent);
            powerAreaInfo.ChangeEnergyLoadValue((short)-hangarAreaInfo.powerLevelCurrent);
            powerAreaInfo.ChangeEnergyLoadValue((short)-workingAreaInfo.powerLevelCurrent);

            return true;
        }

        public bool LoadSaveData(MainShipSaveData saveData)
        {
            powerAreaInfo = new MainShipPowerAreaInfo();

            return powerAreaInfo.LoadSaveData(saveData.powerAreaSaveData);

        }
    }

    /*
     * Base Info
     */
    public class MainShipAreaBaseInfo
    {
        public MainShipAreaState areaState = MainShipAreaState.None;

        public string areaIconPath;
        public int durabilityMax;
        public int currentDurability;

        /// <summary>
        /// 能源等级最大值
        /// </summary>
        public byte powerLevelMax;
        /// <summary>
        /// 能源等级当前分配
        /// </summary>
        public short powerLevelCurrent;
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
            if (powerLevelCurrent == 0)
                areaState = MainShipAreaState.LayOff;
            else if (powerLevelCurrent > 0)
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

        public int durabilityMax;
        public int durabilityCurrent;

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public ushort PowerGenerateValue;
        /// <summary>
        /// 能源负载，用于其余舱室负载分配
        /// </summary>
        public short EnergyLoadValueMax;
        public short EnergyLoadValueCurrent;

        /// <summary>
        /// 能源负载详细分配
        /// </summary>
        public Dictionary<ModifierDetailRootType_Simple, ModifierDetailItem_Simple> EnergyLoadDetailDic = new Dictionary<ModifierDetailRootType_Simple, ModifierDetailItem_Simple>();

        public EnergyGenerateMode currentMode { get; protected set; }
        /// <summary>
        /// False = Change Faild
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool ChangeEnergyLoadValue(short count)
        {
            EnergyLoadValueCurrent += count;
            if (EnergyLoadValueCurrent > EnergyLoadValueMax)
            {
                EnergyLoadValueCurrent = EnergyLoadValueMax;
                return true;
            }
            else if (EnergyLoadValueCurrent < 0)
            {
                EnergyLoadValueCurrent = 0;
                return false;
            }
            return true;
        }

        public void RefreshEnergyLoadDetail(ModifierDetailRootType_Simple type, short changeValue)
        {
            if (EnergyLoadDetailDic.ContainsKey(type))
                EnergyLoadDetailDic[type].value += changeValue;
            else
                EnergyLoadDetailDic.Add(type, new ModifierDetailItem_Simple ( type,changeValue));
        }
        public void ChangeEnergyMode(EnergyGenerateMode mode)
        {
            currentMode = mode;
        }

        /// <summary>
        /// Power Storage
        /// </summary>
        public int MaxStoragePower;
        public int CurrentStoragePower=0;

        public void AddMaxStoragePower(int value)
        {
            MaxStoragePower += value;
        }

        public byte currentOverLoadLevel = 0;

        public void AddOverLoadLevel(byte level=1)
        {
            currentOverLoadLevel += level;
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
                EnergyLoadValueMax = config.energyLoadBase;
                EnergyLoadValueCurrent = EnergyLoadValueMax;
                MaxStoragePower = config.MaxStorageCountBase;
                ChangeEnergyMode(EnergyGenerateMode.Normal);
                return true;
            }
            areaModifier = new MainShipAreaModifier(ModifierTarget.MainShipPowerArea);
            return false;
        }


        public bool LoadSaveData(MainShipPowerAreaSaveData saveData)
        {
            var config = Config.ConfigData.MainShipConfigData.powerAreaConfig;
            if (config == null)
                return false;
            areaIconPath = config.areaIconPath;
            currentMode = saveData.currentMode;
            EnergyLoadDetailDic = saveData.EnergyLoadDetailDic;
            return true;
        }

  

    }

    /// <summary>
    /// Save Data
    /// </summary>
    public class MainShipPowerAreaSaveData
    {

        public int durabilityCurrent;

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public ushort PowerGenerateValue;
        /// <summary>
        /// 能源负载，用于其余舱室负载分配
        /// </summary>
        public short EnergyLoadValueMax;
        public short EnergyLoadValueCurrent;
        public MainShipPowerAreaInfo.EnergyGenerateMode currentMode;
        public Dictionary<ModifierDetailRootType_Simple, ModifierDetailItem_Simple> EnergyLoadDetailDic = new Dictionary<ModifierDetailRootType_Simple, ModifierDetailItem_Simple>();

        public int MaxStoragePower;
        public int CurrentStoragePower = 0;

        public byte currentOverLoadLevel = 0;

        public MainShipPowerAreaSaveData(MainShipPowerAreaInfo info)
        {
            durabilityCurrent = info.durabilityCurrent;
            EnergyLoadDetailDic = info.EnergyLoadDetailDic;
            currentMode = info.currentMode;
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
                durabilityMax = config.baseConfig.Durability_Initial;
                currentDurability = durabilityMax;
                powerLevelMax = config.baseConfig.PowerLevel_Max_Initial;
                powerLevelCurrent = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();
            }
        }

        public void ChangePowerLevel(short level = 1)
        {
            powerLevelCurrent += level;
            if (powerLevelCurrent > powerLevelMax)
                powerLevelCurrent = powerLevelMax;
            else if (powerLevelCurrent < 0)
                powerLevelCurrent = 0;

            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetControlTowerAreaEnergyLevelMapData(powerLevelCurrent);
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
                durabilityMax = config.baseConfig.Durability_Initial;
                currentDurability = durabilityMax;
                powerLevelMax = config.baseConfig.PowerLevel_Max_Initial;
                powerLevelCurrent = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();
            }
        }
        public void ChangePowerLevel(short level = 1)
        {
            powerLevelCurrent += level;
            if (powerLevelCurrent > powerLevelMax)
                powerLevelCurrent = powerLevelMax;
            else if (powerLevelCurrent < 0)
                powerLevelCurrent = 0;

            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetLivingAreaEnergyLevelMapData(powerLevelCurrent);
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
                durabilityMax = config.baseConfig.Durability_Initial;
                currentDurability = durabilityMax;
                powerLevelMax = config.baseConfig.PowerLevel_Max_Initial;
                powerLevelCurrent = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();
            }
        }

        public void ChangePowerLevel(short level = 1)
        {
            powerLevelCurrent += level;
            if (powerLevelCurrent > powerLevelMax)
                powerLevelCurrent = powerLevelMax;
            else if (powerLevelCurrent < 0)
                powerLevelCurrent = 0;

            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetHangarAreaEnergyLevelMapData(powerLevelCurrent);
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
                durabilityMax = config.baseConfig.Durability_Initial;
                currentDurability = durabilityMax;
                powerLevelMax = config.baseConfig.PowerLevel_Max_Initial;
                powerLevelCurrent = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();

                areaModifier = new MainShipAreaModifier(ModifierTarget.MainShipWorkingArea);
            }
        }

        public void ChangePowerLevel(short level = 1)
        {
            powerLevelCurrent += level;
            if (powerLevelCurrent > powerLevelMax)
                powerLevelCurrent = powerLevelMax;
            else if (powerLevelCurrent < 0)
                powerLevelCurrent = 0;

            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetWorkingAreaEnergyLevelMapData(powerLevelCurrent);
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