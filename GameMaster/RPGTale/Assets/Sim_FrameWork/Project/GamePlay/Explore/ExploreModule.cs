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
        }

        public override void Register()
        {
        }

        public ExploreModule()
        {
            InitData();
        }

        #region Explore Area
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
        public static string GetExploreAreaDesc(int areaID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExploreAreaDataByKey(areaID).Desc);
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

        /// <summary>
        /// 随机生成探索区域
        /// </summary>
        /// <param name="areaID"></param>
        public ExploreAreaData GenerateExploreAreaData(int areaID)
        {
            ExploreAreaData data = new ExploreAreaData(areaID);
            return data;
        }


        public static ExploreRandomItem GetRandomArea(int areaID)
        {
            List<ExploreRandomItem> tempList = new List<ExploreRandomItem>();
            var list = Utility.TryParseIntList(GetExploreAreaDataByKey(areaID).ExploreList,',');
            for(int i = 0; i < list.Count; i++)
            {
                var exploreData = GetExploreDataByKey(list[i]);
                if (exploreData != null)
                {
                    ExploreRandomItem item = new ExploreRandomItem(exploreData.ExploreID);
                    tempList.Add(item);
                }
            }
            if (tempList.Count >= 1)
            {
                var result = Utility.GetRandomList<ExploreRandomItem>(tempList, 1);
                return result[0];
            }
            return null;
        }

        public static List<ExploreAreaData> GetTotalExploreAreaData(ExploreAreaType areaType)
        {
            List<ExploreAreaData> result = new List<ExploreAreaData>();
            List<int> areaList = new List<int>();
            if(areaType == ExploreAreaType.earth)
            {
                areaList = Config.ConfigData.GlobalSetting.exploreArea_Earth;
             
            }else if (areaType == ExploreAreaType.space)
            {
                areaList= Config.ConfigData.GlobalSetting.exploreArea_Space;
            }

            for (int i = 0; i < areaList.Count; i++)
            {
                if (GetExploreAreaDataByKey(areaList[i]) != null)
                {
                    ExploreAreaData data = new ExploreAreaData(areaList[i]);
                    result.Add(data);
                }
            }

            return result;
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

        public static List<ExplorePointData> GetExplorePointDataList(int exploreID)
        {
            List<ExplorePointData> result = new List<ExplorePointData>();
            var exploreData = GetExploreDataByKey(exploreID);
            if (exploreData != null)
            {
                foreach (var point in ExplorePointDic.Values)
                {
                    if (point.SeriesID == exploreData.SeriesID)
                    {
                        ExplorePointData data = new ExplorePointData(point.PointID);
                        result.Add(data);
                    }
                }
            }
            return result;
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

        public ExploreRandomItem itemData;
        public string areaName;
        public string areaDesc;

        /// <summary>
        /// 解锁状态
        /// </summary>
        public bool unlock;

        /// <summary>
        /// 探索点位信息
        /// </summary>
        public List<ExplorePointData> pointList=new List<ExplorePointData> ();

        public ExploreAreaData(int areaID)
        {
            var data = ExploreModule.GetExploreAreaDataByKey(areaID);
            if (data != null)
            {
                areaID = data.AreaID;
                itemData = ExploreModule.GetRandomArea(areaID);
                areaName = ExploreModule.GetExploreAreaName(areaID);
                areaDesc = ExploreModule.GetExploreAreaDesc(areaID);
                unlock = data.Unlock;
                pointList = ExploreModule.GetExplorePointDataList(itemData.exploreID);
            }
        }

        public void AddCurrentDepth()
        {
            _currentDepth++;
        }
  
    }

    public class ExploreRandomItem : RandomObject
    {
        /// <summary>
        /// 随机到的探索ID
        /// </summary>
        public int exploreID;
        /// <summary>
        /// 前置探索ID
        /// </summary>
        public int requirePreExploreID;

        public List<ExplorePointData> pointList=new List<ExplorePointData> ();

        public bool finish = false;

        /// <summary>
        /// 难度等级
        /// </summary>
        public ushort areaHardLevel;
        public ushort Depth;


        public ExploreRandomItem(int exploreID)
        {
            var exploreData = ExploreModule.GetExploreDataByKey(exploreID);
            if (exploreData != null)
            {
                this.exploreID = exploreData.ExploreID;
                requirePreExploreID = exploreData.RequirePreID;
                Weight = exploreData.Weight;
            }
        }


    }


    /// <summary>
    /// 探索点位信息
    /// </summary>
    public class ExplorePointData
    {
        public int seriesID;
        public string pointName;
        public string pointDesc;

        /// <summary>
        /// 点位层级
        /// </summary>
        public ushort depthLevel;

        public bool unlock = false;

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


        public ExplorePointData(int pointID)
        {
            var data = ExploreModule.GetExplorePointDataByKey(pointID);
            if (data != null)
            {
                seriesID = data.SeriesID;
                PrePoint = data.PrePoint;
                pointName = ExploreModule.GetExplorePointName(pointID);
                pointDesc = ExploreModule.GetExplorePointDesc(pointID);
                depthLevel = data.DepthLevel;
                EnergyCost = data.EnergyCost;
            }
        }
    }

   


}