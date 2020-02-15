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
        ShowDebugMsg,
        GameAreaStateChange,

        /// <summary>
        /// Menu
        /// </summary>
        PlayMenuAnim,

        UpdateManuSlot,//更新生产格
        UpdateLevelInfo,//更新等级
        UpdateSpeedText,//更新速度
        UpdateWarehouseData,//更新仓库
        ProductLine_Formula_Change,//更新生产线

        GameSave_Select_Group,
        GameSave_Select_Save,

        ///MaimMenu
        MenuPage_Update_BuildPanel,
        UpdateTime,
        MenuPage_Add_Build,

        /// <summary>
        /// Update Resource
        /// </summary>
        Res_Daily_Total,
        Res_Currency,
        Res_DailyCurrency,
        Res_Energy,
        Res_DailyEnergy,
        Res_Research,
        Res_DailyResearch,
        Res_AIRobot_Maintenance,
        Res_AIRobot_Builder,
        Res_AIRobot_Operator,
        Res_RoCore,

        ///Camp
        CampSelectPage_SelectCamp,
        NewGamePage_UpdateCamp,

        ///Order
        RefreshOrder,
        OrderPage_Select_Organization,

        ///WareHouse
        WareHouse_Refresh_Detail,
        WareHouse_Hide_Detail,

        ///Technology
        Tech_Research_Finish,
        Tech_Research_Start,

        ///Random Event
        RandomEventDialog_Update_Effect,

        ///Explore
        ExplorePage_ShowArea_Mission,
        ExplorePage_Show_MissionDetail,
        ExplorePage_Show_PointDetail,
        ExplorePage_Finish_Point,
        ExplorePage_Update_PointTimer,

        ///Assemble
        Assemble_Part_PropertyChange,
        Assemble_PartTab_Select,
        Assemble_PartTab_Select_ChooseDialog,
        Assemble_ShipTab_Select,
        Assemble_PartPreset_Select,
        Assemble_ShipPreset_Select,
        Assemble_ShipDesign_PartSelect,

        ///MainShip
        MainShip_Area_PowerLevel_Change,
        MainShip_Area_EnergyLoad_Change,

        ///Leader
        LeaderDetail_Story_Select,
        
    }
}