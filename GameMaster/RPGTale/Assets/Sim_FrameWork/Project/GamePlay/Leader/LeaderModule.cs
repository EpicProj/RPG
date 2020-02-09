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

        public override void InitData()
        {
            base.InitData();
        }

        public override void Register()
        {
            base.Register();
        }
        public LeaderModule()
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


        public static Config.LeaderPortraitItemConfig GetRandomPortraitItem(LeaderPortraitType type)
        {
            var config = Config.ConfigData.LeaderPortraitConfig;

            int maxNum = 0;
            if (type == LeaderPortraitType.Cloth)
            {
                maxNum = config.portrait_cloth.Count;
                int index = UnityEngine.Random.Range(0, maxNum);
                return config.portrait_cloth[index];
            }
            else if (type == LeaderPortraitType.Ear)
            {
                maxNum = config.portrait_Ear.Count;
                int index = UnityEngine.Random.Range(0, maxNum);
                return config.portrait_Ear[index];
            }
            else if (type == LeaderPortraitType.Eyes)
            {
                maxNum = config.portrait_eyes.Count;
                int index = UnityEngine.Random.Range(0, maxNum);
                return config.portrait_eyes[index];
            }
            else if (type == LeaderPortraitType.Face)
            {
                maxNum = config.portrait_face.Count;
                int index = UnityEngine.Random.Range(0, maxNum);
                return config.portrait_face[index];
            }
            else if (type == LeaderPortraitType.Hair)
            {
                maxNum = config.portrait_hair.Count;
                int index = UnityEngine.Random.Range(0, maxNum);
                return config.portrait_hair[index];
            }
            else if (type == LeaderPortraitType.Mouth)
            {
                maxNum = config.portrait_Mouth.Count;
                int index = UnityEngine.Random.Range(0, maxNum);
                return config.portrait_Mouth[index];
            }
            else if (type == LeaderPortraitType.Nose)
            {
                maxNum = config.portrait_Nose.Count;
                int index = UnityEngine.Random.Range(0, maxNum);
                return config.portrait_Nose[index];
            }
            else if(type == LeaderPortraitType.BG)
            {
                maxNum = config.portrait_bg.Count;
                int index = UnityEngine.Random.Range(0, maxNum);
                return config.portrait_bg[index];
            }
            DebugPlus.LogError("[GetRandomPortraitItem] : RandomError! ");
            return null;
        }
    }
}