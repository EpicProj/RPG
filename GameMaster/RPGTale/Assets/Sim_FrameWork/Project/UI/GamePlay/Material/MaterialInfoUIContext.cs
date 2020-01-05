using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class MaterialInfoUIContext : WindowBase
    {
        private MaterialInfoUI m_UI;
        private MaterialDataModel _model;

        public override void Awake(params object[] paralist)
        {
            m_UI = UIUtility.SafeGetComponent<MaterialInfoUI>(Transform);
            _model = (MaterialDataModel)paralist[0];
            RefreshMaterial();
        }

        public override void OnShow(params object[] paralist)
        {
            _model = (MaterialDataModel)paralist[0];
            RefreshMaterial();
        }

        public override void OnUpdate()
        {
            Transform.localPosition = InventoryManager.Instance.GetCurrentMousePos();
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        private void RefreshMaterial()
        {
            if (_model.ID == 0)
                return;
            m_UI.Name.text = _model.Name;
            m_UI.BG.sprite = _model.BG;
            m_UI.Desc.text = _model.Desc;
            m_UI.RemainNum.text = PlayerManager.Instance.GetMaterialStoreCount(_model.ID).ToString();
        }

        private void RefreshNum()
        {
            m_UI.RemainNum.text = PlayerManager.Instance.GetMaterialStoreCount(_model.ID).ToString();
        }



    }
}