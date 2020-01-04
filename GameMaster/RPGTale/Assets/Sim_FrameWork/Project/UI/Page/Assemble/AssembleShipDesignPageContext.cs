using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public partial class AssembleShipDesignPageContext : WindowBase
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
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
        }


        #endregion


    }


    public partial class AssembleShipDesignPageContext : WindowBase
    {
        private AssembleShipDesignPage m_page;

        protected override void InitUIRefrence()
        {
            m_page = UIUtility.SafeGetComponent<AssembleShipDesignPage>(Transform);
        }


    }
}