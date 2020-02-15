using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class CampModule : BaseModule<CampModule>
    {
        private static Dictionary<int, CampData> campDataDic;
        private static Dictionary<int, CampCreedData> campCreedDataDic;
        private static Dictionary<int, CampAttributeData> campAttributeDataDic;

        public override void InitData()
        {
            var config = ConfigManager.Instance.LoadData<CampMetaData>(ConfigPath.TABLE_CAMP_METADATA_PATH);
            if (config == null)
            {
                DebugPlus.LogError("CampMetaData Read Error");
                return;
            }
            campDataDic = config.AllCampDataDic;
            campCreedDataDic = config.AllCampCreedDataDic;
            campAttributeDataDic = config.AllCampAttributeDataDic;
        }

        public override void Register()
        {
            
        }
        public CampModule()
        {
            InitData();
        }

        public static CampData GetCampDataByKey(int campID)
        {
            CampData data = null;
            campDataDic.TryGetValue(campID, out data);
            if (data == null)
                DebugPlus.LogError("GetCampData null!  campID=" + campID);
            return data;
        }

        public static CampCreedData GetCampCreedDataByKey(int creedID)
        {
            CampCreedData data = null;
            campCreedDataDic.TryGetValue(creedID, out data);
            if(data==null)
                DebugPlus.LogError(" GetCampCreedData null!  creedID=" + creedID);
            return data;
        }
        public static CampAttributeData GetCampAttributeDataByKey(int attributeID)
        {
            CampAttributeData data = null;
            campAttributeDataDic.TryGetValue(attributeID, out data);
            if (data == null)
                DebugPlus.LogError("Get CampAttributeData null!  attributeID=" + attributeID);
            return data;
        }

        public static List<CampAttributeInfo> GetCampAttribueInfoList(int campID)
        {
            List<CampAttributeInfo> result = new List<CampAttributeInfo>();
            var meta = GetCampDataByKey(campID);
            if (meta != null)
            {
                var list = Utility.TryParseIntList(meta.AttributeIDList, ',');
                for(int i = 0; i < list.Count; i++)
                {
                    CampAttributeInfo info = CampAttributeInfo.InitInfo(list[i]);
                    if (info != null)
                        result.Add(info);
                }
            }
            return result;
        }

        public static List<CampInfo> GetAllCampInfo()
        {
            List<CampInfo> result = new List<CampInfo>();
            foreach(var item in campDataDic.Values)
            {
                if (item.CampType == 1)
                {
                    CampInfo campInfo = CampInfo.InitInfo(item.CampID);
                    result.Add(campInfo);
                }
            }
            return result;
        }

        public static List<BaseDataModel> GetCampInfoModel()
        {
            List<BaseDataModel> result = new List<BaseDataModel>();
            var list = GetAllCampInfo();
            for(int i = 0; i < list.Count; i++)
            {
                CampBaseModel model = new CampBaseModel();
                if (model.Create(list[i].CampID))
                {
                    result.Add((BaseDataModel)model);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取阵营初始领袖
        /// </summary>
        /// <param name="campID"></param>
        /// <returns></returns>
        public static List<LeaderInfo> GetCampDefaultLeaderList(int campID)
        {
            List<LeaderInfo> result = new List<LeaderInfo>();
            var campData = GetCampDataByKey(campID);
            if (campData != null)
            {
                var leaderIDList = Utility.TryParseIntList(campData.LeaderPresetList, ',');
                if(leaderIDList.Count==0 || leaderIDList.Count > Config.GlobalConfigData.GamePrepare_Crew_Leader_Max)
                {
                    DebugPlus.LogError("[CampData] : DefaultCrewLeader Member Error! campID = " + campID);
                    return result;
                }
                for(int i = 0; i < leaderIDList.Count; i++)
                {
                    LeaderInfo info = LeaderInfo.CreateLeaderInfo_Preset(leaderIDList[i]);
                    if (info != null)
                    {
                        info.forceSelcet = true;
                        result.Add(info);
                    }
                }
            }
            return result;
        }

    }
}