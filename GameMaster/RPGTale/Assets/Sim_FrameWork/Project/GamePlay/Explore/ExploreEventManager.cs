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


        /// <summary>
        /// Current Explore Misson 
        /// State    Key=AreaID
        /// </summary>
        private Dictionary<int, List<ExploreRandomItem>> _currentExploreMissionDic = new Dictionary<int, List<ExploreRandomItem>>();
        public Dictionary<int, List<ExploreRandomItem>> CurrentExploreMissionDic
        {
            get { return _currentExploreMissionDic; }
        }

        public ExploreRandomItem GetExploreMission(int areaID,int exploreID)
        {
            List<ExploreRandomItem> itemList = new List<ExploreRandomItem>();
            _currentExploreMissionDic.TryGetValue(areaID, out itemList);
            if (itemList != null)
            {
                return itemList.Find(x => x.exploreID == exploreID);
            }
            return null;
        }

        /// <summary>
        /// Finished MissionList
        /// </summary>
        private List<ExploreRandomItem> _currentFinishedExploreList = new List<ExploreRandomItem>();

        public List<ExploreRandomItem> CurrentFinishedExploreList
        {
            get { return _currentFinishedExploreList; }
        }

        public ExploreRandomItem GetFinishedExploreRandomItem(int exploreID)
        {
            return _currentFinishedExploreList.Find(x => x.exploreID == exploreID);
        }

      

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
                            _currentUnlockExploreAreaList.Add(data.areaID);
                            data.GenerateRandomMission();
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
                            _currentUnlockExploreAreaList.Add(data.areaID);
                            data.GenerateRandomMission();
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

        /// <summary>
        /// 检查任务是否已经存在或完成
        /// </summary>
        /// <param name="areaID"></param>
        /// <param name="exploreID"></param>
        /// <returns></returns>
        public bool CheckMissionIsDoing(int areaID, int exploreID)
        {
            List<ExploreRandomItem> list = new List<ExploreRandomItem>();
            _currentExploreMissionDic.TryGetValue(areaID, out list);
            if (list==null)
                return false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].exploreID == exploreID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckMissionIsFinish(int exploreID)
        {
            if (GetFinishedExploreRandomItem(exploreID) == null)
                return false;
            return true;
        }

        /// <summary>
        /// 添加正在进行中的任务
        /// </summary>
        /// <param name="areaID"></param>
        /// <param name="item"></param>
        public void AddExploreMission(int areaID, ExploreRandomItem item)
        {
            if (_currentExploreMissionDic.ContainsKey(areaID))
            {
                if (!_currentExploreMissionDic[areaID].Contains(item))
                {
                    item.currentState = ExploreRandomItem.ExploreMissionState.Start;
                    _currentExploreMissionDic[areaID].Add(item);
                }
            }
            else
            {
                List<ExploreRandomItem> itemList = new List<ExploreRandomItem>();
                item.currentState = ExploreRandomItem.ExploreMissionState.Start;
                itemList.Add(item);
                _currentExploreMissionDic.Add(areaID, itemList);
            }
        }

        public void RemoveExploreDoingMission(int areaID, int exploreID)
        {
            if (_currentExploreMissionDic.ContainsKey(areaID))
            {
                var list = _currentExploreMissionDic[areaID];
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].exploreID == exploreID)
                    {
                        list.RemoveAt(i);
                    }
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
                if(missionData.currentState == ExploreRandomItem.ExploreMissionState.Start)
                {
                    missionData.currentState = ExploreRandomItem.ExploreMissionState.Doing;
                    missionData.teamData = teamData;

                    UIGuide.Instance.ShowExplorePointPage(missionData);

                }
            }
        }


        #region Explore_Point

        private Dictionary<int, ExplorePointData> _currenPointDataDic = new Dictionary<int, ExplorePointData>();
        public Dictionary<int,ExplorePointData> CurrenPointDataDic
        {
            get { return _currenPointDataDic; }
        }

        public void AddExplorePointData(ExplorePointData data)
        {
            if (!_currenPointDataDic.ContainsKey(data.PointID))
            {
                _currenPointDataDic.Add(data.PointID, data);
            }
        }

        public ExplorePointData GetExplorePointData(int  pointID)
        {
            ExplorePointData data = null;
            _currenPointDataDic.TryGetValue(pointID, out data);
            return data;

        }


        /// <summary>
        /// 开启一个点位
        /// </summary>
        /// <param name="pointID"></param>
        public void StartExplorePoint(int pointID)
        {
            var data = GetExplorePointData(pointID);
            if (data != null)
            {
                data.currentState = ExplorePointData.PointState.Doing;
             
            }

        }

        #endregion

        /// <summary>
        /// 区域探索完成
        /// </summary>
        /// <param name="data"></param>
        public void OnExploreAreaFinish(int areaID, ExploreRandomItem explore)
        {
            if (!_currentFinishedExploreList.Contains(explore))
            {
                explore.currentState = ExploreRandomItem.ExploreMissionState.Finish;
                _currentFinishedExploreList.Add(explore);
                RemoveExploreDoingMission(areaID, explore.exploreID);
            }
        }



    }
}