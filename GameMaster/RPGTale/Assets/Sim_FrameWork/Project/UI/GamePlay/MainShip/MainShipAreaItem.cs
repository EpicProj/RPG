using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Sim_FrameWork
{
    public class MainShipAreaItem : BaseElementSimple
    {
        public MainShipAreaType areaType;

        private Text _durabilityText;
        private Slider _durabilitySlider;
        private Text _energyCostText;

        ///PowerArea
        private Text _energyLoadText;

        private List<MainShipPowerItem> powerItemList;

        private const string PowerItemPrefabPath = "Assets/Prefabs/Object/MainShip/MainShipPowerItem.prefab";

        public override void Awake()
        {
            powerItemList = new List<MainShipPowerItem>();
            if(areaType!= MainShipAreaType.PowerArea)
            {
                _durabilityText = transform.FindTransfrom("Left/Value").SafeGetComponent<Text>();
                _durabilitySlider = transform.FindTransfrom("Left/Durability").SafeGetComponent<Slider>();
                _energyCostText = transform.FindTransfrom("Energy/Level").SafeGetComponent<Text>();
            }
            else if(areaType == MainShipAreaType.PowerArea)
            {
                _durabilityText = transform.FindTransfrom("Durability").SafeGetComponent<Text>();
                _energyLoadText = transform.FindTransfrom("EnergyValue").SafeGetComponent<Text>();
            }
        }

        public void InitData()
        {
            UnityAction<byte, short> InitPower = (unlockLevel, initialLevel) =>
            {
                var powerContentTrans = transform.FindTransfrom("Energy/Content");
                byte maxCount = Config.ConfigData.MainShipConfigData.areaEnergyLevelMax;
                powerContentTrans.InitObj(PowerItemPrefabPath, maxCount);

                foreach(Transform trans in powerContentTrans)
                {
                    var item = trans.SafeGetComponent<MainShipPowerItem>();
                    powerItemList.Add(item);
                    item.SwitchState(MainShipPowerItem.PowerState.Lock);
                }
                for(int i= maxCount-1; i > maxCount - unlockLevel-1; i--)
                {
                    powerItemList[i].SwitchState(MainShipPowerItem.PowerState.Empty);
                }
                for(int i = maxCount - 1; i > maxCount - initialLevel - 1; i--)
                {
                    powerItemList[i].SwitchState(MainShipPowerItem.PowerState.Fill);
                }
            };

            powerItemList.Clear();

            if(areaType== MainShipAreaType.LivingArea)
            {
                var data = MainShipManager.Instance.mainShipInfo.livingAreaInfo;
                _durabilityText.text = data.currentDurability.ToString();
                _durabilitySlider.value = (data.currentDurability / data.durabilityMax)*100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(data.areaIconPath, Utility.SpriteType.png);
                InitPower(data.powerLevelMax, data.powerLevelCurrent);

                AddBtnClick(
                    ()=> { AddPowerLevel(data.powerLevelMax, MainShipAreaType.LivingArea); },
                    () => { ReducePowerLevel(MainShipAreaType.LivingArea); });

            }
            else if(areaType == MainShipAreaType.WorkingArea)
            {
                var data = MainShipManager.Instance.mainShipInfo.workingAreaInfo;
                _durabilityText.text = data.currentDurability.ToString();
                _durabilitySlider.value = (data.currentDurability / data.durabilityMax) * 100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(data.areaIconPath, Utility.SpriteType.png);
                InitPower(data.powerLevelMax, data.powerLevelCurrent);

                AddBtnClick(
                  () => { AddPowerLevel(data.powerLevelMax, MainShipAreaType.WorkingArea); },
                  () => { ReducePowerLevel(MainShipAreaType.WorkingArea); });
            }
            else if(areaType == MainShipAreaType.ControlTower)
            {
                var data = MainShipManager.Instance.mainShipInfo.controlTowerInfo;
                _durabilityText.text = data.currentDurability.ToString();
                _durabilitySlider.value = (data.currentDurability / data.durabilityMax) * 100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(data.areaIconPath, Utility.SpriteType.png);
                InitPower(data.powerLevelMax, data.powerLevelCurrent);

                AddBtnClick(
                  () => { AddPowerLevel(data.powerLevelMax, MainShipAreaType.ControlTower); },
                  () => { ReducePowerLevel(MainShipAreaType.ControlTower); });
            }
            else if(areaType == MainShipAreaType.hangar)
            {
                var data = MainShipManager.Instance.mainShipInfo.hangarAreaInfo;
                _durabilityText.text = data.currentDurability.ToString();
                _durabilitySlider.value = (data.currentDurability / data.durabilityMax) * 100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(data.areaIconPath, Utility.SpriteType.png);
                InitPower(data.powerLevelMax, data.powerLevelCurrent);

                AddBtnClick(
                  () => { AddPowerLevel(data.powerLevelMax, MainShipAreaType.hangar); },
                  () => { ReducePowerLevel(MainShipAreaType.hangar); });
            }
            else if(areaType == MainShipAreaType.PowerArea)
            {
                ///Power Area
                var data = MainShipManager.Instance.mainShipInfo.powerAreaInfo;
                _durabilityText.text = data.durabilityMax.ToString();
                _energyLoadText.text = data.EnergyLoadValueCurrent.ToString();
            }
        }

        public void ChangePowerMax(byte currentLevelMax)
        {
            var maxIndex = Config.ConfigData.MainShipConfigData.areaEnergyLevelMax - currentLevelMax - 1;
            for(int i = Config.ConfigData.MainShipConfigData.areaEnergyLevelMax-1; i >maxIndex ; i--)
            {
                var item = powerItemList[i];
                if(item.currentState== MainShipPowerItem.PowerState.Lock)
                {
                    item.SwitchState(MainShipPowerItem.PowerState.Empty);
                }
            }
        }

        private void AddPowerLevel(byte maxCount, MainShipAreaType areaType)
        {
            var powerContentTrans = transform.FindTransfrom("Energy/Content");
            int index = 0;

            for (int i = 0; i < powerItemList.Count; i++)
            {
                if (powerItemList[i].currentState == MainShipPowerItem.PowerState.Fill)
                    break;
                index++;
            }
            if (index < Config.ConfigData.MainShipConfigData.areaEnergyLevelMax - maxCount + 1)
                return;
            
            if (MainShipManager.Instance.ChangeAreaPowerLevel(1, areaType))
            {
                ///Change Success
                powerItemList[index - 1].SwitchState(MainShipPowerItem.PowerState.Fill);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_Page, new UIMessage(UIMsgType.MainShip_Area_EnergyLoad_Change));
            }
        }

        private void ReducePowerLevel(MainShipAreaType areaType)
        {
            var powerContentTrans = transform.FindTransfrom("Energy/Content");
            int index = 0;
            for (int i = 0; i < powerItemList.Count; i++)
            {
                if (powerItemList[i].currentState == MainShipPowerItem.PowerState.Fill)
                    break;
                index++;
            }
            if (index > Config.ConfigData.MainShipConfigData.areaEnergyLevelMax - 1)
                return;
            if (MainShipManager.Instance.ChangeAreaPowerLevel(-1, areaType))
            {
                ///Change Success
                powerItemList[index].SwitchState(MainShipPowerItem.PowerState.Empty);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_Page, new UIMessage(UIMsgType.MainShip_Area_EnergyLoad_Change));
            }
        }

        void AddBtnClick(UnityAction add,UnityAction reduce)
        {
            var addBtn = transform.FindTransfrom("AddBtn").SafeGetComponent<Button>();
            var reduceBtn = transform.FindTransfrom("ReduceBtn").SafeGetComponent<Button>();
            addBtn.onClick.RemoveAllListeners();
            reduceBtn.onClick.RemoveAllListeners();

            addBtn.onClick.AddListener(add);
            reduceBtn.onClick.AddListener(reduce);
        }

        /// <summary>
        /// Change Energy Cost
        /// </summary>
        /// <param name="type"></param>
        public void ChangePowerConsumeValue(MainShipAreaType type )
        {
            if(type== MainShipAreaType.ControlTower)
            {
                _energyCostText.text = MainShipManager.Instance.mainShipInfo.controlTowerInfo.powerConsumeCurrent.ToString();
            }else if(type == MainShipAreaType.hangar)
            {
                _energyCostText.text = MainShipManager.Instance.mainShipInfo.hangarAreaInfo.powerConsumeCurrent.ToString();
            }else if(type == MainShipAreaType.LivingArea)
            {
                _energyCostText.text = MainShipManager.Instance.mainShipInfo.livingAreaInfo.powerConsumeCurrent.ToString();
            }else if(type== MainShipAreaType.WorkingArea)
            {
                _energyCostText.text = MainShipManager.Instance.mainShipInfo.workingAreaInfo.powerConsumeCurrent.ToString();
            }
        }


        #region PowerArea
        public void ChangeEnergyLoadValue()
        {
            if(areaType == MainShipAreaType.PowerArea)
            {
                _energyLoadText.text = MainShipManager.Instance.mainShipInfo.powerAreaInfo.EnergyLoadValueCurrent.ToString();
            }
        }

        #endregion
    }
}