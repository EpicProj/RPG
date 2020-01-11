using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class AssembleShipChooseDialogContext : WindowBase
    {
        private AssembleShipChooseDialog m_dialog;

        private Transform noInfoTrans;
        private Animation noInfoAnim;
        private Transform tabContentTrans;

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        #endregion
    }
}