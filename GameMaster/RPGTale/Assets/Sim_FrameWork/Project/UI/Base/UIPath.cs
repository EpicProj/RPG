﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class UIPath
    {

        public static string TechGroupPrefabPath(TechnologyGroup.GroupType type)
        {
            switch (type)
            {
                case TechnologyGroup.GroupType.Panel_3_1_1:
                    return "Assets/Prefabs/Object/Technology/TechGroupPanel_3-1-1.prefab";
                default:
                    return string.Empty;

            }
        }


        public struct WindowPath
        {
            #region DialogPath
            /// <summary>
            /// 菜单
            /// </summary>
            public const string Menu_Dialog = "Dialog/Main/MenuDialog";

            /// <summary>
            /// 信息确认框
            /// </summary>
            public const string General_Confirm_Dialog = "Dialog/Main/GeneralConfirmDialog";
            public const string General_Hint_Dialog = "Dialog/Main/GeneralHint";
            /// <summary>
            /// 载入存档
            /// </summary>
            public const string MainMenu_GameLoad_Dialog = "Dialog/Main/GameLoadDialog";

            public const string Leader_Detail_Dialog = "Dialog/Leader/LeaderDetailDialog";
            public const string Leader_Select_Dialog= "Dialog/Leader/LeaderSelectDialog";
            /// <summary>
            /// 生产线选择
            /// </summary>
            public const string ProductLine_Change_Dialog = "Dialog/FunctionBlock/ProductLineChangeDialog";
            public const string BlockNormalInfo_Dialog = "Dialog/FunctionBlock/BlockNormalInfoDialog";

            /// <summary>
            /// Tech Detail
            /// </summary>
            public const string Technology_Detail_Dialog = "Dialog/Technology/TechnologyDetailDialog";
            /// <summary>
            /// Random Event Choose Dialog
            /// </summary>
            public const string RandomEvent_Dialog = "Dialog/Main/RandomEventDialog";

            public const string Assemble_Part_Choose_Dialog = "Dialog/Assemble/AssemblePartChooseDialog";
            public const string Assemble_Ship_Choose_Dialog= "Dialog/Assemble/AssembleShipChooseDialog";

            public const string MainShip_Shield_Dialog = "Dialog/MainShip/MainShipShieldDialog";

            #endregion

            #region PagePath
            /// <summary>
            /// 主界面
            /// </summary>
            public const string MainMenu_Page = "Page/MainMenuPage";
            public const string NewGame_Prepare_Page = "Page/Main/NewGamePreparePage";
            /// <summary>
            /// 订单接收界面
            /// </summary>
            public const string Order_Receive_Main_Page = "Page/OrderReceiveMainPage";
            /// <summary>
            /// 开始界面
            /// </summary>
            public const string Game_Entry_Page = "Page/GameEntryPage";
            /// <summary>
            /// 场景加载界面
            /// </summary>
            public const string Loading_Scene_Page = "Page/LoadingPage";

            public const string Console_Page = "Page/ConsolePage";

            /// <summary>
            /// 仓库界面
            /// </summary>
            public const string WareHouse_Page = "Page/WareHousePage";

            /// <summary>
            ///  Block Page
            /// </summary>
            public const string BlockManu_Page = "Page/BlockManuPage";

            /// <summary>
            /// Tech Page
            /// </summary>
            public const string Technology_Page = "Page/Technology/TechnologyMainPage";

            /// <summary>
            /// 探索主界面
            /// </summary>
            public const string Explore_Main_Page = "Page/Explore/ExploreMainPage";
            public const string Explore_Point_Page = "Page/Explore/ExplorePointPage";

            /// <summary>
            /// Assemble Design
            /// </summary>
            public const string Assemble_Part_Design_Page = "Page/Assemble/AssemblePartDesignPage";
            public const string Assemble_Ship_Design_Page= "Page/Assemble/AssembleShipDesignPage";
            public const string Assemble_Ship_Build_Page= "Page/Assemble/AssembleShipBuildPage";

            public const string Camp_SelectMainPage = "Page/Camp/CampSelectMainPage";

            public const string Leader_Custom_Page = "Page/Leader/LeaderCustomPage";
            #endregion


            #region Misc
            public const string PlayerState_Panel = "Misc/PlayerState";

            /// <summary>
            /// 材料详情
            /// </summary>
            public const string Material_Info_UI = "Misc/MaterialInfoUI";
            /// <summary>
            /// 建造栏信息详情
            /// </summary>
            public const string BuildPanel_Detail_UI = "Misc/BuildPanelDetail";
            /// <summary>
            /// 区划详情
            /// </summary>
            public const string District_Detail_UI = "Misc/DistrictDetailUI";
            public const string Assemble_PartInfo_UI = "Misc/AssemblePartInfoUI";

            public const string DebugDialog = "Misc/DebugDialog";

            #endregion

        }



        public struct PrefabPath
        {
            #region ItemPath
            //区划格
            public const string DISTRICT_PREFAB_PATH = "Assets/Prefabs/Object/District/DistrictGrid.prefab";
            //区划底格
            public const string DISTRICTSLOT_PREFAB_PATH = "Assets/Prefabs/Object/District/BlockGrid.prefab";
            //区划建造单位
            public const string DISTRICT_BUILD_PREFAB_PATH = "Assets/Prefabs/Object/District/DistrictBuildElemet.prefab";
            //区划建造花费
            public const string DISTRICT_BUILD_COST_PREFAB_PATH = "Assets/Prefabs/Object/District/CostElement.prefab";


            //仓库主分类
            public const string WareHouse_Maintag_Prefab_Path = "Assets/Prefabs/Object/Main/WareHouseMainTag.prefab";
            //仓库副分类
            public const string WareHouse_Subtag_Prefab_Path = "Assets/Prefabs/Object/Main/WareHouseSubTag.prefab";


            //建造面板中区块
            public const string BUILD_ELEMENT_PREFAB_PATH = "Assets/Prefabs/Object/Main/BlockBuildElement.prefab";

            public const string map_chunk_prefab_path = "Assets/Prefabs/Map/Chunk/Chunk.prefab";

            #endregion

            #region ElementPath

            public const string General_ChooseTab = "Assets/Prefabs/Object/Main/General/GeneralChooseTab.prefab";
            public const string SliderSelectItem_Prepare = "Assets/Prefabs/Object/Main/GamePrepare/SliderSelectItem_Prepare.prefab";
            public const string General_DropDownChooseItem = "Assets/Prefabs/Object/Main/General/DropDownChooseItem.prefab";
            public const string General_InfoItem = "Assets/Prefabs/Object/Main/General/GeneralInfoItem.prefab";

            ///建造主页签
            public const string Construct_MainTab_Element_Path = "Assets/Prefabs/Object/Main/ConstructMainTabElement.prefab";
            public const string Order_Detail_Element = "Assets/Prefabs/Object/Order/OrderDetailElement.prefab";

            public const string Tech_Element_Simple = "Assets/Prefabs/Object/Technology/TechObject.prefab";

            public const string Tech_Effect_Element = "Assets/Prefabs/Object/Technology/TechEffectElement.prefab";
            public const string Tech_Require_Element = "Assets/Prefabs/Object/Technology/TechRequireElement.prefab";

            /// <summary>
            /// 建造详情，区划格
            /// </summary>
            public const string BuildDetail_District_Element = "Assets/Prefabs/Object/District/BuildDetailDistrictElement.prefab";
            public const string BuildDetail_Cost_Element = "Assets/Prefabs/Object/FunctionBlock/BuildRequireElement.prefab";

            public const string Reward_Item = "Assets/Prefabs/Object/Main/RewardItem.prefab";
            public const string MaterialCost_Item = "Assets/Prefabs/Object/Material/MaterialCostElement.prefab";

            public const string Explore_Area_Select_Btn = "Assets/Prefabs/Object/Explore/ExploreAreaSelectBtn.prefab";
            public const string Explore_Mission_Element = "Assets/Prefabs/Object/Explore/ExploreAreaMission.prefab";
            public const string Explore_Mission_Team_Obj = "Assets/Prefabs/Object/Explore/ExploreTeam.prefab";
            public const string Explore_Point_Element= "Assets/Prefabs/Object/Explore/ExplorePoint.prefab";

            /// Assemble
            public const string Assemble_Part_PropertyItem = "Assets/Prefabs/Object/Assemble/AssemblePropertyItem.prefab";
            public const string Assemble_Part_PropertyItemSmall= "Assets/Prefabs/Object/Assemble/AssemblePropertyItemSmall.prefab";
            public const string Assemble_Part_CustomItem_Left = "Assets/Prefabs/Object/Assemble/AssemblePartCustomItem_Left.prefab";
            public const string Assemble_Part_CustomItem_Right = "Assets/Prefabs/Object/Assemble/AssemblePartCustomItem_Right.prefab";

            public const string Assemble_Ship_Part_Top = "Assets/Prefabs/Object/Assemble/AssembleShipCustomPartItem_Top.prefab";
            public const string Assemble_Ship_Part_Bottom= "Assets/Prefabs/Object/Assemble/AssembleShipCustomPartItem_Bottom.prefab";

            ///Leader
            public const string Leader_Prepare_Card = "Assets/Prefabs/Object/Leader/LeaderCard_Prepare.prefab";
            public const string Leader_Skill_Item = "Assets/Prefabs/Object/Leader/LeaderSkillItem.prefab";
            public const string Leader_StoryRoot_Item= "Assets/Prefabs/Object/Leader/LeaderStoryRootItem.prefab";
            public const string Leader_Select_SotryItem = "Assets/Prefabs/Object/Leader/LeaderSelect_StoryItem.prefab";
            #endregion

        }


        public struct ScenePath
        {
            public const string Scene_Loading = "LoadingScene";
            public const string Scene_Test = "TestFactory";
            public const string Scene_GameEntry = "GameEntry";

        }


        public struct Misc
        {
            public const string Block_Selection_UI = "Assets/Prefabs/UI/Map/SelectionUI.prefab";

          
        }

    }

}