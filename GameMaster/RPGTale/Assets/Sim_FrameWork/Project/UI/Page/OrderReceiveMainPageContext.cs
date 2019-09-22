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
                case UIMsgType.Order_Receive_Main:
                    return InitOrderMainContent();
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


        #region Init OrderMain

        private bool InitOrderMainContent()
        {
            Func<Dictionary<string, OrderItemBase>, Dictionary<int, List<BaseDataModel>>> getData = (d) =>
            {
                Dictionary<int, List<BaseDataModel>> result = new Dictionary<int, List<BaseDataModel>>();
                int index = 0;
                foreach (KeyValuePair<string, OrderItemBase> kvp in d)
                {
                    List<BaseDataModel> modelList = new List<BaseDataModel>();
                    modelList.Add(kvp.Value.dataModel);
                    modelList.Add(kvp.Value.belongDataModel);
                    result.Add(index, modelList);
                    index++;
                }
                return result;
            };

            var orderContent = GlobalEventManager.Instance.AllOrderDic;
            if (orderContent == null)
                return false;
            //For Test
           
            var loopList = UIUtility.SafeGetComponent<LoopList>(m_page.OrderContentScroll.transform);
            loopList.InitData(getData(orderContent));
         
            return true;
        }


        #endregion


    }
}