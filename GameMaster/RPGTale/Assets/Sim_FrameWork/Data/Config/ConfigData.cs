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

        private static EventConfig _eventConfigData;
        public static EventConfig EventConfigData
        {
            get
            {
                if (_eventConfigData == null)
                {
                    _eventConfigData = new EventConfig();
                    _eventConfigData.LoadEventConfigData();
                }
                return _eventConfigData;
            }
        }

        private static AssembleConfig _assembleConfig;
        public static AssembleConfig AssembleConfig
        {
            get
            {
                if (_assembleConfig == null)
                {
                    _assembleConfig = new AssembleConfig();
                    _assembleConfig.LoadAssembleConfigData();
                }
                return _assembleConfig;
            }
        }

        private static AssemblePartsConfigData _assemblePartsConfigData;
        public static AssemblePartsConfigData AssemblePartsConfigData
        {
            get
            {
                if (_assemblePartsConfigData == null)
                {
                    _assemblePartsConfigData = new AssemblePartsConfigData();
                    _assemblePartsConfigData.LoadPartsCustomConfig();
                }
                return _assemblePartsConfigData;
            }
        }

        private static AssembleShipPartConfigData _assembleShipPartConfigData;
        public static AssembleShipPartConfigData AssembleShipPartConfigData
        {
            get
            {
                if (_assembleShipPartConfigData == null)
                {
                    _assembleShipPartConfigData = new AssembleShipPartConfigData();
                    _assembleShipPartConfigData.LoadAssembleShipPartConfigData();
                }
                return _assembleShipPartConfigData;
            }
        }

        private static MainShipConfig _mainShipConfigData;
        public static MainShipConfig MainShipConfigData
        {
            get
            {
                if (_mainShipConfigData == null)
                {
                    _mainShipConfigData = new MainShipConfig();
                    _mainShipConfigData.LoadMainShipConfig();
                }
                return _mainShipConfigData;
            }
        }

        private static MainShipMapConfig _mainShipMapConfig;
        public static MainShipMapConfig MainShipMapConfig
        {
            get
            {
                if (_mainShipMapConfig == null)
                {
                    _mainShipMapConfig = new  MainShipMapConfig();
                    _mainShipMapConfig.LoadMainShipMapConfig();
                }
                return _mainShipMapConfig;
            }
        }

        private static BlockConfigData _blockConfigData;
        public static BlockConfigData BlockConfigData
        {
            get
            {
                if (_blockConfigData == null)
                {
                    _blockConfigData = new BlockConfigData();
                    _blockConfigData.LoadBlockConfigData();
                }
                return _blockConfigData;
            }
        }


        public void InitData()
        {
            _globalSetting.LoadGlobalSettting();
            _rewardData.LoadRewardData();
            _exploreConfigData.LoadExploreConfigData();
            _eventConfigData.LoadEventConfigData();
            _assembleConfig.LoadAssembleConfigData();
            _assemblePartsConfigData.LoadPartsCustomConfig();
            _assembleShipPartConfigData.LoadAssembleShipPartConfigData();
            _mainShipConfigData.LoadMainShipConfig();
            _mainShipMapConfig.LoadMainShipMapConfig();
            _blockConfigData.LoadBlockConfigData();
        }
        public bool MapCheck()
        {
            return _rewardData.DataCheck() && 
                _eventConfigData.DataCheck() &&
                _mainShipMapConfig.DataCheck();
        }


    }

    public static class GlobalConfigData
    {
        public static Vector3 InfinityVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

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
        public static readonly ushort Explore_Point_Max_HardLevel = 5;

        /// <summary>
        /// 部件设计
        /// </summary>
        public static readonly ushort AssemblePart_Max_PropertyNum = 4;
        public static readonly ushort AssemblePart_Target_MaxNum = 4;
        public static readonly ushort Assemble_MaterialCost_MaxNum = 4;


    }

    public static class GeneralTextData
    {
        public static string Game_Time_Text_Day = "Game_Time_Text_Day";
    }

    public static class GeneralColorData
    {
        private static string General_Lack_Color_Red_Str = "#FF4949FF";
        public static Color General_Lack_Color_Red
        {
            get { return GeneralModule.TryParseColor(General_Lack_Color_Red_Str); }
        }

        private static string General_Finish_Color_Gray_Str = "#909090FF";
        public static Color General_Finish_Color_Gray
        {
            get { return GeneralModule.TryParseColor(General_Finish_Color_Gray_Str); }
        }

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
            var data = reader.LoadJsonDataConfig<RewardData>(JsonConfigPath.RewardDataJsonPath);
            rewardGroup = data.rewardGroup;
           
            return data;
        }

        public bool DataCheck()
        {
            bool result = true;
            if (rewardGroup == null)
                return result;

            List<int> temp = new List<int>();
            for (int i = 0; i < rewardGroup.Count; i++)
            {
                if (!temp.Contains(rewardGroup[i].GroupID))
                {
                    temp.Add(rewardGroup[i].GroupID);
                }
                else
                {
                    Debug.LogError("[RewardData] : Find Same RewardGroup ID, ID=" + rewardGroup[i].GroupID);
                    result = false;
                    continue;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// Explore Config
    /// </summary>
    public class ExploreGeneralConfig
    {
        public Dictionary<string, int> planetAreaMap;

        public ExploreGeneralConfig LoadExploreConfigData()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<ExploreGeneralConfig>(JsonConfigPath.ExploreConfigDataJsonPath);
            planetAreaMap = data.planetAreaMap;
            return data;
        }
    }

    /// <summary>
    /// 随机事件生成配置
    /// </summary>

    public class EventConfig
    {
        public List<ExploreEventConfigData> exploreEventConfigData;
        public EventConfig LoadEventConfigData()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<EventConfig>(JsonConfigPath.EventConfigDataJsonPath);
            exploreEventConfigData = data.exploreEventConfigData;
            return data;
        }

        public bool DataCheck()
        {
            bool result = true;
            if (exploreEventConfigData == null)
                return false;

            List<int> eventList = new List<int>();
            for (int i = 0; i < exploreEventConfigData.Count; i++)
            {
                if (!eventList.Contains(exploreEventConfigData[i].eventID))
                {
                    eventList.Add(exploreEventConfigData[i].eventID);
                }
                else
                {
                    Debug.LogError("[ExploreEventConfigData] : Find Same ExploreEventID , ID=" + exploreEventConfigData[i].eventID);
                    result = false;
                    continue;
                }
            }
            return result;
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

        public string General_Time_Icon;
        public string General_Time_Cost_TextID;

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

            General_Time_Icon = settting.General_Time_Icon;
            General_Time_Cost_TextID = settting.General_Time_Cost_TextID;
            return settting;
        }

        public class RarityConfig
        {
            public ushort Level;
            public string Color;
        }
    }

}