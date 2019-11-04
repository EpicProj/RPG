using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class GameEntryPageContext : WindowBase
    {
        private GameEntryPage m_page;

       

        public override void Awake(params object[] paralist)
        {
            m_page = UIUtility.SafeGetComponent<GameEntryPage>(Transform);
            AddBtnListener();
        }

        public override void OnShow(params object[] paralist)
        {
         
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.PlayMenuAnim:
                    return PlayMenuAnim();
                default:
                    return false;
            }
        }

        private void AddBtnListener()
        {
            AddButtonClickListener(m_page.StartButton, () =>
            {
                ScenesManager.Instance.LoadingScene(UIPath.Scene_Test);
            });
            AddButtonClickListener(m_page.QuitButton, () =>
            {
                Application.Quit();
            });
            //LoadGame
            AddButtonClickListener(m_page.LoadButton, () =>
            {
                OnLoadGameBtnClick();
            });
        }

        void OnLoadGameBtnClick()
        {
            UIManager.Instance.PopUpWnd(UIPath.MainMenu_GameLoad_Dialog, WindowType.Dialog);
            m_page.Menu.gameObject.SetActive(false);
        }
        bool PlayMenuAnim()
        {
            m_page.Menu.gameObject.SetActive(true);
            m_page.MenuAnim.Play();
            return true;
        }


    }
}