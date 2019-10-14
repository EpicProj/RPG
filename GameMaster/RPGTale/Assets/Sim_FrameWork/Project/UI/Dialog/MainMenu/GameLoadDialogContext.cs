using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class GameLoadDialogContext : WindowBase
    {
        private GameLoadDialog m_dialog;

        public override void Awake(params object[] paralist)
        {
            m_dialog = UIUtility.SafeGetComponent<GameLoadDialog>(Transform);
            AddBtnListener();
        }

        public override void OnShow(params object[] paralist)
        {
            InitSaveList();
        }

        public override void OnUpdate()
        {


        }

        void AddBtnListener()
        {
            AddButtonClickListener(m_dialog.BackBtn, () =>
            {

                UIManager.Instance.HideWnd(UIPath.MainMenu_GameLoad_Dialog);
                UIManager.Instance.SendMessageToWnd(UIPath.Game_Entry_Page, new UIMessage(UIMsgType.PlayMenuAnim));
            });
        }

        void InitSaveList()
        {
            var loopList = UIUtility.SafeGetComponent<LoopList>(m_dialog.SaveScrollView.transform);
            loopList.InitData(GameDataSaveManager.Instance.GetSaveModel());
        }

        

    }
}