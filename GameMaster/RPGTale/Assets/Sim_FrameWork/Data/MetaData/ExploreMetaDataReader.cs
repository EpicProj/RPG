using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class ExploreMetaDataReader 
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
        public static Dictionary<int,ExploreChoose> ExploreChooseDic;

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<ExploreMetaData>(ConfigPath.TABLE_EXPLORE_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("ExploreMetaData Read Error");
                return;
            }

            ExploreAreaList = config.AllExploreAreaList;
            ExploreAreaDic = config.AllExploreAreaDic;
            ExploreDataList = config.AllExploreDataList;
            ExploreDataDic = config.AllExploreDataDic;
            ExplorePointList = config.AllExplorePointList;
            ExplorePointDic = config.AllExplorePointDic;
            ExploreEventList = config.AllExploreEventList;
            ExploreEventDic = config.AllExploreEventDic;
            ExploreChooseList = config.AllExploreChooseList;
            ExploreChooseDic = config.AllExploreChooseDic;
        }

        public static List<ExploreArea> GetExploreAreaList()
        {
            LoadData();
            return ExploreAreaList;
        }
        public static Dictionary<int,ExploreArea> GetExploreAreaDic()
        {
            LoadData();
            return ExploreAreaDic;
        }

        public static List<ExploreData> GetExploreDataList()
        {
            LoadData();
            return ExploreDataList;
        }

        public static Dictionary<int,ExploreData> GetExploreDataDic()
        {
            LoadData();
            return ExploreDataDic;
        }
        public static List<ExplorePoint> GetExplorePointList()
        {
            LoadData();
            return ExplorePointList;
        }

        public static Dictionary<int,ExplorePoint> GetExplorePointDic()
        {
            LoadData();
            return ExplorePointDic;
        }
        public static List<ExploreEvent> GetExploreEventList()
        {
            LoadData();
            return ExploreEventList;
        }
        public static Dictionary<int, ExploreEvent> GetExploreEventDic()
        {
            LoadData();
            return ExploreEventDic;
        }


        public static List<ExploreChoose> GetExploreChooseList()
        {
            LoadData();
            return ExploreChooseList;
        }
        public static Dictionary<int,ExploreChoose> GetExploreChooseDic()
        {
            LoadData();
            return ExploreChooseDic;
        }



    }
}