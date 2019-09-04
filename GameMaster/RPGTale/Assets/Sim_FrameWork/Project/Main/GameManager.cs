﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

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

        public List<FunctionBlockInfoData> FunctionBlockInfoDataList = new List<FunctionBlockInfoData>();

        private Canvas MainCanvas;
        //游戏状态
        private GameStates _gameStates = GameStates.Start;
        public GameStates gameStates { get { return _gameStates; } }

        public static Config.GlobalSetting globalSettings =new Config.GlobalSetting ();
        protected override void Awake()
        {
            base.Awake();
            AssetBundleManager.Instance.LoadAssetBundleConfig();
            ResourceManager.Instance.Init(this);
            ObjectManager.Instance.Init(transform.Find("RecyclePoolTrs"), transform.Find("SceneTrs"));
            UIManager.Instance.Init(GameObject.Find("MainCanvas").transform as RectTransform, GameObject.Find("MainCanvas/Window").transform as RectTransform, GameObject.Find("MainCanvas/UICamera").GetComponent<Camera>(), GameObject.Find("MainCanvas/EventSystem").GetComponent<EventSystem>());
            MainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();

            globalSettings.LoadGlobalSettting();

            RegisterModule();
            RegisterUI();
        }


        public void Update()
        {
            UIManager.Instance.OnUpdate();
            //test
            if (Input.GetKeyDown(KeyCode.M))
            {
                PlayerModule.Instance.AddCurrency(1000.5f);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                PlayerModule.Instance.AddMaterialData(100, 10);
            }
            if (gameStates == GameStates.Start)
            {
                PlayerModule.Instance.UpdateTime();
            }
        
        }


        public void RegisterUI()
        {
            UIManager.Instance.Register<BlockInfoDialogContent>(UIPath.FUCNTIONBLOCK_INFO_DIALOG);
            UIManager.Instance.Register<WareHouseDialogContent>(UIPath.WAREHOURSE_DIALOG);
            UIManager.Instance.Register<MainMenuPageContext>(UIPath.MAINMENU_PAGE);
            UIManager.Instance.PopUpWnd(UIPath.MAINMENU_PAGE);

        }


        public void RegisterModule()
        {
            FunctionBlockModule.Instance.InitData();
            MaterialModule.Instance.InitData();
            DistrictModule.Instance.InitData();
            FormulaModule.Instance.InitData();
            PlayerModule.Instance.InitData();
        }

        #region MainFunction

        public void SetGameStates(GameStates states)
        {
            switch (states)
            {
                case GameStates.Pause:
                    _gameStates = states;
                    break;
                case GameStates.Start:
                    _gameStates = states;
                    break;
            }
          
        }

        #endregion



        #region PlayerData


        #endregion

        #region FunctionBlock

        public void AddFunctionBlockData(FunctionBlockInfoData infoData)
        {
            if (!FunctionBlockInfoDataList.Contains(infoData))
            {
                FunctionBlockInfoDataList.Add(infoData);
            }
        }

        #endregion
    }
}