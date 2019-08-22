using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MaterialMetaDataReader 
    {
        public static List<Material> MaterialList = new List<Material>();
        public static Dictionary<int, Material> MaterialDic = new Dictionary<int, Material>();

        public static List<Material_Artifact> Material_ArtifactList = new List<Material_Artifact>();
        public static Dictionary<int, Material_Artifact> Material_ArtifactDic = new Dictionary<int, Material_Artifact>();

        public static List<Material_Fluid> Material_FluidList = new List<Material_Fluid>();
        public static Dictionary<int, Material_Fluid> Material_FluidDic = new Dictionary<int, Material_Fluid>();

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
            Material_ArtifactList = config.AllMaterial_ArtifactList;
            Material_ArtifactDic = config.AllMaterial_ArtifactDic;
            Material_FluidList = config.AllMaterial_FluidList;
            Material_FluidDic = config.AllMaterial_FluidDic;
        }


        public static List<Material> GetMaterialListData()
        {
            LoadData();
            return MaterialList;
        }
        public static List<Material_Artifact> GetMaterial_ArtifactListData()
        {
            LoadData();
            return Material_ArtifactList;
        }
        public static List<Material_Fluid> GetMaterial_FluidListData()
        {
            LoadData();
            return Material_FluidList;
        }


        public static Dictionary<int,Material> GetMaterialDic()
        {
            LoadData();
            return MaterialDic;
        }
        public static Dictionary<int,Material_Artifact> GetMaterial_ArtifactDic()
        {
            LoadData();
            return Material_ArtifactDic;
        }
        public static Dictionary<int,Material_Fluid> GetMaterial_FluidDic()
        {
            LoadData();
            return Material_FluidDic;
        }


    }
}