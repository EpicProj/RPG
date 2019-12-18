using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class UIGuide : Singleton<UIGuide>
    {
        /// <summary>
        /// Show Main Page
        /// </summary>
        /// <param name="closeAll"></param>
        public void ShowGameMainPage(bool closeAll)
        {
            if (closeAll)
            {
                UIManager.Instance.CloseAllWnd();
            }
            UIManager.Instance.Register<UI.MainMenuPageContext>(UIPath.WindowPath.MainMenu_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.MainMenu_Page, WindowType.Page, true);
        }

        /// <summary>
        /// Tech Page
        /// </summary>
        public void ShowTechnologyMainPage()
        {
            UIManager.Instance.Register<UI.TechnologyMainPageContext>(UIPath.WindowPath.Technology_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Technology_Page, WindowType.Page, true);
        }

        public void ShowTechDetailDialog(int techID)
        {
            var techInfo = TechnologyDataManager.Instance.GetTechInfo(techID);
            if (techInfo != null)
            {
                UIManager.Instance.Register<UI.TechnologyDetailDialogContext>(UIPath.WindowPath.Technology_Detail_Dialog);
                UIManager.Instance.PopUpWnd(UIPath.WindowPath.Technology_Detail_Dialog, WindowType.Dialog, true, techInfo);
            }
        }

        /// <summary>
        /// Block Page
        /// </summary>
        public void ShowBlockManuPage(ManufactoryBase baseData)
        {
            UIManager.Instance.Register<UI.BlockManuPageContext>(UIPath.WindowPath.BlockManu_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.BlockManu_Page, WindowType.Page, true, baseData._blockBase, baseData.manufactoryInfo, baseData.formulaInfo);
        }

        public void ShowDistrictDetail(DistrictDataModel  model)
        {
            if (model.ID == 0)
                return;
            UIManager.Instance.Register<UI.DistrictDetailUIContext>(UIPath.WindowPath.District_Detail_UI);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.District_Detail_UI, WindowType.SPContent, true, model);
        }

        public void ShowOrderReceiveMainPage()
        {
            UIManager.Instance.Register<UI.OrderReceiveMainPageContext>(UIPath.WindowPath.Order_Receive_Main_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Order_Receive_Main_Page, WindowType.Page, true);
        }

        public void ShowWareHouseMainPage()
        {
            UIManager.Instance.Register<UI.WareHousePageContext>(UIPath.WindowPath.WareHouse_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.WareHouse_Page, WindowType.Page, true);
        }

        ///Random Event
        public void ShowRandomEventDialog(int eventID)
        {
            UI.RandomEventDialogItem item = new UI.RandomEventDialogItem(
                ExploreModule.GetEventName(eventID),
                ExploreModule.GetEventTitleName(eventID),
                ExploreModule.GetEventDesc(eventID),
                ExploreModule.GetEventBG(eventID),
                ExploreModule.GetChooseItem(eventID)
                );

            UIManager.Instance.Register<UI.RandomEventDialogContext>(UIPath.WindowPath.RandomEvent_Dialog);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.RandomEvent_Dialog, WindowType.Dialog, true, item);
        }


        ///Explore 
        public void ShowExploreMainPage()
        {
            UIManager.Instance.Register<UI.ExploreMainPageContext>(UIPath.WindowPath.Explore_Main_Page);

            UIManager.Instance.HideWnd(UIPath.WindowPath.MainMenu_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Explore_Main_Page, WindowType.Page, true);
        }

    }
}