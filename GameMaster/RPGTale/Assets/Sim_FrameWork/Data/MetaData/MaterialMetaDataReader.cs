using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class MaterialMetaDataReader 
    {
        public static List<Material> MaterialList;
        public static Dictionary<int, Material> MaterialDic;

        public static List<MaterialType> MaterialTypeList;
        public static Dictionary<string, MaterialType> MaterialTypeDic;

        public static List<MaterialSubType> MaterialSubTypeList;
        public static Dictionary<string, MaterialSubType> MaterialSubTypeDic;

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<MaterialMetaData>(ConfigPath.TABLE_MATERIAL_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("MaterialMetaData Read Error");
                return;
            }

            MaterialList = config.AllMaterialList;
            MaterialDic = config.AllMaterialDic;
            MaterialTypeList = config.AllMaterialTypeList;
            MaterialTypeDic = config.AllMaterialTypeDic;
            MaterialSubTypeList = config.AllMaterialSubTypeList;
            MaterialSubTypeDic = config.AllMaterialSubTypeDic;
        }


        public static List<Material> GetMaterialListData()
        {
            LoadData();
            return MaterialList;
        }


        public static Dictionary<int,Material> GetMaterialDic()
        {
            LoadData();
            return MaterialDic;
        }

        public static List<MaterialType> GetMaterialTypeListData()
        {
            LoadData();
            return MaterialTypeList;
        }
        public static Dictionary<string,MaterialType> GetMaterialTypeDic()
        {
            LoadData();
            return MaterialTypeDic;
        }

        public static List<MaterialSubType> GetMaterialSubTypeListData()
        {
            LoadData();
            return MaterialSubTypeList;
        }
        public static Dictionary<string, MaterialSubType> GetMaterialSubTypeDic()
        {
            LoadData();
            return MaterialSubTypeDic;
        }
    }
}