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

        public MainShipInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.basePropertyConfig;
            if (config != null)
            {
                ShieldMax = config.ShieldBase_Max;
                ShieldInit = config.ShieldBase_Initial;
            }
            powerAreaInfo = new MainShipPowerAreaInfo();

            livingAreaInfo = new MainShipLivingAreaInfo();
            controlTowerInfo = new MainShipControlTowerInfo();
            hangarAreaInfo = new MainShipHangarInfo();
            workingAreaInfo = new MainShipWorkingAreaInfo();

            ///InitEnergyLoad
            powerAreaInfo.ChangeEnergyLoadValue((short)-livingAreaInfo.powerLevelCurrent);
            powerAreaInfo.ChangeEnergyLoadValue((short)-controlTowerInfo.powerLevelCurrent);
            powerAreaInfo.ChangeEnergyLoadValue((short)-hangarAreaInfo.powerLevelCurrent);
            powerAreaInfo.ChangeEnergyLoadValue((short)-workingAreaInfo.powerLevelCurrent);
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
        /// 当前能量消耗
        /// </summary>
        public ushort powerConsumeCurrent;

        public void UpdateAreaState()
        {
            if (powerLevelCurrent == 0)
                areaState = MainShipAreaState.LayOff;
            else if (powerLevelCurrent > 0)
                areaState = MainShipAreaState.Working;
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

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public ushort PowerGenerateValue;
        /// <summary>
        /// 能源负载，用于其余舱室负载分配
        /// </summary>
        public short EnergyLoadValueMax;
        public short EnergyLoadValueCurrent;

        public int MaxStoragePower;
        public int CurrentStoragePower=0;

        public byte currentOverLoadLevel = 0;

        public void AddOverLoadLevel(byte level=1)
        {
            currentOverLoadLevel += level;
        }

        public MainShipPowerAreaInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.powerAreaConfig;
            if (config != null)
            {
                areaIconPath = config.areaIconPath;
                PowerGenerateValue = config.energyGenerateBase;
                EnergyLoadValueMax = config.energyLoadBase;
                EnergyLoadValueCurrent = EnergyLoadValueMax;
                MaxStoragePower = config.MaxStorageCountBase;
            }
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
                powerConsumeCurrent = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
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
                powerConsumeCurrent = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
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
                powerConsumeCurrent = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
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
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.MainShip_Area_PowerLevel_Change, new List<object>() { MainShipAreaType.hangar }));
        }
    }

    public class MainShipWorkingAreaInfo: MainShipAreaBaseInfo
    {

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
                powerConsumeCurrent = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
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
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.MainShip_Area_PowerLevel_Change, new List<object>() { MainShipAreaType.WorkingArea }));
        }
    }
}