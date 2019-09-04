using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sim_FrameWork
{
    public class MainMenuPageContext : WindowBase
    {
        public PlayerData playerData;
        public PlayerModule.TimeData timeData;

        public MainMenuPage m_page;

        //Time Data
        private Text CurrentYearText;
        private Text CurrentMonthText;
        private Text CurrentSeasonText;
        private Image SeasonSprite;
        private float currentTimeProgress = 0f;

        //GameStates
        private Button PauseBtn;
        private Image StatesBtnImage;

        public override void Awake(params object[] paralist)
        {
            playerData = PlayerModule.Instance.InitPlayerData();
            InitBaseData();
            AddBtnListener();
            UpdatePlayerBaseData();
            InitBuildPanel();
        }

        private void InitBaseData()
        {
            m_page = GameObject.GetComponent<MainMenuPage>();
            CurrentYearText = m_page.TimePanel.transform.Find("Time/CurrentYear").GetComponent<Text>();
            CurrentMonthText= m_page.TimePanel.transform.Find("Time/CurrentMonth").GetComponent<Text>();
            CurrentSeasonText= m_page.TimePanel.transform.Find("Time/Season").GetComponent<Text>();
            SeasonSprite = m_page.TimePanel.transform.Find("Time/SeasonIcon").GetComponent<Image>();
            PauseBtn = m_page.GameStatesObj.transform.Find("PauseButton").GetComponent<Button>();
            StatesBtnImage=m_page.GameStatesObj.transform.Find("PauseButton").GetComponent<Image>();
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


        public override bool OnMessage(string msgID, params object[] paralist)
        {
            switch (msgID)
            {
                case "UpdateResourceData":
                    //更新资源面板
                    playerData = (PlayerData)paralist[0];
                    UpdatePlayerBaseData();
                    return true;

                case "UpdateBuildPanelData":
                    //更新建造列表
                    playerData.UnLockBuildingPanelDataList = (List<BuildingPanelData>)paralist[0];
                    return true;
                case "UpdateTime":
                    //更新时间
                    timeData = (PlayerModule.TimeData)paralist[0];
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


        public void UpdatePlayerBaseData()
        {
            m_page.CurrencyNum.text = playerData.Currency.ToString();
            m_page.FoodNum.text = playerData.Food.ToString();
        }



        //Button
        private void AddBtnListener()
        {
            AddButtonClickListener(m_page.MaterialBtn, delegate () 
            {
                UIManager.Instance.PopUpWnd(UIPath.WAREHOURSE_DIALOG, true ,playerData.materialStorageDataList);
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