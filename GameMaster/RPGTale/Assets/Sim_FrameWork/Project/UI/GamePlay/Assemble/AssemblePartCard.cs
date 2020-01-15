using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssemblePartCard : BaseElement
    {

        private Text _titleName;
        private Image _partImage;
        private Text _partDesc;
        private Transform _partPropertyTrans;
        private Transform _partEquipTargetTrans;
        private Transform _chooseEffect;
        private Transform _partCostTrans;

        private Button _btn;

        private AssembleTypePresetModel _model;
        public override void Awake()
        {
            _titleName = transform.FindTransfrom("TitleName").SafeGetComponent<Text>();
            _partDesc = transform.FindTransfrom("Content/Desc").SafeGetComponent<Text>();
            _partImage= transform.FindTransfrom("Content/IconBG/Icon").SafeGetComponent<Image>();
            _partPropertyTrans = transform.FindTransfrom("Content/PartProperty/Content");
            _partEquipTargetTrans = transform.FindTransfrom("Content/PartEquipTarget/Content");
            _chooseEffect = transform.FindTransfrom("ChooseEffect");
            _partCostTrans = transform.FindTransfrom("Content/PartCost/Content");
            _chooseEffect.SafeSetActive(false);

            _btn = transform.FindTransfrom("Btn").SafeGetComponent<Button>();
            _btn.onClick.AddListener(OnBtnClick);
        }

        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (AssembleTypePresetModel)model[0];
            SetUpElement();
        }


        void SetUpElement()
        {
            _titleName.text = _model.PresetInfo.partName;
            _partImage.sprite = _model.PresetInfo.partSprite;
            _partDesc.text = _model.PresetInfo.partDesc;

            RefreshPartProperty();
            RefreshPartEquipTarget();
            RefreshPartCost();
        }

        void RefreshPartProperty()
        {
            _partPropertyTrans.SafeSetActiveAllChild(false);

            for (int i = 0; i < _model.PresetInfo.partsPropertyConfig.configData.Count; i++)
            {
                if (i > Config.GlobalConfigData.AssemblePart_Max_PropertyNum)
                    break;
                var data = _model.PresetInfo.partsPropertyConfig.configData[i];
                var trans = _partPropertyTrans.GetChild(i);

                var typeData = AssembleModule.GetAssemblePartPropertyTypeData(data.Name);
                if (typeData != null)
                {
                    trans.FindTransfrom("Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(typeData.PropertyIcon, Utility.SpriteType.png);
                    trans.FindTransfrom("Name").SafeGetComponent<Text>().text = MultiLanguage.Instance.GetTextValue(typeData.PropertyName);
                }
                if (data.PropertyType == 1)
                {
                    ///Set Value Fix
                    trans.FindTransfrom("Value").SafeGetComponent<Text>().text = data.PropertyValue.ToString();
                }
                else if (data.PropertyType == 2)
                {
                    ///Set Value Range
                    trans.FindTransfrom("Value").SafeGetComponent<Text>().text = string.Format("{0} ~ {1}", data.PropertyRangeMin.ToString(), data.PropertyRangeMax.ToString());
                }

               
                trans.gameObject.SetActive(true);
            }
        }

        void RefreshPartEquipTarget()
        {
            _partEquipTargetTrans.SafeSetActiveAllChild(false);

            var equipType = AssembleModule.GetAssemblePartEquipType(_model.ID);
            for (int i = 0; i < equipType.Count; i++)
            {
                if (i > Config.GlobalConfigData.AssemblePart_Target_MaxNum)
                    break;
                var equipData = AssembleModule.GetAssembleMainTypeData(equipType[i]);
                if (equipData != null)
                {
                    var trans = _partEquipTargetTrans.GetChild(i);
                    trans.FindTransfrom("Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(equipData.IconPath, Utility.SpriteType.png);
                    trans.FindTransfrom("Name").SafeGetComponent<Text>().text = MultiLanguage.Instance.GetTextValue(equipData.TypeNameText);
                    trans.gameObject.SetActive(true);
                }
            }
        }

        void RefreshPartCost()
        {
            _partCostTrans.SafeSetActiveAllChild(false);

             var costList = AssembleModule.GetPartMaterialCost(_model.ID);
            for(int i = 0; i < costList.Count; i++)
            {
                if (i > Config.GlobalConfigData.Assemble_MaterialCost_MaxNum)
                    break;
                var cmpt = UIUtility.SafeGetComponent<MaterialCostCmpt>(_partCostTrans.GetChild(i));
                if (cmpt != null)
                {
                    cmpt.SetUpItem(costList[i]);
                    cmpt.gameObject.SetActive(true);
                }
            }

        }

        void OnBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Part_Design_Page, new UIMessage(UIMsgType.Assemble_PartPreset_Select, new List<object>() { _model.ID }));
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            _chooseEffect.SafeSetActive(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _chooseEffect.SafeSetActive(false);
        }

    }
}