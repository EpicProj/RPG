using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public partial class AssembleShipBuildPageContext : WindowBase
    {
        #region Override Method

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

    public partial class AssembleShipBuildPageContext : WindowBase
    {

        protected override void InitUIRefrence()
        {
        }
    }
}
