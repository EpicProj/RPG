using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sim_FrameWork
{
    public class ExploreModule : BaseModule<ExploreModule>
    {
        public static List<ExploreArea> ExploreAreaList;
        public static Dictionary<int, ExploreArea> ExploreAreaDic;

        public static List<ExploreData> ExploreDataList;
        public static Dictionary<int, ExploreData> ExploreDataDic;

        public static List<ExploreEvent> ExploreEventList;
        public static Dictionary<int, ExploreEvent> ExploreEventDic;

        public static List<ExploreChoose> ExploreChooseList;
        public static Dictionary<int, ExploreChoose> ExploreChooseDic;

        public override void InitData()
        {
            ExploreAreaList = ExploreMetaDataReader.GetExploreAreaList();

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

        #region BaseData
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


        public static string GetEventName(int exploreID)
        {
            var data = GetExploreEventDataByKey(exploreID);
            return MultiLanguage.Instance.GetTextValue(data.Name);
        }
        public static string GetEventDesc(int exploreID)
        {
            var data = GetExploreEventDataByKey(exploreID);
            return MultiLanguage.Instance.GetTextValue(data.Desc);
        }

        public static string GetChooseContent(int chooseID)
        {
            return MultiLanguage.Instance.GetTextValue(GetExploreChooseDataByKey(chooseID).Content);
        }


        private static List<ExploreChoose> GetExploreChooseList(int exploreID)
        {
            List<ExploreChoose> result = new List<ExploreChoose>();
            var data = GetExploreEventDataByKey(exploreID);
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


        #endregion

    }

    public class ExploreChooseItem 
    {
        public int ChooseID;
        public int nextEvent;
        public string content;
        public UnityAction clickAction;

        public ExploreChooseItem(int chooseID,UnityAction clickActon=null)
        {
            var data = ExploreModule.GetExploreChooseDataByKey(chooseID);
            if (data != null)
            {
                ChooseID = chooseID;
                nextEvent = data.NextEvent;
                content = ExploreModule.GetChooseContent(chooseID);
            }
        }
    }


}