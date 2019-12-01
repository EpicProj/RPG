using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class TechnologyInfo 
    {
        public enum TechState
        {
            Unlock,
            Lock,
            OnResearch,
            PauseResearch,
            Require_Lack,
            Done,
        }

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
        public TechState currentState;

        public List<TechRequireData.Require> techRequireList;
        public List<TechFinishEffect.TechEffect> techFinishEffectList;

        /// <summary>
        /// 研究进度
        /// </summary>
        public float researchProgress = 0f;

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
                currentState = TechState.Unlock;
            }
            else
            {
                currentState = TechState.Lock;
            }
        }

    }
}