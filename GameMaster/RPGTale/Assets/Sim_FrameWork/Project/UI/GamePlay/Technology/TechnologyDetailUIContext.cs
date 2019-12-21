using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class TechnologyDetailUIContext : WindowBase
    {
        private TechnologyDetailUI m_UI;

        private TechnologyInfo _info;
        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            _info = (TechnologyInfo)paralist[0];
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnClose()
        {
            base.OnClose();
        }

        protected override void InitUIRefrence()
        {
            m_UI = UIUtility.SafeGetComponent<TechnologyDetailUI>(Transform);
        }


        #endregion
    }
}