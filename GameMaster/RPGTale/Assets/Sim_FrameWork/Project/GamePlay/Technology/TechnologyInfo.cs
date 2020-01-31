using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public enum TechnologyState
    {
        Unlock,
        Lock,
        OnResearch,
        PauseResearch,
        Require_Lack,
        Done,
    }
    public class TechnologyInfo 
    {
        public enum TechType
        {
            /// <summary>
            /// 唯一
            /// </summary>
            Unique,
            /// <summary>
            /// 有等级
            /// </summary>
            Series
        }

        public int techID;
        public TechnologyDataModel _model;
        public TechType baseType;
        public TechnologyState currentState;

        public List<TechRequireData.Require> techRequireList;
        public List<TechFinishEffect.TechEffect> techFinishEffectList;

        /// <summary>
        /// 研究进度
        /// </summary>
        public float researchProgress = 0f;

        public TechnologyInfo() { }
        public TechnologyInfo(int techID)
        {
            _model = new TechnologyDataModel();
            if (!_model.Create(techID))
                return ;
            this.techID = techID;
            baseType = TechnologyModule.GetTechBaseType(techID);

            techRequireList = TechnologyModule.Instance.GetTechRequireList(techID);
            techFinishEffectList = TechnologyModule.Instance.GetTechCompleteEffect(techID);

            if (TechnologyModule.GetTechDataByID(techID).Unlock)
            {
                currentState = TechnologyState.Unlock;
            }
            else
            {
                currentState = TechnologyState.Lock;
            }
        }

        public TechnologyInfo LoadSaveData(TechnologyInfoSaveData saveData)
        {
            TechnologyInfo info = new TechnologyInfo(saveData.technolgyID);
            info.currentState = saveData.currentState;
            info.researchProgress = saveData.progress;
            return info;
        }

    }
    #region GameSaveData
    public class TechnologyInfoSaveData
    {
        public int technolgyID;
        public TechnologyState currentState;
        public float progress;

        public TechnologyInfoSaveData(int techID,TechnologyState state,float progress)
        {
            this.technolgyID = techID;
            this.currentState = state;
            this.progress = progress;
        }
    }
    #endregion
}