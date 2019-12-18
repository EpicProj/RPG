using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class TechnologyDataManager : Singleton<TechnologyDataManager>
    {
        public void InitData()
        {
            InitAllTechInfo();
        }

        public Dictionary<int, TechnologyInfo> AllTechDataDic = new Dictionary<int, TechnologyInfo>();

        public List<TechnologyInfo> TechOnResearchList = new List<TechnologyInfo>();


        private void InitAllTechInfo()
        {
            var list = TechnologyModule.Instance.GetAllTech();
            for (int i = 0; i < list.Count; i++)
            {
                TechnologyInfo info = new TechnologyInfo(list[i]);
                AllTechDataDic.Add(info.techID, info);
            }
        }

        public TechnologyInfo GetTechInfo(int techID)
        {
            TechnologyInfo info = null;
            AllTechDataDic.TryGetValue(techID, out info);
            return info;
        }

        public List<int> GetTechStateInfoList(TechnologyInfo.TechState state)
        {
            List<int> result = new List<int>();
            foreach (var info in AllTechDataDic)
            {
                if (info.Value.currentState == state)
                    result.Add(info.Key);
            }
            return result;
        }

        /// <summary>
        /// 检查研究前置条件
        /// </summary>
        /// <param name="techID"></param>
        /// <returns></returns>
        public bool CheckTechCanResearch(int techID)
        {
            bool canResearch = true;

            var requireList = TechnologyModule.Instance.GetTechRequireList(techID);
            for (int i = 0; i < requireList.Count; i++)
            {
                var type = TechnologyModule.Instance.GetTechRequireType(requireList[i]);
                switch (type)
                {
                    case TechRequireType.PreTech:
                        var techList = TechnologyModule.ParseTechParam_Unlock_Tech(requireList[i].Param);
                        for (int j = 0; j < techList.Count; j++)
                        {
                            var info = GetTechInfo(techList[j]);
                            if (info.currentState == TechnologyInfo.TechState.Lock)
                                canResearch = false;
                        }
                        continue;
                    case TechRequireType.Material:
                        var materialDic = TechnologyModule.parseTechParam_Require_Material(requireList[i].Param);
                        foreach (KeyValuePair<int, int> kvp in materialDic)
                        {
                            if (PlayerManager.Instance.GetMaterialStoreCount(kvp.Key) < kvp.Value)
                                canResearch = false;
                        }
                        continue;
                }
            }

            return canResearch;
        }


        /// <summary>
        /// 开始研究
        /// </summary>
        /// <param name="techID"></param>
        public bool OnTechResearchStart(int techID)
        {
            if (CheckTechCanResearch(techID))
            {
                var info = GetTechInfo(techID);
                if (info.currentState == TechnologyInfo.TechState.Unlock)
                {
                    TechOnResearchList.Add(info);
                    info.currentState = TechnologyInfo.TechState.OnResearch;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 科技研究完成
        /// </summary>
        /// <param name="techID"></param>
        public void OnTechResearchFinish(int techID)
        {
            var info = GetTechInfo(techID);
            if (info != null && info.researchProgress >= 100 && info.currentState == TechnologyInfo.TechState.OnResearch)
            {
                if (TechOnResearchList.Contains(info))
                {
                    TechOnResearchList.Remove(info);
                }
                info.currentState = TechnologyInfo.TechState.Done;
                switch (info.baseType)
                {
                    case TechnologyInfo.TechType.Unique:
                        HandleTechCompleteEvent(info.techID);
                        break;
                    case TechnologyInfo.TechType.Series:
                        break;
                }

                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Technology_Page, new UIMessage(UIMsgType.Tech_Research_Finish, new List<object>() { techID }));
            }
        }

        private void HandleTechCompleteEvent(int techID)
        {
            var effect = TechnologyModule.Instance.GetTechCompleteEffect(techID);
            for (int i = 0; i < effect.Count; i++)
            {
                var type = TechnologyModule.Instance.GetTechCompleteType(effect[i]);
                switch (type)
                {
                    case TechCompleteEffect.Unlock_Tech:
                        var techList = TechnologyModule.ParseTechParam_Unlock_Tech(effect[i].effectParam);
                        for (int j = 0; j < techList.Count; j++)
                        {
                            var info = GetTechInfo(techList[j]);
                            info.currentState = TechnologyInfo.TechState.Unlock;
                        }
                        break;
                    case TechCompleteEffect.Unlock_Block:
                        var blockList = TechnologyModule.ParseTechParam_Unlock_Block(effect[i].effectParam);
                        for (int j = 0; j < blockList.Count; j++)
                        {
                            var buildData = PlayerModule.GetBuildingPanelDataByKey(blockList[i]);
                            if (buildData != null)
                            {
                                PlayerManager.Instance.AddUnLockBuildData(buildData);
                            }
                        }
                        break;
                }
            }

        }

    }
}