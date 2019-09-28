using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sim_FrameWork.UI
{
    public partial class OrderReceiveMainPageContext : WindowBase
    {

        public OrderReceiveMainPage m_page;

        private Text Organization_Detail_Name;
        private Text Organization_Detail_Name_En;
        private Image Organization_Detail_Icon;
        private Text Organization_Detail_Content;

        private bool IsSelectOrganizaiton = false;
        private int CurrentOrganizationID = -1;
        private GameObject OrganizationContent;

        public override void Awake(params object[] paralist)
        {
            m_page = UIUtility.SafeGetComponent<OrderReceiveMainPage>(Transform);
            InitBaseData();
            AddBtnListener();
            InitOrderMainContent();
            InitOrganizationContent();
            RefreshOrganizationDetail();
        }


        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.RefreshOrder:
                    //TODO
                    return RefreshOrderContent();
                case UIMsgType.OrderPage_Select_Organization:
                    return SelectOrganization((int)msg.content[0]);
                default:
                    return false;
            }
        }

        public override void OnShow(params object[] paralist)
        {
            RefreshOrderContent();
        }

        void InitBaseData()
        {
            Organization_Detail_Icon = UIUtility.SafeGetComponent<Image>(m_page.Organization_Detail.transform.Find("Title/Icon"));
            Organization_Detail_Name = UIUtility.SafeGetComponent<Text>(m_page.Organization_Detail.transform.Find("Title/Name"));
            Organization_Detail_Name_En= UIUtility.SafeGetComponent<Text>(m_page.Organization_Detail.transform.Find("Title/Name_En"));
            Organization_Detail_Content = UIUtility.SafeGetComponent<Text>(m_page.Organization_Detail.transform.Find("Info/Desc/Scroll View/Viewport/Content"));
            OrganizationContent = m_page.Organization_ContentScroll.transform.Find("Viewport/Content").gameObject;
           
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

        #region Organization
        bool RefreshOrganizationDetail()
        {
            Action<bool> showDetail = (b) =>
            {
                m_page.Organization_Detail.gameObject.SetActive(b);
                m_page.Organization_No_Info.gameObject.SetActive(!b);
            };
            OrganizationDataModel model = new OrganizationDataModel();
            if (!IsSelectOrganizaiton)
            {
                showDetail(false);
                return true;
            }
            if(model.Create(CurrentOrganizationID) && IsSelectOrganizaiton) 
            {
                showDetail(true);
                Organization_Detail_Icon.sprite = model.Icon;
                Organization_Detail_Name.text = model.Name;
                Organization_Detail_Name_En.text = model.Name_En;
                Organization_Detail_Content.text = model.BriefDesc;
                if (OrganizationContent != null)
                {
                    foreach (Transform trans in OrganizationContent.transform)
                    {
                        var element= UIUtility.SafeGetComponent<GeneralOrganizationElement>(trans);
                        element.Light.SetActive(false);
                        if (element._model.ID == CurrentOrganizationID)
                        {
                            element.Light.SetActive(true);
                        }
                    }
                }
                return true;
            }
            showDetail(false);
            return false;
        }

        bool SelectOrganization(int id)
        {
            IsSelectOrganizaiton = true;
            CurrentOrganizationID = id;
            return RefreshOrganizationDetail();
        }

        void InitOrganizationContent()
        {
            var loopList = UIUtility.SafeGetComponent<LoopList>(m_page.Organization_ContentScroll.transform);
            loopList.InitData(GlobalEventManager.Instance.CurrrentOrganizationModel);
        }

        #endregion

    }
}