using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class GeneralModule : BaseModule<GeneralModule>
    {
  
        public override void InitData()
        {
        }
        public override void Register()
        {
        }

        public GeneralModule()
        {
            InitData();
        }

 

        /// <summary>
        /// Get Rarity Data
        /// </summary>
        /// <param name="rarityID"></param>
        /// <returns></returns>
        public GeneralRarity GetRarity(int rarityID)
        {
            var config = Config.ConfigData.GlobalSetting.rarityConfig.Find(x => x.Level == rarityID);
            if (config != null)
            {
                var color = TryParseColor(config.Color);
                GeneralRarity rarity = new GeneralRarity(config.Level, color);
                return rarity;
            }
            else
            {
                Debug.LogError("Rarity Error! ID=" + rarityID);
            }
            return null;
        }


        public static Color TryParseColor(string color)
        {
            Color result = new Color();
            ColorUtility.TryParseHtmlString(color, out result);
            if (result == null)
            {
                Debug.LogError("Parse Color Error! color=" + color);
            }
            return result;
        }


        public static List<GeneralRewardItem> GetRewardItem(int groupID)
        {
            List<GeneralRewardItem> result = new List<GeneralRewardItem>();
            var group = Config.ConfigData.RewardData.rewardGroup.Find(x => x.GroupID == groupID);
            if(group != null)
            {
                for(int i = 0; i < group.itemList.Count; i++)
                {
                    GeneralRewardItem item = new GeneralRewardItem(
                        (GeneralRewardItem.RewardType)group.itemList[i].type,
                        group.itemList[i].ItemID, 
                        group.itemList[i].count);
                    result.Add(item);
                }
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
            /// 虹石
            Currency,
            /// Ro_Core
            Ro_Core,
            /// 科技点
            TechPoints,
            Tech_Unlock,
            /// 材料
            Material,
        }

        public RewardType type;
        public int ItemID;
        public int count;

        public GeneralRewardItem(RewardType type,int itemID,int count)
        {
            this.type = type;
            this.ItemID = itemID;
            this.count = count;
        }

    }

}