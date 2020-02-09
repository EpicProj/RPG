using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{

    public class PlayerConfig 
    {
        public GamePrepareConfig gamePrepareConfig;
        public TimeDataConfig timeConfig;
        
        public static PlayerConfig LoadPlayerConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            var config = reader.LoadJsonDataConfig<PlayerConfig>(Config.JsonConfigPath.PlayerConfigJsonPath);
            return config;
        }

        public bool DataCheck()
        {
            bool result = true;
            if (gamePrepareConfig.prepareProperty == null)
            {
                DebugPlus.LogError("[GamePrepareConfig] : prepareProperty null!");
                return false;
            }

            for(int i = 0; i < gamePrepareConfig.prepareProperty.Count; i++)
            {
                var item = gamePrepareConfig.prepareProperty[i];
                for(int j = 0; j < item.levelMax; j++)
                {
                    GamePreapre_ConfigItem.ConfigLevelMap mapData = null;
                    mapData=item.levelMap.Find(x => x.Level == j + 1);
                    if (mapData == null)
                    {
                        DebugPlus.LogError("[GamePrepareConfig] : levelMap Empty!  configName=" + item.configID + " levelID=" + (j + 1).ToString());
                        result = false;
                        continue;
                    }
                }

                if(item.defaultSelectLevel<=0 || item.defaultSelectLevel > item.levelMax)
                {
                    DebugPlus.LogError("[GamePrepareConfig] : DefaultSelect Error!  configName=" + item.configID);
                }
            }

            return result;
        }

    }

    public class TimeDataConfig 
    {
        public int OriginalYear;
        public ushort OriginalMonth;
        public ushort OriginalDay;
        public float RealSecondsPerDay;

    }


    public class GamePrepareConfig
    {
        public int hardLevelBase;

        public int OriginalCurrencyMax;
        ///初始能量
        public int OriginalEnergy;
        public int OriginalEnergyMax;
        ///初始研究
        public int OriginalResearch;
        public int OriginalResearchMax;

        ///初始智核数量
        public ushort OriginalRoCore;
        public ushort OriginalRoCoreMax;

        /// <summary>
        /// 设置参数关联
        /// </summary>
        public string GamePrepareConfig_PropertyLink_BornPosition;  //出生地
        public string GamePrepareConfig_PropertyLink_ResourceRichness;  //资源丰富度

        public string GamePrepareConfig_PropertyLink_Currency;  //初始资金
        public int GamePrepareConfig_Currency_Default;

        public string GamePrepareConfig_PropertyLink_EnemyHardLevel;    //敌人强度
        public double GamePrepareConfig_EnemyHardLevel_Default;

        public string GamePrepareConfig_PropertyLink_Research_Coefficient;  //研究系数
        public double GamePrepareConfig_Research_Coefficient_Default;

        public List<GamePreapre_ConfigItem> prepareProperty;
    }

    public class GamePreapre_ConfigItem
    {
        public string configID;
        public string configNameText;
        public string configIconPath;
        /// <summary>
        /// 是否显示乘号
        /// </summary>
        public bool showScaleSymbol;
        /// <summary>
        /// 1 = DropDown
        /// 2 = Slider
        /// </summary>
        public byte configType;
        public byte levelMax;
        public byte defaultSelectLevel;
        public List<ConfigLevelMap> levelMap;
      

        public class ConfigLevelMap
        {
            public byte Level;
            public double numParam;
            public string strParam;
            public int hardLevelChange;
        }
    }

}