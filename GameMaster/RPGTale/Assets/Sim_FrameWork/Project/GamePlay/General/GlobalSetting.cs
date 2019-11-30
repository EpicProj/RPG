using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public class GlobalSetting 
    {
        public List<RarityConfig> rarityConfig;
        public GlobalSetting LoadGlobalSettting()
        {
            Config.JsonReader config = new Config.JsonReader();
            GlobalSetting settting = config.LoadJsonDataConfig<GlobalSetting>(Config.JsonConfigPath.GlobalSettingJsonPath);
            rarityConfig = settting.rarityConfig;

            return settting;
        }

        public class RarityConfig
        {
            public ushort Level;
            public string Color;
        }

     





    }
}