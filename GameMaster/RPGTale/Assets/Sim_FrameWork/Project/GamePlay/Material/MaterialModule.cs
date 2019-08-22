using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class MaterialModule : Singleton<MaterialModule> {

        public enum MaterialType
        {
            Artifact,
            Fluid
        }
        public enum MaterialRarity
        {
            Normal,
            Rare,
            Epic,
            Legend
        }

        public List<Material> MaterialList = new List<Material>();
        public Dictionary<int, Material> MaterialDic = new Dictionary<int, Material>();
        public List<Material_Artifact> Material_ArtifactList = new List<Material_Artifact>();
        public Dictionary<int, Material_Artifact> Material_ArtifactDic = new Dictionary<int, Material_Artifact>();
        public List<Material_Fluid> Material_FluidList = new List<Material_Fluid>();
        public Dictionary<int, Material_Fluid> Material_FluidDic = new Dictionary<int, Material_Fluid>();


        private bool HasInit = false;

        private const string MaterialPrefabPath = "Assets/Prefabs/Object/Material.prefab";

        #region data
        public void InitData()
        {
            if (HasInit)
                return;
            MaterialList = MaterialMetaDataReader.GetMaterialListData();
            MaterialDic = MaterialMetaDataReader.GetMaterialDic();
            Material_ArtifactList = MaterialMetaDataReader.GetMaterial_ArtifactListData();
            Material_ArtifactDic = MaterialMetaDataReader.GetMaterial_ArtifactDic();
            Material_FluidList = MaterialMetaDataReader.GetMaterial_FluidListData();
            Material_FluidDic = MaterialMetaDataReader.GetMaterial_FluidDic();

            HasInit = true;
        }

        public string GetMaterialName(int materialID)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialByMaterialID(materialID).MaterialName);
        }
        public string GetMaterialName(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(ma.MaterialName);
        }
        public string GetMaterialDesc(int materialID)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialByMaterialID(materialID).MaterialDesc);
        }
        public string GetmaterialDesc(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(ma.MaterialDesc);
        }
        public ushort GetMaterialRarity(int materialID)
        {
            return GetMaterialByMaterialID(materialID).Rarity;
        }

      

        //Type
        public MaterialType GetMaterialType(int materialID)
        {
            Material ma = GetMaterialByMaterialID(materialID);
            return GetMaterialType(ma);
        }
        public MaterialType GetMaterialType(Material ma)
        {
            switch (ma.Rarity)
            {
                case 1:
                    return MaterialType.Artifact;
                case 2:
                    return MaterialType.Fluid;
                default:
                    Debug.LogError("Material Type Error , ID=" + ma.MaterialID);
                    return MaterialType.Artifact;
            }
        }
        public T FetchMaterialTypeData<T>(int materialID) where T:class
        {
            switch (GetMaterialType(materialID))
            {
                case MaterialType.Artifact:
                    return GetMaterial_ArtifactDataByKey(GetMaterialByMaterialID(materialID).TypeIndex) as T;
                case MaterialType.Fluid:
                    return GetMaterial_FluidDataByKey(GetMaterialByMaterialID(materialID).TypeIndex) as T;
                default:
                    Debug.LogError("Fetch MaterialType Error  Material ID = " + materialID);
                    return null;
            }
        }

        public Material GetMaterialByMaterialID(int materialID)
        {
            Material ma = null;
            MaterialDic.TryGetValue(materialID, out ma);
            if(ma == null)
                Debug.LogError("GetMaterial Error  materialID=" + materialID);
            return ma;
        }
        public Material_Artifact GetMaterial_ArtifactDataByKey(int artifactID)
        {
            Material_Artifact ma = null;
            Material_ArtifactDic.TryGetValue(artifactID, out ma);
            if (ma == null)
                Debug.LogError("Get Material Artifact Error , ID = "+ artifactID);
            return ma;
        }
        public Material_Fluid GetMaterial_FluidDataByKey(int fluidID)
        {
            Material_Fluid mf = null;
            Material_FluidDic.TryGetValue(fluidID, out mf);
            if (mf == null)
                Debug.LogError("Get Material_Fluid  Error , ID = " + fluidID);
            return mf;
        }

        //Artifact
        public float GetMaterialArtifactProcessingTime(int materialID)
        {
            return FetchMaterialTypeData<Material_Artifact>(materialID).ProcessingTime;
        }
        public ushort GetMaterialArtifactQuality(int materialID)
        {
            return FetchMaterialTypeData<Material_Artifact>(materialID).ArtifactQuality;
        }


        #endregion


        #region Method

        public Sprite GetMaterialSprite(int materialID)
        {
            string path = GetMaterialByMaterialID(materialID).MaterialIcon;
            return Utility.LoadSprite(path,Utility.SpriteType.png);
        }

        public GameObject InitMaterialObj(int materialID)
        {
            GameObject MaterialObj = ObjectManager.Instance.InstantiateObject(MaterialPrefabPath);
            MaterialObj.transform.Find("Image").GetComponent<Image>().sprite = GetMaterialSprite(materialID);
            MaterialObj.transform.Find("Name").GetComponent<Text>().text = GetMaterialName(materialID);
            return MaterialObj;
        }
        #endregion
    }

    public class MaterialRarityData
    {
        public string RarityLevel;
        public string RarityColor;
        public string RarityName;
    }

}