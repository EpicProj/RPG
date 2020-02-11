using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sim_FrameWork
{
    public class CampInfo
    {
        public int CampID;
        public string campName;
        public string campDesc;

        public string campIconPath;
        public string campBGSmallPath;

        public ushort hardLevel;
        public ushort defualtHardLevelValue;

        public List<CampAttributeInfo> attributeInfo;
        public CampCreedInfo creedInfo;

        /// <summary>
        /// Camp Leader
        /// </summary>
        public List<LeaderInfo> campLeaderList=new List<LeaderInfo> ();

        public static CampInfo InitInfo(int campID)
        {
            CampInfo info = new CampInfo();
            var _meta = CampModule.GetCampDataByKey(campID);
            if (_meta != null)
            {
                info.CampID = _meta.CampID;
                info.campName = MultiLanguage.Instance.GetTextValue(_meta.CampName);
                info.campDesc = MultiLanguage.Instance.GetTextValue(_meta.CampDesc);

                info.campIconPath = _meta.CampIcon;
                info.campBGSmallPath = _meta.CampBGSmall;

                info.hardLevel = _meta.HardLevel;
                info.defualtHardLevelValue = _meta.DefaultHardLevel;

                info.creedInfo = CampCreedInfo.InitInfo(_meta.CreedID);
                info.attributeInfo = CampModule.GetCampAttribueInfoList(_meta.CampID);

                ///Init DefaultLeader
                info.campLeaderList = CampModule.GetCampDefaultLeaderList(_meta.CampID);

                return info;
            }
            DebugPlus.LogError("[CampInfo] : Init Fail! campID=" + campID);
            return null;
        }
    }

    public class CampAttributeInfo
    {
        public int attributeID;
        public string attributeName;
        public string attributeDesc;
        public string iconPath;

        public static CampAttributeInfo InitInfo(int attributeID)
        {
            CampAttributeInfo info = new CampAttributeInfo();
            var meta = CampModule.GetCampAttributeDataByKey(attributeID);
            if (meta != null)
            {
                info.attributeID = meta.AttributeID;
                info.attributeName = MultiLanguage.Instance.GetTextValue(meta.Name);
                info.attributeDesc = MultiLanguage.Instance.GetTextValue(meta.Desc);
                info.iconPath = meta.Icon;
                return info;
            }
            DebugPlus.LogError("[CampAttributeInfo] : Init Fail!  attributeID=" + attributeID);
            return null;
        }
    }

    public class CampCreedInfo
    {
        public int creedID;
        public string creedName;
        public string creedDesc;
        public string creedIconPath;

        public static CampCreedInfo InitInfo(int creedID)
        {
            CampCreedInfo info = new CampCreedInfo();
            var meta = CampModule.GetCampCreedDataByKey(creedID);
            if (meta != null)
            {
                info.creedID = creedID;
                info.creedName = MultiLanguage.Instance.GetTextValue(meta.CreedName);
                info.creedDesc = MultiLanguage.Instance.GetTextValue(meta.CreedDesc);
                info.creedIconPath = meta.CreedIcon;
                return info;
            }
            DebugPlus.LogError("[CampCreedInfo] : Init Fail! creedID=" + creedID);
            return null;
        }
    }


    public struct CampBaseModel : BaseDataModel
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool Create(int campID)
        {
            if (CampModule.GetCampDataByKey(campID) == null)
                return false;
            _id = campID;
            return true;
        }

        private CampInfo _campInfo;
        public CampInfo CampInfo
        {
            get
            {
                if (_campInfo == null)
                    _campInfo = GameManager.Instance.GetCampInfoData(_id);
                return _campInfo;
            }
        }

    }

}