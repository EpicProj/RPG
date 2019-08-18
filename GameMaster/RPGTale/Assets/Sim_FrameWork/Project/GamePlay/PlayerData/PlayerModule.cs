using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sim_FrameWork
{
    public class PlayerModule :Singleton<PlayerModule>
    {
        public enum HardLevel
        {
            easy=1<<0,
            normal=1<<1,
            hard=1<<2
        }


        public BaseResourcesData resourceData;
        public PlayerConfig config;

        public HardLevel currentHardLevel = HardLevel.easy;

        private int food;
        public int Food { get { return food; } set { food = value; } }
        private float currency;
        public float Currency { get { return currency; } set { currency = value; } }
        private int energy;
        public int Energy { get { return energy; } set { energy = value; } }
        private int laber;
        public int Laber { get { return laber; } set { laber = value; } }
        private int reputation;
        public int Reputation { get { return reputation; } set { reputation = value; } }
        private float technologyConversionRate;
        public float TechnologyConversionRate { get { return technologyConversionRate; } set { technologyConversionRate = value; } }


        public void InitData()
        {
            //resourceData = new BaseResourcesData();
            //resourceData.ReadData();
            config = new PlayerConfig();
            config.ReadPlayerConfigData();
            InitPlayerData();
        }

        private void InitPlayerData()
        {
            HardLevelData data = GetHardlevelData(HardLevel.easy);
            food = data.OriginalFood;
            currency = data.OriginalCurrency;
            energy = data.OriginalEnergy;
            laber = data.OriginalLaber;
            reputation = data.OriginalReputation;
            technologyConversionRate = data.TechnologyConversionRate;
        }

        public HardLevelData GetHardlevelData(HardLevel level)
        {
            if (config.hardlevelData.Count == 0)
            {
                Debug.LogError("HardlevelData is null");
                return null;
            }
            switch (level)
            {
                case HardLevel.easy:
                    return config.hardlevelData[0];
                case HardLevel.normal:
                    return config.hardlevelData[1];
                case HardLevel.hard:
                    return config.hardlevelData[2];
                default:
                    Debug.LogError("HardLevelMode Error");
                    return config.hardlevelData[0];
            }
        }

    }


    public class PlayerConfig
    {
        public List<HardLevelData> hardlevelData;



        public void ReadPlayerConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            PlayerConfig config = reader.LoadPlayerConfig();
            hardlevelData = config.hardlevelData;
        }
    }


    public class HardLevelData
    {
        public string HardName;
        //初始货币
        public float OriginalCurrency;
        //初始食物
        public int OriginalFood;
        //初始能量
        public int OriginalEnergy;
        //初始劳动力
        public int OriginalLaber;
        //初始信誉
        public int OriginalReputation;

        //初始科技转化率
        public float TechnologyConversionRate;
    }
}