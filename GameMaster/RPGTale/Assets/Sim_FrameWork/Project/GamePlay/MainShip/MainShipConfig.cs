using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public class MainShipConfig
    {
        public byte areaEnergyLevelMax;


        public MainShipBasePropertyConfig basePropertyConfig;

        public PowerAreaConfig powerAreaConfig;
        public ControlTowerAreaConfig controlTowerAreaConfig;
        public LivingAreaConfig livingAreaConfig;
        public HangarAreaConfig hangarAreaConfig;
        public WorkingAreaConfig workingAreaConfig;

        public MainShipConfig LoadMainShipConfig()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<MainShipConfig>(JsonConfigPath.MainShipConfigJsonPath);
            areaEnergyLevelMax = data.areaEnergyLevelMax;
            basePropertyConfig = data.basePropertyConfig;
            powerAreaConfig = data.powerAreaConfig;
            controlTowerAreaConfig = data.controlTowerAreaConfig;
            livingAreaConfig = data.livingAreaConfig;
            hangarAreaConfig = data.hangarAreaConfig;
            workingAreaConfig = data.workingAreaConfig;
            return data;
        }

        public bool DataCheck()
        {

            return true;
        }
    }

    public class MainShipMapConfig
    {
        public MainShipAreaMapConfigData powerAreaConfig;


        public MainShipMapConfig LoadMainShipMapConfig()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<MainShipMapConfig>(JsonConfigPath.MainShipAreaMapConfigJsonPath);
            powerAreaConfig = data.powerAreaConfig;
            return data;
        }

        public bool DataCheck()
        {
            if (powerAreaConfig == null)
                return false;
            if(powerAreaConfig.gridConfig.Count!= powerAreaConfig.mapLength* powerAreaConfig.mapWidth)
            {
                Debug.LogError("[Power Area MapConfig] : MapSize Not fit map Size!");
                return false;
            }

            ///Check Coordinate Repeat
            List<Vector2> mapList = new List<Vector2>();
            for(int i = 0; i < powerAreaConfig.gridConfig.Count; i++)
            {
                if (powerAreaConfig.gridConfig[i].coordinate.Length != 2)
                {
                    Debug.LogError("[Power Area MapConfig] : Coordinate Format Error! index=" + i);
                    return false;
                }
                Vector2 v = new Vector2(powerAreaConfig.gridConfig[i].coordinate[0], powerAreaConfig.gridConfig[i].coordinate[1]);
                if (mapList.Contains(v))
                {
                    Debug.LogError("[Power Area MapConfig] : Find Same Coordinate!  value =" + v.ToString());
                    return false;
                }
            }
            ///Check Map
            for(int i = 0; i < powerAreaConfig.mapLength; i++)
            {
                for(int j = 0; i < powerAreaConfig.mapWidth; j++)
                {
                    if (mapList.Contains(new Vector2(i, j)) == false)
                    {
                        Debug.LogError("[Power Area MapConfig] : Can not Find Coordinate Config, value=" + i + "," + j);
                        return false;
                    }
                }
            }
            return true;
                
        }
    }

    /// <summary>
    /// Map Config
    /// </summary>
    public class MainShipAreaMapConfigData
    {
        public ushort mapLength;
        public ushort mapWidth;
        public List<MapGridConfig> gridConfig;

        public class MapGridConfig
        {
            public ushort[] coordinate;
            public bool isBarrier;
        }
    }

    public class MainShipBasePropertyConfig
    {
        /// <summary>
        /// 护盾基础最大值
        /// </summary>
        public int ShieldBase_Max;
        /// <summary>
        /// 护盾默认初始值
        /// </summary>
        public int ShieldBase_Initial;
        /// <summary>
        /// 每回合回盾，未受攻击
        /// </summary>
        public int ShieldChargeEachRound_NotAttack;
        /// <summary>
        /// 每回合回盾，受到攻击
        /// </summary>
        public int ShieldCahrgeEachRound_UnderAttack;
        /// <summary>
        /// 初始减伤
        /// </summary>
        public float DamageReduceInit;

        public float SpeedBase;

        public List<ShieldLevelMap> shieldLevelMap;

        public class ShieldLevelMap
        {
            public int Level;
            public float DamageReduceRate;
            public int ShieldChargeEachRound_NotAttack;
            public int ShieldCahrgeEachRound_UnderAttack;
        }

    }

    public class MainShipAreaBaseConfig
    {
        public string areaIconPath;
        public int Durability_Initial;
        public byte PowerLevel_Max_Initial;
        public byte PowerLevel_Current_Initial;
        public int PowerConsumeBase;
    }

    public class PowerAreaConfig
    {
        public string areaIconPath;
        public int Durability_Initial;
        public ushort energyGenerateBase;
        public byte energyLoadBase;
        public int MaxStorageCountBase;
        public bool unlockOverLoad;
        public byte overLoadLevelMax;

        public List<OverLoadLevelMap> overLoadMap;

        public class OverLoadLevelMap
        {
            public int Level;
            public float FuelConsumeRate;
            public float PowerPruduceRate;
            public float CloseDownRate;
        }
    }

    /// <summary>
    /// 塔台
    /// </summary>
    public class ControlTowerAreaConfig
    {
        public MainShipAreaBaseConfig baseConfig;
        public List<EnergyLevelMap> energyLevelMap;

        public class EnergyLevelMap
        {
            public int level;
            public double energyCostRate;
            public ushort extraValue;
        }

    }
    /// <summary>
    /// 生活区
    /// </summary>
    public class LivingAreaConfig
    {
        public MainShipAreaBaseConfig baseConfig;
        public List<EnergyLevelMap> energyLevelMap;

        public class EnergyLevelMap
        {
            public int level;
            public double energyCostRate;
            public ushort extraValue;
        }
    }

    public class WorkingAreaConfig
    {
        public MainShipAreaBaseConfig baseConfig;
        public List<EnergyLevelMap> energyLevelMap;

        public class EnergyLevelMap
        {
            public int level;
            public double energyCostRate;
            public ushort extraValue;
        }
    }

    public class HangarAreaConfig
    {
        public MainShipAreaBaseConfig baseConfig;
        public List<EnergyLevelMap> energyLevelMap;

        public class EnergyLevelMap
        {
            public int level;
            public double energyCostRate;
            public ushort extraValue;
        }
    }



}