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

        public List<int> TechOnFinishIDList = new List<int>();


        private void InitAllTechInfo()
        {
            var list = TechnologyModule.Instance.GetAllTech();
            for (int i = 0; i < list.Count; i++)
            {
                TechnologyInfo info = new TechnologyInfo(list[i]);
                if (!AllTechDataDic.ContainsKey(info.techID))
                {
                    AllTechDataDic.Add(info.techID, info);
                }
            }
        }

        public TechnologyInfo GetTechInfo(int techID)
        {
            TechnologyInfo info = null;
            AllTechDataDic.TryGetValue(techID, out info);
            return info;
        }

        public List<int> GetTechStateInfoList(TechnologyState state)
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
                            if (info.currentState == TechnologyState.Lock)
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
                if (info.currentState == TechnologyState.Unlock)
                {
                    TechOnResearchList.Add(info);
                    info.currentState = TechnologyState.OnResearch;
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
            if (info != null && info.researchProgress >= 100 && info.currentState == TechnologyState.OnResearch)
            {
                if (TechOnResearchList.Contains(info))
                {
                    TechOnResearchList.Remove(info);
                }
                info.currentState = TechnologyState.Done;
                switch (info.baseType)
                {
                    case TechnologyInfo.TechType.Unique:
                        HandleTechCompleteEvent(info.techID);
                        if (!TechOnFinishIDList.Contains(info.techID))
                            TechOnFinishIDList.Add(info.techID);
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
                            info.currentState = TechnologyState.Unlock;
                        }
                        break;
                    case TechCompleteEffect.Unlock_Block:
                        var blockList = TechnologyModule.ParseTechParam_Unlock_Block(effect[i].effectParam);
                        for (int j = 0; j < blockList.Count; j++)
                        {
                            var buildData = PlayerModule.GetBuildingPanelDataByKey(blockList[j]);
                            if (buildData != null)
                            {
                                PlayerManager.Instance.AddUnLockBuildData(buildData);
                            }
                        }
                        break;
                    case TechCompleteEffect.Unlock_Assemble_Part_Preset:
                        var partList = TechnologyModule.ParseTechParam_Unlock_Assemble_Part(effect[i].effectParam);
                        for(int j = 0; j < partList.Count; j++)
                        {
                            PlayerManager.Instance.AddUnlockAssemblePartID(partList[j]);
                        }
                        break;
                    case TechCompleteEffect.Unlock_Assemble_Ship_Preset:
                        var shipList = TechnologyModule.ParseTechParam_Unlock_Assemble_Ship(effect[i].effectParam);
                        for(int j = 0; j < shipList.Count; j++)
                        {
                            PlayerManager.Instance.AddUnlockAssembleShipID(shipList[j]);
                        }
                        break;
                }
            }

        }
        #region Game Save Data

        public void LoadTechSaveData(TechnologySaveData saveData)
        {
            InitAllTechInfo();
            TechOnResearchList.Clear();
            TechOnFinishIDList.Clear();
            // Load TechStates
            for (int i = 0; i < saveData.saveList.Count; i++)
            {
                TechnologyInfo info = new TechnologyInfo();
                info = info.LoadSaveData(saveData.saveList[i]);
                TechOnResearchList.Add(info);
            }

            TechOnFinishIDList = saveData.finishTechList;
        }

        #endregion
    }

    #region Game Save
    public class TechnologySaveData
    {
        public List<TechnologyInfoSaveData> saveList;
        public List<int> finishTechList;

        public static TechnologySaveData CreateSave()
        {
            TechnologySaveData data = new TechnologySaveData();
            data.saveList = new List<TechnologyInfoSaveData>();
            for(int i = 0; i < TechnologyDataManager.Instance.TechOnResearchList.Count; i++)
            {
                var info = TechnologyDataManager.Instance.TechOnResearchList[i];
                TechnologyInfoSaveData saveItem = new TechnologyInfoSaveData(info.techID, info.currentState, info.researchProgress);
                data.saveList.Add(saveItem);
            }

            data.finishTechList = TechnologyDataManager.Instance.TechOnFinishIDList;
            return data;
        }
    }
    #endregion
}