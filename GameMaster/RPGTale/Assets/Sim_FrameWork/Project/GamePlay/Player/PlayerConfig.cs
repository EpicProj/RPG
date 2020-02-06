using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public enum GameHardLevel
    {
        easy = 1 << 0,
        normal = 1 << 1,
        hard = 1 << 2
    }

    public class PlayerConfig 
    {
        public List<HardLevelData> hardlevelData;
        public TimeDataConfig timeConfig;


        public PlayerConfig LoadPlayerConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            var config = reader.LoadJsonDataConfig<PlayerConfig>(Config.JsonConfigPath.PlayerConfigJsonPath);
            hardlevelData = config.hardlevelData;
            timeConfig = config.timeConfig;
            return config;
        }

        public bool DataCheck()
        {
            return true;
        }

    }

    public class TimeDataConfig 
    {
        public int OriginalYear;
        public ushort OriginalMonth;
        public ushort OriginalDay;
        public float RealSecondsPerDay;

    }


    public class HardLevelData
    {
        public string HardName;
        ///初始货币
        public int OriginalCurrency;
        public int OriginalCurrencyMax;

        ///初始能量
        public float OriginalEnergy;
        public float OriginalEnergyMax;
        ///初始研究
        public float OriginalResearch;
        public float OriginalResearchMax;
        ///初始信誉
        public int OriginalReputation;
        public int OriginalReputationMax;

        ///初始建设者数量
        public ushort OriginalBuilder;
        public ushort OriginalBuilderMax;

        ///初始智核数量
        public ushort OriginalRoCore;
        public ushort OriginalRoCoreMax;

        ///初始科技转化率
        public float TechnologyConversionRate;
    }
}