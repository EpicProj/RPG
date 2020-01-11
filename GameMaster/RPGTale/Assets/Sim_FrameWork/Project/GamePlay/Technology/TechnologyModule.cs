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
        public TechnologyConfigCommon configCommon = new TechnologyConfigCommon();

        public override void InitData()
        {
            TechnologyList = TechnologyMetaDataReader.GetTechnologyList();
            TechnologyDic = TechnologyMetaDataReader.GetTechnologyDic();
            config.LoadData();
            configCommon.LoadData();
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

        /// <summary>
        /// 获取科技所在的GroupID
        /// </summary>
        /// <param name="techID"></param>
        /// <returns></returns>
        public int GetTechGroupIndex(int techID)
        {
            for(int i = 0; i < config.InitGroupIndexList.Count; i++)
            {
                var group = GetTechGroupConfig(config.InitGroupIndexList[i]);
                for(int j = 0; j < group.techElementList.Count; j++)
                {
                    if (group.techElementList[j].TechID == techID)
                        return group.groupIndex;
                }
            }
            return -1;
        }

        public TechnologyGroup.GroupType GetTechGroupType(int index)
        {
            var groupData = config.configList.Find(x => x.groupIndex == index);
            if (groupData != null)
            {
                switch (groupData.groupType)
                {
                    case "Panel_3_1_1":
                        return TechnologyGroup.GroupType.Panel_3_1_1;
                    default:
                        return TechnologyGroup.GroupType.None;
                }
            }
            return TechnologyGroup.GroupType.None;
        }


        #region TechFinish Effect

        /// <summary>
        /// 获取科技完成数据
        /// </summary>
        /// <param name="effectID"></param>
        /// <returns></returns>
        public TechFinishEffect GetTechFinishEffect(int effectID)
        {
            var effect = configCommon.techFinishEffect.Find(x => x.ID == effectID);
            if (effect != null)
            {
                return effect;
            }
            else
            {
                Debug.LogError("Get Tech FinishEffect Data Error ! EffectID = " + effectID);
            }
            return null;
        }

        public TechCompleteEffect GetTechCompleteType(TechFinishEffect.TechEffect data)
        {
            switch (data.effectType)
            {
                case 1:
                    return TechCompleteEffect.Unlock_Block;
                case 2:
                    return TechCompleteEffect.Unlock_Tech;
                case 3:
                    return TechCompleteEffect.Unlock_Assemble_Part_Preset;
                case 4:
                    return TechCompleteEffect.Unlock_Assemble_Ship_Preset;
                default:
                    return TechCompleteEffect.None;
            }
        }

        public List<TechFinishEffect.TechEffect> GetTechCompleteEffect(int techID)
        {
            var data = GetTechDataByID(techID);
            if (data != null)
            {
                var finishEffect = GetTechFinishEffect(data.TechEffect);
                if(finishEffect.techEffectList.Count> Config.GlobalConfigData.TechDetail_Dialog_MaxEffect_Count)
                {
                    Debug.LogError("TechDetail_Dialog_MaxEffect_Count Cound not Larger than 4!  ID="+finishEffect.ID);
                }
                return finishEffect.techEffectList;
            }
            return null;
        }
        

        public static List<int> ParseTechParam_Unlock_Block(string content)
        {
            List<int> result = new List<int>();
            var list = Utility.TryParseIntList(content, ',');
            for (int i = 0; i < list.Count; i++)
            {
                if (FunctionBlockModule.GetFunctionBlockByBlockID(list[i]) != null)
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }

        public static List<int> ParseTechParam_Unlock_Tech(string content)
        {
            List<int> result = new List<int>();
            var list = Utility.TryParseIntList(content, ',');
            for (int i = 0; i < list.Count; i++)
            {
                if (GetTechDataByID(list[i]) != null)
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }

        public static List<int> ParseTechParam_Unlock_Assemble_Part(string content)
        {
            List<int> result = new List<int>();
            var list = Utility.TryParseIntList(content, ',');
            for (int i = 0; i < list.Count; i++)
            {
                if (AssembleModule.GetAssemblePartDataByKey(list[i]) != null)
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }
        public static List<int> ParseTechParam_Unlock_Assemble_Ship(string content)
        {
            List<int> result = new List<int>();
            var list = Utility.TryParseIntList(content, ',');
            for (int i = 0; i < list.Count; i++)
            {
                if (AssembleModule.GetWarshipDataByKey(list[i]) != null)
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }

        #endregion

        #region Tech Require Data

        public TechRequireType GetTechRequireType(TechRequireData.Require data)
        {
            switch (data.Type)
            {
                case 1:
                    return TechRequireType.PreTech;
                case 2:
                    return TechRequireType.Material;
                default:
                    return TechRequireType.None;
            }
        }

        public TechRequireData GetTechRequireData(int requireID)
        {
            var data=configCommon.techRequireData.Find(x=>x.ID==requireID);
            if (data == null)
            {
                Debug.LogError("Find Tech RequireData Error! requireID=" + requireID);
            }
            return data;
        }

        public List<TechRequireData.Require> GetTechRequireList(int techID)
        {
            var tech = GetTechDataByID(techID);
            if (tech != null)
            {
                var data = GetTechRequireData(tech.TechRequireID);
                if (data != null)
                {
                    if (data.requireList.Count > Config.GlobalConfigData.TechDetail_Dialog_MaxRequire_Count - 1)
                    {
                        Debug.LogError("TechDetail_Dialog_MaxRequire_Count Count not Larger than 3 !  ID="+data.ID);
                    }
                    return data.requireList;
                }
            }
            return null;
        }

        public static List<int> ParseTechParam_Require_PreTech(string content)
        {
            List<int> result = new List<int>();
            var list = Utility.TryParseIntList(content, ',');
            for(int i = 0; i < list.Count; i++)
            {
                var techData = GetTechDataByID(list[i]);
                if (techData != null)
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }

        public static Dictionary<int,int> parseTechParam_Require_Material(string content)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            var list = Utility.TryParseStringList(content, ',');
            for (int i = 0; i < list.Count; i++)
            {
                var countList = Utility.TryParseIntList(list[i], ':');
                if (countList.Count != 2)
                {
                    Debug.LogError("Parse TechRequire Error! content=" + content);
                    break;
                }
                var maData = MaterialModule.GetMaterialByMaterialID(countList[0]);
                if (maData != null)
                {
                    result.Add(countList[0], countList[1]);
                }
            }
            return result;
        }

        #endregion

    }

    public enum TechCompleteEffect
    {
        None,
        Unlock_Tech,
        /// <summary>
        /// 解锁Build ID
        /// </summary>
        Unlock_Block,
        Unlock_Assemble_Part_Preset,
        Unlock_Assemble_Ship_Preset
    }

    public enum TechRequireType
    {
        None,
        Material,
        PreTech
    }
    

    public class TechnologyConfigCommon
    {
        public List<TechFinishEffect> techFinishEffect;
        public List<TechRequireData> techRequireData;


        public void LoadData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            var config= reader.LoadJsonDataConfig<TechnologyConfigCommon>(Config.JsonConfigPath.TechnologyConfigCommon);
            techFinishEffect = config.techFinishEffect;
            techRequireData = config.techRequireData;
        }
    }

    public class TechFinishEffect
    {
        public int ID;
        public List<TechEffect> techEffectList;
        public class TechEffect
        {
            public ushort effectType;
            public string effectParam;
        }
    }

    public class TechRequireData
    {
        public int ID;
        public List<Require> requireList;

        public class Require
        {
            public ushort Type;
            public string Param;
        }
    }

}