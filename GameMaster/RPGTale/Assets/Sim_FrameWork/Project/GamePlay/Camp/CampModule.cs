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
    }
}