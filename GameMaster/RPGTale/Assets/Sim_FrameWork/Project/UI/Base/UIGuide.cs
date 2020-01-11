﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sim_FrameWork.UI;

namespace Sim_FrameWork
{
    public class UIGuide : Singleton<UIGuide>
    {

        #region General
        /// <summary>
        /// Show Main Page
        /// </summary>
        /// <param name="closeAll"></param>
        public void ShowGameMainPage(bool closeAll)
        {
            //if (closeAll)
            //{
            //    UIManager.Instance.CloseAllWnd();
            //}
            UIManager.Instance.Register<UI.MainMenuPageContext>(UIPath.WindowPath.MainMenu_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.MainMenu_Page, WindowType.Page, true);
        }

        /// <summary>
        /// 菜单
        /// </summary>
        public void ShowMenuDialog()
        {
            UIManager.Instance.Register<UI.MenuDialogContext>(UIPath.WindowPath.Menu_Dialog);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Menu_Dialog, WindowType.Dialog, true);
        }

        public void ShowGeneralHint(GeneralHintDialogItem item)
        {
            UIManager.Instance.Register<UI.GeneralHintDialogContext>(UIPath.WindowPath.General_Hint_Dialog);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.General_Hint_Dialog, WindowType.SPContent, true, item);
        }
        /// <summary>
        /// 通用确认框
        /// </summary>
        /// <param name="item"></param>
        public void ShowGeneralConfirmDialog(GeneralConfirmDialogItem item)
        {
            UIManager.Instance.Register<UI.GeneralConfirmDialogContext>(UIPath.WindowPath.General_Confirm_Dialog);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.General_Confirm_Dialog, WindowType.Dialog, true, item);
        }


        public void ShowMaterialDetailInfo(MaterialDataModel model)
        {
            UIManager.Instance.Register<UI.MaterialInfoUIContext>(UIPath.WindowPath.Material_Info_UI);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Material_Info_UI, WindowType.SPContent, true, model);
        }

        #endregion

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
        public void ShowRandomEventDialog(int eventID, int areaID,int exploreID,int pointID)
        {
            UIManager.Instance.Register<UI.RandomEventDialogContext>(UIPath.WindowPath.RandomEvent_Dialog);
            UI.RandomEventDialogItem item = new UI.RandomEventDialogItem(eventID, areaID, exploreID, pointID);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.RandomEvent_Dialog, WindowType.Dialog, true, item);
        }

        ///Explore 
        public void ShowExploreMainPage()
        {
            UIManager.Instance.Register<UI.ExploreMainPageContext>(UIPath.WindowPath.Explore_Main_Page);

            UIManager.Instance.HideWnd(UIPath.WindowPath.MainMenu_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Explore_Main_Page, WindowType.Page, true);
        }

        public void ShowExplorePointPage(ExploreRandomItem item)
        {
            UIManager.Instance.Register<UI.ExplorePointPageContext>(UIPath.WindowPath.Explore_Point_Page);
            UIManager.Instance.HideWnd(UIPath.WindowPath.Explore_Main_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Explore_Point_Page, WindowType.Page, true,item);
        }

        ///Assemble
        public void ShowAssemblePartDesignPage(AssemblePartInfo info)
        {
            UIManager.Instance.Register<UI.AssemblePartDesignPageContext>(UIPath.WindowPath.Assemble_Part_Design_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Assemble_Part_Design_Page, WindowType.Page,true,info);
        }

        public void ShowAssembleShipDesignPage(AssembleShipInfo info)
        {
            UIManager.Instance.Register<UI.AssembleShipDesignPageContext>(UIPath.WindowPath.Assemble_Ship_Design_Page);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Assemble_Ship_Design_Page, WindowType.Page,true, info);
        }

        /// <summary>
        /// 展示方式
        /// 0=null
        /// 1=查看
        /// 2=选择
        /// </summary>
        public void ShowAssemblePartChooseDialog(List<string> sortList,string defaultSelectTab,byte displayMode, int configID=0)
        {
            UIManager.Instance.Register<UI.AssemblePartChooseDialogContext>(UIPath.WindowPath.Assemble_Part_Choose_Dialog);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Assemble_Part_Choose_Dialog, WindowType.Dialog, true, sortList, defaultSelectTab,displayMode,configID);
        }

        public void ShowAssembleShipChooseDialog(List<string> sortList,string defaultSelectTab)
        {
            UIManager.Instance.Register<UI.AssembleShipChooseDialogContext>(UIPath.WindowPath.Assemble_Ship_Choose_Dialog);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.Assemble_Ship_Choose_Dialog, WindowType.Dialog, true, sortList, defaultSelectTab);
        }

    }
}