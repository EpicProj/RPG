using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class PlayerModule :BaseModule<PlayerModule>
    {

        //Build Panel Config
        public static List<BuildingPanelData> buildPanelDataList;
        public static Dictionary<int, BuildingPanelData> buildPanelDataDic;
        public List<string> AllBuildMainTagList = new List<string>();
        public BaseResourcesData resourceData;
        public PlayerConfig config;

        public PlayerConfig.HardLevel currentHardLevel = PlayerConfig.HardLevel.easy;

        public override void InitData()
        {        
            buildPanelDataList = BuildingPanelMetaDataReader.GetBuildingPanelDataList();
            buildPanelDataDic = BuildingPanelMetaDataReader.GetBuildingPanelDataDic();
            config = new PlayerConfig();
            config.ReadPlayerConfigData();
        }

        public override void Register()
        {
        }

        public PlayerModule()
        {
            InitData();
        }


        
        public PlayerData InitPlayerData()
        {
            HardLevelData data = config.GetHardlevelData(currentHardLevel);
            PlayerData playerData = new PlayerData();
            playerData.timeData = new TimeData(config.timeConfig);
            //Init Currency
            playerData.resourceData.AddCurrencyMax(data.OriginalCurrencyMax);
            playerData.resourceData.AddCurrency(data.OriginalCurrency);
            //Init Labor
            playerData.resourceData.AddLaborMax(data.OriginalLaborMax);
            playerData.resourceData.AddLabor(data.OriginalLabor);
            //Init Reputation
            playerData.resourceData.AddReputationMax(data.OriginalReputationMax);
            playerData.resourceData.AddReputation(data.OriginalReputation);
            //Init Energy
            playerData.resourceData.AddEnergyMax(data.OriginalEnergyMax);
            playerData.resourceData.AddEnergy(data.OriginalEnergy);
            
            //Init BuildPanel
            playerData.AllBuildingPanelDataList = buildPanelDataList;
            playerData.UnLockBuildingPanelDataList = GetUnLockBuildData();

            //Init Camp
            playerData.campData.AddJusticeValue(CampModule.campConfig.Player_OriginValue_Default);


            return playerData;

        }



        #region BuildPanel Data
        public static BuildingPanelData GetBuildingPanelDataByKey(int buildID)
        {
            BuildingPanelData data = null;
            buildPanelDataDic.TryGetValue(buildID, out data);
            if (data == null)
            {
                Debug.LogError("Get BuildingPanelData Error BuildID=" + buildID);
            }
            return data;
        }

        /// <summary>
        /// 获取区块
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public FunctionBlock GetBuildFunctionBlock(BuildingPanelData data)
        {
            return FunctionBlockModule.GetFunctionBlockByBlockID(data.FunctionBlockID);
        }
  
        /// <summary>
        /// 获取建造材料消耗
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Dictionary<Material,ushort> GetBuildMaterialCost(BuildingPanelData data)
        {
            Dictionary<Material, ushort> result = new Dictionary<Material, ushort>();
            List<string> maList = Utility.TryParseStringList(data.MaterialCost, ',');
            for(int i = 0; i < maList.Count; i++)
            {
                List<int> str = Utility.TryParseIntList(maList[i], ':');
                if (str.Count != 2)
                {
                    Debug.LogError("BuildPanel Parse Error , ID=" + data.BuildID);
                    return result;
                }
                result.Add(MaterialModule.GetMaterialByMaterialID(str[0]), (ushort)str[1]);
            }
            return result;
        }
        ///获取所有解锁的BuildID
        public static List<BuildingPanelData> GetUnLockBuildData()
        {
            List<BuildingPanelData> result = new List<BuildingPanelData>();
            foreach (var bd in buildPanelDataDic)
            {
                if(string.Compare(bd.Value.UnLockParam,"0")==0)
                {
                    result.Add(bd.Value);
                }
            }
            return result;
        }

        //获取所有解锁的区块ID
        public static List<FunctionBlock> GetUnLockBuildBlockID()
        {
            List<FunctionBlock> result = new List<FunctionBlock>();
            List<BuildingPanelData> unlockList = GetUnLockBuildData();
            for(int i = 0; i < unlockList.Count; i++)
            {
                result.Add(FunctionBlockModule.GetFunctionBlockByBlockID(unlockList[i].FunctionBlockID));
            }
            return result;
        }

 

        #endregion

        #region Time
       
  
        //季节转化
        public static TimeData.Season ConvertMonthToSeason(int month)
        {
            switch (month)
            {
                case 3:
                case 4:
                case 5:
                    return TimeData.Season.Spring;
                case 6:
                case 7:
                case 8:
                    return TimeData.Season.Summer;
                case 9:
                case 10:
                case 11:
                    return TimeData.Season.Autumn;
                case 12:
                case 1:
                case 2:
                    return TimeData.Season.Winter;
                default:
                    Debug.LogError("SeasonError ,month=" + month);
                    return TimeData.Season.Spring;

            }
        }
        public static TimeData.Season IntConvertToSeason(int i)
        {
            if (Enum.IsDefined(typeof(TimeData.Season), i))
            {
                return (TimeData.Season)Enum.ToObject(typeof(TimeData.Season), i);
            }
            Debug.LogError("SeasonConvertError Season=" + i);
            return TimeData.Season.Spring;
        }

        public SeasonConfig GetSeasonConfig(int season)
        {
            SeasonConfig seasonConfig = null;
            seasonConfig = config.timeConfig.SeasonConfigList.Find(x => x.SeasonIndex == season);
            if (seasonConfig == null)
            {
                Debug.LogError("SeasonIndex Error, seasonID=" + season);
            }
            return seasonConfig;
        }

        public string GetSeasonName(int season)
        {
            return MultiLanguage.Instance.GetTextValue(GetSeasonConfig(season).SeasonName);
        }

        public Sprite GetSeasonSprite(int season)
        {
            return Utility.LoadSprite(GetSeasonConfig(season).SeasonIconPath, Utility.SpriteType.png);
        }

        #endregion

    
    }
    public class TimeData
    {
        public enum Season
        {
            Spring = 1,
            Summer = 2,
            Autumn = 3,
            Winter = 4
        }

        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }


        //初始时间
        public Season OriginSeason;
        public Month OriginMonth;
        public int OriginYear;
        //当前时间
        public Season currentSeason;
        public ushort currentMonth;
        public int currentYear;

        public float realSecondsPerMonth;


        public TimeData(TimeDataConfig timeConfig)
        {
            currentYear = timeConfig.OriginalYear;
            currentMonth = timeConfig.OriginalMonth;
            currentSeason = PlayerModule.ConvertMonthToSeason(timeConfig.OriginalMonth);
            realSecondsPerMonth = timeConfig.RealSecondsPerMonth;
        }
        

    }



    public class PlayerConfig
    {
        public List<HardLevelData> hardlevelData;
        public TimeDataConfig timeConfig;

        public enum HardLevel
        {
            easy = 1 << 0,
            normal = 1 << 1,
            hard = 1 << 2
        }

        public void ReadPlayerConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            PlayerConfig config = reader.LoadJsonDataConfig<PlayerConfig>(Config.JsonConfigPath.PlayerConfigJsonPath);
            hardlevelData = config.hardlevelData;
            timeConfig = config.timeConfig;
        }


        /// <summary>
        /// 获取Hardlevel信息
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public HardLevelData GetHardlevelData(HardLevel level)
        {
            if (hardlevelData.Count == 0)
            {
                Debug.LogError("HardlevelData is null");
                return null;
            }
            switch (level)
            {
                case HardLevel.easy:
                    return hardlevelData[0];
                case HardLevel.normal:
                    return hardlevelData[1];
                case HardLevel.hard:
                    return hardlevelData[2];
                default:
                    Debug.LogError("HardLevelMode Error");
                    return hardlevelData[0];
            }
        }
    }

    public class TimeDataConfig
    {
        public int OriginalYear;
        public ushort OriginalMonth;
        public float RealSecondsPerMonth;
        public List<SeasonConfig> SeasonConfigList;

    }
    public class SeasonConfig
    {
        public int SeasonIndex;
        //季节名
        public string SeasonName;
        //季节图标
        public string SeasonIconPath;
    }

    public class HardLevelData
    {
        public string HardName;
        //初始货币
        public float OriginalCurrency;
        public float OriginalCurrencyMax;
        //初始食物
        public float OriginalFood;
        public float OriginalFoodMax;
        //初始能量
        public float OriginalEnergy;
        public float OriginalEnergyMax;
        //初始劳动力
        public float OriginalLabor;
        public float OriginalLaborMax;
        //初始信誉
        public int OriginalReputation;
        public int OriginalReputationMax;

        //初始科技转化率
        public float TechnologyConversionRate;
    }


}