using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class OrganizationModule : BaseModule<OrganizationModule>
    {
        public static List<OrganizationData> OrganizationDataList;
        public static Dictionary<int, OrganizationData> OrganizationDataDic;
        public static List<OrganizationEventData> OrganizationEventDataList;
        public static Dictionary<int, OrganizationEventData> OrganizationEventDataDic;


        public override void InitData()
        {
            OrganizationDataList = OrganizationMetaDataReader.OrganizationDataList;
            OrganizationDataDic = OrganizationMetaDataReader.OrganizationDataDic;
            OrganizationEventDataDic = OrganizationMetaDataReader.OrganizationEventDataDic;
            OrganizationEventDataList = OrganizationMetaDataReader.OrganizationEventDataList;
        }

        public override void Register()
        {
            
        }

        public OrganizationModule()
        {
            InitData();
        }

    }
}