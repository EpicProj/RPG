using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public enum GameSaveMode
    {
        None,
        LoadGame,
        CoverSave
    }
    public class GameLoadDialogContext : WindowBase
    {
        private GameSaveMode currentMode = GameSaveMode.None;


        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            AddBtnListener();
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            InitSaveList();
        }

        void AddBtnListener()
        {
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(UIPath.WindowPath.MainMenu_GameLoad_Dialog);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Game_Entry_Page, new UIMessage(UIMsgType.PlayMenuAnim));
            });
        }

        void InitSaveList()
        {
            var tran = Transform.FindTransfrom("Content/Content/Scroll View");
            var loopList = tran.SafeGetComponent<LoopList>();
            loopList.InitData(GameDataSaveManager.Instance.GetSaveGroupModel());
        }

        

    }
}