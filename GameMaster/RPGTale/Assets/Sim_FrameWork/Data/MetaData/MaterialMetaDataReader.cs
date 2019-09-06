using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MaterialMetaDataReader 
    {
        public static List<Material> MaterialList = new List<Material>();
        public static Dictionary<int, Material> MaterialDic = new Dictionary<int, Material>();

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


    }
}