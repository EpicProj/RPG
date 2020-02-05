using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class MenuDialogContext : WindowBase
    {
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            AddBtnClick();
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("BG").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.CloseWnd(this,true);
            });

            AddButtonClickListener(Transform.FindTransfrom("Content/Button/Save").SafeGetComponent<Button>(), OnSaveBtnClick);

            AddButtonClickListener(Transform.FindTransfrom("Content/Button/Load").SafeGetComponent<Button>(), () =>
            {
                UIGuide.Instance.ShowGameLoadDialog();
            });

            AddButtonClickListener(Transform.FindTransfrom("Content/Button/BackMenu").SafeGetComponent<Button>(), () =>
            {
                ScenesManager.Instance.LoadingScene(UIPath.ScenePath.Scene_GameEntry);
            });
        }

        void OnSaveBtnClick()
        {
            GameDataSaveManager.Instance.SaveGameFile();
        }
    }
}