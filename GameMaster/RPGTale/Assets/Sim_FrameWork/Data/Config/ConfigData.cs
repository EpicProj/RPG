using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public class ConfigData
    {
        private static GlobalSetting _globalSetting;
        public static GlobalSetting GlobalSetting
        {
            get
            {
                if (_globalSetting == null)
                    _globalSetting = new GlobalSetting();
                    _globalSetting.LoadGlobalSettting();
                return _globalSetting;
            }
        }

        private static RewardData _rewardData;
        public static RewardData RewardData
        {
            get
            {
                if (_rewardData == null)
                    _rewardData = new RewardData();
                    _rewardData.LoadRewardData();
                return _rewardData;
            }
        }

        private static ExploreGeneralConfig _exploreConfigData;
        public static ExploreGeneralConfig ExploreConfigData
        {
            get
            {
                if (_exploreConfigData == null)
                {
                    _exploreConfigData = new ExploreGeneralConfig();
                    _exploreConfigData.LoadExploreConfigData();
                }
                return _exploreConfigData;
            }
        }



        public void InitData()
        {
            _globalSetting.LoadGlobalSettting();
            _rewardData.LoadRewardData();
            _exploreConfigData.LoadExploreConfigData();
        }


    }

    public static class GlobalConfigData
    {
        /// <summary>
        /// 最大效果数量
        /// </summary>
        public static readonly int TechDetail_Dialog_MaxEffect_Count = 4;
        /// <summary>
        /// 科技最大要求数量
        /// </summary>
        public static readonly int TechDetail_Dialog_MaxRequire_Count = 4;

        public static readonly int BuildDetail_Cost_MaxInit_Count = 4;

        public static readonly int BuildDetail_District_Area_Max = 25;

        public static readonly int RandomEvent_Dialog_Reward_Max = 4;

        /// <summary>
        /// 探索界面最大任务数量
        /// </summary>
        public static readonly int ExplorePage_Mission_Max_Count = 5;
        public static readonly ushort Explore_Mission_Max_Team_Count = 3;
    }



    /// <summary>
    /// Reward Item
    /// </summary>
    public class GeneralRewardItem
    {
        public ushort type;
        public int ItemID;
        public int count;

    }
    public class RewardGroup
    {
        public int GroupID;
        public List<GeneralRewardItem> itemList;
    }

    public class RewardData
    {
        public List<RewardGroup> rewardGroup;
      

        public RewardData LoadRewardData()
        {
            JsonReader reader = new JsonReader();
            List<int> temp = new List<int>();
            var data = reader.LoadJsonDataConfig<RewardData>(JsonConfigPath.RewardDataJsonPath);
            rewardGroup = data.rewardGroup;
            ///Check
            for(int i = 0; i < rewardGroup.Count; i++)
            {
                if (!temp.Contains(rewardGroup[i].GroupID))
                {
                    temp.Add(rewardGroup[i].GroupID);
                }
                else
                {
                    Debug.LogError("Find Same RewardGroup ID, ID=" + rewardGroup[i].GroupID);
                    continue;
                }
            }

            return data;
        }
    }

    /// <summary>
    /// Explore Config
    /// </summary>
    public class ExploreGeneralConfig
    {


        public ExploreGeneralConfig LoadExploreConfigData()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<ExploreGeneralConfig>(JsonConfigPath.ExploreConfigDataJsonPath);

            return data;
        }
    }


    public class GlobalSetting
    {
        public List<RarityConfig> rarityConfig;

        /// <summary>
        /// Default Area Explore
        /// </summary>
        public List<int> exploreArea_Space;
        public List<int> exploreArea_Earth;

        public string Resource_Research_Icon_Path;
        public string Resource_Currency_Icon_Path;
        public string Resource_Energy_Icon_Path;
        public string Resource_Builder_Icon_Path;
        public string Resource_Rocore_Icon_Path;


        public GlobalSetting LoadGlobalSettting()
        {
            JsonReader config = new JsonReader();
            GlobalSetting settting = config.LoadJsonDataConfig<GlobalSetting>(JsonConfigPath.GlobalSettingJsonPath);
            rarityConfig = settting.rarityConfig;
            exploreArea_Space = settting.exploreArea_Space;
            exploreArea_Earth = settting.exploreArea_Earth;
            Resource_Research_Icon_Path = settting.Resource_Research_Icon_Path;
            Resource_Currency_Icon_Path = settting.Resource_Currency_Icon_Path;
            Resource_Energy_Icon_Path = settting.Resource_Energy_Icon_Path;
            Resource_Builder_Icon_Path = settting.Resource_Builder_Icon_Path;
            Resource_Rocore_Icon_Path = settting.Resource_Rocore_Icon_Path;
            return settting;
        }

        public class RarityConfig
        {
            public ushort Level;
            public string Color;
        }
    }

}