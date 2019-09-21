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
            m_page = UIUtility.SafeGetComponent<OrderReceiveMainPage>(Transform);
            AddBtnListener();
        }


        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.Order_Receive_Main:
                    return InitOrderMainContent();
                default:
                    return false;
            }
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

        private bool InitOrderMainContent()
        {
            var orderContent = GlobalEventManager.Instance.AllOrderDic;
            if (orderContent == null)
                return false;
            //For Test
            foreach(var info in orderContent.Values)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.OrderMain_Content_Element_Path);
                var element = UIUtility.SafeGetComponent<OrderReceiveElement>(obj.transform);
                obj.transform.SetParent(m_page.OrderMainContent.transform,false);
            }
            return true;
        }


        #endregion


    }
}