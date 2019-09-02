using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class UIPath
    {
        #region DialogPath
        public const string FUCNTIONBLOCK_INFO_DIALOG = "Dialog/FunctionBlock/BlockInfoDialog.prefab";
        //材料界面
        public const string WAREHOURSE_DIALOG = "Dialog/Main/WareHouseDialog.prefab";

        #endregion

        #region PagePath
        //主界面
        public const string MAINMENU_PAGE= "Page/MainMenuPage.prefab";

        #endregion



        #region ItemPath
        //区划格
        public const string DISTRICT_PREFAB_PATH = "Assets/Prefabs/Object/District/District.prefab";
        //材料单位
        public const string MATERIAL_PREFAB_PATH = "Assets/Prefabs/Object/Material/MaterialItem.prefab";
        //区划建造单位
        public const string DISTRICT_BUILD_PREFAB_PATH= "Assets/Prefabs/Object/District/DistrictBuildElemet.prefab";
        //区划建造花费
        public const string DISTRICT_BUILD_COST_PREFAB_PATH = "Assets/Prefabs/Object/District/CostElement.prefab";
        //仓库材料单位
        public const string MATERIAL_WAREHOUSE_PREFAB_PATH = "Assets/Prefabs/Object/Material/MaterialObj.prefab";

        public const string FUNCTIONBLOCK_PREFAB_PATH = "Assets/Prefabs/Object/ItemUIPrefab.prefab";
        #endregion

    }
}