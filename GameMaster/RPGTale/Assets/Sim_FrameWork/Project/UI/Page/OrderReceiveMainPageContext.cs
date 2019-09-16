using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class OrderReceiveMainPageContext : WindowBase
    {

        public OrderReceiveMainPage m_page;



        public override void Awake(params object[] paralist)
        {
            m_page = GameObject.GetComponent<OrderReceiveMainPage>();
            AddBtnListener();
        }


        public override bool OnMessage(UIMessage msg)
        {
            return true;
        }

        public override void OnShow(params object[] paralist)
        {

        }

        private void AddBtnListener()
        {
            AddButtonClickListener(m_page.BackBtn,  () =>
            {
                UIManager.Instance.HideWnd(UIPath.Order_Receive_Main_Page);
                UIManager.Instance.ShowWnd(UIPath.MainMenu_Page);
            });
        }


        #region Init OrderMain

        private void InitOrderMainContent()
        {
            //For Test
            var obj= ObjectManager.Instance.InstantiateObject(UIPath.OrderMain_Content_Element_Path);
            OrderReceiveElement element = obj.GetComponent<OrderReceiveElement>();

        }


        #endregion


    }
}