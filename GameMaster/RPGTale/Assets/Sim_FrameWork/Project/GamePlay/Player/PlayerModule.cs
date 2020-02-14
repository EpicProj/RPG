using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class PlayerModule :BaseModule<PlayerModule>
    {

        //Build Panel Config
        public static Dictionary<int, BuildingPanelData> buildPanelDataDic;
        public List<string> AllBuildMainTagList = new List<string>();
        public BaseResourcesData resourceData;

        public override void InitData()
        {
            var config = ConfigManager.Instance.LoadData<BuildingPanelMetaData>(ConfigPath.TABLE_BUILDPANEL_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("BuildingPanelMetaData Read Error");
                return;
            }
            buildPanelDataDic = config.AllBuildingPanelDataDic;
        }

        public override void Register()
        {
        }

        public PlayerModule()
        {
            InitData();
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

        #region GamePrepare

        public static Config.GamePreapre_ConfigItem GetGamePrepareConfigItem(string configID)
        {
            Config.GamePreapre_ConfigItem result = null;
            var config = Config.ConfigData.PlayerConfig.gamePrepareConfig;

            if (config != null)
            {
                result = config.prepareProperty.Find(x => x.configID == configID);
                if (result == null)
                    DebugPlus.LogError("GetGamePrepareConfigItem null  configID=" + configID);
            }
            return result;
        }


        #endregion

    }
    public class TimeData
    {
        public DateTime date; 
        public float realSecondsPerDay;
        /// <summary>
        /// 当前进度
        /// </summary>
        public float timer;
        
        public TimeData() { }
        public static TimeData InitData(Config.TimeDataConfig timeConfig)
        {
            TimeData data = new TimeData();
            data.realSecondsPerDay = timeConfig.RealSecondsPerDay;
            data.date = new DateTime(timeConfig.OriginalYear, timeConfig.OriginalMonth, timeConfig.OriginalDay);
            return data;
        }

        public static TimeData LoadGameSave(TimeDataSave save)
        {
            TimeData data = new TimeData();
            data.date = new DateTime(save.currentYear, save.currentMonth, save.currentDay);
            data.timer = save.timer;

            var config = Config.ConfigData.PlayerConfig.timeConfig;
            if (config == null)
            {
                DebugPlus.LogError("[PlayerTimeData] : Find TimeConfig Error!");
            }
            data.realSecondsPerDay = config.RealSecondsPerDay;
            return data;
        }
    }

    public class TimeDataSave
    {
        public int currentYear;
        public int currentMonth;
        public int currentDay;
        public float timer;

        public static TimeDataSave CreateSave()
        {
            TimeDataSave data = new TimeDataSave();
            TimeData time = PlayerManager.Instance.playerData.timeData;
            data.currentYear = time.date.Year;
            data.currentMonth = time.date.Month;
            data.currentDay = time.date.Day;
            data.timer = time.timer;
            return data;
        }
    }

    #region GamePrepare
    public class GamePrepareData
    {
        public CampInfo currentCampInfo;

        public int hardLevelValue;

        public void ChangeHardLevelValue(int value)
        {
            hardLevelValue += value;
            if (hardLevelValue < 0)
                hardLevelValue = 0;
        }

        public List<GamePreparePropertyData> preparePropertyDataList = new List<GamePreparePropertyData>();


        public int  GamePrepare_BornPosition;  //出生地


        public int GamePrepare_ResourceRichness = 0;  //资源丰富度
        public void GetPrepare_ResourceRichness(int level)
        {
            var propertyData = preparePropertyDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_Resource_Richness);
            if (propertyData == null)
            {
                GetPrepare_ResourceRichness_Default();
                return;
            }
            var data = PlayerModule.GetGamePrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_ResourceRichness = levelData.Level;
        }
        protected void GetPrepare_ResourceRichness_Default()
        {
            DebugPlus.Log("[GamePrepare_ResourceRichness] : Config not Find! Use Default Value");
            GamePrepare_Currency = Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_Resource_Richness_Default;
        }

        /// <summary>
        /// 初始资金
        /// </summary>
        public int GamePrepare_Currency = 0;  
        public void GetPrepare_Currency(int level)
        {
            var propertyData = preparePropertyDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_Currency);
            if (propertyData == null)
            {
                GetPrepare_Currency_Default();
                return;
            }
            var data = PlayerModule.GetGamePrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_Currency = (int)levelData.numParam;
        }
        protected void GetPrepare_Currency_Default()
        {
            DebugPlus.Log("[GamePrepare_Currency] : Config not Find! Use Default Value");
            GamePrepare_Currency = Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_Currency_Default;
        }

        /// <summary>
        /// 敌人强度
        /// </summary>
        public float GamePrepare_EnemyHardLevel = 1;   
        public void GetPrepare_EnermyHardLevel(int level)
        {
            var propertyData = preparePropertyDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_EnemyHardLevel);
            if (propertyData == null)
            {
                GetPrepare_EnermyHardLevel_Default();
                return;
            }
            var data = PlayerModule.GetGamePrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_EnemyHardLevel = (float)levelData.numParam;
        }
        protected void GetPrepare_EnermyHardLevel_Default()
        {
            DebugPlus.Log("[EnermyHardLevel] : Config not Find! Use Default Value");
            GamePrepare_EnemyHardLevel = (float)Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_EnemyHardLevel_Default;
        }


        public float GamePrepare_Research_Coefficient = 1;  //研究系数
        public void GetPrepare_Research_Coefficient(int level)
        {
            var propertyData = preparePropertyDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_Research_Coefficient);
            if (propertyData == null)
            {
                GetPrepare_Research_Coefficient_Default();
                return;
            }
            var data = PlayerModule.GetGamePrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_Research_Coefficient = (float)levelData.numParam;
        }
        protected void GetPrepare_Research_Coefficient_Default()
        {
            DebugPlus.Log("[Research_Coefficient] : Config not Find! Use Default Value");
            GamePrepare_EnemyHardLevel = (float)Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_Research_Coefficient_Default;
        }

        public static GamePrepareData InitData()
        {
            GamePrepareData data = new GamePrepareData();
            var config = Config.ConfigData.PlayerConfig.gamePrepareConfig;
            if (config == null)
                return null;
            for(int i = 0; i < config.prepareProperty.Count; i++)
            {
                GamePreparePropertyData propertyData = new GamePreparePropertyData
                {
                    configID = config.prepareProperty[i].configID,
                    configType = config.prepareProperty[i].configType,
                    currentSelectLevel=config.prepareProperty[i].defaultSelectLevel
                };
                data.preparePropertyDataList.Add(propertyData);
            }

            return data;
        }

        public void RefreshData()
        {
            var config = Config.ConfigData.PlayerConfig.gamePrepareConfig;
            for(int i = 0; i < preparePropertyDataList.Count; i++)
            {
                ///Currency
                if (preparePropertyDataList[i].configID == config.GamePrepareConfig_PropertyLink_Currency)
                {
                    GetPrepare_Currency(preparePropertyDataList[i].currentSelectLevel);
                }
                ///Research Richness
                else if (preparePropertyDataList[i].configID == config.GamePrepareConfig_PropertyLink_Resource_Richness)
                {
                    GetPrepare_ResourceRichness(preparePropertyDataList[i].currentSelectLevel);
                }
                ///Enemy HardLevel
                else if (preparePropertyDataList[i].configID == config.GamePrepareConfig_PropertyLink_EnemyHardLevel)
                {
                    GetPrepare_EnermyHardLevel(preparePropertyDataList[i].currentSelectLevel);
                }
            }
        }

    }

    public class GamePreparePropertyData
    {
        public string configID;
        public byte configType;
        public byte currentSelectLevel;
    }

    #endregion


}