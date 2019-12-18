﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sim_FrameWork.UI;

namespace Sim_FrameWork
{
    public class GameManager : MonoSingleton<GameManager>
    {

        public enum GameStates
        {
            Start =1,
            Pause =2
        }


        public const string ITEM_UI_PATH = "ItemUIPrefab.prefab";

        [HideInInspector]
        public Canvas MainCanvas;
        [HideInInspector]
        public GraphicRaycaster raycaster;
        //游戏状态
        private GameStates _gameStates = GameStates.Start;
        public GameStates gameStates { get { return _gameStates; } }
        private bool ConsolePageShow = false;

        protected override void Awake()
        {
            base.Awake();
            AssetBundleManager.Instance.LoadAssetBundleConfig();
            ResourceManager.Instance.Init(this);
            
            DataManager.Instance.InitData();
            GlobalEventManager.Instance.InitData();
            DontDestroyOnLoad(gameObject);
            
        }

        void Start()
        {
            InitBaseData();
            //UIManager.Instance.PopUpWnd(UIPath.WindowPath.Game_Entry_Page);
        }

        public void InitBaseData()
        {
           
            UIManager.Instance.Init(GameObject.Find("MainCanvas").transform as RectTransform, GameObject.Find("MainCanvas/Window").transform as RectTransform,
                GameObject.Find("MainCanvas/Dialog").transform as RectTransform,GameObject.Find("MainCanvas/SPContent").transform as RectTransform, GameObject.Find("MainCanvas/UICamera").GetComponent<Camera>(), GameObject.Find("EventSystem").GetComponent<EventSystem>());
            MainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
            raycaster = UIUtility.SafeGetComponent<GraphicRaycaster>(MainCanvas.transform);
        }


        public void Update()
        {
            UIManager.Instance.OnUpdate();
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                if (ConsolePageShow)
                {
                    UIManager.Instance.HideWnd(UIPath.WindowPath.Console_Page);
                    ConsolePageShow = false;
                }
                else
                {
                    UIManager.Instance.PopUpWnd(UIPath.WindowPath.Console_Page, WindowType.Page);
                    ConsolePageShow = true;
                }
            }

            //UIClose
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var list = UIManager.Instance._currentWindowNameList;
                for (int i = list.Count -1 ; i >= 0; i--)
                {
                    if (string.Compare(list[i], UIPath.WindowPath.MainMenu_Page) == 0)
                        continue;
                    UIManager.Instance.HideWnd(list[i]);
                    AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);
                    break;
                }
            }

        }

        #region MainFunction

        public void SetGameStates(GameStates states)
        {
            switch (states)
            {
                case GameStates.Pause:
                    _gameStates = states;
                    Time.timeScale = 0;
                    break;
                case GameStates.Start:
                    _gameStates = states;
                    Time.timeScale = 1;
                    break;
            }
          
        }

        #endregion

        #region PlayerData


        #endregion

 
    }
}