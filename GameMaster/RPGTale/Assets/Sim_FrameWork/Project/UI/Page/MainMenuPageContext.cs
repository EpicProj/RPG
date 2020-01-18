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
            InitBuildMainTab();
            InitResImage();
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
            AddButtonClickListener(m_page.MenuBtn, () =>
            {
                UIGuide.Instance.ShowMenuDialog();
            });
            /// Order Receive Page
            AddButtonClickListener(m_page.OrderBtn, () =>
            {
                UIGuide.Instance.ShowOrderReceiveMainPage();
            });

            AddButtonClickListener(m_page.ReserachBtn, () =>
            {
                UIGuide.Instance.ShowTechnologyMainPage();
            });
            AddButtonClickListener(m_page.ExploreBtn, () =>
            {
                UIGuide.Instance.ShowExploreMainPage();
            });
            AddButtonClickListener(m_page.AssembleBtn, () =>
            {
                AssemblePartInfo info = new AssemblePartInfo(1);
                UIGuide.Instance.ShowAssemblePartDesignPage(info);
            });
            AddButtonClickListener(m_page.ShipDesignBtn, () =>
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
            var loopList = UIUtility.SafeGetComponent<LoopList>(m_page.BuildContent.transform);
            var list =PlayerManager.Instance.GetBuildPanelModelData(type);
            loopList.InitData(list);

            return true;
        }

        /// <summary>
        /// 初始化建造主页签
        /// </summary>
        public void InitBuildMainTab()
        {
            List<FunctionBlockTypeData> mainTypeList = FunctionBlockModule.GetInitMainType();
            var toggleGroup = UIUtility.SafeGetComponent<ToggleGroup>(m_page.BuildTabContent.transform);
            for(int i = 0; i < mainTypeList.Count; i++)
            {
                var mainTab = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Construct_MainTab_Element_Path);
                ConstructMainTabElement element = UIUtility.SafeGetComponent<ConstructMainTabElement>(mainTab.transform);
                if (toggleGroup != null)
                {
                    element.toggle.group = toggleGroup;
                }
                element.InitMainTabElement(mainTypeList[i]);
                mainTab.transform.SetParent(m_page.BuildTabContent.transform, false);
                mainTab.name = mainTypeList[i].Type.ToString();
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

        private MainMenuPage m_page;

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
            m_page = UIUtility.SafeGetComponent<MainMenuPage>(Transform);
            //Resource
            _currencyNumText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceLeft/Currency/Value") );
            _currencyAddNumText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceLeft/Currency/Value/AddValue"));
            _currencyIcon= UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceLeft/Currency/Icon"));

            _researchPointText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceLeft/Research/Value"));
            _researchPointAddText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceLeft/Research/Value/AddValue"));
            _researchIcon= UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceLeft/Research/Icon"));

            _energyNumText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceLeft/Energy/Value"));
            _energyAddNumText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceLeft/Energy/Value/AddValue"));
            _energyIcon= UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceLeft/Energy/Icon"));

            _builderText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceRight/Builder/Value"));
            _builderIcon= UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceRight/Builder/Icon"));

            _roCoreText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceRight/RoCore/Value"));
            _roCoreIcon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.ResourcePanel, "ResourceRight/RoCore/Icon"));

            currentTimeText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.TimePanel, "Time/Text"));
            timeSlider = UIUtility.SafeGetComponent<Slider>(UIUtility.FindTransfrom(m_page.TimePanel, "Slider"));
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