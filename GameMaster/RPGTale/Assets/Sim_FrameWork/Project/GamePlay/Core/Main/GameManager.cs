using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sim_FrameWork.UI;

namespace Sim_FrameWork
{
    public enum GameStates
    {
        Start = 1,
        Pause = 2,
        None=3,
    }


    public enum AreaState
    {
        OutSide,
        MainShip_PowerArea,
    }

    public class GameManager : MonoSingleton<GameManager>
    {
        [HideInInspector]
        public GraphicRaycaster raycaster;
        //游戏状态
        private GameStates _gameStates = GameStates.None;
        public GameStates GameStates { get { return _gameStates; } }

        public AreaState currentAreaState= AreaState.OutSide;

        private bool ConsolePageShow = false;

        protected override void Awake()
        {
            base.Awake();
            
            AssetBundleManager.Instance.LoadAssetBundleConfig();
            ResourceManager.Instance.Init(this);
        }

        void Start()
        {
            InitBaseData();
           
            DataManager.Instance.InitManager();
            DataManager.Instance.InitGameBaseData();
            GameDataSaveManager.Instance.InitData();
            InitCampPrepareData();

            UIGuide.Instance.ShowGameEntryPage();
            UIGuide.Instance.ShowGameDebugDialog();
        }

        public void InitBaseData()
        {
            UIManager.Instance.Init(GameObject.Find("MainCanvas").transform as RectTransform, GameObject.Find("MainCanvas/Window").transform as RectTransform,
                GameObject.Find("MainCanvas/Dialog").transform as RectTransform,GameObject.Find("MainCanvas/SPContent").transform as RectTransform, GameObject.Find("MainCanvas/UICamera").GetComponent<Camera>(), GameObject.Find("EventSystem").GetComponent<EventSystem>());
            raycaster = GameObject.Find("MainCanvas").transform.SafeGetComponent<GraphicRaycaster>();  
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

           
            if (GameStates== GameStates.Start)
            {
                PlayerManager.Instance.UpdateTime();
                ModifierManager.Instance.UpdateModifier();
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

        public void SwitchAreaState(AreaState state)
        {
            currentAreaState = state;
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_Page, new UIMessage(UIMsgType.GameAreaStateChange));
        }

        #endregion

        #region GamePrepare Data
        public List<CampInfo> campPrepareInfoList = new List<CampInfo>();


        private void InitCampPrepareData()
        {
            campPrepareInfoList = CampModule.GetAllCampInfo();
        }

        public CampInfo GetCampInfoData(int campID)
        {
            CampInfo result = null;
            result = campPrepareInfoList.Find(x => x.CampID == campID);
            if (result == null)
                DebugPlus.LogError("GetCampData Info Null!  campID=" + campID);
            return result;
        }


        #endregion


    }
}