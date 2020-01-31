using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sim_FrameWork
{
    public enum ExploreAreaType
    {
        space,
        earth,
    }

    public class ExploreModule : BaseModule<ExploreModule>
    {
        public static List<ExploreArea> ExploreAreaList;
        public static Dictionary<int, ExploreArea> ExploreAreaDic;

        public static List<ExploreData> ExploreDataList;
        public static Dictionary<int, ExploreData> ExploreDataDic;

        public static List<ExplorePoint> ExplorePointList;
        public static Dictionary<int, ExplorePoint> ExplorePointDic;

        public static List<ExploreEvent> ExploreEventList;
        public static Dictionary<int, ExploreEvent> ExploreEventDic;

        public static List<ExploreChoose> ExploreChooseList;
        public static Dictionary<int, ExploreChoose> ExploreChooseDic;


        public static List<int> ExploreAreaListSpace = new List<int>();
        public static List<int> ExploreAreaListEarth = new List<int>();

        public override void InitData()
        {
            ExploreAreaList = ExploreMetaDataReader.GetExploreAreaList();
            ExploreAreaDic = ExploreMetaDataReader.GetExploreAreaDic();

            ExploreDataList = ExploreMetaDataReader.GetExploreDataList();
            ExploreDataDic = ExploreMetaDataReader.GetExploreDataDic();

            ExplorePointList = ExploreMetaDataReader.GetExplorePointList();
            ExplorePointDic = ExploreMetaDataReader.GetExplorePointDic();

            ExploreEventList = ExploreMetaDataReader.GetExploreEventList();
            ExploreEventDic = ExploreMetaDataReader.GetExploreEventDic();
            
            ExploreChooseList = ExploreMetaDataReader.GetExploreChooseList();
            ExploreChooseDic = ExploreMetaDataReader.GetExploreChooseDic();

            GenerateExploreArea();
        }

        public override void Register()
        {
        }

        public ExploreModule()
        {
            InitData();
        }

        public Vector3 GetPlanetNevigatorCameraPos(int pointID)
        {
            var pointData = GetExplorePointDataByKey(pointID);
            if (pointData != null)
            {
                var list = Utility.TryParseIntList(pointData.PointNevigator, ',');
                if (list.Count == 2)
                {
                    int missionID = list[0];
                    var missionData = GetExploreDataByKey(missionID);
                    if (missionData != null)
                    {
                        return GetExploreMissionCameraPos(missionData.ExploreID);
                    }
                }
                else
                {
                    Debug.LogError("Point Nevigator Error! PointID=" + pointID);
                }
            }
            return new Vector3();
        }

        public Quaternion GetPlanetNevigatorCameraRotation(int pointID)
        {
            var pointData = GetExplorePointDataByKey(pointID);
            if (pointData != null)
            {
                var list = Utility.TryParseIntList(pointData.PointNevigator, ',');
                if (list.Count == 2)
                {
                    int missionID = list[0];
                    var missionData = GetExploreDataByKey(missionID);
                    if (missionData != null)
                    {
                        return GetExploreMissionCameraRotation(missionData.ExploreID);
                    }
                }
                else
                {
                    Debug.LogError("Point Nevigator Error! PointID=" + pointID);
                }
            }
            return new Quaternion();
        }

        #region Explore Area
        void GenerateExploreArea()
        {
            ExploreAreaListSpace = GetTotalExploreAreaData(ExploreAreaType.space);
            ExploreAreaListEarth = GetTotalExploreAreaData(ExploreAreaType.earth);
        }

        public static ExploreArea GetExploreAreaDataByKey(int areaID)
        {
            ExploreArea area = null;
            ExploreAreaDic.TryGetValue(areaID, out area);
            if (area == null)
            {
                Debug.LogError("Get ExploreAreaData Error ! ID=" + areaID);
            }
            return area;
        }

        public static string GetExploreAreaName(int areaID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExploreAreaDataByKey(areaID).Name);
        }

        public static string GetExploreAreaTitleName(int areaID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExploreAreaDataByKey(areaID).NameTitle);
        }
        public static string GetExploreAreaDesc(int areaID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExploreAreaDataByKey(areaID).Desc);
        }
        public static Sprite GetExploreAreaIcon(int areaID)
        {
            return Utility.LoadSprite(GetExploreAreaDataByKey(areaID).IconPath, Utility.SpriteType.png);
        }
      

        public static ExploreData GetExploreDataByKey(int exploreID)
        {
            ExploreData data = null;
            ExploreDataDic.TryGetValue(exploreID, out data);
            if (data == null)
            {
                Debug.LogError("Get ExploreData Error ! ID=" + exploreID);
            }
            return data;
        }

        public static string GetExploreMissionName(int exploreID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExploreDataByKey(exploreID).MissionName);
        }
        public static string GetExplorMissionAreaName(int exploreID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExploreDataByKey(exploreID).AreaName);
        }
        public static string GetExploreMissionDesc(int exploreID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExploreDataByKey(exploreID).MissionDesc);
        }
        public static Sprite GetExploreMissionBG(int exploreID)
        {
            return Utility.LoadSprite(GetExploreDataByKey(exploreID).BGPath, Utility.SpriteType.png);
        }

        /// <summary>
        /// 随机生成探索区域
        /// </summary>
        /// <param name="areaID"></param>
        public static List<ExploreRandomItem> GetRandomArea(int areaID , int maxCount)
        {
            if (maxCount <= 0)
                return null;

            List<ExploreRandomItem> tempList = new List<ExploreRandomItem>();
            var list = Utility.TryParseIntList(GetExploreAreaDataByKey(areaID).ExploreList,',');
            for(int i = 0; i < list.Count; i++)
            {
                var exploreData = GetExploreDataByKey(list[i]);
                if (exploreData != null)
                {
                    ExploreRandomItem item = new ExploreRandomItem(areaID,exploreData.ExploreID);
                    tempList.Add(item);
                }
            }
            ///Delect Doing Misson And Finished Mission
            for(int i = 0; i < tempList.Count; i++)
            {
                if(ExploreEventManager.Instance.CheckMissionIsDoing(areaID,tempList[i].exploreID) == true || ExploreEventManager.Instance.CheckMissionIsFinish(areaID,tempList[i].exploreID) == true)
                {
                    tempList.Remove(tempList[i]);
                }
            }

            if (tempList.Count >= maxCount)
            {
                return Utility.GetRandomList<ExploreRandomItem>(tempList, maxCount);
            }
            else
            {
                return tempList;
            }
        }

        public static List<int> GetTotalExploreAreaData(ExploreAreaType areaType)
        {
            if(areaType == ExploreAreaType.earth)
            {
                return Config.ConfigData.GlobalSetting.exploreArea_Earth;
             
            }else if (areaType == ExploreAreaType.space)
            {
                return Config.ConfigData.GlobalSetting.exploreArea_Space;
            }
            return null;
        }
        
        /// <summary>
        /// 获取任务相机位置信息
        /// </summary>
        /// <param name="exploreID"></param>
        /// <returns></returns>
        public static Vector3 GetExploreMissionCameraPos(int exploreID)
        {
            var exploreData = GetExploreDataByKey(exploreID);
            if (exploreData != null)
            {
                var list = Utility.TryParseFloatList(exploreData.CameraPos, ',');
                if (list.Count == 3)
                {
                    Vector3 pos = new Vector3(list[0],list[1],list[2]);
                    return pos;
                }
                else
                {
                    Debug.LogError("ExploreMission Camera Pos Error! exploreID=" + exploreID);
                }
            }
            return Vector3.zero;
        }
        public static Quaternion GetExploreMissionCameraRotation(int exploreID)
        {
            var exploreData = GetExploreDataByKey(exploreID);
            if (exploreData != null)
            {
                var list = Utility.TryParseFloatList(exploreData.CameraRotation, ',');
                if (list.Count == 3)
                {
                    Vector3 pos = new Vector3(list[0], list[1], list[2]);
                    return Quaternion.Euler(pos);
                }
                else
                {
                    Debug.LogError("ExploreMission Camera Rotation Error! exploreID=" + exploreID);
                }
            }
            return new Quaternion();
        }

        public static Vector3 GetExploreAreaCameraPos(int areaID)
        {
            var areaData = GetExploreAreaDataByKey(areaID);
            if (areaData != null)
            {
                var list = Utility.TryParseFloatList(areaData.CameraPos, ',');
                if (list.Count == 3)
                {
                    Vector3 pos = new Vector3(list[0], list[1], list[2]);
                    return pos;
                }
                else
                {
                    Debug.LogError("ExploreArea Camera Pos Error! areaID=" + areaID);
                }
            }
            return Vector3.zero;
        }
        public static Quaternion GetExploreAreaCameraRotation(int areaID)
        {
            var areaData = GetExploreAreaDataByKey(areaID);
            if (areaData != null)
            {
                var list = Utility.TryParseFloatList(areaData.CameraRotation, ',');
                if (list.Count == 3)
                {
                    Vector3 pos = new Vector3(list[0], list[1], list[2]);
                    return Quaternion.Euler(pos);
                }
                else
                {
                    Debug.LogError("ExploreArea Camera Rotation Error! areaID=" + areaID);
                }
            }
            return new Quaternion();
        }

        public static string GetPointMapData(int areaID)
        {
            if (GetExploreAreaDataByKey(areaID) != null)
            {
                var dic = Config.ConfigData.ExploreConfigData.planetAreaMap;
                foreach(KeyValuePair<string,int> kvp in dic)
                {
                    if (kvp.Value == areaID)
                        return kvp.Key;
                }
            }
            return string.Empty;
        }

        #endregion



        #region Explore Event
        public static ExploreEvent GetExploreEventDataByKey(int exploreID)
        {
            ExploreEvent result = null;
            ExploreEventDic.TryGetValue(exploreID, out result);
            if (result == null)
            {
                Debug.LogError("Get ExploreData Error ! ID=" + exploreID);
            }
            return result;
        }

        public static string GetEventName(int eventID)
        {
            var data = GetExploreEventDataByKey(eventID);
            return MultiLanguage.Instance.GetTextValue(data.Name);
        }
        public static string GetEventTitleName(int eventID)
        {
            var data = GetExploreEventDataByKey(eventID);
            return MultiLanguage.Instance.GetTextValue(data.TitleName);
        }

        public static string GetEventDesc(int eventID)
        {
            var data = GetExploreEventDataByKey(eventID);
            return MultiLanguage.Instance.GetTextValue(data.Desc);
        }
        public static Sprite GetEventBG(int eventID)
        {
            return Utility.LoadSprite(GetExploreEventDataByKey(eventID).EventBG, Utility.SpriteType.png);
        }

        #endregion


        #region Explore Point

        public static ExplorePoint GetExplorePointDataByKey(int pointID)
        {
            ExplorePoint point = null;
            ExplorePointDic.TryGetValue(pointID, out point);
            if (point == null)
            {
                Debug.LogError("Get ExplorePointData Error ! ID=" + pointID);
            }
            return point;
        }


        public static string GetExplorePointName(int pointID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExplorePointDataByKey(pointID).Name);
        }
        public static string GetExplorePointDesc(int pointID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExplorePointDataByKey(pointID).Desc);
        }

        public static List<ExplorePointData> GetExplorePointDataList(int areaID,int exploreID)
        {
            List<ExplorePointData> result = new List<ExplorePointData>();
            var exploreData = GetExploreDataByKey(exploreID);
            if (exploreData != null)
            {
                foreach (var point in ExplorePointDic.Values)
                {
                    if (point.SeriesID == exploreData.SeriesID)
                    {
                        ExplorePointData data = new ExplorePointData(areaID,exploreID,point.PointID);
                        ExploreEventManager.Instance.AddExplorePointData(areaID,exploreID,data);
                        result.Add(data);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 初始化点位生成
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="data"></param>
        public static void GetUnlockExplorePoint(List<ExplorePointData> rowData,ref List<ExplorePointData> data)
        {
            if (rowData == null)
                return;
            for (int i = 0; i < rowData.Count; i++)
            {
                if (rowData[i].PrePoint != 0)
                {
                    var pointData = ExploreEventManager.Instance.GetExplorePointData(rowData[i].AreaID,rowData[i].ExploreID,rowData[i].PrePoint);
                    if (pointData != null)
                    {
                        if (pointData.currentState == ExplorePointData.PointState.Finish)
                        {
                            data.Add(rowData[i]);
                        }
                    }
                } else if (rowData[i].PrePoint == 0)
                {
                    data.Add(rowData[i]);
                }
            }
        }

        public static ushort GetMissionPointIndex(int pointID)
        {
            var pointData = GetExplorePointDataByKey(pointID);
            if (pointData != null)
            {
                var list = Utility.TryParseIntList(pointData.PointNevigator, ',');
                if (list.Count == 2)
                {
                    return (ushort)list[1];
                }
                else
                {
                    Debug.LogError("GetMissionPointIndex Error! pointID=" + pointID);
                }
            }
            return 0;
        }

        public static int GetMissionAreaIndex(int pointID)
        {
            var pointData = GetExplorePointDataByKey(pointID);
            if (pointData != null)
            {
                var list = Utility.TryParseIntList(pointData.PointNevigator, ',');
                if (list.Count == 2)
                {
                    return list[0];
                }
                else
                {
                    Debug.LogError("GetMissionAreaIndex Error! pointID=" + pointID);
                }
            }
            return 0;
        }


        #endregion

        #region ExploreChoose

        public static ExploreChoose GetExploreChooseDataByKey(int chooseID)
        {
            ExploreChoose choose = null;
            ExploreChooseDic.TryGetValue(chooseID, out choose);
            if (choose == null)
            {
                Debug.LogError("Get ExploreChooseData Error ! ID=" + chooseID);
            }
            return choose;
        }

        public static string GetChooseContent(int chooseID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExploreChooseDataByKey(chooseID).Content);
        }
        private static List<ExploreChoose> GetExploreChooseList(int eventID)
        {
            List<ExploreChoose> result = new List<ExploreChoose>();
            var data = GetExploreEventDataByKey(eventID);
            var contentList = Utility.TryParseIntList(data.ChooseList, ',');
            for(int i = 0; i < contentList.Count; i++)
            {
                var choose = GetExploreChooseDataByKey(contentList[i]);
                if (choose != null)
                {
                    result.Add(choose);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取事件选项
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
        public static List<ExploreChooseItem> GetChooseItem(int eventID)
        {
            List<ExploreChooseItem> result = new List<ExploreChooseItem>();
            var List = GetExploreChooseList(eventID);
            for (int i = 0; i < List.Count; i++)
            {
                ExploreChooseItem item = new ExploreChooseItem(
                    List[i].ChooseID);
                result.Add(item);
            }
            return result;
        }


        #endregion


        #region ExploreEventConfigData

        public static ExploreEventConfigData GetExploreEventConfigData(int eventID)
        {
            ExploreEventConfigData data = null;
            var configData = Config.ConfigData.EventConfigData;
            if (configData != null)
            {
                data = configData.exploreEventConfigData.Find(x => x.eventID == eventID);
                if (data == null)
                    Debug.Log("Find Explore Event Config Empty!  EventID=" + eventID);
            }
            return data;
        }

        public static Dictionary<string,bool> GetExploreEventGlobalFlag(int eventID)
        {
            var data = GetExploreEventConfigData(eventID);
            if (data != null)
            {
                return data.trigger.Global;
            }
            return null;
        }


        /// <summary>
        /// 事件触发前置
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
        public static bool CheckEventTrigger(int eventID)
        {
            var playerData = GetExploreEventConfigData(eventID).trigger.player;
            ///Check GlobalFlag
            var trigger = GetExploreEventGlobalFlag(eventID);
            if (trigger != null)
            {
                foreach (KeyValuePair<string, bool> kvp in trigger)
                {
                    if (!GlobalEventManager.Instance.CheckGlobalFlagExist(kvp.Key) == kvp.Value)
                        return false;
                }
            }
            ///Currency
            if (playerData != null)
            {
                if (!string.IsNullOrEmpty(playerData.CurrencyCompare))
                {
                    if (!Utility.ParseGeneralCompare(playerData.CurrencyValue, playerData.CurrencyCompare))
                        return false;
                }
                if (playerData.Material != null)
                {
                    foreach(KeyValuePair<string,string> kvp in playerData.Material)
                    {
                        int materialID = Utility.TryParseInt(kvp.Key);
                        if (MaterialModule.GetMaterialByMaterialID(materialID) != null)
                        {
                            if (!Utility.ParseGeneralCompare(PlayerManager.Instance.GetMaterialStoreCount(materialID), kvp.Value))
                                return false;
                        }
                    }
                }
                if (playerData.Technology != null)
                {
                    foreach(KeyValuePair<string,string> kvp in playerData.Technology)
                    {
                        int techID = Utility.TryParseInt(kvp.Value);
                        if (TechnologyModule.GetTechDataByID(techID) != null)
                        {
                            var techData = TechnologyDataManager.Instance.GetTechInfo(techID);
                            if (techData != null)
                            {
                                if(string.Compare(kvp.Value, "Complete") ==0)
                                {
                                    if (techData.currentState != TechnologyState.Done)
                                        return false;
                                }else if(string.Compare(kvp.Value, "Lock") == 0)
                                {
                                    if (techData.currentState != TechnologyState.Lock)
                                        return false;
                                }
                                else
                                {
                                    Debug.LogError("ExploreEvent Tech Format Error! EventID=" + eventID);
                                }
                            }
                        }
                    }
                }
            }
            /// CheckPre Event
            var preList = GetExploreEventConfigData(eventID).trigger.PreEvent;
            if (preList != null)
            {
                for (int i = 0; i < preList.Count; i++)
                {
                    if (ExploreEventManager.Instance.CheckRandomEventFinish(eventID) == false)
                        return false;
                }
            }
            return true;
        }


        #endregion
    }

    public class ExploreChooseItem 
    {
        public int ChooseID;
        public int nextEvent;
        public string content;
        public int rewardID;

        public ExploreChooseItem(int chooseID)
        {
            var data = ExploreModule.GetExploreChooseDataByKey(chooseID);
            if (data != null)
            {
                ChooseID = chooseID;
                nextEvent = data.NextEvent;
                content = ExploreModule.GetChooseContent(chooseID);
                this.rewardID = data.RewardID;
            }
        }
    }

    /// <summary>
    /// 探索大区
    /// </summary>
    public class ExploreAreaData
    {
        public int areaID;

        /// <summary>
        /// 区域总进度
        /// </summary>
        public float areaTotalProgress = 0f;

        private ushort _currentDepth = 1;
        public ushort CurrentDepth
        {
            get { return _currentDepth; }
        }

        public List<ExploreRandomItem> currentMissionList =new List<ExploreRandomItem> ();
        public List<ExploreRandomItem> finishedMissionList = new List<ExploreRandomItem>();

        public string areaName;
        public string areaTitleName;
        public string areaDesc;
        public Sprite areaIcon;

        /// <summary>
        /// 解锁状态
        /// </summary>
        public bool unlock;
        /// <summary>
        /// 初始化解锁的任务数量
        /// </summary>
        public ushort defaultMissionCount;

        public bool InitData = false;

        
        public List<ExplorePointData> pointList=new List<ExplorePointData> ();

        public ExploreAreaData(int areaID)
        {
            var data = ExploreModule.GetExploreAreaDataByKey(areaID);
            if (data != null)
            {
                this.areaID = data.AreaID;
                areaName = ExploreModule.GetExploreAreaName(areaID);
                areaTitleName = ExploreModule.GetExploreAreaTitleName(areaID);
                areaDesc = ExploreModule.GetExploreAreaDesc(areaID);
                areaIcon = ExploreModule.GetExploreAreaIcon(areaID);
                unlock = data.Unlock;
                defaultMissionCount = data.DefaultMissionCount;
                InitData = true;
            }
        }

        /// <summary>
        /// 生成随机任务
        /// </summary>
        public void GenerateRandomMission()
        {
            if (InitData)
            {
                currentMissionList = ExploreModule.GetRandomArea(areaID,defaultMissionCount);
                if (currentMissionList != null)
                {
                    for(int i = 0; i < currentMissionList.Count; i++)
                    {
                        Debug.Log("Add Mission, [AreaID] " + areaID +" ; [MissionID] " + currentMissionList[i].exploreID);
                        ExploreEventManager.Instance.AddExploreMission(areaID, currentMissionList[i]);
                    }
                }
            }
            else
            {
                Debug.LogError("Can not GenerateMission withOut Init");
            }
        }


        public void AddCurrentDepth()
        {
            _currentDepth++;
        }
  
    }

    public class ExploreRandomItem : RandomObject
    {
        public enum ExploreMissionState
        {
            None,
            Init,
            Doing,
            Finish
        }

        public ExploreMissionState currentState;

        public int AreaID;

        /// <summary>
        /// 随机到的探索ID
        /// </summary>
        public int exploreID;
        /// <summary>
        /// 前置探索ID
        /// </summary>
        public int requirePreExploreID;
        /// <summary>
        /// 最大派遣队伍数量
        /// </summary>
        public ushort maxTeamNum;
        /// <summary>
        /// 探索点位信息
        /// </summary>
        public List<ExplorePointData> totalPointList=new List<ExplorePointData> ();

        public List<ExplorePointData> currentPointlist = new List<ExplorePointData>();
        public List<ExplorePointData> finishedPointList = new List<ExplorePointData>();


        public string missionName;
        public string missionAreaName;
        public string missionDesc;
        public Sprite missionBG;
        
        /// <summary>
        /// 难度等级
        /// </summary>
        public ushort areaHardLevel;
        public ushort Depth;

        public PlayerExploreTeamData teamData;

        public ExploreRandomItem(int areaID, int exploreID)
        {
            var exploreData = ExploreModule.GetExploreDataByKey(exploreID);
            currentState = ExploreMissionState.None;
            if (exploreData != null)
            {
                AreaID = areaID;

                this.exploreID = exploreData.ExploreID;
                missionName = ExploreModule.GetExploreMissionName(exploreID);
                missionAreaName = ExploreModule.GetExplorMissionAreaName(exploreID);
                missionDesc = ExploreModule.GetExploreMissionDesc(exploreID);
                missionBG = ExploreModule.GetExploreMissionBG(exploreID);
                requirePreExploreID = exploreData.RequirePreID;
                totalPointList = ExploreModule.GetExplorePointDataList(areaID,exploreID);

                areaHardLevel = exploreData.HardLevel;
                maxTeamNum = exploreData.TeamMaxNum >= Config.GlobalConfigData.Explore_Mission_Max_Team_Count ? Config.GlobalConfigData.Explore_Mission_Max_Team_Count : exploreData.TeamMaxNum;
                
                Weight = exploreData.Weight;

                currentState = ExploreMissionState.Init;

                RefreshUnlockPoint();
            }
        }

        /// <summary>
        /// 刷新解锁点位
        /// </summary>
        public void RefreshUnlockPoint()
        {
            List<ExplorePointData> pointList = new List<ExplorePointData>();
            ExploreModule.GetUnlockExplorePoint(totalPointList, ref pointList);
            currentPointlist = pointList;
        }

    }


    /// <summary>
    /// 探索点位信息
    /// </summary>
    public class ExplorePointData
    {
        public enum PointState
        {
            None,
            Lock,
            Unlock,
            Doing,
            Finish
        }
        public int AreaID;
        public int ExploreID;

        public int PointID;
        public int seriesID;
        public string pointName;
        public string pointDesc;

        /// <summary>
        /// 点位层级
        /// </summary>
        public ushort depthLevel;

        public PointState currentState = PointState.None;
        /// <summary>
        /// 前置点位
        /// </summary>
        public int PrePoint;
        /// <summary>
        /// 触发事件ID
        /// </summary>
        public int eventID;
        /// <summary>
        /// 资源消耗
        /// </summary>
        public ushort EnergyCost;
        /// <summary>
        /// 时间消耗
        /// </summary>
        public ushort TimeCost;
        public ushort RemainTime;
        public Timer pointTimer;

        public ushort HardLevel;


        public int PointAreaNevigator;
        public ushort PointPlanetpointNevigator;

        public Transform pointMapTrans;

        public ExplorePointData(int areaID,int exploreID, int pointID)
        {
            var data = ExploreModule.GetExplorePointDataByKey(pointID);
            if (data != null)
            {
                AreaID = areaID;
                ExploreID = exploreID;

                PointID = data.PointID;
                seriesID = data.SeriesID;
                PrePoint = data.PrePoint;
                if (PrePoint == 0)
                    currentState = PointState.Unlock;
                else
                    currentState = PointState.Lock;
                pointName = ExploreModule.GetExplorePointName(pointID);
                pointDesc = ExploreModule.GetExplorePointDesc(pointID);
                depthLevel = data.DepthLevel;
                EnergyCost = data.EnergyCost;
                eventID = data.EventID;
                TimeCost = data.Time;
                RemainTime = data.Time;


                if (data.HardLevel > Config.GlobalConfigData.Explore_Point_Max_HardLevel)
                    HardLevel = Config.GlobalConfigData.Explore_Point_Max_HardLevel;
                else
                    HardLevel = data.HardLevel;

                PointAreaNevigator = ExploreModule.GetMissionAreaIndex(pointID);
                PointPlanetpointNevigator = ExploreModule.GetMissionPointIndex(pointID);
                pointMapTrans = null;

            }
        }
    }

    public class PlayerExploreTeamData
    {


        /// <summary>
        /// 初始携带能量
        /// </summary>
        public ushort EnergyStartNum;
        /// <summary>
        /// 当前携带能量
        /// </summary>
        private ushort _energyCurrentNum;
        public ushort EnergyCurrentNum
        {
            get
            {
                return _energyCurrentNum;
            }
        }
        public void ChangeEnergyNum(ushort count)
        {
            _energyCurrentNum += count;
            if (_energyCurrentNum < 0)
                _energyCurrentNum = 0;
        }

        /// <summary>
        /// 最大负重上线
        /// </summary>
        public ushort GoodsMaxNum;
        /// <summary>
        /// 当前最大负重
        /// </summary>
        private ushort _goodsCurrentNum;
        public ushort GoodsCurrentNum
        {
            get
            {
                return _goodsCurrentNum;
            }
        }

        public void ChangeCurrentGoods(ushort count)
        {
            _goodsCurrentNum += count;
            if (_goodsCurrentNum > GoodsMaxNum)
            {
                _goodsCurrentNum = GoodsMaxNum;
            }
            if (_goodsCurrentNum < 0)
                _goodsCurrentNum = 0;
        }

        public PlayerExploreTeamData(ushort energy,ushort goodsMax)
        {
            EnergyStartNum = energy;
            GoodsMaxNum = goodsMax;
        }

    }


    public class ExploreEventConfigData
    {
        public int eventID;
        public Trigger trigger;
        public Effect effect;

        public class Trigger
        {
            public Dictionary<string, bool> Global;
            public Player player;
            public List<int> PreEvent;

            public class Player
            {
                public string CurrencyCompare;
                public int CurrencyValue;
                public Dictionary<string, string> Material;
                public Dictionary<string, string> Technology;

            }
        }

        public class Effect
        {
            public Dictionary<string, string> SetFlag;
            public List<string> RemoveFlag;
        }
    }
}