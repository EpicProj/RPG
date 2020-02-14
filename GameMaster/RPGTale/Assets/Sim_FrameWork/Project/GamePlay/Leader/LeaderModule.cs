using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class LeaderModule : BaseModule<LeaderModule>
    {

        private static Dictionary<int, LeaderPresetData> leaderPresetDataDic;
        private static Dictionary<int, LeaderSpeciesData> leaderSpeciesDataDic;
        private static Dictionary<int, LeaderSkillData> leaderSkillDataDic;
        private static Dictionary<int, LeaderDutyData> leaderDutyDataDic;
        private static Dictionary<int, LeaderCreedData> leaderCreedDataDic;
        private static Dictionary<int, LeaderAttributeData> leaderAttributeDataDic;
        private static Dictionary<int, LeaderBirthlandData> leaderBirthlandDataDic;
        private static Dictionary<int, LeaderStoryData> leaderStoryDataDic;

        private const string Leader_Gender_Male_Text = "Leader_Gender_Male_Text";
        private const string Leader_Gender_Female_Text = "Leader_Gender_Female_Text";
        private const string Leader_Gender_None_Text = "Leader_Gender_None_Text";

        public override void InitData()
        {
            var config = ConfigManager.Instance.LoadData<LeaderMetaData>(ConfigPath.TABLE_LEADER_METADATA_PATH);
            if (config == null)
            {
                DebugPlus.LogError("LeaderMetaData Read Error");
                return;
            }
            leaderPresetDataDic = config.AllLeaderPresetDataDic;
            leaderSkillDataDic = config.AllLeaderSkillDataDic;
            leaderDutyDataDic = config.AllLeaderDutyDataDic;
            leaderSpeciesDataDic = config.AllLeaderSpeciesDataDic;
            leaderCreedDataDic = config.AllLeaderCreedDataDic;
            leaderAttributeDataDic = config.AllLeaderAttributeDataDic;
            leaderBirthlandDataDic = config.AllLeaderBirthlandDataDic;
            leaderStoryDataDic = config.AllLeaderStoryDataDic;
        }

        public override void Register()
        {
            base.Register();
        }
        public LeaderModule()
        {
            InitData();
        }

        public bool DataCheck()
        {
            bool result = true;
            foreach(var creedData in leaderCreedDataDic)
            {
                if (CampModule.GetCampCreedDataByKey(creedData.Key) == null)
                {
                    DebugPlus.LogError("[LeaderCreedData] : Can not Find CampCreedID , LeaderCreedID=" + creedData.Key);
                    result = false;
                    continue;
                }
            }

            return result;
        }

        public static LeaderPresetData GetLeaderPresetDataByKey(int leaderID)
        {
            LeaderPresetData data = null;
            leaderPresetDataDic.TryGetValue(leaderID, out data);
            if (data == null)
                DebugPlus.LogError("Get LeaderPresetData Error!  leaderID=" + leaderID);
            return data;
        }
        
        public static LeaderSpeciesData GetLeaderSpeciesDataByKey(int speciesID)
        {
            LeaderSpeciesData data = null;
            leaderSpeciesDataDic.TryGetValue(speciesID, out data);
            if (data == null)
                DebugPlus.LogError("Get LeaderSpeciesData Error! speciesID=" + speciesID);
            return data;
        }

        public static LeaderCreedData GetLeaderCreedDataByKey(int creedID)
        {
            LeaderCreedData data = null;
            leaderCreedDataDic.TryGetValue(creedID, out data);
            if (data == null)
                DebugPlus.LogError("Get LeaderCreedData Error! CreedID=" + creedID);
            return data;
        }

        public static LeaderSkillData GetLeaderSkillDataByKey(int skillID)
        {
            LeaderSkillData data = null;
            leaderSkillDataDic.TryGetValue(skillID, out data);
            if (data == null)
                DebugPlus.LogError("Get LeaderSkillData Error! SkillID=" + skillID);
            return data;
        }

        public static LeaderAttributeData GetLeaderAttributeDataByKey(int attributeID)
        {
            LeaderAttributeData data = null;
            leaderAttributeDataDic.TryGetValue(attributeID, out data);
            if (data == null)
                DebugPlus.LogError("Get LeaderAttributeData Error! attributeID=" + attributeID);
            return data;
        }

        public static LeaderBirthlandData GetLeaderBirthlandDataByKey(int landID)
        {
            LeaderBirthlandData data = null;
            leaderBirthlandDataDic.TryGetValue(landID, out data);
            if (data == null)
                DebugPlus.LogError("Get LeaderBirthlandData Error! landID=" + landID);
            return data;
        }

        public static LeaderStoryData GetLeaderStoryDataByKey(int storyID)
        {
            LeaderStoryData data = null;
            leaderStoryDataDic.TryGetValue(storyID, out data);
            if (data == null)
                DebugPlus.LogError("Get LeaderStoryData Error! storyID=" + storyID);
            return data;
        }

        #region Basic
        /// <summary>
        /// 获取性别描述
        /// </summary>
        /// <param name="GenderID"></param>
        /// <returns></returns>
        public static string GetLeaderGenderText(byte GenderID)
        {
            if (GenderID == 1)
                return MultiLanguage.Instance.GetTextValue(Leader_Gender_Male_Text);
            else if (GenderID == 2)
                return MultiLanguage.Instance.GetTextValue(Leader_Gender_Female_Text);
            else
                return MultiLanguage.Instance.GetTextValue(Leader_Gender_None_Text);
        }


        #endregion


        public static List<LeaderSkillInfo> GetLeaderSkillInfoDefault(int leaderID)
        {
            List<LeaderSkillInfo> result = new List<LeaderSkillInfo>();
            var meta = GetLeaderPresetDataByKey(leaderID);
            if (meta != null)
            {
                var list = Utility.TryParseStringList(meta.LeaderSkillIDList, ',');
                for(int i = 0; i < list.Count; i++)
                {
                    var skill = Utility.TryParseIntList(list[i], ':');
                    if (skill.Count != 2)
                    {
                        DebugPlus.LogError("LeaderSkill FormateError!  leaderID=" + leaderID);
                        continue;
                    }
                    int skillID = skill[0];
                    if (GetLeaderSkillDataByKey(skillID) != null)
                    {
                        LeaderSkillInfo info = LeaderSkillInfo.InitSkillInfo(skillID, skill[1]);
                        result.Add(info);
                    }
                }
            }

            return result;
        }

        #region Portrait

        public static Config.LeaderPortraitItemConfig GetRandomPortraitItem(LeaderPortraitType type,int speciesID,int sexID)
        {
            var config = Config.ConfigData.LeaderPortraitConfig;

            Func<List<Config.LeaderPortraitItemConfig>, Config.LeaderPortraitItemConfig> GetRandom = (o) =>
            {
                LeaderPortraitListFilter(speciesID, sexID, ref o);
                int index = UnityEngine.Random.Range(0, o.Count);
                return o[index];
            };

            if (type == LeaderPortraitType.Cloth)
            {
                return GetRandom(config.portrait_cloth);
            }
            else if (type == LeaderPortraitType.Ear)
            {
                return GetRandom(config.portrait_Ear);
            }
            else if (type == LeaderPortraitType.Eyes)
            {
                return GetRandom(config.portrait_eyes);
            }
            else if (type == LeaderPortraitType.Face)
            {
                return GetRandom(config.portrait_face);
            }
            else if (type == LeaderPortraitType.Hair)
            {
                return GetRandom(config.portrait_hair);
            }
            else if (type == LeaderPortraitType.Mouth)
            {
                return GetRandom(config.portrait_Mouth);
            }
            else if (type == LeaderPortraitType.Nose)
            {
                return GetRandom(config.portrait_Nose);
            }
            else if(type == LeaderPortraitType.BG)
            {
                return GetRandom(config.portrait_bg);
            }
            DebugPlus.LogError("[GetRandomPortraitItem] : RandomError! ");
            return null;
        }

        private static void LeaderPortraitListFilter(int speciesID, int sexID, ref List<Config.LeaderPortraitItemConfig> list)
        {
            var meta = GetLeaderSpeciesDataByKey(speciesID);
            if (meta != null && sexID==1)
            {
                list = list.FindAll(x => x.configGroupIDList.Contains(meta.Portrait_MaleGroup));
            }
            else if(meta !=null && sexID == 2)
            {
                list = list.FindAll(x => x.configGroupIDList.Contains(meta.Portrait_FemaleGroup));
            }
        }
        #endregion


        #region Attribute
        public static List<LeaderAttributeInfo> GetLeaderAttributePreset(int leaderID)
        {
            List<LeaderAttributeInfo> result = new List<LeaderAttributeInfo>();
            var leaderMeta = GetLeaderPresetDataByKey(leaderID);
            if (leaderMeta != null)
            {
                var list = Utility.TryParseIntList(leaderMeta.AttributeIDList,',');
                for(int i = 0; i < list.Count; i++)
                {
                    LeaderAttributeInfo info = LeaderAttributeInfo.InitAttributeInfo(list[i]);
                    if (info != null)
                        result.Add(info);
                }
            }
            return result;
        }


        #endregion

        #region Story
        public static List<LeaderStoryInfo> GetLeaderPresetStory(int leaderID)
        {
            List<LeaderStoryInfo> result = new List<LeaderStoryInfo>();
            var leaderMeta = GetLeaderPresetDataByKey(leaderID);
            if (leaderMeta != null)
            {
                var list = Utility.TryParseIntList(leaderMeta.StoryList, ',');
                for (int i = 0; i < list.Count; i++)
                {
                    LeaderStoryInfo info = LeaderStoryInfo.InitStoryInfo(list[i]);
                    if (info != null)
                        result.Add(info);
                }
            }
            return result;
        }


        #endregion
    }
}