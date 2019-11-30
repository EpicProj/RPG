using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class TechnologyInfo 
    {
        public enum TechState
        {
            Lock,
            OnResearch,
            PauseResearch,
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

        public TechnologyInfo(int techID)
        {
            _model = new TechnologyDataModel();
            if (!_model.Create(techID))
                return ;
            this.techID = techID;
            baseType = TechnologyModule.GetTechBaseType(techID);
            currentState = TechState.Lock;
        }

    }
}