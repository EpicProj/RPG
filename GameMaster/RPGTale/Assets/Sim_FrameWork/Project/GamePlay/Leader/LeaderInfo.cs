using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Leader Info
 * SOMA
 * 
 */

namespace Sim_FrameWork
{
    public enum LeaderPortraitType
    {
        BG,
        Cloth,
        Ear,
        Hair,
        Eyes,
        Face,
        Mouth,
        Nose
    }

    public class LeaderInfo
    {
        public int leaderID;
        public ushort leaderUID;

        public string leaderName;
        public string leaderDesc;

        public ushort currentAge;
        public byte Gender;

        /// <summary>
        /// 初始角色强制选择
        /// </summary>
        public bool forceSelcet =false;

        public LeaderSpeciesInfo speciesInfo;
        public LeaderPortraitInfo portraitInfo;
        public LeaderCreedInfo creedInfo;
        public LeaderBirthlandInfo birthlandInfo;

        public List<LeaderSkillInfo> skillInfoList;
        public List<LeaderAttributeInfo> attributeInfoList;
        public List<LeaderStoryInfo> storyInfoList;

        public static LeaderInfo CreateLeaderInfo_Preset(int leaderID)
        {
            LeaderInfo info = new LeaderInfo();
            var meta = LeaderModule.GetLeaderPresetDataByKey(leaderID);
            if (meta == null)
            {
                DebugPlus.LogError("CreateLeaderInfo_Preset Fail! leaderID=" + leaderID);
                return null;
            }
            info.leaderID = meta.LeaderID;
            info.leaderName = MultiLanguage.Instance.GetTextValue(meta.LeaderName);
            info.leaderDesc = MultiLanguage.Instance.GetTextValue(meta.LeaderDesc);
            info.currentAge = meta.Age;

            info.speciesInfo = LeaderSpeciesInfo.InitSpeciesInfo(meta.SpeciesID);
            info.creedInfo = LeaderCreedInfo.InitCreedInfo(meta.CreedID);
            info.skillInfoList = LeaderModule.GetLeaderSkillInfoDefault(leaderID);
            info.attributeInfoList = LeaderModule.GetLeaderAttributePreset(leaderID);
            info.birthlandInfo = LeaderBirthlandInfo.InitBirthlandInfo(meta.BirthlandID);
            info.storyInfoList = LeaderModule.GetLeaderPresetStory(leaderID);

            info.portraitInfo = LeaderPortraitInfo.Generate_PresetInfo(meta.Portrait_BG, meta.Portrait_Cloth, meta.Portrait_Ear,meta.Portrait_Hair,meta.Portrait_Eyes, meta.Portrait_Face, meta.Portrait_Mouth, meta.Portrait_Nose);

            return info;
        }

        /// <summary>
        /// Create Random
        /// </summary>
        /// <param name="speciesID"></param>
        /// <param name="sexID"></param>
        /// <returns></returns>
        public static LeaderInfo CreateRandomInfo(int speciesID,int sexID)
        {
            LeaderInfo info = new LeaderInfo();

            return info;
        }
        

    }
    /// <summary>
    /// 角色头像信息
    /// </summary>
    public class LeaderPortraitInfo
    {
        public Config.LeaderPortraitItemConfig portrait_bg;

        public Config.LeaderPortraitItemConfig portrait_cloth;   //服装
        public Config.LeaderPortraitItemConfig portrait_ear;     //耳
        public Config.LeaderPortraitItemConfig portrait_hair;    //头发
        public Config.LeaderPortraitItemConfig portrait_eyes;    //眼
        public Config.LeaderPortraitItemConfig portrait_face;    //脸
        public Config.LeaderPortraitItemConfig portrait_mouth;   //嘴
        public Config.LeaderPortraitItemConfig portrait_nose;    //鼻

