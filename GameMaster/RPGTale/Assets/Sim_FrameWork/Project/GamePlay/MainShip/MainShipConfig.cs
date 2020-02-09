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

        public static MainShipConfig LoadMainShipConfig()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<MainShipConfig>(JsonConfigPath.MainShipConfigJsonPath);
            return data;
        }

        public bool DataCheck()
        {
            return basePropertyConfig.DataCheck();
        }
    }

    public class MainShipMapConfig
    {
        public MainShipAreaMapConfigData powerAreaConfig;


        public static MainShipMapConfig LoadMainShipMapConfig()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<MainShipMapConfig>(JsonConfigPath.MainShipAreaMapConfigJsonPath);
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
        public short shield_energy_total_max_base;
        public short shield_energy_total_max_limit;
        public short[] shield_slot_unlock_energycost_map;
        
        public string shield_direction_left_name;
        public string shield_direction_right_name;
        public string shield_direction_front_name;
        public string shield_direction_back_name;

        public bool shield_state_default_open_left;
        public bool shield_state_default_open_right;
        public bool shield_state_default_open_front;
        public bool shield_state_default_open_back;

        public List<MainShipShieldLayerMap> shieldLayerMap;
        public List<MainShipShieldLevelMap> shieldLevelMap;
        
        public float SpeedBase;

        public bool DataCheck()
        {
            bool result = true;
            if (shield_slot_unlock_energycost_map == null || shield_slot_unlock_energycost_map.Length == 0)
            {
                DebugPlus.LogError("[MainShipBasePropertyConfig] : shield_slot_unlock_energycost_map config is null");
                return false;
            }
            if (shieldLevelMap == null || shieldLevelMap.Count == 0)
            {
                DebugPlus.LogError("[MainShipBasePropertyConfig] : shieldLevelMap config is null!");
                return false;
            }
            List<int> levelMapList = new List<int>();
            for(int i = 0; i < shieldLevelMap.Count; i++)
            {
                if (levelMapList.Contains(shieldLevelMap[i].Level))
                {
                    DebugPlus.LogError("[MainShipShieldLevelMap] : find same levelID!  levelID=" + shieldLevelMap[i].Level);
                    result = false;
                    continue;
                }
            }
            ///Check Range
            if(shield_slot_unlock_energycost_map[shield_slot_unlock_energycost_map.Length-1]> shield_energy_total_max_limit)
            {
                DebugPlus.LogError("[shield_slot_unlock_energycost_map] : Out of Range! max=" + shield_energy_total_max_limit);
                result = false;
            }
            ///Check Data
            for(int i = 0; i <= shield_energy_total_max_limit; i++)
            {
                if (!levelMapList.Contains(i))
                {
                    DebugPlus.LogError("[MainShipShieldLevelMap] : find empty config ! Level=" + i);
                    result = false;
                    continue;
                }
            }
            return result;
        }

    }


    public class MainShipShieldLevelMap
    {
        public int Level;
        public int shieldMax_base;
        public int shieldOpenInit_base;
        public int shieldChargeSpeed_base;
        public short shieldEnergyCost_base;
        public double shieldDamageReduce_base;
        public double shieldDamageReduceProbability_base;
    }

    public class MainShipShieldLayerMap
    {
        public int layerIndex;
        public float damageReudce_ratio;
        public float damageReduceProbability_ratio;
        public float energyCostRatio;
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
        public short energyGenerateBase;
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