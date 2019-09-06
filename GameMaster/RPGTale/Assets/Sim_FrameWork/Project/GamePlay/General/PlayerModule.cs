using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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



        //Build Panel Config
        public List<BuildingPanelData> buildPanelDataList=new List<BuildingPanelData> ();
        public Dictionary<int, BuildingPanelData> buildPanelDataDic=new Dictionary<int, BuildingPanelData> ();
        public List<string> AllBuildMainTagList = new List<string>();
        public BaseResourcesData resourceData;
        public PlayerConfig config;
        public PlayerData playerData;

        public HardLevel currentHardLevel = HardLevel.easy;

        //Time
        public TimeData timeData;

        public float timer;
        public float realSecondsPerMonth;


        private bool HasInit = false;

        public void InitData()
        {
            if(HasInit)
                return;
            //resourceData = new BaseResourcesData();
            //resourceData.ReadData();
            config = new PlayerConfig();
            config.ReadPlayerConfigData();
            buildPanelDataList = BuildingPanelMetaDataReader.GetBuildingPanelDataList();
            buildPanelDataDic = BuildingPanelMetaDataReader.GetBuildingPanelDataDic();
            AllBuildMainTagList = GetAllBuildPanelType();
            //Init Time
            //INIT CONFIG
            realSecondsPerMonth = config.timeConfig.RealSecondsPerMonth;
            timeData = new TimeData();
            timeData.currentYear = config.timeConfig.OriginalYear;
            timeData.currentMonth = config.timeConfig.OriginalMonth;
            timeData.currentSeason = ConvertMonthToSeason(config.timeConfig.OriginalMonth);
            timeData.realSecondsPerMonth = config.timeConfig.RealSecondsPerMonth;
            HasInit = true;
        }


        
        public PlayerData InitPlayerData()
        {
            HardLevelData data = GetHardlevelData(currentHardLevel);
            playerData = new PlayerData();
            //Init Food
            playerData.AddFoodMax(data.OriginalFoodMax);
            playerData.AddFood(data.OriginalFood);
            //Init Currency
            playerData.AddCurrencyMax(data.OriginalCurrencyMax);
            playerData.AddCurrency(data.OriginalCurrency);
            //Init Labor
            playerData.AddLaborMax(data.OriginalLaborMax);
            playerData.AddLabor(data.OriginalLabor);
            //Init Reputation
            playerData.AddReputationMax(data.OriginalReputationMax);
            playerData.AddReputation(data.OriginalReputation);

            //Init BuildPanel
            playerData.AllBuildingPanelDataList = buildPanelDataList;
            playerData.UnLockBuildingPanelDataList = GetUnLockBuildData();
            playerData.buildTagList = config.BuildTagList;

            return playerData;

        }

        public void AddCurrency(float num)
        {
            playerData.AddCurrency(num);
            UIManager.Instance.SendMessageToWnd(UIPath.MAINMENU_PAGE, "UpdateResourceData", playerData);
        }


        public void AddMaterialData(int id,ushort count)
        {
            playerData.AddMaterialStoreData(id, count);
            UIManager.Instance.SendMessageToWnd(UIPath.WAREHOURSE_DIALOG, "UpdateWarehouseData", playerData.materialStorageDataList);
        }


        /// <summary>
        /// 获取Hardlevel信息
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
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


        #region BuildPanel Data
        public BuildingPanelData GetBuildingPanelDataByKey(int buildID)
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
        /// 获取所有建造主Tag
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<string> GetAllBuildPanelType()
        {
            List<string> result = new List<string>();
            foreach(var data in config.BuildTagList)
            {
                if (result.Contains(data.Tag))
                {
                    Debug.LogError("Find Same Build Main Tag, Tag =" + data.Tag);
                    continue;
                }
                result.Add(data.Tag);
            }
            return result;
        }

        /// <summary>
        /// 获取所有副标签
        /// </summary>
        /// <returns></returns>
        public List<string> GetBuildPanelSubTag(string mainTag)
        {
            List<string> result = new List<string>();
            if (AllBuildMainTagList.Contains(mainTag))
            {
                var subdata = config.BuildTagList.Find(x => x.Tag == mainTag);
                if (subdata == null)
                {
                    Debug.LogError("Get BuildPanel Sub Type Error! MainTag=" + mainTag);
                    return result;
                }
                foreach(var data in subdata.BuildSubTagList)
                {
                    if (result.Contains(data.Tag))
                    {
                        Debug.LogError("Find Same Build SubTag ,tag=" + data.Tag);
                        continue;
                    }
                    result.Add(data.Tag);
                }
            }
            else
            {
                Debug.LogError("Build MainTag Error! mainTag=" + mainTag);
            }
           
            return result;
        }

        public BuildMainTag GetBuildMainTag(int BuildID)
        {
            string tag = GetBuildingPanelDataByKey(BuildID).BuildType;
            if (AllBuildMainTagList.Contains(tag))
            {
                return config.BuildTagList.Find(x => x.Tag == tag);
            }
            else
            {
                Debug.LogError("Get BuildTag Error BuildType=" + tag);
                return null;
            }
        }
        public BuildMainTag GetBuildMainTag(BuildingPanelData Data)
        {
            return GetBuildMainTag(Data.BuildID);
        }

        public BuildMainTag.BuildSubTag GetBuildSubTag(int BuildID)
        {
            BuildMainTag mainTag = GetBuildMainTag(BuildID);
            if (mainTag != null)
            {
                string subType= GetBuildingPanelDataByKey(BuildID).BuildSubType;
                if (GetBuildPanelSubTag(mainTag.Tag).Contains(subType))
                {
                    return mainTag.BuildSubTagList.Find(x => x.Tag == subType);
                }
                else
                {
                    Debug.LogError("BuildSub Tag Error Type=" + subType);
                    return null;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取区块
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public FunctionBlock GetBuildFunctionBlock(BuildingPanelData data)
        {
            return FunctionBlockModule.Instance.GetFunctionBlockByBlockID(data.FunctionBlockID);
        }
  

        public Dictionary<Material,ushort> GetBuildMaterialCost(BuildingPanelData data)
        {
            Dictionary<Material, ushort> result = new Dictionary<Material, ushort>();
            List<string> maList = Utility.TryParseStringList(data.MaterialCost, ';');
            for(int i = 0; i < maList.Count; i++)
            {
                List<int> str = Utility.TryParseIntList(maList[i], ':');
                if (str.Count != 2)
                {
                    Debug.LogError("BuildPanel Parse Error , ID=" + data.BuildID);
                    return result;
                }
                result.Add(MaterialModule.Instance.GetMaterialByMaterialID(str[0]), (ushort)str[1]);
            }
            return result;
        }
        //获取所有解锁的BuildID
        public List<BuildingPanelData> GetUnLockBuildData()
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
        public List<FunctionBlock> GetUnLockBuildBlockID()
        {
            List<FunctionBlock> result = new List<FunctionBlock>();
            List<BuildingPanelData> unlockList = GetUnLockBuildData();
            for(int i = 0; i < unlockList.Count; i++)
            {
                result.Add(FunctionBlockModule.Instance.GetFunctionBlockByBlockID(unlockList[i].FunctionBlockID));
            }
            return result;
        }

        public void AddUnLockBuildData(BuildingPanelData data)
        {
            if (!playerData.UnLockBuildingPanelDataList.Contains(data))
            {
                playerData.UnLockBuildingPanelDataList.Add(data);
            }
            //UpdateUI
            UIManager.Instance.SendMessageToWnd(UIPath.MAINMENU_PAGE, "UpdateBuildPanelData", playerData.UnLockBuildingPanelDataList);
        }

        #endregion

        #region Time
        public class TimeData
        {
            //初始时间
            public Season OriginSeason;
            public Month OriginMonth;
            public int OriginYear;
            //当前时间
            public Season currentSeason;
            public int currentMonth;
            public int currentYear;

            public float realSecondsPerMonth;
        }
  
        //季节转化
        public Season ConvertMonthToSeason(int month)
        {
            switch (month)
            {
                case 3:
                case 4:
                case 5:
                    return Season.Spring;
                case 6:
                case 7:
                case 8:
                    return Season.Summer;
                case 9:
                case 10:
                case 11:
                    return Season.Autumn;
                case 12:
                case 1:
                case 2:
                    return Season.Winter;
                default:
                    Debug.LogError("SeasonError ,month=" + month);
                    return Season.Spring;

            }
        }
        public Season IntConvertToSeason(int i)
        {
            if (Enum.IsDefined(typeof(Season), i))
            {
                return (Season)Enum.ToObject(typeof(Season), i);
            }
            Debug.LogError("SeasonConvertError Season=" + i);
            return Season.Spring;
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


        public void UpdateTime()
        {
            timer += Time.deltaTime;
            if (timer >= realSecondsPerMonth)
            {
                timer = 0;
                timeData.currentMonth++;
                if (timeData.currentMonth >= 13)
                {
                    timeData.currentMonth = 1;
                }
                timeData.currentSeason = ConvertMonthToSeason(timeData.currentMonth);
                UIManager.Instance.SendMessageToWnd(UIPath.MAINMENU_PAGE, "UpdateTime", timeData);
            }
        }

        #endregion

    
    }


    public class PlayerConfig
    {
        public List<HardLevelData> hardlevelData;
        public TimeDataConfig timeConfig;
        public List<BuildMainTag> BuildTagList;



        public void ReadPlayerConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            PlayerConfig config = reader.LoadJsonDataConfig<PlayerConfig>(Config.JsonConfigPath.PlayerConfigJsonPath);
            hardlevelData = config.hardlevelData;
            timeConfig = config.timeConfig;
            BuildTagList = config.BuildTagList;
        }
    }

    public class TimeDataConfig
    {
        public int OriginalYear;
        public int OriginalMonth;
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
        public int OriginalFood;
        public int OriginalFoodMax;
        //初始能量
        public int OriginalEnergy;
        //初始劳动力
        public int OriginalLabor;
        public int OriginalLaborMax;
        //初始信誉
        public int OriginalReputation;
        public int OriginalReputationMax;

        //初始科技转化率
        public float TechnologyConversionRate;
    }

    public class BuildMainTag
    {
        public string Tag;
        public string BuildMainTagName;
        public string BuildMainTagDesc;
        public string BuildMainTagIcon;
        public List<BuildSubTag> BuildSubTagList;

        public class BuildSubTag
        {
            public string Tag;
            public string BuildSubTagName;
            public string BuildSubTagDesc;
            public string BuildSubTagIcon;
        }
    }

}