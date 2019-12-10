using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class UIMessage 
    {
        public UIMsgType type;
        public List<object> content;

        public UIMessage(UIMsgType Msgtype, List<object> Content=null)
        {
            type = Msgtype;
            content = Content;
        }
    }

    public enum UIMsgType
    {
        /// <summary>
        /// Menu
        /// </summary>
        PlayMenuAnim,

        UpdateManuSlot,//更新生产格
        UpdateLevelInfo,//更新等级
        UpdateSpeedText,//更新速度
        UpdateWarehouseData,//更新仓库
        ProductLine_Formula_Change,//更新生产线


        ///MaimMenu
        MenuPage_Update_BuildPanel,
        UpdateTime,
        MenuPage_Add_Build,

        /// <summary>
        /// Update Resource
        /// </summary>
        Res_Currency,
        Res_MonthCurrency,
        Res_Energy,
        Res_MonthEnergy,
        Res_Research,
        Res_MonthResearch,
        Res_Builder,
        Res_RoCore,

        ///Order
        RefreshOrder,
        OrderPage_Select_Organization,

        ///WareHouse
        WareHouse_Refresh_Detail,
        WareHouse_Hide_Detail,

        ///Technology
        Tech_Research_Finish,
        Tech_Research_Start

    }
}