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
            Labor,
            Energy,
            Material
        }



        private float currentTimeProgress = 0f;
        /// <summary>
        /// Camp Data
        /// </summary>
        private const float CampRotateValue_Min = -62.0f;
        private const float CampRotateValue_Max = 117.0f;
        private const float CampRotateValue_Zero = 25.5f;

        private GameObject CampPointer;


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
            InitCampData();
            UpdateResData(ResourceType.All);
            UpdateResMonthData(ResourceType.All);
            InitBuildMainTab();
        }

        public override void OnClose()
        {
            base.OnClose();
        }


        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                #region Resource
                case UIMsgType.Res_Currency:         
                    return UpdateResData(ResourceType.Currency);
                case UIMsgType.Res_Labor:
                    return UpdateResData(ResourceType.Labor);
                case UIMsgType.Res_MonthLabor:
                    return UpdateResMonthData(ResourceType.Labor);
                case UIMsgType.Res_Energy:
                    return UpdateResData(ResourceType.Energy);
                case UIMsgType.Res_MonthEnergy:
                    return UpdateResMonthData(ResourceType.Energy);
                #endregion
                case UIMsgType.MenuPage_Update_BuildPanel:
                    FunctionBlockTypeData typeData = (FunctionBlockTypeData)msg.content[0];
                    var type = FunctionBlockModule.GetBlockType(typeData);
                    return RefreshBuildMainPanel(type);
                case  UIMsgType.UpdateTime:
                    //更新时间
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
            CurrentMonthText.text = timedata.currentMonth.ToString();
            CurrentYearText.text = timedata.currentYear.ToString();
            CurrentSeasonText.text = PlayerModule.Instance.GetSeasonName((int)PlayerManager.Instance.playerData.timeData.currentSeason);
            //SeasonSprite.sprite = PlayerModule.Instance.GetSeasonSprite((int)PlayerManager.Instance.playerData.timeData.currentSeason);
            return true;
        }
        private void UpdateTimeProgress()
        {
            if (GameManager.Instance.gameStates == GameManager.GameStates.Pause)
                return;
            currentTimeProgress+=Time.deltaTime;
            if (currentTimeProgress >= PlayerManager.Instance.playerData.timeData.realSecondsPerMonth)
            {
                currentTimeProgress = 0;
            }
            m_page.TimeSlider.value = currentTimeProgress / PlayerManager.Instance.playerData.timeData.realSecondsPerMonth;
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
                    CurrencyNumText.text = data.Currency.ToString();
                    LaborNumText.text = data.Labor.ToString();
                    EnergyNumText.text = data.Energy.ToString();
                    return true;
                case ResourceType.Currency:
                    CurrencyNumText.text = data.Currency.ToString();
                    return true;
                case ResourceType.Labor:
                    LaborNumText.text = data.Labor.ToString();
                    return true;
                case ResourceType.Energy:
                    EnergyNumText.text = data.Energy.ToString();
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
                    EnergyAddNumText.text = "+" + data.EnergyPerMonth.ToString();
                    LaborAddNumText.text = "+" + data.LaborPerMonth.ToString();
                    return true;
                case ResourceType.Energy:
                    EnergyAddNumText.text = "+" + data.EnergyPerMonth.ToString();
                    return true;
                case ResourceType.Labor:
                    LaborAddNumText.text = "+" + data.LaborPerMonth.ToString();
                    return true;
                default:
                    return false;
            }
        }

        //Button
        private void AddBtnListener()
        {
            AddButtonClickListener(m_page.MaterialBtn, ()=>
            {
                UIManager.Instance.PopUpWnd(UIPath.WindowPath.WareHouse_Page, WindowType.Page, true);
            });
            AddButtonClickListener(PauseBtn, () =>
            {
                OnPauseBtnClick();
            });

            /// Order Receive Page
            AddButtonClickListener(m_page.OrderBtn, () =>
            {
                UIManager.Instance.Register<OrderReceiveMainPageContext>(UIPath.WindowPath.Order_Receive_Main_Page);
                UIManager.Instance.PopUpWnd(UIPath.WindowPath.Order_Receive_Main_Page, WindowType.Page, true);
            });

            AddButtonClickListener(m_page.ReserachBtn, () =>
            {
                UIManager.Instance.PopUpWnd(UIPath.WindowPath.Technology_Page, WindowType.Page, true);
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

        #region Camp
        private void InitCampData()
        {
            CampValueMinText.text = CampModule.campConfig.minValue.ToString();
            CampValueMaxText.text = CampModule.campConfig.maxValue.ToString();
            CampValueCurrentText.text = PlayerManager.Instance.playerData.campData.Current_Justice_Value.ToString();
            
            UpdateCampPointer();

        }
        private void UpdateCampPointer()
        {
            //更新指针
            if (PlayerManager.Instance.playerData.campData.Current_Justice_Value == 0)
            {
                SetCampPointerPos(CampRotateValue_Zero);
            }
            else if (PlayerManager.Instance.playerData.campData.Current_Justice_Value > 0)
            {
                float ratio= PlayerManager.Instance.playerData.campData.Current_Justice_Value / Mathf.Abs(CampModule.campConfig.maxValue);
                SetCampPointerPos((CampRotateValue_Max - CampRotateValue_Zero) * ratio);
            }else if (PlayerManager.Instance.playerData.campData.Current_Justice_Value < 0)
            {
                float ratio = PlayerManager.Instance.playerData.campData.Current_Justice_Value / Mathf.Abs(CampModule.campConfig.minValue);
                SetCampPointerPos((CampRotateValue_Zero - CampRotateValue_Min) * ratio);
            }
        }
        private void SetCampPointerPos(float pos)
        {
            //TODO BUG
            UIUtility.SafeGetComponent<RectTransform>(CampPointer.transform).localRotation = new Quaternion(0f, 0f,pos, 0f);
        }

        #endregion

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
            loopList.RefrshData(list);

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
        private Text CurrentYearText;
        private Text CurrentMonthText;
        private Text CurrentSeasonText;
        private Image SeasonSprite;

        /// <summary>
        /// Resource
        /// </summary>
        private Text CurrencyNumText;
        private Text LaborNumText;
        private Text LaborAddNumText;
        private Text EnergyNumText;
        private Text EnergyAddNumText;

        private Text CampValueMinText;
        private Text CampValueMaxText;
        private Text CampValueCurrentText;

        protected override void InitUIRefrence()
        {
            m_page = UIUtility.SafeGetComponent<MainMenuPage>(Transform);
            //Resource
            CurrencyNumText = UIUtility.SafeGetComponent<Text>(m_page.Currency.transform.Find("Num"));
            LaborNumText = UIUtility.SafeGetComponent<Text>(m_page.Labor.transform.Find("Num"));
            LaborAddNumText = UIUtility.SafeGetComponent<Text>(m_page.Labor.transform.Find("AddNum"));
            EnergyNumText = UIUtility.SafeGetComponent<Text>(m_page.Energy.transform.Find("Num"));
            EnergyAddNumText = UIUtility.SafeGetComponent<Text>(m_page.Energy.transform.Find("AddNum"));
            //Camp
            CampValueMinText = UIUtility.SafeGetComponent<Text>(m_page.CampValue.transform.Find("ValueMin"));
            CampValueMaxText = UIUtility.SafeGetComponent<Text>(m_page.CampValue.transform.Find("ValueMax"));
            CampValueCurrentText = UIUtility.SafeGetComponent<Text>(m_page.CampContent.transform.Find("Value"));
            CampPointer = m_page.CampValue.transform.Find("Current").gameObject;

            CurrentYearText = UIUtility.SafeGetComponent<Text>(m_page.TimePanel.transform.Find("Time/CurrentYear"));
            CurrentMonthText = UIUtility.SafeGetComponent<Text>(m_page.TimePanel.transform.Find("Time/CurrentMonth"));
            CurrentSeasonText = UIUtility.SafeGetComponent<Text>(m_page.TimePanel.transform.Find("Time/Season"));
            SeasonSprite = UIUtility.SafeGetComponent<Image>(m_page.TimePanel.transform.Find("Time/SeasonIcon"));
            PauseBtn = UIUtility.SafeGetComponent<Button>(m_page.GameStatesObj.transform.Find("Pause"));
        }

        protected override void ClearUIRefrence()
        {
            m_page = null;
            CurrencyNumText = null;
        }
    }
}