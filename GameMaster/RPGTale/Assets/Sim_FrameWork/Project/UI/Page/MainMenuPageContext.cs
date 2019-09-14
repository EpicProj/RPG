using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sim_FrameWork
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

        //GameStates
        private Button PauseBtn;
        private Image StatesBtnImage;

        public override void Awake(params object[] paralist)
        {
            playerData = PlayerModule.Instance.InitPlayerData();
            InitBaseData();
            AddBtnListener();
            UpdateResData(ResourceType.All);
            UpdateResMonthData(ResourceType.All);
            InitBuildPanel();
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

            CurrentYearText = m_page.TimePanel.transform.Find("Time/CurrentYear").GetComponent<Text>();
            CurrentMonthText= m_page.TimePanel.transform.Find("Time/CurrentMonth").GetComponent<Text>();
            CurrentSeasonText= m_page.TimePanel.transform.Find("Time/Season").GetComponent<Text>();
            SeasonSprite = m_page.TimePanel.transform.Find("Time/SeasonIcon").GetComponent<Image>();
            PauseBtn = m_page.GameStatesObj.transform.Find("Pause").GetComponent<Button>();
            StatesBtnImage=m_page.GameStatesObj.transform.Find("Pause").GetComponent<Image>();
            StatesBtnImage.sprite = Utility.LoadSprite(GameManager.globalSettings.basicSpriteConfig.StartBtn_Sprite_Path, Utility.SpriteType.png);
            timeData = PlayerModule.Instance.timeData;

            //Update Time
            UpdateTimePanel();
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
        }

        private void OnPauseBtnClick()
        {
            if (GameManager.Instance.gameStates == GameManager.GameStates.Start)
            {
                GameManager.Instance.SetGameStates(GameManager.GameStates.Pause);
                StatesBtnImage.sprite = Utility.LoadSprite(GameManager.globalSettings.basicSpriteConfig.PauseBtn_Sprite_Path, Utility.SpriteType.png);
            }
            else if (GameManager.Instance.gameStates == GameManager.GameStates.Pause)
            {
                GameManager.Instance.SetGameStates(GameManager.GameStates.Start);
                StatesBtnImage.sprite = Utility.LoadSprite(GameManager.globalSettings.basicSpriteConfig.StartBtn_Sprite_Path, Utility.SpriteType.png);
            }
        }


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

        #endregion

    }
}