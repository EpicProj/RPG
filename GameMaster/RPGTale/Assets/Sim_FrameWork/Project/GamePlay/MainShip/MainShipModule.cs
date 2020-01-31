using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MainShipModule : BaseModule<MainShipModule>
    {

        public override void Register()
        {
        }
        public override void InitData()
        {

        }

        public MainShipModule()
        {
            InitData();
        }



        #region Method
        public static Config.MainShipBasePropertyConfig.ShieldLevelMap GetShieldLevelData(int levelID)
        {
            var configData = Config.ConfigData.MainShipConfigData.basePropertyConfig;
            return configData.shieldLevelMap.Find(x => x.Level == levelID);
        }

        public static Config.ControlTowerAreaConfig.EnergyLevelMap GetControlTowerAreaEnergyLevelMapData(int levelID)
        {
            var configData = Config.ConfigData.MainShipConfigData.controlTowerAreaConfig;
            return configData.energyLevelMap.Find(x => x.level == levelID);
        }
        public static Config.LivingAreaConfig.EnergyLevelMap GetLivingAreaEnergyLevelMapData(int levelID)
        {
            var configData = Config.ConfigData.MainShipConfigData.livingAreaConfig;
            return configData.energyLevelMap.Find(x => x.level == levelID);
        }
        public static Config.WorkingAreaConfig.EnergyLevelMap GetWorkingAreaEnergyLevelMapData(int levelID)
        {
            var configData = Config.ConfigData.MainShipConfigData.workingAreaConfig;
            return configData.energyLevelMap.Find(x => x.level == levelID);
        }
        public static Config.HangarAreaConfig.EnergyLevelMap GetHangarAreaEnergyLevelMapData(int levelID)
        {
            var configData = Config.ConfigData.MainShipConfigData.hangarAreaConfig;
            return configData.energyLevelMap.Find(x => x.level == levelID);
        }

        #endregion
    }


    public enum MainShipAreaType
    {
        /// <summary>
        /// 塔台
        /// </summary>
        ControlTower,
        /// <summary>
        /// 生活区
        /// </summary>
        LivingArea,
        /// <summary>
        /// 工作区
        /// </summary>
        WorkingArea,
        /// <summary>
        /// 机库
        /// </summary>
        hangar,
        /// <summary>
        /// 动力区
        /// </summary>
        PowerArea,
        /// <summary>
        /// 引擎区
        /// </summary>
        EngineArea,

        Weapon,

        Shield,
    }
}