        public static LeaderPortraitInfo GenerateRandomInfo(int speciesID,int sexID)
        {
            LeaderPortraitInfo info = new LeaderPortraitInfo();
            info.portrait_bg = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.BG,speciesID,sexID);
            info.portrait_cloth = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Cloth,speciesID,sexID);
            info.portrait_ear = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Ear,speciesID,sexID);
            info.portrait_hair = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Hair,speciesID,sexID);
            info.portrait_eyes = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Eyes,speciesID,sexID);
            info.portrait_face = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Face,speciesID,sexID);
            info.portrait_mouth = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Mouth,speciesID,sexID);
            info.portrait_nose = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Nose,speciesID,sexID);
            return info;
        }

        public static LeaderPortraitInfo Generate_PresetInfo(int bgID,int clothID,int earID ,int hairID,int eyesID,int faceID,int mouthID,int noseID)
        {
            LeaderPortraitInfo info = new LeaderPortraitInfo();
            var config = Config.ConfigData.LeaderPortraitConfig;
            info.portrait_bg = config.portrait_bg.Find(x => x.configID == bgID);
            info.portrait_cloth = config.portrait_cloth.Find(x => x.configID == clothID);
            info.portrait_ear = config.portrait_Ear.Find(x => x.configID == earID);
            info.portrait_hair = config.portrait_hair.Find(x => x.configID == hairID);
            info.portrait_eyes = config.portrait_eyes.Find(x => x.configID == eyesID);
            info.portrait_face = config.portrait_face.Find(x => x.configID == faceID);
            info.portrait_mouth = config.portrait_Mouth.Find(x => x.configID == mouthID);
            info.portrait_nose = config.portrait_Nose.Find(x => x.configID == noseID);
            if(info.portrait_bg==null||
                info.portrait_cloth==null ||
                info.portrait_ear==null||
                info.portrait_eyes==null || 
                info.portrait_face==null || 
                info.portrait_hair==null||
                info.portrait_mouth==null||
                info.portrait_nose == null)
            {
                DebugPlus.LogError("[Generate_PresetInfo] : Error!");
                return null;
            }
            return info;
        }
    }

    public class LeaderSkillInfo
    {
        public int skillID;
        public string skillName;
        public string skillDesc;
        public string skillIconPath;

        public int currentLevel;
        public int maxLevel;

        public static LeaderSkillInfo InitSkillInfo(int skillID,int defaultLevel)
        {
            LeaderSkillInfo info = new LeaderSkillInfo();
            var meta = LeaderModule.GetLeaderSkillDataByKey(skillID);
            if (meta == null)
            {
                DebugPlus.LogError("Init LeaderSkill Info Error! skillID=" + skillID);
                return null;
            }
            info.skillID = meta.SkillID;
            info.skillName = MultiLanguage.Instance.GetTextValue(meta.SkillName);
            info.skillDesc = MultiLanguage.Instance.GetTextValue(meta.SkillDesc);
            info.skillIconPath = meta.SkillIcon;
            info.currentLevel = defaultLevel;
            info.maxLevel = meta.MaxLevel;

            if (info.currentLevel > meta.MaxLevel)
            {
                DebugPlus.Log("LeaderSkill Info SkillLevelDefault larger than MaxLevel, skillID=" + skillID);
                info.currentLevel = meta.MaxLevel;
            }
            return info;
        }
    }


    /// <summary>
    /// Leader Species
    /// </summary>
    public class LeaderSpeciesInfo
    {
        public int speciesID;
        public string speciesName;
        public string speciesDesc;

        public string Portrait_MaleGroup;
        public string Portrait_FemaleGroup;

        public static LeaderSpeciesInfo InitSpeciesInfo(int speciesID)
        {
            LeaderSpeciesInfo info = new LeaderSpeciesInfo();
            var meta = LeaderModule.GetLeaderSpeciesDataByKey(speciesID);
            if (meta == null)
            {
                DebugPlus.LogError("Init LeaderSpeciesInfo Fail! speciesID=" + speciesID);
                return null;
            }
            info.speciesID = speciesID;
            info.speciesName = MultiLanguage.Instance.GetTextValue(meta.SpeciesName);
            info.speciesDesc = MultiLanguage.Instance.GetTextValue(meta.SpeciesDesc);
            info.Portrait_MaleGroup = meta.Portrait_MaleGroup;
            info.Portrait_FemaleGroup = meta.Portrait_FemaleGroup;
            return info;
        }
    }

    /// <summary>
    /// 领袖信仰
    /// </summary>
    public class LeaderCreedInfo
    {
        public int creedID;
        public string creedName;
        public string creedDesc;
        public string creedIconPath;

        public static LeaderCreedInfo InitCreedInfo(int creedID)
        {
            LeaderCreedInfo info = new LeaderCreedInfo();
            var meta = LeaderModule.GetLeaderCreedDataByKey(creedID);
            if (meta == null)
            {
                DebugPlus.LogError("Init LeaderCreedInfo Error! creedID=" + creedID);
                return null;
            }
            info.creedID = meta.CreedID;
            info.creedName = MultiLanguage.Instance.GetTextValue(meta.CreedName);
            info.creedDesc = MultiLanguage.Instance.GetTextValue(meta.CreedDesc);
            info.creedIconPath = meta.IconPath;
            return info;
        }
    }

    public class LeaderBirthlandInfo
    {
        public int birthlandID;
        public string landName;
        public string landDesc;
        public string landIconPath;
        public string landBGPath;

        public static LeaderBirthlandInfo InitBirthlandInfo(int landID)
        {
            LeaderBirthlandInfo info = new LeaderBirthlandInfo();
            var meta = LeaderModule.GetLeaderBirthlandDataByKey(landID);
            if (meta == null)
            {
                DebugPlus.LogError("Init LeaderBirthlandInfo Fail ! landID=" + landID);
                return null;
            }
            info.birthlandID = meta.LandID;
            info.landName = MultiLanguage.Instance.GetTextValue(meta.LandName);
            info.landDesc = MultiLanguage.Instance.GetTextValue(meta.LandDesc);
            info.landIconPath = meta.LandIconPath;
            info.landBGPath = meta.LandBGPath;

            return info;
        }
    }

    /// <summary>
    /// Leader Attribute
    /// </summary>
    public class LeaderAttributeInfo
    {
        public int attributeID;
        public string attributeName;
        public string attributeDesc;
        public string attributeIconPath;

        public static LeaderAttributeInfo InitAttributeInfo(int attributeID)
        {
            LeaderAttributeInfo info = new LeaderAttributeInfo();
            var meta = LeaderModule.GetLeaderAttributeDataByKey(attributeID);
            if (meta == null)
            {
                DebugPlus.LogError("Init LeaderAttribute Error! attributeID=" + attributeID);
                return null;
            }
            info.attributeID = meta.AttributeID;
            info.attributeName = MultiLanguage.Instance.GetTextValue(meta.Name);
            info.attributeDesc = MultiLanguage.Instance.GetTextValue(meta.Desc);
            info.attributeIconPath = meta.IconPath;

            return info;
        }
    }

    /// <summary>
    /// Leader Story
    /// </summary>
    public class LeaderStoryInfo
    {
        public int storyID;
        public string storyContent;
        public int year;
        public int poolLevel;

        public static LeaderStoryInfo InitStoryInfo(int storyID)
        {
            LeaderStoryInfo info = new LeaderStoryInfo();
            var meta = LeaderModule.GetLeaderStoryDataByKey(storyID);
            if (meta == null)
            {
                DebugPlus.LogError("Init LeaderStoryInfo Error! storyID=" + storyID);
                return null;
            }
            info.storyID = meta.StoryID;
            info.storyContent = MultiLanguage.Instance.GetTextValue(meta.Content);
            info.year = meta.StoryYearStart;
            info.poolLevel = meta.PoolLevel;

            return info;
        }


    }
}