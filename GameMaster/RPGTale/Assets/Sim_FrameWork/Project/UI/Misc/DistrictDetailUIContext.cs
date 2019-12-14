using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class DistrictDetailUIContext : WindowBase
    {
        private DistrictDetailUI m_UI;
        private DistrictDataModel _model;

        #region Override Method

        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _model = (DistrictDataModel)paralist[0];
        }

        public override void OnShow(params object[] paralist)
        {
            _model = (DistrictDataModel)paralist[0];
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void InitUIRefrence()
        {
            m_UI = UIUtility.SafeGetComponent<DistrictDetailUI>(Transform);
        }

        #endregion


    }
}