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
        public string leaderName;
        public string leaderDesc;

        public LeaderSpeciesInfo speciesInfo;
        public LeaderPortraitInfo portraitInfo;


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

            info.speciesInfo = LeaderSpeciesInfo.InitSpeciesInfo(meta.SpeciesID);

            info.portraitInfo = LeaderPortraitInfo.Generate_PresetInfo(meta.Portrait_BG, meta.Portrait_Cloth, meta.Portrait_Ear,meta.Portrait_Hair,meta.Portrait_Eyes, meta.Portrait_Face, meta.Portrait_Mouth, meta.Portrait_Nose);

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

        public static LeaderPortraitInfo GenerateRandomInfo()
        {
            LeaderPortraitInfo info = new LeaderPortraitInfo();
            info.portrait_bg = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.BG);
            info.portrait_cloth = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Cloth);
            info.portrait_ear = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Ear);
            info.portrait_hair = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Hair);
            info.portrait_eyes = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Eyes);
            info.portrait_face = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Face);
            info.portrait_mouth = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Mouth);
            info.portrait_nose = LeaderModule.GetRandomPortraitItem(LeaderPortraitType.Nose);
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

    /// <summary>
    /// Leader Species
    /// </summary>
    public class LeaderSpeciesInfo
    {
        public int speciesID;
        public string speciesName;
        public string speciesDesc;

        public int Portrait_MaleGroup;
        public int Portrait_FemaleGroup;

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
}