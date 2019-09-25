using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

namespace Sim_FrameWork.UI
{
    public class OrderReceiveMainPageContext : WindowBase
    {

        public OrderReceiveMainPage m_page;


        public override void Awake(params object[] paralist)
        {
            m_page = UIUtility.SafeGetComponent<OrderReceiveMainPage>(Transform);
            AddBtnListener();
        }


        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.RefreshOrder:
                    //TODO
                    return RefreshOrderContent();
                default:
                    return false;
            }
        }

        public override void OnShow(params object[] paralist)
        {
            InitOrderMainContent();
        }

        private void AddBtnListener()
        {
            AddButtonClickListener(m_page.BackBtn,  () =>
            {
                UIManager.Instance.HideWnd(UIPath.Order_Receive_Main_Page);
                UIManager.Instance.ShowWnd(UIPath.MainMenu_Page);
            });
        }



        #region  OrderMain

        private bool InitOrderMainContent()
        {
            //For Test
            var loopList = UIUtility.SafeGetComponent<LoopList>(m_page.OrderContentScroll.transform);
            loopList.InitData(GlobalEventManager.Instance.AllOrderDataModelList);
         
            return true;
        }

        private bool RefreshOrderContent()
        {
            var loopList = UIUtility.SafeGetComponent<LoopList>(m_page.OrderContentScroll.transform);
            loopList.RefrshData(GlobalEventManager.Instance.AllOrderDataModelList);
            return true;
        }



        #endregion


    }
}