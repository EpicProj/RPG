using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public class GlobalSetting 
    {
        public List<RarityConfig> rarityConfig;
        public string Resource_Research_Icon_Path;
        public string Resource_Currency_Icon_Path;
        public string Resource_Energy_Icon_Path;
        public string Resource_Builder_Icon_Path;
        public string Resource_Rocore_Icon_Path;


        public GlobalSetting LoadGlobalSettting()
        {
            Config.JsonReader config = new Config.JsonReader();
            GlobalSetting settting = config.LoadJsonDataConfig<GlobalSetting>(Config.JsonConfigPath.GlobalSettingJsonPath);
            rarityConfig = settting.rarityConfig;
            Resource_Research_Icon_Path = settting.Resource_Research_Icon_Path;
            Resource_Currency_Icon_Path = settting.Resource_Currency_Icon_Path;
            Resource_Energy_Icon_Path = settting.Resource_Energy_Icon_Path;
            Resource_Builder_Icon_Path = settting.Resource_Builder_Icon_Path;
            Resource_Rocore_Icon_Path = settting.Resource_Rocore_Icon_Path;
            return settting;
        }

        public class RarityConfig
        {
            public ushort Level;
            public string Color;
        }

     





    }
}