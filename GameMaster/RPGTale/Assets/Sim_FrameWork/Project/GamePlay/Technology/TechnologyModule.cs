using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class TechnologyModule : BaseModule<TechnologyModule>
    {
        public static List<Technology> TechnologyList;
        public static Dictionary<int, Technology> TechnologyDic;

        public TechGroupConfig config = new TechGroupConfig();

        public override void InitData()
        {
            TechnologyList = TechnologyMetaDataReader.GetTechnologyList();
            TechnologyDic = TechnologyMetaDataReader.GetTechnologyDic();
            config.LoadData();
        }
        public override void Register()
        {
        }

        public TechnologyModule()
        {
            InitData();
        }

        #region Data
        public static Technology GetTechDataByID(int techID)
        {
            Technology tech = null;
            TechnologyDic.TryGetValue(techID, out tech);
            if (tech == null)
            {
                Debug.LogError("Get TechData Error!  ID= " + techID);
            }
            return tech;
        }

        public static string GetTechName(int techID)
        {
            return MultiLanguage.Instance.GetTextValue(GetTechDataByID(techID).TechName);
        }
        public static string GetTechName(Technology tech)
        {
            return MultiLanguage.Instance.GetTextValue(tech.TechName);
        }
        public static string GetTechDesc(int techID)
        {
            return MultiLanguage.Instance.GetTextValue(GetTechDataByID(techID).TechDesc);
        }
        public static string GetTechDesc(Technology tech)
        {
            return MultiLanguage.Instance.GetTextValue(tech.TechDesc);
        }
        
        public static Sprite GetTechIcon(int techID)
        {
            return Utility.LoadSprite(GetTechDataByID(techID).TechIcon, Utility.SpriteType.png);
        }

        public TechGroupConfig.GroupConfig GetTechGroupConfig(int index)
        {
            if (config.configList.Count != 0)
            {
                var result= config.configList.Find(x => x.groupIndex == index);
                if (result != null)
                    return result;
            }
            return null;
        }

        #endregion



    }
}