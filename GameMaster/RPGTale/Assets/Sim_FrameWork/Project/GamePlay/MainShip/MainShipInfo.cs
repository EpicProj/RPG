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
        public MainShipInfo InitInfo()
        {
            MainShipInfo info = new MainShipInfo();
            var config = Config.ConfigData.MainShipConfigData.basePropertyConfig;
            if (config != null)
            {
                info.ShieldMax = config.ShieldBase_Max;
                info.ShieldInit = config.ShieldBase_Initial;
            }
            info.powerAreaInfo = new MainShipPowerAreaInfo();
            info.powerAreaInfo = info.powerAreaInfo.InitData();
            info.livingAreaInfo = new MainShipLivingAreaInfo();
            info.controlTowerInfo = new MainShipControlTowerInfo();
            info.hangarAreaInfo = new MainShipHangarInfo();
            info.workingAreaInfo = new MainShipWorkingAreaInfo();

            ///InitEnergyLoad
            info.powerAreaInfo.ChangeEnergyLoadValue((short)-info.livingAreaInfo.powerLevelCurrent);
            info.powerAreaInfo.ChangeEnergyLoadValue((short)-info.controlTowerInfo.powerLevelCurrent);
            info.powerAreaInfo.ChangeEnergyLoadValue((short)-info.hangarAreaInfo.powerLevelCurrent);
            info.powerAreaInfo.ChangeEnergyLoadValue((short)-info.workingAreaInfo.powerLevelCurrent);

            return info;
        }

        public MainShipInfo LoadSaveData(MainShipSaveData saveData)
        {
            MainShipInfo info = new MainShipInfo();
            info.powerAreaInfo = info.powerAreaInfo.LoadSaveData(saveData.powerAreaSaveData);

            return info;

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
        public Dictionary<MainShipAreaType, short> EnergyLoadDetailDic = new Dictionary<MainShipAreaType, short>();

        public EnergyGenerateMode currentMode { get; protected set; }

        public int MaxStoragePower;
        public int CurrentStoragePower=0;

        public byte currentOverLoadLevel = 0;

        public void AddOverLoadLevel(byte level=1)
        {
            currentOverLoadLevel += level;
        }

        public MainShipAreaModifier areaModifier;

        public MainShipPowerAreaInfo() { }
        public MainShipPowerAreaInfo InitData()
        {
            MainShipPowerAreaInfo info = new MainShipPowerAreaInfo();
            var config = Config.ConfigData.MainShipConfigData.powerAreaConfig;
            if (config != null)
            {
                info.areaIconPath = config.areaIconPath;
                info.PowerGenerateValue = config.energyGenerateBase;
                info.EnergyLoadValueMax = config.energyLoadBase;
                info.EnergyLoadValueCurrent = info.EnergyLoadValueMax;
                info.MaxStoragePower = config.MaxStorageCountBase;
                info.ChangeEnergyMode(EnergyGenerateMode.Normal);
            }
            info.areaModifier = new MainShipAreaModifier(ModifierTarget.MainShipPowerArea);
            return info;
        }

        public MainShipPowerAreaInfo LoadSaveData(MainShipPowerAreaSaveData saveData)
        {
            MainShipPowerAreaInfo info = new MainShipPowerAreaInfo();
            var config = Config.ConfigData.MainShipConfigData.powerAreaConfig;
            if (config != null)
            {
                info.areaIconPath = config.areaIconPath;
            }
            info.currentMode = saveData.currentMode;

            return info;
        }

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

        public void RefreshEnergyLoadDetail(MainShipAreaType area,short value)
        {
            if (EnergyLoadDetailDic.ContainsKey(area))
                EnergyLoadDetailDic[area] = value;
            else
                EnergyLoadDetailDic.Add(area, value);
        }

        public void AddMaxStoragePower(int value)
        {
            MaxStoragePower += value;
        }

        public void ChangeEnergyMode(EnergyGenerateMode mode)
        {
            currentMode = mode;
        }

    }

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

    public class MainShipPowerAreaSaveData
    {
        /// <summary>
        /// 电能产生效率
        /// </summary>
        public ushort PowerGenerateValue;
        /// <summary>
        /// 能源负载，用于其余舱室负载分配
        /// </summary>
        public short EnergyLoadValueMax;
        public short EnergyLoadValueCurrent;
        public MainShipPowerAreaInfo.EnergyGenerateMode  currentMode;

        public int MaxStoragePower;
        public int CurrentStoragePower = 0;

        public byte currentOverLoadLevel = 0;

        public MainShipPowerAreaSaveData(MainShipPowerAreaInfo info)
        {

            currentMode = info.currentMode;
        }

    }


    #endregion
}