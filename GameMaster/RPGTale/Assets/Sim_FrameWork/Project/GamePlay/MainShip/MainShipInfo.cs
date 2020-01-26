using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
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

            livingAreaInfo = new MainShipLivingAreaInfo();
            powerAreaInfo = new MainShipPowerAreaInfo();
            controlTowerInfo = new MainShipControlTowerInfo();
            hangarAreaInfo = new MainShipHangarInfo();
            workingAreaInfo = new MainShipWorkingAreaInfo();
        }
    }

    public class MainShipPowerAreaInfo
    {

        public enum EnergyGenerateMode
        {
            Normal,
            Overload
        }

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public int PowerGenerateCount;
        public int MaxStoragePower;
        public int CurrentStoragePower;

        public byte currentOverLoadLevel = 0;

    }

    public class MainShipControlTowerInfo
    {
        public Sprite areaIcon;
        public int durabilityMax;
        public int currentDurability;


        public byte powerLevelMax;
        public byte powerLevelCurrent;

        public ushort powerConsumeCurrent;

        public MainShipControlTowerInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.controlTowerAreaConfig;
            if (config != null)
            {
                areaIcon = Utility.LoadSprite(config.baseConfig.areaIconPath, Utility.SpriteType.png);
                durabilityMax = config.baseConfig.Durability_Initial;
                currentDurability = durabilityMax;
                powerLevelMax = config.baseConfig.PowerLevel_Max_Initial;
                powerLevelCurrent = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeCurrent = (ushort)config.baseConfig.PowerConsumeBase;
            }
        }

    }

    public class MainShipLivingAreaInfo
    {
        public Sprite areaIcon;
        public int durabilityMax;
        public int currentDurability;


        public byte powerLevelMax;
        public byte powerLevelCurrent;

        public ushort powerConsumeCurrent;

        public MainShipLivingAreaInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.livingAreaConfig;
            if (config != null)
            {
                areaIcon = Utility.LoadSprite(config.baseConfig.areaIconPath, Utility.SpriteType.png);
                durabilityMax = config.baseConfig.Durability_Initial;
                currentDurability = durabilityMax;
                powerLevelMax = config.baseConfig.PowerLevel_Max_Initial;
                powerLevelCurrent = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeCurrent = (ushort)config.baseConfig.PowerConsumeBase;
            }
        }
    }

    public class MainShipHangarInfo
    {
        public Sprite areaIcon;
        public int durabilityMax;
        public int currentDurability;


        public byte powerLevelMax;
        public byte powerLevelCurrent;

        public ushort powerConsumeCurrent;

        public MainShipHangarInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.hangarAreaConfig;
            if (config != null)
            {
                areaIcon = Utility.LoadSprite(config.baseConfig.areaIconPath, Utility.SpriteType.png);
                durabilityMax = config.baseConfig.Durability_Initial;
                currentDurability = durabilityMax;
                powerLevelMax = config.baseConfig.PowerLevel_Max_Initial;
                powerLevelCurrent = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeCurrent = (ushort)config.baseConfig.PowerConsumeBase;
            }
        }

    }

    public class MainShipWorkingAreaInfo
    {
        public Sprite areaIcon;
        public int durabilityMax;
        public int currentDurability;


        public byte powerLevelMax;
        public byte powerLevelCurrent;

        public ushort powerConsumeCurrent;

        public MainShipWorkingAreaInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.workingAreaConfig;
            if (config != null)
            {
                areaIcon = Utility.LoadSprite(config.baseConfig.areaIconPath, Utility.SpriteType.png);
                durabilityMax = config.baseConfig.Durability_Initial;
                currentDurability = durabilityMax;
                powerLevelMax = config.baseConfig.PowerLevel_Max_Initial;
                powerLevelCurrent = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeCurrent = (ushort)config.baseConfig.PowerConsumeBase;
            }
        }
    }
}