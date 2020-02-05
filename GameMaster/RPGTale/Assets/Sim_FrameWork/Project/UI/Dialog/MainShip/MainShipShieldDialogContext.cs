using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class MainShipShieldDialogContext : WindowBase
    {
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            SetUpDialog();
        }

        void SetUpDialog()
        {
            _totalEnergyLevelText.text = MainShipManager.Instance.mainShipInfo.shieldEnergy_Max_current.ToString();
        }



    }


    public partial class MainShipShieldDialogContext : WindowBase
    {
        private Text _totalEnergyLevelText;
        private Text _totalEnergyCostText;
        protected override void InitUIRefrence()
        {
            _totalEnergyLevelText = Transform.FindTransfrom("Content/Content/GeneralInfoContent/LeftInfo/EnergyTotal/Value").SafeGetComponent<Text>();
            _totalEnergyCostText= Transform.FindTransfrom("Content/Content/GeneralInfoContent/LeftInfo/EnergyCost/Value").SafeGetComponent<Text>();
        }
    }
}