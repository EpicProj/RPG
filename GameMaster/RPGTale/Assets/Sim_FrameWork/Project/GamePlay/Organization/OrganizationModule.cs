using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class OrganizationModule : BaseModule<OrganizationModule>
    {
        protected static List<OrganizationData> OrganizationDataList;
        protected static Dictionary<int, OrganizationData> OrganizationDataDic;
        protected static List<OrganizationTypeData> OrganizaitonTypeDataList;
        protected static Dictionary<string, OrganizationTypeData> OrganizaitonTypeDataDic;
        protected static List<OrganizationEventData> OrganizationEventDataList;
        protected static Dictionary<int, OrganizationEventData> OrganizationEventDataDic;


        public override void InitData()
        {
            OrganizationDataList = OrganizationMetaDataReader.GetOrganizationData();
            OrganizationDataDic = OrganizationMetaDataReader.GetOrganizationDataDic();
            OrganizaitonTypeDataList = OrganizationMetaDataReader.GetOrganizaitonTypeData();
            OrganizaitonTypeDataDic = OrganizationMetaDataReader.GetOrganizaitonTypeDataDic();
            OrganizationEventDataDic = OrganizationMetaDataReader.GetOrganizationEventDataDic();
            OrganizationEventDataList = OrganizationMetaDataReader.GetOrganizationEventData();
        }

        public override void Register()
        {
            
        }

        public OrganizationModule()
        {
            InitData();
        }

        public static OrganizationData GetOrganizationDataByID(int id)
        {
            OrganizationData data = null;
            OrganizationDataDic.TryGetValue(id, out data);
            if (data == null)
                Debug.LogError("Get Organization Data Error,ID=" + id);
            return data;
        }

        public static string GetOrganizationName(int id)
        {
            return MultiLanguage.Instance.GetTextValue(GetOrganizationDataByID(id).Name);
        }
        public static string GetOrganizationName(OrganizationData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.Name);
        }
        public static string GetOrganizationName_En(int id)
        {
            return MultiLanguage.Instance.GetTextValue(GetOrganizationDataByID(id).Name_En);
        }
        public static string GetOrganizationName_En(OrganizationData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.Name_En);
        }
        public static string GetOrganizationBriefDesc(int id)
        {
            return MultiLanguage.Instance.GetTextValue(GetOrganizationDataByID(id).BGDescBrief);
        }
        public static string GetOrganizationBriefDesc(OrganizationData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.BGDescBrief);
        }

        public static Sprite GetOrganizationSprite(int id)
        {
            return Utility.LoadSprite(GetOrganizationDataByID(id).Icon, Utility.SpriteType.png);
        }
        public static Sprite GetOrganizationSpriteBig(int id)
        {
            return Utility.LoadSprite(GetOrganizationDataByID(id).IconBig, Utility.SpriteType.png);
        }

        public static OrganizationTypeData GetOrganizaitonTypeDataByID(string id)
        {
            OrganizationTypeData data = null;
            OrganizaitonTypeDataDic.TryGetValue(id, out data);
            if (data == null)
                Debug.LogError("Get OrganizaitonTypeData Error!  id=" + id);
            return data;
        }
        public static OrganizationTypeData FetchOrganizationType(int id)
        {
            return GetOrganizaitonTypeDataByID(GetOrganizationDataByID(id).AreaType);
        }

        public static string GetTypeName(string id)
        {
            return MultiLanguage.Instance.GetTextValue(GetOrganizaitonTypeDataByID(id).Name);
        }
        public static string GetTypeName(int orID)
        {
            return MultiLanguage.Instance.GetTextValue(GetOrganizaitonTypeDataByID(GetOrganizationDataByID(orID).AreaType).Name);
        }
        public static string GetTypeDesc(string id)
        {
            return MultiLanguage.Instance.GetTextValue(GetOrganizaitonTypeDataByID(id).Desc);
        }
        public static string GetTypeDesc(int orID)
        {
            return MultiLanguage.Instance.GetTextValue(GetOrganizaitonTypeDataByID(GetOrganizationDataByID(orID).AreaType).Desc);
        }
        public static Sprite GetTypeIcon(string id)
        {
            return Utility.LoadSprite(GetOrganizaitonTypeDataByID(id).IconPath, Utility.SpriteType.png);
        }
        public static Sprite GetTypeIcon(int orID)
        {
            return Utility.LoadSprite(GetOrganizaitonTypeDataByID(GetOrganizationDataByID(orID).AreaType).IconPath, Utility.SpriteType.png);
        }

    }



    public class OrganizationInfo
    {
        public int ID;
        public OrganizationDataModel dataModel;

        public OrganizationInfo(int id)
        {
            var data = OrganizationModule.GetOrganizationDataByID(id);
            if (data == null)
                return;
            ID = data.ID;
            dataModel = new OrganizationDataModel();
            dataModel.Create(ID);
        }



    }

}