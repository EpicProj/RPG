using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class MainShipAreaItem : BaseElementSimple
    {
        public MainShipAreaType areaType;

        private Text _durabilityText;
        private Slider _durabilitySlider;
        private Text _energyCostText;

        public override void Awake()
        {
            if(areaType!= MainShipAreaType.PowerArea)
            {
                _durabilityText = transform.FindTransfrom("Left/Value").SafeGetComponent<Text>();
                _durabilitySlider = transform.FindTransfrom("Left/Durability").SafeGetComponent<Slider>();
                _energyCostText = transform.FindTransfrom("Energy/Level").SafeGetComponent<Text>();
            }
            else if(areaType == MainShipAreaType.PowerArea)
            {

            }
        }

        public void InitData()
        {

            if(areaType== MainShipAreaType.LivingArea)
            {
                var data = PlayerManager.Instance.mainShipInfo.livingAreaInfo;
                _durabilityText.text = data.currentDurability.ToString();
                _durabilitySlider.value = (data.currentDurability / data.durabilityMax)*100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = data.areaIcon;
            }
            else if(areaType == MainShipAreaType.WorkingArea)
            {
                var data = PlayerManager.Instance.mainShipInfo.workingAreaInfo;
                _durabilityText.text = data.currentDurability.ToString();
                _durabilitySlider.value = (data.currentDurability / data.durabilityMax) * 100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = data.areaIcon;
            }
            else if(areaType == MainShipAreaType.ControlTower)
            {
                var data = PlayerManager.Instance.mainShipInfo.controlTowerInfo;
                _durabilityText.text = data.currentDurability.ToString();
                _durabilitySlider.value = (data.currentDurability / data.durabilityMax) * 100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = data.areaIcon;
            }
            else if(areaType == MainShipAreaType.hangar)
            {
                var data = PlayerManager.Instance.mainShipInfo.hangarAreaInfo;
                _durabilityText.text = data.currentDurability.ToString();
                _durabilitySlider.value = (data.currentDurability / data.durabilityMax) * 100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = data.areaIcon;
            }
           
        }

    }
}