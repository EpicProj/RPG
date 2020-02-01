using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sim_FrameWork.UI
{
    public partial class MainMenuPageContext : WindowBase
    {
        
        private float currentTimeProgress = 0f;
        private List<BuildingPanelData> buildPanelDataList = new List<BuildingPanelData>();
        private List<ConstructMainTabElement> mainTabElementList = new List<ConstructMainTabElement>();

        private List<MainShipAreaItem> mainShipAreaItemList = new List<MainShipAreaItem>();
        //GameStates
        private Button PauseBtn;


        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake();
            AddBtnListener();
            InitMainShipAreaItem();
        }


        public override void OnUpdate()
        {
            UpdateTimeProgress();
        }

        public override void OnShow(params object[] paralist)
        {
            UpdateTimePanel();
            RefreshAreaState();
        }

        public override void OnClose()
        {
            base.OnClose();
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.GameAreaStateChange:
                    RefreshAreaState();
                    return true;
                case UIMsgType.MainShip_Area_EnergyLoad_Change:
                    UpdateEnergyLoad();
                    return true;
                case UIMsgType.MainShip_Area_PowerLevel_Change:
                    return UpdateAreaEnergyCost((MainShipAreaType)msg.content[0]);
                case UIMsgType.MenuPage_Update_BuildPanel:
                    FunctionBlockTypeData typeData = (FunctionBlockTypeData)msg.content[0];
                    var type = FunctionBlockModule.GetBlockType(typeData);
                    return RefreshBuildMainPanel(type);
                case UIMsgType.UpdateTime:
                    return UpdateTimePanel();
                default:
                    return false;
            }
        }

        #endregion

        void RefreshAreaState()
        {
            if (GameManager.Instance.currentAreaState == AreaState.OutSide)
            {
                Transform.FindTransfrom("MainShipGeneral").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(true);
                Transform.FindTransfrom("ConstructPanel").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(false);
                for (int i = 0; i < mainShipAreaItemList.Count; i++)
                {
                    mainShipAreaItemList[i].InitData();
                }
            }
            else if (GameManager.Instance.currentAreaState == AreaState.MainShip_PowerArea)
            {
                Transform.FindTransfrom("MainShipGeneral").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(false);
                Transform.FindTransfrom("ConstructPanel").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(true);
                RefreshBuildMainTab();
            }
        }


        private bool UpdateTimePanel()
        {
            var timedata = PlayerManager.Instance.playerData.timeData;
            if (timedata == null)
                return false;
            currentTimeText.text = string.Format("{0} . {1} . {2}", timedata.date.Year, timedata.date.Month, timedata.date.Day);
            return true;
        }
        private void UpdateTimeProgress()
        {
            if (GameManager.Instance.gameStates == GameStates.Pause)
                return;
            currentTimeProgress+=Time.deltaTime;
            if (currentTimeProgress >= PlayerManager.Instance.playerData.timeData.realSecondsPerDay)
            {
                currentTimeProgress = 0;
            }
            timeSlider.value = currentTimeProgress / PlayerManager.Instance.playerData.timeData.realSecondsPerDay;
        }

       
        //Button
        private void AddBtnListener()
        {
            AddButtonClickListener(PauseBtn, () =>
            {
                OnPauseBtnClick();
            });
            /// Order Receive Page
            AddButtonClickListener(Transform.FindTransfrom("ButtonTab/Order").SafeGetComponent<Button>(), () =>
            {
                UIGuide.Instance.ShowOrderReceiveMainPage();
            });

            AddButtonClickListener(Transform.FindTransfrom("ButtonTab/Research").SafeGetComponent<Button>(), () =>
            {
                UIGuide.Instance.ShowTechnologyMainPage();
            });
            AddButtonClickListener(Transform.FindTransfrom("ButtonTab/Explore").SafeGetComponent<Button>(), () =>
            {
                UIGuide.Instance.ShowExploreMainPage();
            });
            AddButtonClickListener(Transform.FindTransfrom("ButtonTab/Assemble").SafeGetComponent<Button>(), () =>
            {
                AssemblePartInfo info = new AssemblePartInfo();
                info.InitData(1);
                UIGuide.Instance.ShowAssemblePartDesignPage(info);
            });
            AddButtonClickListener(Transform.FindTransfrom("ButtonTab/ShipDesign").SafeGetComponent<Button>(), () =>
            {
                AssembleShipInfo info = new AssembleShipInfo();
                info.InitData(1);
                UIGuide.Instance.ShowAssembleShipDesignPage(info);
            });
            
        }

        private void OnPauseBtnClick()
        {
            if (GameManager.Instance.gameStates == GameStates.Start)
            {
                GameManager.Instance.SetGameStates(GameStates.Pause);
            }
            else if (GameManager.Instance.gameStates == GameStates.Pause)
            {
                GameManager.Instance.SetGameStates(GameStates.Start);
            }
        }

        #region BuildPanel

        /// <summary>
        /// 刷新建造面板
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool RefreshBuildMainPanel(FunctionBlockType type)
        {
            if (type == FunctionBlockType.None)
                return false;
            var loopList = Transform.FindTransfrom("ConstructPanel/Content/BuildContent/Scroll View").SafeGetComponent<LoopList>();
            var list =PlayerManager.Instance.GetBuildPanelModelData(type);
            loopList.InitData(list);

            return true;
        }

        /// <summary>
        /// 初始化建造主页签
        /// </summary>
        public void RefreshBuildMainTab()
        {
            var buildtab = Transform.FindTransfrom("ConstructPanel/TabPanel");

            List<FunctionBlockTypeData> mainTypeList = FunctionBlockModule.GetInitMainType();
            var toggleGroup = buildtab.SafeGetComponent<ToggleGroup>();

            buildtab.InitObj(UIPath.PrefabPath.Construct_MainTab_Element_Path, mainTypeList.Count);
            for(int i = 0; i < buildtab.childCount; i++)
            {
                ConstructMainTabElement element = buildtab.GetChild(i).SafeGetComponent<ConstructMainTabElement>();
                if (toggleGroup != null)
                    element.toggle.group = toggleGroup;
                element.InitMainTabElement(mainTypeList[i]);
                element.transform.name = mainTypeList[i].Type.ToString();
                mainTabElementList.Add(element);
            }
            ///Set Default Select
            if (mainTabElementList.Count != 0)
            {
                mainTabElementList[0].toggle.isOn = true;
            }
        }

        #endregion

        #region MainShip
        private void UpdateEnergyLoad()
        {
            var item = mainShipAreaItemList.Find(x => x.areaType == MainShipAreaType.PowerArea);
            if (item != null)
                item.ChangeEnergyLoadValue();
        }

        private bool UpdateAreaEnergyCost(MainShipAreaType type)
        {
            if(type== MainShipAreaType.ControlTower)
            {
                var item = mainShipAreaItemList.Find(x => x.areaType == MainShipAreaType.ControlTower);
                if (item != null)
                {
                    item.ChangePowerConsumeValue(MainShipAreaType.ControlTower);
                    return true;
                }
                return false;
            }
            else if(type== MainShipAreaType.hangar)
            {
                var item = mainShipAreaItemList.Find(x => x.areaType == MainShipAreaType.hangar);
                if (item != null)
                {
                    item.ChangePowerConsumeValue(MainShipAreaType.hangar);
                    return true;
                }
                return false;
            }
            else if (type == MainShipAreaType.WorkingArea)
            {
                var item = mainShipAreaItemList.Find(x => x.areaType == MainShipAreaType.WorkingArea);
                if (item != null)
                {
                    item.ChangePowerConsumeValue(MainShipAreaType.WorkingArea);
                    return true;
                }
                return false;
            }
            else if (type == MainShipAreaType.LivingArea)
            {
                var item = mainShipAreaItemList.Find(x => x.areaType == MainShipAreaType.LivingArea);
                if (item != null)
                {
                    item.ChangePowerConsumeValue(MainShipAreaType.LivingArea);
                    return true;
                }
                return false;
            }
            return false;
        }

        #endregion

    }

    public partial class MainMenuPageContext : WindowBase
    {
        //Time Data
        private Text currentTimeText;
        private Slider timeSlider;


        protected override void InitUIRefrence()
        {
            currentTimeText = Transform.FindTransfrom("Time/Time/Text").SafeGetComponent<Text>();
            timeSlider = Transform.FindTransfrom("Time/Slider").SafeGetComponent<Slider>();
        }
        void InitMainShipAreaItem()
        {
            mainShipAreaItemList.Add(Transform.FindTransfrom("MainShipGeneral/AreaPanel/PowerArea").SafeGetComponent<MainShipAreaItem>());
            mainShipAreaItemList.Add(Transform.FindTransfrom("MainShipGeneral/AreaPanel/Content/ControlTower").SafeGetComponent<MainShipAreaItem>());
            mainShipAreaItemList.Add(Transform.FindTransfrom("MainShipGeneral/AreaPanel/Content/LivingArea").SafeGetComponent<MainShipAreaItem>());
            mainShipAreaItemList.Add(Transform.FindTransfrom("MainShipGeneral/AreaPanel/Content/WorkingArea").SafeGetComponent<MainShipAreaItem>());
            mainShipAreaItemList.Add(Transform.FindTransfrom("MainShipGeneral/AreaPanel/Content/Hangar").SafeGetComponent<MainShipAreaItem>());
            mainShipAreaItemList.Add(Transform.FindTransfrom("MainShipGeneral/AreaPanel/Content/EngineArea").SafeGetComponent<MainShipAreaItem>());
        }
    }
}