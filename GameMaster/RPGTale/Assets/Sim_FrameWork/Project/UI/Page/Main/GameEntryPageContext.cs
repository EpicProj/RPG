using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class GameEntryPageContext : WindowBase
    {
        public override void Awake(params object[] paralist)
        {
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
            AddButtonClickListener(Transform.FindTransfrom("Menu/StartBtn").SafeGetComponent<Button>(), () =>
            {
                OnGameStartBtnClick();
            });
            AddButtonClickListener(Transform.FindTransfrom("Menu/QuitBtn").SafeGetComponent<Button>(), () =>
            {
                Application.Quit();
            });
            //LoadGame
            AddButtonClickListener(Transform.FindTransfrom("Menu/LoadBtn").SafeGetComponent<Button>(), () =>
            {
                OnLoadGameBtnClick();
            });
        }
        void OnGameStartBtnClick()
        {
            UIManager.Instance.HideWnd(this);
            UIGuide.Instance.ShowNewGamePreparePage();
        }


        void OnLoadGameBtnClick()
        {
            UIManager.Instance.HideWnd(this);
            UIGuide.Instance.ShowGameLoadDialog();
        }
        bool PlayMenuAnim()
        {
            var anim = Transform.FindTransfrom("Menu").SafeGetComponent<Animation>();
            anim.SafePlayAnim();
            return true;
        }


    }
}