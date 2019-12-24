using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ExploreEventManager : Singleton<ExploreEventManager>
    {
        public void InitData()
        {
            InitExploreArea(_currentExploreAreaType);
        }

        /// <summary>
        /// 当前所处探索区域
        /// </summary>
        public ExploreAreaType _currentExploreAreaType = ExploreAreaType.space;


        #region Area
        public List<int> _currentUnlockExploreAreaList = new List<int>();

        private List<ExploreAreaData> _currentExploreAreaList_Earth = new List<ExploreAreaData>();
        private List<ExploreAreaData> _currentExploreAreaList_Space = new List<ExploreAreaData>();
        public List<ExploreAreaData> CurrentExploreAreaList(ExploreAreaType type)
        {
            if (type == ExploreAreaType.earth)
                return _currentExploreAreaList_Earth;
            else if (type == ExploreAreaType.space)
                return _currentExploreAreaList_Space;
            else
                return null;
        }
        public ExploreAreaData GetExploreAreaData(int areaID, ExploreAreaType type)
        {
            if (type == ExploreAreaType.earth)
                return _currentExploreAreaList_Earth.Find(x => x.areaID == areaID);
            else if (type == ExploreAreaType.space)
                return _currentExploreAreaList_Space.Find(x => x.areaID == areaID);
            else
                return null;
        }


        /// <summary>
        /// 初始化区域任务
        /// </summary>
        private void InitExploreArea(ExploreAreaType areaType)
        {
            List<int> list = new List<int>();
            if (areaType == ExploreAreaType.earth)
            {
                list = ExploreModule.ExploreAreaListEarth;
                for (int i = 0; i < list.Count; i++)
                {
                    ExploreAreaData data = new ExploreAreaData(list[i]);
                    if (data.areaID != 0)
                    {
                        _currentExploreAreaList_Earth.Add(data);
                        ///Area Unlock
                        if (data.unlock == true)
                        {
                            data.GenerateRandomMission();
                            _currentUnlockExploreAreaList.Add(data.areaID);
                        }
                    }
                }
            }
            else if (areaType == ExploreAreaType.space)
            {
                list = ExploreModule.ExploreAreaListSpace;
                for (int i = 0; i < list.Count; i++)
                {
                    ExploreAreaData data = new ExploreAreaData(list[i]);
                    if (data.areaID != 0)
                    {
                        _currentExploreAreaList_Space.Add(data);
                        ///Area Unlock
                        if (data.unlock == true)
                        {
                            data.GenerateRandomMission();
                            _currentUnlockExploreAreaList.Add(data.areaID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 解锁区域
        /// </summary>
        /// <param name="areaID"></param>
        public void UnlockExploreArea(int areaID, ExploreAreaType type)
        {
            var data = ExploreModule.GetExploreAreaDataByKey(areaID);
            if (data != null)
            {
                if (!_currentUnlockExploreAreaList.Contains(areaID))
                {
                    _currentUnlockExploreAreaList.Add(areaID);
                    var areaData = GetExploreAreaData(areaID, type);
                    if (areaData != null)
                    {
                        areaData.unlock = true;
                        _currentUnlockExploreAreaList.Add(areaData.areaID);
                        areaData.GenerateRandomMission();
                    }
                }
            }
        }



        #endregion

        #region Mission / ExploreRandomItem

        /// <summary>
        /// Current Explore Misson 
        /// State    Key=AreaID
        /// </summary>

        public ExploreRandomItem GetExploreMission(int areaID, int exploreID)
        {
            var areaData = GetExploreAreaData(areaID, _currentExploreAreaType);
            if (areaData != null)
            {
                var list = areaData.currentMissionList;
                return list.Find(x => x.exploreID == exploreID);
            }
            return null;
        }

        public ExploreRandomItem GetFinishedExploreMission(int areaID,int exploreID)
        {
            var areaData = GetExploreAreaData(areaID, _currentExploreAreaType);
            if (areaData != null)
            {
                var list = areaData.finishedMissionList;
                return list.Find(x => x.exploreID == exploreID);
            }
            return null;
        }

        /// <summary>
        /// 检查任务是否存在
        /// </summary>
        /// <param name="areaID"></param>
        /// <param name="exploreID"></param>
        /// <returns></returns>
        public bool CheckMissionIsDoing(int areaID, int exploreID)
        {
            var item = GetExploreMission(areaID, exploreID);
            if (item != null)
            {
                if (item.currentState != ExploreRandomItem.ExploreMissionState.None && item.currentState != ExploreRandomItem.ExploreMissionState.Finish)
                    return true;
            }
            return false;
        }

        public bool CheckMissionIsFinish(int areaID, int exploreID)
        {
            var item = GetFinishedExploreMission(areaID, exploreID);
            if (item != null)
            {
                if (item.currentState == ExploreRandomItem.ExploreMissionState.Finish)
                    return true;
            }
            return false;
        }

        public bool CheckMissionExists(int areaID, int exploreID)
        {
            var item = GetExploreMission(areaID, exploreID);
            return item == null ? false : true;
        }

        /// <summary>
        /// 添加正在进行中的任务
        /// </summary>
        /// <param name="areaID"></param>
        /// <param name="item"></param>
        public void AddExploreMission(int areaID, ExploreRandomItem item)
        {
            var areaData = GetExploreAreaData(areaID,_currentExploreAreaType);
            if (areaData != null)
            {
                if (!CheckMissionExists(areaID,item.exploreID))
                {
                    areaData.currentMissionList.Add(item);
                }
            }
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="areaID"></param>
        /// <param name="exploreID"></param>
        public void StartExplore(int areaID , int exploreID, PlayerExploreTeamData teamData)
        {
            var missionData = GetExploreMission(areaID, exploreID);
            if (missionData != null)
            {
                if(missionData.currentState == ExploreRandomItem.ExploreMissionState.Init)
                {
                    missionData.currentState = ExploreRandomItem.ExploreMissionState.Doing;
                    missionData.teamData = teamData;
                    UIGuide.Instance.ShowExplorePointPage(missionData);
                }
            }
        }

        /// <summary>
        /// 区域探索完成
        /// </summary>
        /// <param name="data"></param>
        public void OnExploreMissionFinish(int areaID, int exploreID)
        {
            var areaData = GetExploreAreaData(areaID,_currentExploreAreaType);
            if (areaData != null)
            {
                var missionData = GetExploreMission(areaID, exploreID);
                if (missionData != null)
                {
                    missionData.currentState = ExploreRandomItem.ExploreMissionState.Finish;
                    areaData.finishedMissionList.Add(missionData);
                    areaData.currentMissionList.Remove(missionData);
                }
            }
        }



        #endregion

        #region Explore_Point

        public ExplorePointData GetExplorePointData(int areaID,int exploreID,int pointID)
        {
            var missionData = GetExploreMission(areaID, exploreID);
            if (missionData != null)
            {
                var list = missionData.currentPointlist;
                return list.Find(x => x.PointID == pointID);
            }
            return null;
        }

        public void AddExplorePointData(int areaID, int exploreID, ExplorePointData data)
        {
            var missionData = GetExploreMission(areaID, exploreID);
            if (GetExplorePointData(areaID, exploreID, data.PointID) == null && missionData!=null)
            {
                missionData.currentPointlist.Add(data);
            }
        }

        public ExplorePointData GetFinishedPointData(int areaID, int exploreID, int pointID)
        {
            var missionData = GetExploreMission(areaID, exploreID);
            var list = missionData.finishedPointList;
            return list.Find(x => x.PointID == pointID);
        }


        /// <summary>
        /// 开启一个点位
        /// </summary>
        /// <param name="pointID"></param>
        public void StartExplorePoint(int areaID, int exploreID, int pointID)
        {
            var data = GetExplorePointData(areaID,exploreID,pointID);
            if (data != null)
            {
                data.currentState = ExplorePointData.PointState.Doing;
                data.ExploreTimer = ApplicationManager.StartTimer(data.TimeCost, 100);
                data.ExploreTimer.Pause();
             
            }
        }

        /// <summary>
        /// 完成一个探索点位
        /// </summary>
        /// <param name="pointID"></param>
        public void FinishExplorePoint(int areaID, int exploreID, int pointID)
        {
            var data = GetExplorePointData(areaID,exploreID,pointID);
            if (data != null)
            {
                data.currentState = ExplorePointData.PointState.Finish;
                if(GetFinishedPointData(areaID,exploreID,pointID) == null)
                {
                    var exploreData = GetExploreMission(areaID, exploreID);
                    exploreData.finishedPointList.Add(data);
                    exploreData.currentPointlist.Remove(data);
                    
                    UIManager.Instance.SendMessage(new UIMessage(UIMsgType.ExplorePage_Finish_Point));
                }
            }
        }


        #endregion





    }
}