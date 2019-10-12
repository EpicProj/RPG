﻿using System.Collections;
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



        private void AddBtnListener()
        {
            AddButtonClickListener(m_page.StartButton, () =>
            {
                ScenesManager.Instance.LoadingScene(UIPath.Scene_Test);
            });


        }
    }
}