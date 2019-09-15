using System.Collections;
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

        public Canvas MainCanvas;
        public GraphicRaycaster raycaster;
        //游戏状态
        private GameStates _gameStates = GameStates.Start;
        public GameStates gameStates { get { return _gameStates; } }
        private GameObject PausePage;

        public static Config.GlobalSetting globalSettings =new Config.GlobalSetting ();
        protected override void Awake()
        {
            base.Awake();
            AssetBundleManager.Instance.LoadAssetBundleConfig();
            ResourceManager.Instance.Init(this);
            ObjectManager.Instance.Init(transform.Find("RecyclePoolTrs"), transform.Find("SceneTrs"));
            UIManager.Instance.Init(GameObject.Find("MainCanvas").transform as RectTransform, GameObject.Find("MainCanvas/Window").transform as RectTransform, GameObject.Find("MainCanvas/UICamera").GetComponent<Camera>(), GameObject.Find("MainCanvas/EventSystem").GetComponent<EventSystem>());
            MainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
            raycaster = MainCanvas.GetComponent<GraphicRaycaster>();
            PausePage = MainCanvas.transform.Find("Window/PausePage").gameObject;
            PausePage.transform.GetComponent<CanvasGroup>().alpha = 0;
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
                PlayerModule.Instance.AddCurrency(1000.5f, PlayerModule.ResourceAddType.current);
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
            UIManager.Instance.Register<BlockInfoDialogContent>(UIPath.FUNCTIONBLOCK_INFO_DIALOG);
            UIManager.Instance.Register<WareHouseDialogContent>(UIPath.WAREHOURSE_DIALOG);
            UIManager.Instance.Register<MainMenuPageContext>(UIPath.MainMenu_Page);
            UIManager.Instance.PopUpWnd(UIPath.MainMenu_Page);

        }


        public void RegisterModule()
        {
            FunctionBlockModule.Instance.Register();
            MaterialModule.Instance.Register();
            DistrictModule.Instance.Register();
            FormulaModule.Instance.Register();
            PlayerModule.Instance.Register();
            CampModule.Instance.Register();
            OrganizationModule.Instance.Register();
        }

        #region MainFunction

        public void SetGameStates(GameStates states)
        {
            switch (states)
            {
                case GameStates.Pause:
                    _gameStates = states;
                    Time.timeScale = 0;
                    PausePage.transform.GetComponent<CanvasGroup>().alpha = 1;
                    break;
                case GameStates.Start:
                    _gameStates = states;
                    Time.timeScale = 1;
                    PausePage.transform.GetComponent<CanvasGroup>().alpha = 0;
                    break;
            }
          
        }

        #endregion

        #region PlayerData


        #endregion

 
    }
}