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
        private Button _generalBtn;
        private Text _btnText;

        public AssembleChooseItemModel _model;

        /// <summary>
        /// 展示方式
        /// 0=null
        /// 1=查看
        /// 2=选择
        /// </summary>
        byte modeType = 0;
        int configID = 0;

        private const string AssembleShip_EuqipParts_Success_Hint = "AssembleShip_EuqipParts_Success_Hint";
        private const string AssembleChooseItem_EquipParts_Btn = "AssembleChooseItem_EquipParts_Btn";
        private const string AssembleChooseItem_ReEdit_Btn = "AssembleChooseItem_ReEdit_Btn";

        public override void Awake()
        {
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/Icon"));
            _typeIcon= UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/Type/Icon"));
            _typeText= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/Type/Text"));
            _propertyContentTrans = UIUtility.FindTransfrom(transform, "Content/Property/Content");
            _name= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));

            _clickBtn = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(transform, "Content/BG"));
            _generalBtn = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(transform, "Button/ButtonGeneral02/"));
            _btnText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(_generalBtn.transform, "Text"));

        }

        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (AssembleChooseItemModel)model[0];
            modeType = (byte)paramList[0];
            configID = (int)paramList[1];
            InitElement();
        }

        private void InitElement()
        {
            
            _generalBtn.onClick.RemoveAllListeners();

            _icon.sprite = _model.Info.typePresetData.partSprite;
            _typeIcon.sprite = _model.Info.typePresetData.TypeIcon;
            _typeText.text = _model.Info.typePresetData.TypeName;
            _name.text = _model.Info.customName;

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

            RefreshModeType();
        }

        void RefreshModeType()
        {
            if (modeType == 1)
            {
                _generalBtn.onClick.AddListener(OnReEditClick);
                _btnText.text = MultiLanguage.Instance.GetTextValue(AssembleChooseItem_ReEdit_Btn);
            }
            else if(modeType == 2)
            {
                _generalBtn.onClick.AddListener(OnEquipShipPartsClick);
                _btnText.text = MultiLanguage.Instance.GetTextValue(AssembleChooseItem_EquipParts_Btn);
            }
        }

        void OnEquipShipPartsClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Ship_Design_Page, new UIMessage(UIMsgType.Assemble_ShipDesign_PartSelect, new List<object>() {_model.Info.UID ,configID }));
            UIManager.Instance.HideWnd(UIPath.WindowPath.Assemble_Part_Choose_Dialog);

            UIGuide.Instance.ShowGeneralHint(new GeneralHintDialogItem(
                Utility.ParseStringParams(MultiLanguage.Instance.GetTextValue(AssembleShip_EuqipParts_Success_Hint), new string[] { _model.Info.customName }), 1.5f));
        }

        /// <summary>
        /// 重新编辑
        /// </summary>
        void OnReEditClick()
        {

        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
        }
    }
}