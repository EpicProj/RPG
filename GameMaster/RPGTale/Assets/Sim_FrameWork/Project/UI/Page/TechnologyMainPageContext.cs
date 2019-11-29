using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class TechnologyMainPageContext : WindowBase
    {
        private TechnologyMainPage m_page;

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            m_page = UIUtility.SafeGetComponent<TechnologyMainPage>(Transform);
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }
        #endregion
    }
}