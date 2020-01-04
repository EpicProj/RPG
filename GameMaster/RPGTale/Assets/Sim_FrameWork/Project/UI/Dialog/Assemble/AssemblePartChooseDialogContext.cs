using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class AssemblePartChooseDialogContext : WindowBase
    {
        #region OverrideMethod
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
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