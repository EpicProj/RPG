using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class GeneralModule : BaseModule<GeneralModule>
    {
        private static Config.GlobalSetting _globalSetting=new Config.GlobalSetting ();
        public static Config.GlobalSetting GlobalSetting
        {
            get
            {
                if (_globalSetting == null)
                    _globalSetting.LoadGlobalSettting();
                return _globalSetting;
            }
        }

        public RewardData rewardData;

        public override void InitData()
        {
            _globalSetting.LoadGlobalSettting();
            LoadRewardData();
        }
        public override void Register()
        {
        }

        public GeneralModule()
        {
            InitData();
        }

        public void LoadRewardData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            var data= reader.LoadJsonDataConfig<RewardData>(Config.JsonConfigPath.RewardDataJsonPath);
            rewardData.rewardGroup = data.rewardGroup;
        }



        /// <summary>
        /// Get Rarity Data
        /// </summary>
        /// <param name="rarityID"></param>
        /// <returns></returns>
        public GeneralRarity GetRarity(int rarityID)
        {
            var config = _globalSetting.rarityConfig.Find(x => x.Level == rarityID);
            if (config != null)
            {
                var color = TryParseRarityColor(config.Color);
                GeneralRarity rarity = new GeneralRarity(config.Level, color);
                return rarity;
            }
            else
            {
                Debug.LogError("Rarity Error! ID=" + rarityID);
            }
            return null;
        }


        private Color TryParseRarityColor(string color)
        {
            Color result = new Color();
            ColorUtility.TryParseHtmlString(color, out result);
            if (result == null)
            {
                Debug.LogError("Parse Color Error! color=" + color);
            }
            return result;
        }


    }

    /// <summary>
    /// 通用稀有度
    /// </summary>
    public class GeneralRarity
    {
        public ushort rarityLevel;
        public Color color;

        public GeneralRarity(ushort level,Color color)
        {
            this.rarityLevel = level;
            this.color = color;
        }
    }

    /// <summary>
    /// Loading 界面
    /// </summary>
    public class LoadingPageItem
    {
        public Image BG;
        public string Title;
        public string Content;

        public LoadingPageItem(int itemID)
        {

        }

    }

    /// <summary>
    /// Reward Item
    /// </summary>
    public class GeneralRewardItem
    {
        public enum RewardType
        {
            None,
            /// <summary>
            /// 虹石
            /// </summary>
            Currency,
            /// <summary>
            /// 科技点
            /// </summary>
            TechPoints,
            Tech_Unlock,
            /// <summary>
            /// 材料
            /// </summary>
            Material,

        }

        public ushort type;
        public int ItemID;
        public int count;

    }

    public class RewardData
    {
        public List<RewardGroup> rewardGroup;

        public class RewardGroup
        {
            public int GroupID;
            public List<GeneralRewardItem> itemList;
        }
       
    }

}