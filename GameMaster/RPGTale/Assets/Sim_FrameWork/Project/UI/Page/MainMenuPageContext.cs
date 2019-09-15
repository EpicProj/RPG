using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sim_FrameWork.UI
{
    public class MainMenuPageContext : WindowBase
    {
        public enum ResourceType
        {
            All,
            Currency,
            Food,
            Labor,
            Energy,
            Material
        }

        public PlayerData playerData;
        public PlayerModule.TimeData timeData;

        public MainMenuPage m_page;

        //Time Data
        private Text CurrentYearText;
        private Text CurrentMonthText;
        private Text CurrentSeasonText;
        private Image SeasonSprite;
        private float currentTimeProgress = 0f;

        /// <summary>
        /// Resource
        /// </summary>
        private Text CurrencyNumText;
        private Text FoodNumText;
        private Text FoodAddNumText;
        private Text LaborNumText;
        private Text LaborAddNumText;
        private Text EnergyNumText;
        private Text EnergyAddNumText;

        /// <summary>
        /// Camp Data
        /// </summary>
        private const float CampRotateValue_Min = -62.0f;
        private const float CampRotateValue_Max = 117.0f;
        private const float CampRotateValue_Zero = 25.5f;
        private Text CampValueMinText;
        private Text CampValueMaxText;
        private Text CampValueCurrentText;
        private GameObject CampPointer;

        //GameStates
        private Button PauseBtn;

        public override void Awake(params object[] paralist)
        {
            playerData = PlayerModule.Instance.InitPlayerData();
            InitBaseData();
            AddBtnListener();
            UpdateResData(ResourceType.All);
            UpdateResMonthData(ResourceType.All);
            InitBuildPanel();
            InitBuildMainTab();
        }

        private void InitBaseData()
        {
            m_page = GameObject.GetComponent<MainMenuPage>();
            //Resource
            CurrencyNumText = m_page.Currency.transform.Find("Num").GetComponent<Text>();
            FoodNumText = m_page.Food.transform.Find("Num").GetComponent<Text>();
            FoodAddNumText = m_page.Food.transform.Find("AddNum").GetComponent<Text>();
            LaborNumText= m_page.Labor.transform.Find("Num").GetComponent<Text>();
            LaborAddNumText= m_page.Labor.transform.Find("AddNum").GetComponent<Text>();
            EnergyNumText= m_page.Energy.transform.Find("Num").GetComponent<Text>();
            EnergyAddNumText = m_page.Energy.transform.Find("AddNum").GetComponent<Text>();
            //Camp
            CampValueMinText = m_page.CampValue.transform.Find("ValueMin").GetComponent<Text>();
            CampValueMaxText = m_page.CampValue.transform.Find("ValueMax").GetComponent<Text>();
            CampValueCurrentText = m_page.CampContent.transform.Find("Value").GetComponent<Text>();
            CampPointer = m_page.CampValue.transform.Find("Current").gameObject;

            CurrentYearText = m_page.TimePanel.transform.Find("Time/CurrentYear").GetComponent<Text>();
            CurrentMonthText= m_page.TimePanel.transform.Find("Time/CurrentMonth").GetComponent<Text>();
            CurrentSeasonText= m_page.TimePanel.transform.Find("Time/Season").GetComponent<Text>();
            SeasonSprite = m_page.TimePanel.transform.Find("Time/SeasonIcon").GetComponent<Image>();
            PauseBtn = m_page.GameStatesObj.transform.Find("Pause").GetComponent<Button>();
            timeData = PlayerModule.Instance.timeData;

            //Update Time
            UpdateTimePanel();
            InitCampData();
        }

        public override void OnUpdate()
        {
            UpdateTimeProgress();
        }

        public override void OnShow(params object[] paralist)
        {


        }


        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                #region Resource
                case UIMsgType.Res_Currency:
                    playerData = (PlayerData)msg.content;
                    UpdateResData(ResourceType.Currency);
                    return true;
                case UIMsgType.Res_Food:
                    playerData.resourceData = (PlayerData.PlayerResourceData)msg.content;
                    UpdateResData(ResourceType.Food);
                    return true;
                case UIMsgType.Res_MonthFood:
                    playerData.resourceData = (PlayerData.PlayerResourceData)msg.content;
                    UpdateResMonthData(ResourceType.Food);
                    return true;
                case UIMsgType.Res_Labor:
                    playerData.resourceData = (PlayerData.PlayerResourceData)msg.content;
                    UpdateResData(ResourceType.Labor);
                    return true;
                case UIMsgType.Res_MonthLabor:
                    playerData.resourceData = (PlayerData.PlayerResourceData)msg.content;
                    UpdateResMonthData(ResourceType.Labor);
                    return true;
                case UIMsgType.Res_Energy:
                    playerData.resourceData = (PlayerData.PlayerResourceData)msg.content;
                    UpdateResData(ResourceType.Energy);
                    return true;
                case UIMsgType.Res_MonthEnergy:
                    playerData.resourceData = (PlayerData.PlayerResourceData)msg.content;
                    UpdateResMonthData(ResourceType.Energy);
                    return true;
                #endregion
                case UIMsgType.UpdateBuildPanelData:
                    //更新建造列表
                    playerData.UnLockBuildingPanelDataList = (List<BuildingPanelData>)msg.content;
                    return true;
                case  UIMsgType.UpdateTime:
                    //更新时间
                    timeData = (PlayerModule.TimeData)msg.content;
                    UpdateTimePanel();
                    return true;
                default:
                    return false;
            }

          
            
        }

        private void UpdateTimePanel()
        {
            CurrentMonthText.text = timeData.currentMonth.ToString();
            CurrentYearText.text = timeData.currentYear.ToString();
            CurrentSeasonText.text = PlayerModule.Instance.GetSeasonName((int)timeData.currentSeason);
            SeasonSprite.sprite = PlayerModule.Instance.GetSeasonSprite((int)timeData.currentSeason);
        }
        private void UpdateTimeProgress()
        {
            if (GameManager.Instance.gameStates == GameManager.GameStates.Pause)
                return;
            currentTimeProgress+=Time.deltaTime;
            if (currentTimeProgress >= timeData.realSecondsPerMonth)
            {
                currentTimeProgress = 0;
            }
            m_page.TimeSlider.value = currentTimeProgress / timeData.realSecondsPerMonth;
        }

        /// <summary>
        /// 更新当前资源
        /// </summary>
        /// <param name="type"></param>
        public void UpdateResData(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.All:
                    CurrencyNumText.text = playerData.resourceData.Currency.ToString();
                    FoodNumText.text = playerData.resourceData.Food.ToString();
                    LaborNumText.text = playerData.resourceData.Labor.ToString();
                    EnergyNumText.text = playerData.resourceData.Energy.ToString();
                    break;
                case ResourceType.Currency:
                    CurrencyNumText.text = playerData.resourceData.Currency.ToString();
                    break;
                case ResourceType.Food:
                    FoodNumText.text = playerData.resourceData.Food.ToString();
                    break;
                case ResourceType.Labor:
                    LaborNumText.text = playerData.resourceData.Labor.ToString();
                    break;
                case ResourceType.Energy:
                    EnergyNumText.text = playerData.resourceData.Energy.ToString();
                    break;
                default:
                    break;
            }   
        }
        /// <summary>
        /// 更新当前资源增加值
        /// </summary>
        /// <param name="type"></param>
        public void UpdateResMonthData(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.All:
                    FoodAddNumText.text = "+" + playerData.resourceData.FoodPerMonth.ToString();
                    EnergyAddNumText.text = "+" + playerData.resourceData.EnergyPerMonth.ToString();
                    LaborAddNumText.text = "+" + playerData.resourceData.LaborPerMonth.ToString();
                    break;
                case ResourceType.Energy:
                    EnergyAddNumText.text = "+" + playerData.resourceData.EnergyPerMonth.ToString();
                    break;
                case ResourceType.Food:
                    FoodAddNumText.text = "+" + playerData.resourceData.FoodPerMonth.ToString();
                    break;
                case ResourceType.Labor:
                    LaborAddNumText.text = "+" + playerData.resourceData.LaborPerMonth.ToString();
                    break;
            }
        }

        //Button
        private void AddBtnListener()
        {
            AddButtonClickListener(m_page.MaterialBtn,  delegate () 
            {
                UIManager.Instance.PopUpWnd(UIPath.WAREHOURSE_DIALOG, true,playerData.wareHouseInfo);
            });
            AddButtonClickListener(PauseBtn, delegate ()
            {
                OnPauseBtnClick();
            });

            /// Order Receive Page
            AddButtonClickListener(m_page.OrderBtn, delegate ()
            {
                UIManager.Instance.Register<OrderReceiveMainPageContext>(UIPath.Order_Receive_Main_Page);
                UIManager.Instance.PopUpWnd(UIPath.Order_Receive_Main_Page, true);
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
            CampValueCurrentText.text = playerData.campData.Current_Justice_Value.ToString();
            
            UpdateCampPointer();

        }
        private void UpdateCampPointer()
        {
            //更新指针
            if (playerData.campData.Current_Justice_Value == 0)
            {
                SetCampPointerPos(CampRotateValue_Zero);
            }
            else if (playerData.campData.Current_Justice_Value > 0)
            {
                float ratio= playerData.campData.Current_Justice_Value / Mathf.Abs(CampModule.campConfig.maxValue);
                SetCampPointerPos((CampRotateValue_Max - CampRotateValue_Zero) * ratio);
            }else if (playerData.campData.Current_Justice_Value < 0)
            {
                float ratio = playerData.campData.Current_Justice_Value / Mathf.Abs(CampModule.campConfig.minValue);
                SetCampPointerPos((CampRotateValue_Zero - CampRotateValue_Min) * ratio);
            }
        }
        private void SetCampPointerPos(float pos)
        {
            //TODO BUG
            CampPointer.transform.GetComponent<RectTransform>().localRotation = new Quaternion(0f, 0f,pos, 0f);
        }

        #endregion

        #region BuildPanel
        public void InitBuildPanel()
        {
            for(int i = 0; i < playerData.UnLockBuildingPanelDataList.Count; i++)
            {
                GameObject buildObj = ObjectManager.Instance.InstantiateObject(UIPath.BUILD_ELEMENT_PREFAB_PATH);
                BlockBuildElement element = buildObj.GetComponent<BlockBuildElement>();
                element.InitBuildElement(playerData.UnLockBuildingPanelDataList[i]);
                buildObj.transform.SetParent(m_page.BuildContent.transform, false);
            }
        }

        public void InitBuildMainTab()
        {
            List<FunctionBlockTypeData> mainTypeList = FunctionBlockModule.GetInitMainType();
            for(int i = 0; i < mainTypeList.Count; i++)
            {
                GameObject mainTab = ObjectManager.Instance.InstantiateObject(UIPath.Construct_MainTab_Element_Path);
                ConstructMainTabElement element = mainTab.GetComponent<ConstructMainTabElement>();
                element.InitMainTabElement(mainTypeList[i]);
                mainTab.transform.SetParent(m_page.BuildTabContent.transform, false);
            }
        }

        #endregion

    }
}