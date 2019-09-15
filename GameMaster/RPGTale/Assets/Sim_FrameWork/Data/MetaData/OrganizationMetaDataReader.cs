using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class OrganizationMetaDataReader
    {
        public static List<OrganizationData> OrganizationDataList;
        public static Dictionary<int, OrganizationData> OrganizationDataDic;

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