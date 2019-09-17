using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class OrderReceiveMainPageContext : WindowBase
    {

        public OrderReceiveMainPage m_page;

        private Dictionary<string, OrderInfo> AllOrderContent;

        public override void Awake(params object[] paralist)
        {
            m_page = GameObject.GetComponent<OrderReceiveMainPage>();
            AddBtnListener();
        }


        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.Order_Receive_Main:
                    AllOrderContent = (Dictionary<string, OrderInfo>)msg.content;
                    InitOrderMainContent();
                    return true;
            }
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
            foreach(var info in AllOrderContent.Values)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.OrderMain_Content_Element_Path);
                var element = obj.GetComponent<OrderReceiveElement>();
                element.InitOrderReceiveElement(info);
                obj.transform.SetParent(m_page.OrderMainContent.transform);
            }
        }


        #endregion


    }
}