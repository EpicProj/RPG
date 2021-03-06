﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public class LeaderConfig 
    {

    }



    public class LeaderPortraitConfig
    {
        public List<LeaderPortraitItemConfig> portrait_bg;
        public List<LeaderPortraitItemConfig> portrait_face;
        public List<LeaderPortraitItemConfig> portrait_hair;
        public List<LeaderPortraitItemConfig> portrait_cloth;
        public List<LeaderPortraitItemConfig> portrait_eyes;
        public List<LeaderPortraitItemConfig> portrait_Ear;
        public List<LeaderPortraitItemConfig> portrait_Mouth;
        public List<LeaderPortraitItemConfig> portrait_Nose;

        public static LeaderPortraitConfig LoadData()
        {
            JsonReader config = new JsonReader();
            LeaderPortraitConfig settting = config.LoadJsonDataConfig<LeaderPortraitConfig>(JsonConfigPath.LeaderPortraitConfigJsonPath);
            return settting;
        }
    }

    public class LeaderPortraitItemConfig
    {
        public int configID;
        public List<string> configGroupIDList;
        public string spritePath;
    }

    /// <summary>
    /// Name Config
    /// </summary>
    public class LeaderNameConfig
    {
        public List<LeaderNameConfigItem> config;

        public static LeaderNameConfig LoadData()
        {
            JsonReader config = new JsonReader();
            LeaderNameConfig setting = config.LoadJsonDataConfig<LeaderNameConfig>(JsonConfigPath.LeaderNameConfigJsonPath);
            return setting;
        }

    }
    public class LeaderNameConfigItem
    {
        public string configGroupID;
        public List<string> nameTextID;
    }

}