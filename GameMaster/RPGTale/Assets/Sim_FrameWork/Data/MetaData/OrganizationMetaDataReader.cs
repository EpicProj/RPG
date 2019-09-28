using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class OrganizationMetaDataReader
    {
        public static List<OrganizationData> OrganizationDataList;
        public static Dictionary<int, OrganizationData> OrganizationDataDic;

        public static List<OrganizationTypeData> OrganizationTypeDataList;
        public static Dictionary<string, OrganizationTypeData> OrganizationTypeDataDic;

        public static List<OrganizationEventData> OrganizationEventDataList;
        public static Dictionary<int, OrganizationEventData> OrganizationEventDataDic;

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<OrganizationMetaData>(ConfigPath.TABLE_ORGANIZATION_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("OrganizationMetaData Read Error");
                return;
            }

            OrganizationDataList = config.AllOrganizationDataList;
            OrganizationDataDic = config.AllOrganizationDataDic;
            OrganizationTypeDataList = config.AllOrganizationTypeDataList;
            OrganizationTypeDataDic = config.AllOrganizationTypeDataDic;
            OrganizationEventDataList = config.AllOrganizationEventDataList;
            OrganizationEventDataDic = config.AllOrganizationEventDataDic;
        }

        public static List<OrganizationData> GetOrganizationData()
        {
            LoadData();
            return OrganizationDataList;
        }
        public static Dictionary<int, OrganizationData> GetOrganizationDataDic()
        {
            LoadData();
            return OrganizationDataDic;
        }
        public static List<OrganizationTypeData> GetOrganizaitonTypeData()
        {
            LoadData();
            return OrganizationTypeDataList;
        }
        public static Dictionary<string, OrganizationTypeData> GetOrganizaitonTypeDataDic()
        {
            LoadData();
            return OrganizationTypeDataDic;
        }
        public static List<OrganizationEventData> GetOrganizationEventData()
        {
            LoadData();
            return OrganizationEventDataList;
        }
        public static Dictionary<int, OrganizationEventData> GetOrganizationEventDataDic()
        {
            LoadData();
            return OrganizationEventDataDic;
        }
    }
}