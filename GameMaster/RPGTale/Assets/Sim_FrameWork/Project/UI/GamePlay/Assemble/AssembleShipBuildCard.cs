using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssembleShipBuildCard : BaseElement
    {
        private AssembleShipModel _model;


        public override void Awake()
        {
        }

        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (AssembleShipModel)model[0];
            SetUpCart();
        }

        void SetUpCart()
        {
            transform.FindTransfrom("TitleName").SafeGetComponent<Text>().text = _model.Info.shipCustomName;
            transform.FindTransfrom("Content/IconBG/Icon").SafeGetComponent<Image>().sprite = _model.Info.presetData.ShipSprite;
            transform.FindTransfrom("Content/Desc").SafeGetComponent<Text>().text = _model.Info.presetData.shipClassDesc;

            transform.FindTransfrom("Content/ShipProperty/Content/CrewNum/Value").SafeGetComponent<Text>().text = _model.Info.shipCrewMax.ToString();
            transform.FindTransfrom("Content/ShipProperty/Content/Durability/Value").SafeGetComponent<Text>().text = _model.Info.shipDurability.ToString();
            transform.FindTransfrom("Content/ShipProperty/Content/Storage/Value").SafeGetComponent<Text>().text = _model.Info.shipStorage.ToString();

            transform.FindTransfrom("Content/ShipProperty/Content/Speed/Value").SafeGetComponent<Text>().text = _model.Info.shipSpeed.ToString();
            transform.FindTransfrom("Content/ShipProperty/Content/FirePower/Value").SafeGetComponent<Text>().text = _model.Info.shipFirePower.ToString();
            transform.FindTransfrom("Content/ShipProperty/Content/Explore/Value").SafeGetComponent<Text>().text = _model.Info.Explore.ToString();

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