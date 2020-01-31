using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public class BlockConfigData 
    {
        public List<Block_GeneralConfig> configData;
        public BlockConfigData LoadBlockConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            BlockConfigData info = reader.LoadJsonDataConfig<BlockConfigData>(Config.JsonConfigPath.BlockConfigDataJsonPath);
            configData = info.configData;
            return info;
        }

        public bool DataCheck()
        {
            bool result = true;
            List<string> configName = new List<string>();
            for(int i = 0; i < configData.Count; i++)
            {
                if (configName.Contains(configData[i].configName))
                {
                    DebugPlus.LogError("[BlockConfigData] : Find Same Block Config Name ! configName=" + configData[i].configName);
                    result = false;
                    continue;
                }
                else
                {
                    configName.Add(configData[i].configName);
                }
                if (CheckDistrictData(configData[i].districtConfig) == false)
                {
                    result = false;
                    continue;
                }
            }

            return result;
        }

        private bool CheckDistrictData(Block_District_Config config)
        {
            bool result = true;
            if (config == null)
                return false;
            List<Vector2> posList = new List<Vector2>();
            for(int i = 0; i < config.gridConfig.Count; i++)
            {
                byte[] posArray = config.gridConfig[i].coordinate;
                if (posArray.Length != 2)
                {
                    DebugPlus.LogError("[BlockConfigData] : Pos FormatError! array= "+ posArray.ToString() +" Name=" + config.configName);
                    result = false;
                    continue;
                }
                Vector2 pos = new Vector2(posArray[0], posArray[1]);
                if (posList.Contains(pos))
                {
                    DebugPlus.LogError("[BlockConfigData] : Find Same Coordinate! array=" + posArray.ToString()+" Name=" + config.configName);
                    result = false;
                    continue;
                }
            }

            ///Check Match
            for(int i = 0; i < config.areaX;i++)
            {
                for(int j = 0; j < config.areaY; j++)
                {
                    Vector2 pos = new Vector2(i, j);
                    if (!posList.Contains(pos))
                    {
                        DebugPlus.LogError("[BlockConfigData] : Find Empty Coordinate! posX=" + i + " ,  PosY= " + j+ "Name = "+ config.configName);
                        result = false;
                        continue;
                    }
                }
            }

            ///Check Size
            if(Utility.TryParseInt((config.realityRatio* config.areaX).ToString()) ==0 || Utility.TryParseInt((config.realityRatio * config.areaY).ToString()) == 0)
            {
                DebugPlus.LogError("[BlockConfigData] : DistrictRealSize is not interger!" + config.configName);
                result = false;
            }
            return result;
        }
    }

    public class Block_GeneralConfig
    {
        public string configName;
        public bool hasLevel;
        public bool hasDistrict;
        public BlockLevelConfig levelConfig;
        public Block_District_Config districtConfig;
        public Block_Manufact_Config manuConfig;
        public List<Block_Effect_Config> effectConfig;
    }

    /// <summary>
    /// 生产类设置
    /// </summary>
    public class Block_Manufact_Config
    {
        public double speedBase;
        public List<int> formulaIDList;
        public ushort maintainBase;
        public ushort workBase;
        public ushort energyConsumptionBase;
    }

    /// <summary>
    /// 普通加成类设置
    /// </summary>
    public class Block_Effect_Config
    {
        public string modifierName;
        public string iconPath;
    } 

    /// <summary>
    /// District Config
    /// </summary>
    public class Block_District_Config
    {
        public string configName;
        public int areaX;
        public int areaY;
        /// <summary>
        /// 区划缩放比例，相乘必须为整数
        /// </summary>
        public double realityRatio;
        public List<Block_District_GridConfig> gridConfig;
    }

    public class Block_District_GridConfig
    {
        public byte[] coordinate;
        /// <summary>
        /// 是否为空块
        /// </summary>
        public bool isNone;
        public bool unlockDefault;
        public int defualtDistrict;
    }

    #region LevelData

    /// <summary>
    /// EXP Data
    /// </summary>

    public class BlockLevelConfig
    {
        public List<int> EXPMap;
        public BlockDistrictUnlockData districtUnlock;
        public List<ManufactureInherentLevelData> inherentLevel;
        
    }

    public class ManufactureInherentLevelData
    {
        public ushort LevelID;
        public ushort MaxLevel;
        public string Name;
        public string LevelName;
        public string LevelDesc;
        public string IconPath;
    }

    public class BlockDistrictUnlockData
    {
        public List<DistrictUnlockData> UnlockData;

        public class DistrictUnlockData
        {
            public int DistrictID;
            public bool UnlockDefault;
        }
    }
    #endregion
}