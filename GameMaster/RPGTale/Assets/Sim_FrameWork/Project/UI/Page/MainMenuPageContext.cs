using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sim_FrameWork.UI
{
    public partial class MainMenuPageContext : WindowBase
    {
        public enum ResourceType
        {
            All,
            Currency,
            Research,
            Energy,
            Builder,
            RoCore,
        }

        private float currentTimeProgress = 0f;
        private List<BuildingPanelData> buildPanelDataList = new List<BuildingPanelData>();
        private List<ConstructMainTabElement> mainTabElementList = new List<ConstructMainTabElement>();

        //GameStates
        private Button PauseBtn;


        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake();
            InitResImage();
            AddBtnListener();
        }


        public override void OnUpdate()
        {
            UpdateTimeProgress();
        }

        public override void OnShow(params object[] paralist)
        {
            UpdateTimePanel();
            UpdateResData(ResourceType.All);
            UpdateResMonthData(ResourceType.All);
            if(GameManager.Instance.currentAreaState == GameManager.AreaState.OutSide)
            {
                RefreshBuildMainTab();
            }
            else if(GameManager.Instance.currentAreaState== GameManager.AreaState.MainShipInside)
            {

            }
           
            
        }

        public override void OnClose()
        {
            base.OnClose();
        }


        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.Res_Currency:         
                    return UpdateResData(ResourceType.Currency);
                case UIMsgType.Res_MonthCurrency:
                    return UpdateResMonthData(ResourceType.Currency);
                case UIMsgType.Res_Research:
                    return UpdateResData(ResourceType.Research);
                case UIMsgType.Res_MonthResearch:
                    return UpdateResMonthData(ResourceType.Research);
                case UIMsgType.Res_Energy:
                    return UpdateResData(ResourceType.Energy);
                case UIMsgType.Res_MonthEnergy:
                    return UpdateResMonthData(ResourceType.Energy);
                case UIMsgType.Res_Builder:
                    return UpdateResData(ResourceType.Builder);
                case UIMsgType.Res_RoCore:
                    return UpdateResData(ResourceType.RoCore);
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
            if (GameManager.Instance.gameStates == GameManager.GameStates.Pause)
                return;
            currentTimeProgress+=Time.deltaTime;
            if (currentTimeProgress >= PlayerManager.Instance.playerData.timeData.realSecondsPerDay)
            {
                currentTimeProgress = 0;
            }
            timeSlider.value = currentTimeProgress / PlayerManager.Instance.playerData.timeData.realSecondsPerDay;
        }

        /// <summary>
        /// 更新当前资源
        /// </summary>
        /// <param name="type"></param>
        public bool UpdateResData(ResourceType type)
        {
            var data = PlayerManager.Instance.playerData.resourceData;
            if (data == null)
                return false;
            switch (type)
            {
                case ResourceType.All:
                    _currencyNumText.text = data.Currency.ToString();
                    _researchPointText.text = data.Research.ToString();
                    _energyNumText.text = data.Energy.ToString();
                    _builderText.text = data.Builder.ToString();
                    _roCoreText.text = data.RoCore.ToString();
                    return true;
                case ResourceType.Currency:
                    _currencyNumText.text = data.Currency.ToString();
                    return true;
                case ResourceType.Research:
                    _researchPointText.text = data.Research.ToString();
                    return true;
                case ResourceType.Energy:
                    _energyNumText.text = data.Energy.ToString();
                    return true;
                case ResourceType.Builder:
                    _builderText.text = data.Builder.ToString();
                    return true;
                case ResourceType.RoCore:
                    _roCoreText.text = data.RoCore.ToString();
                    return true;
                default:
                    return false;
            }   
        }
        /// <summary>
        /// 更新当前资源增加值
        /// </summary>
        /// <param name="type"></param>
        public bool UpdateResMonthData(ResourceType type)
        {
            var data = PlayerManager.Instance.playerData.resourceData;
            if (data == null)
                return false;
            switch (type)
            {
                case ResourceType.All:
                    _energyAddNumText.text = "+" + data.EnergyPerMonth.ToString();
                    _currencyAddNumText.text = "+" + data.CurrencyPerMonth.ToString();
                    return true;
                case ResourceType.Energy:
                    _energyAddNumText.text = "+" + data.EnergyPerMonth.ToString();
                    return true;
                case ResourceType.Research:
                    _researchPointAddText.text = "+" + data.ResearchPerMonth.ToString();
                    return true;
                default:
                    return false;
            }
        }

        //Button
        private void AddBtnListener()
        {
            AddButtonClickListener(PauseBtn, () =>
            {
                OnPauseBtnClick();
            });
            AddButtonClickListener(Transform.FindTransfrom("TopPanel/Menu").SafeGetComponent<Button>(), () =>
            {
                UIGuide.Instance.ShowMenuDialog();
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
                AssemblePartInfo info = new AssemblePartInfo(1);
                UIGuide.Instance.ShowAssemblePartDesignPage(info);
            });
            AddButtonClickListener(Transform.FindTransfrom("ButtonTab/ShipDesign").SafeGetComponent<Button>(), () =>
            {
                AssembleShipInfo info = new AssembleShipInfo(1);
                UIGuide.Instance.ShowAssembleShipDesignPage(info);
            });
            
        }

        private void OnPauseBtnClick()
        {
            if (GameManager.Instance.gameStates == GameManager.GameStates.Start)
            {
                GameManager.Instance.SetGameStates(GameManager.GameStates.Pause);
            }
            else if (GameManager.Instance.gameStates == GameManager.GameStates.Pause)
            {
                GameManager.Instance.SetGameStates(GameManager.GameStates.Start);
            }
        }

        #region BuildPanel

        /// <summary>
        /// 刷新建造面板
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool RefreshBuildMainPanel(FunctionBlockType.Type type)
        {
            if (type == FunctionBlockType.Type.None)
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

    }

    public partial class MainMenuPageContext : WindowBase
    {
        //Time Data
        private Text currentTimeText;
        private Slider timeSlider;

        /// <summary>
        /// Resource
        /// </summary>
        private Text _currencyNumText;
        private Text _currencyAddNumText;
        private Image _currencyIcon;

        private Text _researchPointText;
        private Text _researchPointAddText;
        private Image _researchIcon;

        private Text _energyNumText;
        private Text _energyAddNumText;
        private Image _energyIcon;

        private Text _builderText;
        private Image _builderIcon;

        private Text _roCoreText;
        private Image _roCoreIcon;

        protected override void InitUIRefrence()
        {
            //Resource
            _currencyNumText = Transform.FindTransfrom("TopPanel/Resource/ResourceLeft/Currency/Value").SafeGetComponent<Text>();
            _currencyAddNumText = Transform.FindTransfrom("TopPanel/Resource/ResourceLeft/Currency/Value/AddValue").SafeGetComponent<Text>();
            _currencyIcon= Transform.FindTransfrom("TopPanel/Resource/ResourceLeft/Currency/Icon").SafeGetComponent<Image>();

            _researchPointText = Transform.FindTransfrom("TopPanel/Resource/ResourceLeft/Research/Value").SafeGetComponent<Text>();
            _researchPointAddText = Transform.FindTransfrom("TopPanel/Resource/ResourceLeft/Research/Value/AddValue").SafeGetComponent<Text>();
            _researchIcon= Transform.FindTransfrom("TopPanel/Resource/ResourceLeft/Research/Icon").SafeGetComponent<Image>();

            _energyNumText = Transform.FindTransfrom("TopPanel/Resource/ResourceLeft/Energy/Value").SafeGetComponent<Text>();
            _energyAddNumText = Transform.FindTransfrom("TopPanel/Resource/ResourceLeft/Energy/Value/AddValue").SafeGetComponent<Text>();
            _energyIcon= Transform.FindTransfrom("TopPanel/Resource/ResourceLeft/Energy/Icon").SafeGetComponent<Image>();

            _builderText = Transform.FindTransfrom("TopPanel/Resource/ResourceRight/Builder/Value").SafeGetComponent<Text>();
            _builderIcon= Transform.FindTransfrom("TopPanel/Resource/ResourceRight/Builder/Icon").SafeGetComponent<Image>();

            _roCoreText = Transform.FindTransfrom("TopPanel/Resource/ResourceRight/RoCore/Value").SafeGetComponent<Text>();
            _roCoreIcon = Transform.FindTransfrom("TopPanel/Resource/ResourceRight/RoCore/Icon").SafeGetComponent<Image>();

            currentTimeText = Transform.FindTransfrom("Time/Time/Text").SafeGetComponent<Text>();
            timeSlider = Transform.FindTransfrom("Time/Slider").SafeGetComponent<Slider>();
        }
        
        void InitResImage()
        {
            _currencyIcon.sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Currency_Icon_Path, Utility.SpriteType.png);
            _energyIcon.sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Energy_Icon_Path, Utility.SpriteType.png);
            _researchIcon.sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Research_Icon_Path, Utility.SpriteType.png);
            _builderIcon.sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Builder_Icon_Path, Utility.SpriteType.png);
            _roCoreIcon.sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Rocore_Icon_Path, Utility.SpriteType.png);
        }

    }
}