﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class CampModule : BaseModule<CampModule> {

        public static List<CampData> CampDataList;

        public static CampBaseConfig campConfig;

        public override void InitData()
        {
            campConfig = new CampBaseConfig();
            campConfig.LoadData();
            CampDataList = CampMetaDataReader.GetCampData();
        }




        public class CampBaseConfig
        {
            public float minValue;
            public float maxValue;
            public float Player_OriginValue_Default;

            public void LoadData()
            {
                Config.JsonReader reader = new Config.JsonReader();
                CampBaseConfig config = reader.LoadJsonDataConfig<CampBaseConfig>(Config.JsonConfigPath.CampBaseConfigJsonPath);
                minValue = config.minValue;
                maxValue = config.maxValue;
                Player_OriginValue_Default = config.Player_OriginValue_Default;
            }



        }

    }
}