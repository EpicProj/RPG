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

    }
}