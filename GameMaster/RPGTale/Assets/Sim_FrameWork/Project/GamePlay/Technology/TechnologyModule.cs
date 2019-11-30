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

        public TechGroupConfig.GroupConfig GetTechGroupConfig(int index)
        {
            if (config.configList.Count != 0)
            {
                var result = config.configList.Find(x => x.groupIndex == index);
                if (result != null)
                    return result;
            }
            return null;
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

        public static GeneralRarity GetTechRarity(int techID)
        {
            return GeneralModule.Instance.GetRarity(GetTechDataByID(techID).Rarity);
        }

        public static TechnologyInfo.TechType GetTechBaseType(int techID)
        {
            var data = GetTechDataByID(techID);
            switch (data.BaseType)
            {
                case 1:
                    return TechnologyInfo.TechType.Unique;
                case 2:
                    return TechnologyInfo.TechType.Series;
                default:
                    return TechnologyInfo.TechType.Unique;

            }
        }

        #endregion

        /// <summary>
        /// 获取所有被用到的科技ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllTech()
        {
            List<int> result = new List<int>();
            for(int i = 0; i < config.InitGroupIndexList.Count; i++)
            {
                var group = GetTechGroupConfig(config.InitGroupIndexList[i]);
                for(int j = 0; j < group.techElementList.Count; j++)
                {
                    if (!result.Contains(group.techElementList[j].TechID))
                    {
                        result.Add(group.techElementList[j].TechID);
                    }
                }
            }
            return result;
        }

        public static TechCompleteEffect GetTechCompleteEffect(int techID)
        {
            var data = GetTechDataByID(techID);
            if (data != null)
            {
                switch (data.TechEffect)
                {
                    case 1:
                        return TechCompleteEffect.Unlock_Block;
                    case 2:
                        return TechCompleteEffect.Unlock_Tech;
                }
            }
            return TechCompleteEffect.None;
        }
        
        public static List<int> ParseTechParam_Unlock_Block(int techID)
        {
            List<int> result = new List<int>();
            TechCompleteEffect effect = GetTechCompleteEffect(techID);
            if (effect == TechCompleteEffect.Unlock_Block)
            {
                var str = GetTechDataByID(techID).EffectParam;
                var list = Utility.TryParseIntList(str, ',');
                for(int i = 0; i < list.Count; i++)
                {
                    if (FunctionBlockModule.GetFunctionBlockByBlockID(list[i]) != null)
                    {
                        result.Add(list[i]);
                    }
                }
            }
            return result;
        }

        public static List<int> ParseTechParam_Unlock_Tech(int techID)
        {
            List<int> result = new List<int>();
            TechCompleteEffect effect = GetTechCompleteEffect(techID);
            if (effect == TechCompleteEffect.Unlock_Tech)
            {
                var str = GetTechDataByID(techID).EffectParam;
                var list = Utility.TryParseIntList(str, ',');
                for (int i = 0; i < list.Count; i++)
                {
                    if (GetTechDataByID(list[i]) != null)
                    {
                        result.Add(list[i]);
                    }
                }
            }
            return result;
        }

    }

    public enum TechCompleteEffect
    {
        None,
        Unlock_Tech,
        Unlock_Block,
    }

}