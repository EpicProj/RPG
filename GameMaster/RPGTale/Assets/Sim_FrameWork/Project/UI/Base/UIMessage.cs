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

        /// <summary>
        /// Update Resource
        /// </summary>
        Res_Food,
        Res_MonthFood,
        Res_Currency,
        Res_Energy,
        Res_MonthEnergy,
        Res_Labor,
        Res_MonthLabor,


        UpdateBuildPanelData, //更新建造列表
        UpdateTime,//更新时间

        ///Order
        RefreshOrder,
        OrderPage_Select_Organization,

        ///WareHouse
        WareHouse_Refresh_Detail,
        WareHouse_Hide_Detail,

        ///Technology
        Tech_Research_Finish
    }
}