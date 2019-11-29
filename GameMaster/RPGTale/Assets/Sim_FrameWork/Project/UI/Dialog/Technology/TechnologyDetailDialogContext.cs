using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class TechnologyDetailDialogContext : WindowBase
    {
        private TechnologyDetailDialog m_dialog;


        #region Override Method

        public override void Awake(params object[] paralist)
        {
            m_dialog = UIUtility.SafeGetComponent<TechnologyDetailDialog>(Transform);
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
        }


        #endregion


    }
}