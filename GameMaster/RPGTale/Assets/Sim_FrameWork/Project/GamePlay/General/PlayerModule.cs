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

            playerData.resourceData.AddCurrencyMax(data.OriginalCurrencyMax);
            playerData.resourceData.AddCurrency(data.OriginalCurrency);

            playerData.resourceData.AddResearch(data.OriginalResearch);
            playerData.resourceData.AddResearchMax(data.OriginalResearchMax);

            playerData.resourceData.AddReputationMax(data.OriginalReputationMax);
            playerData.resourceData.AddReputation(data.OriginalReputation);

            playerData.resourceData.AddEnergyMax(data.OriginalEnergyMax);
            playerData.resourceData.AddEnergy(data.OriginalEnergy);

            playerData.resourceData.AddBuilder(data.OriginalBuilder);
            playerData.resourceData.AddBuilderMax(data.OriginalBuilderMax);

            playerData.resourceData.AddRoCore(data.OriginalRoCore);
            playerData.resourceData.AddRoCoreMax(data.OriginalRoCoreMax);
            
            //Init BuildPanel
            playerData.AllBuildingPanelDataList = buildPanelDataList;
            playerData.UnLockBuildingPanelDataList = GetUnLockBuildData();

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
    
    }
    public class TimeData
    {

        public DateTime date; 
        public float realSecondsPerDay;


        public TimeData(TimeDataConfig timeConfig)
        {
            realSecondsPerDay = timeConfig.RealSecondsPerDay;
            date = new DateTime(timeConfig.OriginalYear, timeConfig.OriginalMonth, timeConfig.OriginalDay);
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