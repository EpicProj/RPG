using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public class GlobalSetting 
    {

        public class BasicSpriteConfig
        {

            public string PauseBtn_Sprite_Path;
            public string StartBtn_Sprite_Path;
        }

        public BasicSpriteConfig basicSpriteConfig;



        public GlobalSetting LoadGlobalSettting()
        {
            Config.JsonReader config = new Config.JsonReader();
            GlobalSetting settting = config.LoadGlobalSettingData();
            basicSpriteConfig = settting.basicSpriteConfig;

            return settting;
        }





    }
}