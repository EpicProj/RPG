using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sim_FrameWork
{
    public class AchievementModule : BaseModule<AchievementModule>
    {
        #region statistics
        /// <summary>
        /// 统计
        /// </summary>
        public Dictionary<int, GameStatisticsData> GameStatisticsDic = new Dictionary<int, GameStatisticsData>();

        public override void InitData()
        {
            
        }

        public override void Register()
        {
            
        }


        public void AddMaterialStatistics(int saveID, int materialID,int Count)
        {
            if (GameStatisticsDic.ContainsKey(saveID))
            {
                var dic = GameStatisticsDic[saveID].MaterialCounterDic;
                if (dic.ContainsKey(materialID))
                {
                    dic[materialID] += Count;
                }
                else
                {
                    dic.Add(materialID, Count);
                }
            }
         
        }



        #endregion




    }

    public class GameStatisticsData
    {
        public Dictionary<int, int> MaterialCounterDic;

    }


    [System.Serializable]
    public class GameStatisticsSaveData
    {
        public Dictionary<int, int> MaterialCounterDic;
    }
}