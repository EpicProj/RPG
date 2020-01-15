using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class AssemblePartInfoUIContext : WindowBase
    {
        private Image _bgImage;
        private Text _titleName;
        private Image _partIcon;
        private Image _typeIcon;
        private Text _typeText;
        private Text _desc;
        private Transform _partPropertyContentTrans;

        private Transform _trans;

        private AssemblePartInfo _info;

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _info = (AssemblePartInfo)paralist[0];
        }

        protected override void InitUIRefrence()
        {
            _trans = this.Transform;
            _bgImage = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(Transform, "Icon"));
            _titleName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(Transform, "Content/Title"));
            _partIcon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(Transform, "Content/PartlBG"));
            _typeIcon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(Transform, "Content/PartlBG/Type/Icon"));
            _typeText= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(Transform, "Content/PartlBG/Type/Text"));
            _desc = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(Transform, "Content/Desc"));
            _partPropertyContentTrans = UIUtility.FindTransfrom(Transform, "Content/PartProperty/Content");
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        public override void OnShow(params object[] paralist)
        {
            _info = (AssemblePartInfo)paralist[0];
            SetUpInfo();
        }

        public override void OnUpdate()
        {
            _trans.localPosition = InventoryManager.Instance.GetCurrentMousePos();
        }

        #endregion

        void SetUpInfo()
        {
            if (_info == null)
                return;
            _titleName.text = _info.customName;
            _partIcon.sprite = _info.typePresetData.partSprite;
            _typeIcon.sprite = _info.typePresetData.TypeIcon;
            _typeText.text = _info.typePresetData.TypeName;
            _desc.text = _info.typePresetData.partDesc;

            RefreshProperty();
        }

        void RefreshProperty()
        {
            foreach (Transform trans in _partPropertyContentTrans)
            {
                trans.gameObject.SetActive(false);
            }

            if (_info == null)
                return;

            int index = 0;
            foreach (var customItem in _info.customDataInfo.propertyDic.Values)
            {
                var trans = _partPropertyContentTrans.GetChild(index);
                UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(trans, "Icon")).sprite = customItem.propertyIcon;
                UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(trans, "Name")).text = customItem.propertyNameText;

                var value = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(trans, "Value"));
                if (customItem.propertyType == 1)
                {
                    ///Fix
                    value.text = customItem.propertyValueMax.ToString();
                }
                else if (customItem.propertyType == 2)
                {
                    value.text = string.Format("{0} ~ {1}", customItem.propertyValueMin.ToString(), customItem.propertyValueMax.ToString());
                }

                trans.gameObject.SetActive(true);
            }

        }

    }
}