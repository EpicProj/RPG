using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssembleShipBuildCard : BaseElement
    {
        private Text _titleName;
        private Image _sprite;
        private Text _desc;

        private Text _crewNum;
        private Text _durability;
        private Text _speed;
        private Text _firePower;
        private Text _explore;
        private Text _storage;

        private AssembleShipModel _model;


        public override void Awake()
        {
            _titleName = transform.FindTransfrom("TitleName").SafeGetComponent<Text>();
            _desc = transform.FindTransfrom("Content/Desc").SafeGetComponent<Text>();
            _sprite = transform.FindTransfrom("Content/IconBG/Icon").SafeGetComponent<Image>();

            _crewNum = transform.FindTransfrom("Content/ShipProperty/Content/CrewNum/Value").SafeGetComponent<Text>();
            _durability = transform.FindTransfrom("Content/ShipProperty/Content/Durability/Value").SafeGetComponent<Text>();
            _speed = transform.FindTransfrom("Content/ShipProperty/Content/Speed/Value").SafeGetComponent<Text>();
            _firePower = transform.FindTransfrom("Content/ShipProperty/Content/FirePower/Value").SafeGetComponent<Text>();
            _explore = transform.FindTransfrom("Content/ShipProperty/Content/Explore/Value").SafeGetComponent<Text>();
            _storage = transform.FindTransfrom("Content/ShipProperty/Content/Storage/Value").SafeGetComponent<Text>();
        }

        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (AssembleShipModel)model[0];
            SetUpCart();
        }

        void SetUpCart()
        {
            _titleName.text = _model.Info.shipCustomName;
            _sprite.sprite = _model.Info.presetData.ShipSprite;
            _desc.text = _model.Info.presetData.shipClassDesc;

            _crewNum.text = _model.Info.shipCrewMax.ToString();
            _durability.text = _model.Info.shipDurability.ToString();
            _storage.text = _model.Info.shipStorage.ToString();
            //_speed.text = _model.Info.shipSpeed.ToString();
            //_firePower.text = _model.Info.shipFirePower.ToString();

            RefreshShipBuildCost();

            transform.FindTransfrom("BtnConfirm").SafeGetComponent<Button>().onClick.AddListener(OnConfirmBtnClick);
            transform.FindTransfrom("BtnReDesign").SafeGetComponent<Button>().onClick.AddListener(OnConfirmBtnClick);
        }

        void RefreshShipBuildCost()
        {
            var contentTrans = transform.FindTransfrom("Content/PartCost/Content");
            contentTrans.SafeSetActiveAllChild(false);
            for (int i = 0; i < _model.Info.presetData.shipCostBase.Count;i++)
            {
                if (i > Config.GlobalConfigData.Assemble_MaterialCost_MaxNum)
                    break;
                var item = contentTrans.GetChild(i).SafeGetComponent<MaterialCostCmpt>();
                if (item != null)
                {
                    item.SetUpItem(_model.Info.presetData.shipCostBase[i]);
                    item.transform.SafeSetActive(true);
                }
            }
            ///Init Time
            transform.FindTransfrom("Content/PartCost/TimeCost/Text").SafeGetComponent<Text>().text = _model.Info.presetData._metaData.BaseTimeCost.ToString();
        }

        void OnConfirmBtnClick()
        {

        }

        void OnReDesignBtnClick()
        {

        }
    }
}