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

        public static Config.PowerAreaConfig.OverLoadLevelMap GetPowerAreaOverLoadMap(int levelID)
        {
            var config = Config.ConfigData.MainShipConfigData.powerAreaConfig;
            return config.overLoadMap.Find(x => x.Level == levelID);
        }

        /// <summary>
        /// Get MainShip Shield Level Data
        /// </summary>
        /// <param name="levelID"></param>
        /// <returns></returns>
        public static Config.MainShipShieldLevelMap GetMainShipShieldLevelData(int levelID)
        {
            var config = Config.ConfigData.MainShipConfigData.basePropertyConfig;
            if (config == null)
                DebugPlus.LogError("[MainShipShieldLevelMap] : config is null ! levelID=" + levelID);
            return config.shieldLevelMap.Find(x => x.Level == levelID);
        }
        public static string GetMainShipShieldDirectionName(MainShip_ShieldDirection direction)
        {
            var config = Config.ConfigData.MainShipConfigData.basePropertyConfig;
            if (direction == MainShip_ShieldDirection.back)
                return MultiLanguage.Instance.GetTextValue(config.shield_direction_back_name);
            else if (direction == MainShip_ShieldDirection.front)
                return MultiLanguage.Instance.GetTextValue(config.shield_direction_front_name);
            else if (direction == MainShip_ShieldDirection.Left)
                return MultiLanguage.Instance.GetTextValue(config.shield_direction_left_name);
            else if (direction == MainShip_ShieldDirection.Right)
                return MultiLanguage.Instance.GetTextValue(config.shield_direction_right_name);
            else
                return string.Empty;
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