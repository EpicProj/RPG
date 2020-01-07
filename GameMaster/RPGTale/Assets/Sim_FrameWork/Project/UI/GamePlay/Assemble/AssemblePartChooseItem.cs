using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssemblePartChooseItem : BaseElement
    {
        private Image _icon;
        private Image _typeIcon;
        private Text _typeText;
        private Transform _propertyContentTrans;
        private Text _name;

        private Button _clickBtn;
        private Button _detailBtn;


        public AssembleChooseItemModel _model;

        public override void Awake()
        {
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/Icon"));
            _typeIcon= UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/Type/Icon"));
            _typeText= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/Type/Text"));
            _propertyContentTrans = UIUtility.FindTransfrom(transform, "Content/Property/Content");
            _name= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));

            _clickBtn = UIUtility.SafeGetComponent<Button>(transform);
            _detailBtn= UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(transform, "Detail"));
        }

        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (AssembleChooseItemModel)model[0];
            InitElement();
        }

        private void InitElement()
        {
            _clickBtn.onClick.RemoveAllListeners();
            _detailBtn.onClick.RemoveAllListeners();

            _icon.sprite = _model.Info.typePresetData.partSprite;
            _typeIcon.sprite = _model.Info.typePresetData.TypeIcon;
            _typeText.text = _model.Info.typePresetData.TypeName;
            _name.text = _model.Info.typePresetData.partName;

            foreach(Transform trans in _propertyContentTrans)
            {
                trans.gameObject.SetActive(false);
            }

            var propertyDic = _model.Info.customDataInfo.propertyDic;

            int i = 0;
            foreach(var data in propertyDic.Values)
            {
                if (i >= Config.GlobalConfigData.AssemblePart_Max_PropertyNum)
                    break;
                Transform propertyItem = _propertyContentTrans.GetChild(i);
                UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(propertyItem, "Icon")).sprite = data.propertyIcon;
                UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(propertyItem, "Name")).text = data.propertyNameText;
                UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(propertyItem, "Value")).text = string.Format("{0} ~ {1}", data.propertyValueMin.ToString(), data.propertyValueMax.ToString());
                propertyItem.gameObject.SetActive(true);
                i++;
            }

        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
        }
    }
}