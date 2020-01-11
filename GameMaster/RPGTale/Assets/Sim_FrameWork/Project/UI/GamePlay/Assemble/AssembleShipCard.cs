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

        private Transform _materialCostTrans;
        private Transform _chooseEffect;
        private Button _btn;

        private AssembleShipTypePresetModel _model;
        public override void Awake()
        {
            _titleName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "TitleName"));
            _shipDesc= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/Desc"));
            _shipSprite= UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/IconBG/Icon"));

            _moduleNum = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/ShipProperty/Content/ModuleNum/Value"));
            _crewNum = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/ShipProperty/Content/CrewNum/Value"));
            _durability = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/ShipProperty/Content/Durability/Value"));
            _speed = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/ShipProperty/Content/Speed/Value"));
            _firePower = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/ShipProperty/Content/FirePower/Value"));
            _explore = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/ShipProperty/Content/Explore/Value"));
            _storage = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/ShipProperty/Content/Storage/Value"));

            _btn = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(transform, "Btn"));
            _materialCostTrans = UIUtility.FindTransfrom(transform, "Content/PartCost/Content");
            _chooseEffect = UIUtility.FindTransfrom(transform, "ChooseEffect");
            _chooseEffect.gameObject.SetActive(false);
            _btn.onClick.AddListener(OnBtnClick);
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
            foreach(Transform trans in _materialCostTrans)
            {
                trans.gameObject.SetActive(false);
            }

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