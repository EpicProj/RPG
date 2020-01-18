using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssembleShipCard : BaseElement
    {
        private Text _titleName;
        private Text _shipDesc;
        private Image _shipSprite;

        private Text _moduleNum;
        private Text _crewNum;
        private Text _durability;
        private Text _speed;
        private Text _firePower;
        private Text _explore;
        private Text _storage;

        private Transform _chooseEffect;

        private AssembleShipTypePresetModel _model;
        public override void Awake()
        {
            _titleName = transform.FindTransfrom("TitleName").SafeGetComponent<Text>();
            _shipDesc= transform.FindTransfrom("Content/Desc").SafeGetComponent<Text>();
            _shipSprite= transform.FindTransfrom("Content/IconBG/Icon").SafeGetComponent<Image>();

            _moduleNum = transform.FindTransfrom("Content/ShipProperty/Content/ModuleNum/Value").SafeGetComponent<Text>();
            _crewNum = transform.FindTransfrom("Content/ShipProperty/Content/CrewNum/Value").SafeGetComponent<Text>();
            _durability = transform.FindTransfrom("Content/ShipProperty/Content/Durability/Value").SafeGetComponent<Text>();
            _speed = transform.FindTransfrom("Content/ShipProperty/Content/Speed/Value").SafeGetComponent<Text>();
            _firePower = transform.FindTransfrom("Content/ShipProperty/Content/FirePower/Value").SafeGetComponent<Text>();
            _explore = transform.FindTransfrom("Content/ShipProperty/Content/Explore/Value").SafeGetComponent<Text>();
            _storage = transform.FindTransfrom("Content/ShipProperty/Content/Storage/Value").SafeGetComponent<Text>();

            _chooseEffect = transform.FindTransfrom("ChooseEffect");
            _chooseEffect.gameObject.SetActive(false);
            transform.FindTransfrom("Btn").SafeGetComponent<Button>().onClick.AddListener(OnBtnClick);
        }

        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (AssembleShipTypePresetModel)model[0];
            SetUpCard();
        }


        void SetUpCard()
        {
            _titleName.text = _model.PresetInfo.shipClassName;
            _shipDesc.text = _model.PresetInfo.shipClassDesc;
            _shipSprite.sprite = _model.PresetInfo.ShipSprite;

            _moduleNum.text = _model.PresetInfo.ModuleNum.ToString();
            _crewNum.text = _model.PresetInfo._metaData.CrewMax.ToString();
            _durability.text = _model.PresetInfo._metaData.HPBase.ToString();
            _speed.text = _model.PresetInfo._metaData.SpeedBase.ToString();
            _firePower.text = _model.PresetInfo._metaData.FirePowerBase.ToString();
            _storage.text = _model.PresetInfo._metaData.StorageBase.ToString();

            RefreshMaterialCost();
        }

        void RefreshMaterialCost()
        {
            var _materialCostTrans = transform.FindTransfrom("Content/PartCost/Content");
            _materialCostTrans.SafeSetActiveAllChild(false);

            var costList = _model.PresetInfo.shipCostBase;
            for(int i = 0; i < costList.Count; i++)
            {
                if (i > Config.GlobalConfigData.Assemble_MaterialCost_MaxNum)
                    break;
                var cmpt = UIUtility.SafeGetComponent<MaterialCostCmpt>(_materialCostTrans.GetChild(i));
                if (cmpt != null)
                {
                    cmpt.SetUpItem(costList[i]);
                    cmpt.gameObject.SetActive(true);
                }
            }

            transform.FindTransfrom("Content/PartCost/TimeCost/Text").SafeGetComponent<Text>().text = _model.PresetInfo._metaData.BaseTimeCost.ToString();
        }

        void OnBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Ship_Design_Page, new UIMessage(UIMsgType.Assemble_ShipPreset_Select, new List<object>() {_model.ID }));
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            _chooseEffect.gameObject.SetActive(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _chooseEffect.gameObject.SetActive(false);
        }
    }
}