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

        public override void InitData()
        {        
            buildPanelDataList = BuildingPanelMetaDataReader.GetBuildingPanelDataList();
            buildPanelDataDic = BuildingPanelMetaDataReader.GetBuildingPanelDataDic();
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

        /// <summary>
        /// 获取Hardlevel信息
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Config.HardLevelData GetHardlevelData(Config.GameHardLevel level)
        {
            var hardlevelData = Config.ConfigData.PlayerConfig.hardlevelData;
            if (hardlevelData.Count == 0 || hardlevelData==null)
            {
                Debug.LogError("HardlevelData is null");
                return null;
            }
            switch (level)
            {
                case Config.GameHardLevel.easy:
                    return hardlevelData[0];
                case Config.GameHardLevel.normal:
                    return hardlevelData[1];
                case Config.GameHardLevel.hard:
                    return hardlevelData[2];
                default:
                    Debug.LogError("HardLevelMode Error");
                    return hardlevelData[0];
            }
        }

    }
    public class TimeData
    {

        public DateTime date; 
        public float realSecondsPerDay;


        public TimeData(Config.TimeDataConfig timeConfig)
        {
            realSecondsPerDay = timeConfig.RealSecondsPerDay;
            date = new DateTime(timeConfig.OriginalYear, timeConfig.OriginalMonth, timeConfig.OriginalDay);
        }
    }



 

}