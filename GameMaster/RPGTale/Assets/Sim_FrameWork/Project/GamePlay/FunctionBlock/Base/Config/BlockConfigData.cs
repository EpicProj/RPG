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
            info.configData = configData;
            return info;
        }
    }

    public class Block_GeneralConfig
    {
        public string configName;
        public string configType;
        public BlockLevelConfig levelConfig;
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