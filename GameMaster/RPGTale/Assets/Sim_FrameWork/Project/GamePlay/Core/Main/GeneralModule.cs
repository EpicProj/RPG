using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class GeneralModule : BaseModule<GeneralModule>
    {
        private Config.GlobalSetting globalSetting=new Config.GlobalSetting ();
        public override void InitData()
        {
            globalSetting.LoadGlobalSettting();
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
            var config = globalSetting.rarityConfig.Find(x => x.Level == rarityID);
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
}