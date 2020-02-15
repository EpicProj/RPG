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

        public static Config.GamePreapre_ConfigItem GetAIPrepareConfigItem(string configID)
        {
            Config.GamePreapre_ConfigItem result = null;
            var config = Config.ConfigData.PlayerConfig.gamePrepareConfig;

            if (config != null)
            {
                result = config.AIPrepareConfig.Find(x => x.configID == configID);
                if (result == null)
                    DebugPlus.LogError("GetAIPrepareConfigItem null  configID=" + configID);
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

}