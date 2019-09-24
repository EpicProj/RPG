using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class UIPath
    {
        #region DialogPath
        public const string FUNCTIONBLOCK_INFO_DIALOG = "Dialog/FunctionBlock/BlockInfoDialog";
        //材料界面
        public const string WAREHOURSE_DIALOG = "Dialog/Main/WareHouseDialog";
        /// <summary>
        /// 信息确认框
        /// </summary>
        public const string General_Confirm_Dialog = "Dialog/Main/GeneralConfirmDialog";

        #endregion

        #region PagePath
        /// <summary>
        /// 主界面
        /// </summary>
        public const string MainMenu_Page= "Page/MainMenuPage";
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

        #endregion



        #region ItemPath
        //区划格
        public const string DISTRICT_PREFAB_PATH = "Assets/Prefabs/Object/District/District.prefab";
        //区划底格
        public const string DISTRICTSLOT_PREFAB_PATH = "Assets/Prefabs/Object/District/BlockGrid.prefab";
        //材料单位
        public const string MATERIAL_PREFAB_PATH = "Assets/Prefabs/Object/Material/MaterialItem.prefab";
        //区划建造单位
        public const string DISTRICT_BUILD_PREFAB_PATH= "Assets/Prefabs/Object/District/DistrictBuildElemet.prefab";
        //区划建造花费
        public const string DISTRICT_BUILD_COST_PREFAB_PATH = "Assets/Prefabs/Object/District/CostElement.prefab";
        //仓库材料单位
        public const string MATERIAL_WAREHOUSE_PREFAB_PATH = "Assets/Prefabs/Object/Material/MaterialObj.prefab";
        //仓库主分类
        public const string WareHouse_Maintag_Prefab_Path = "Assets/Prefabs/Object/Main/WareHouseMainTag.prefab";
        //仓库副分类
        public const string WareHouse_Subtag_Prefab_Path= "Assets/Prefabs/Object/Main/WareHouseSubTag.prefab";

        //物品
        public const string FUNCTIONBLOCK_MATERIAL_PREFAB_PATH = "Assets/Prefabs/Object/ItemUIPrefab.prefab";

        //建造面板中区块
        public const string BUILD_ELEMENT_PREFAB_PATH= "Assets/Prefabs/Object/Main/BlockBuildElement.prefab";




        #endregion

        #region ElementPath
        ///建造主页签
        public const string Construct_MainTab_Element_Path = "Assets/Prefabs/Object/Main/ConstructMainTabElement.prefab";
        public const string Test = "Assets/Prefabs/Object/Order/OrderDetailElement.prefab";
        #endregion


        public const string Scene_Loading = "LoadingScene";
        public const string Scene_Test = "TestFactory";
        public const string Scene_GameEntry = "GameEntry";
            

    }

}