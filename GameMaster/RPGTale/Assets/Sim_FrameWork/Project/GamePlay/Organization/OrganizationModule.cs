using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class OrganizationModule : BaseModule<OrganizationModule>
    {
        protected static List<OrganizationData> OrganizationDataList;
        protected static Dictionary<int, OrganizationData> OrganizationDataDic;
        protected static List<OrganizationEventData> OrganizationEventDataList;
        protected static Dictionary<int, OrganizationEventData> OrganizationEventDataDic;


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



    public class OrganizationInfo
    {
        public string Name;
        public string Desc;




    }

}