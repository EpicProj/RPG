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
                _durabilityText.text = data.durability_current.ToString();
                _durabilitySlider.value = (data.durability_current / data.durability_max)*100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(data.areaIconPath, Utility.SpriteType.png);
                InitPower(data.powerLevel_max, data.powerLevel_current);

                AddBtnClick(
                    ()=> { AddPowerLevel(data.powerLevel_max, ModifierDetailRootType_Simple.LivingArea); },
                    () => { ReducePowerLevel(ModifierDetailRootType_Simple.LivingArea); });

            }
            else if(areaType == MainShipAreaType.WorkingArea)
            {
                var data = MainShipManager.Instance.mainShipInfo.workingAreaInfo;
                _durabilityText.text = data.durability_current.ToString();
                _durabilitySlider.value = (data.durability_current / data.durability_max) * 100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(data.areaIconPath, Utility.SpriteType.png);
                InitPower(data.powerLevel_max, data.powerLevel_current);

                AddBtnClick(
                  () => { AddPowerLevel(data.powerLevel_max, ModifierDetailRootType_Simple.WorkingArea); },
                  () => { ReducePowerLevel(ModifierDetailRootType_Simple.WorkingArea); });
            }
            else if(areaType == MainShipAreaType.ControlTower)
            {
                var data = MainShipManager.Instance.mainShipInfo.controlTowerInfo;
                _durabilityText.text = data.durability_current.ToString();
                _durabilitySlider.value = (data.durability_current / data.durability_max) * 100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(data.areaIconPath, Utility.SpriteType.png);
                InitPower(data.powerLevel_max, data.powerLevel_current);

                AddBtnClick(
                  () => { AddPowerLevel(data.powerLevel_max, ModifierDetailRootType_Simple.ControlTower); },
                  () => { ReducePowerLevel(ModifierDetailRootType_Simple.ControlTower); });
            }
            else if(areaType == MainShipAreaType.hangar)
            {
                var data = MainShipManager.Instance.mainShipInfo.hangarAreaInfo;
                _durabilityText.text = data.durability_current.ToString();
                _durabilitySlider.value = (data.durability_current / data.durability_max) * 100;
                _energyCostText.text = data.powerConsumeCurrent.ToString();
                transform.FindTransfrom("Left/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(data.areaIconPath, Utility.SpriteType.png);
                InitPower(data.powerLevel_max, data.powerLevel_current);

                AddBtnClick(
                  () => { AddPowerLevel(data.powerLevel_max, ModifierDetailRootType_Simple.Hangar); },
                  () => { ReducePowerLevel(ModifierDetailRootType_Simple.Hangar); });
            }
            else if(areaType == MainShipAreaType.PowerArea)
            {
                ///Power Area
                var data = MainShipManager.Instance.mainShipInfo.powerAreaInfo;
                _durabilityText.text = data.durability_max.ToString();
                _energyLoadText.text = data.energyLoadValue_current.ToString();

                AddMapBtnClick(() =>
                {
                    MapManager.Instance.GeneratePowerAreaContainer();
                });
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

        private void AddPowerLevel(byte maxCount, ModifierDetailRootType_Simple type)
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
            
            if (MainShipManager.Instance.ChangeAreaPowerLevel(1, type))
            {
                ///Change Success
                powerItemList[index - 1].SwitchState(MainShipPowerItem.PowerState.Fill);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_Page, new UIMessage(UIMsgType.MainShip_Area_EnergyLoad_Change));
            }
        }

        private void ReducePowerLevel(ModifierDetailRootType_Simple areaType)
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

        void AddMapBtnClick(UnityAction action)
        {
            var btn = transform.SafeGetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
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
                _energyLoadText.text = MainShipManager.Instance.mainShipInfo.powerAreaInfo.energyLoadValue_current.ToString();
            }
        }

        #endregion
    }
}