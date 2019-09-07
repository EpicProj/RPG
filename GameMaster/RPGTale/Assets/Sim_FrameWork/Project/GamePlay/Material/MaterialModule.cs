using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class MaterialModule : Singleton<MaterialModule> {


        public List<Material> MaterialList = new List<Material>();
        public Dictionary<int, Material> MaterialDic = new Dictionary<int, Material>();

        public MaterialConfig maConfig;
        public List<string> AllMaterialRarityList = new List<string>();
        public List<string> AllMaterialTypeList = new List<string>();

        private bool HasInit = false;



        #region data
        public void InitData()
        {
            if (HasInit)
                return;
            MaterialList = MaterialMetaDataReader.GetMaterialListData();
            MaterialDic = MaterialMetaDataReader.GetMaterialDic();
            maConfig = new MaterialConfig();
            maConfig.LoadConfigData();
            AllMaterialRarityList = GetAllMainMaterialTypeList();
            AllMaterialTypeList = GetAllMainMaterialTypeList();
            HasInit = true;
        }

        public Material GetMaterialByMaterialID(int materialID)
        {
            Material ma = null;
            MaterialDic.TryGetValue(materialID, out ma);
            if (ma == null)
                Debug.LogError("GetMaterial Error  materialID=" + materialID);
            return ma;
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

        public string GetMaterialUnitName(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(ma.UnitName);
        }
        public string GetMaterialUnitName(int materialID)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialByMaterialID(materialID).UnitName);
        }



        //Rarity
        public List<string> GetAllMaterialRarityList()
        {
            List<string> result = new List<string>();
            foreach(var data in maConfig.rarityDataList)
            {
                if (result.Contains(data.RarityLevel))
                {
                    Debug.LogError("Find Same RarityID ,ID=" + data.RarityLevel);
                    continue;
                }
                else
                {
                    result.Add(data.RarityLevel);
                }
            }
            return result;
        }

        public string GetMaterialRarity(int materialID)
        {
            return GetMaterialRarityData(materialID).RarityLevel;
        }
        /// <summary>
        /// 获取稀有度信息
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public MaterialConfig.MaterialRarityData GetMaterialRarityData(int materialID)
        {
            MaterialConfig.MaterialRarityData data = null;
            string rarity = GetMaterialByMaterialID(materialID).Rarity;
            if (AllMaterialRarityList.Contains(rarity))
            {
                return maConfig.rarityDataList.Find(x => x.RarityLevel == rarity);
            }
            else
            {
                Debug.LogError("Material Rarity Error!");
            }
            return data;
        }
        public string GetRarityName(MaterialConfig.MaterialRarityData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.RarityName);
        }



        //Type

        /// <summary>
        /// 获取所有主要材料类型
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllMainMaterialTypeList()
        {
            List<string> result = new List<string>();
            foreach(var data in maConfig.materialTypeList)
            {
                if (result.Contains(data.Type))
                {
                    Debug.LogError("Find Same Material Main Type!  TypeName=" + data.Type);
                    continue;
                }
                else
                {
                    result.Add(data.Type);
                }
            }
            return result;
           
        }

        public MaterialConfig.MaterialType GetMaterialTypeData(string tagName)
        {
            return maConfig.materialTypeList.Find(x => x.Type == tagName);
        }
        /// <summary>
        /// 获取所有副类型
        /// </summary>
        /// <param name="mainTypeName"></param>
        /// <returns></returns>
        public List<string> GetMaterialSubTypeList(string mainTypeName)
        {
            List<string> result = new List<string>();
            if (AllMaterialTypeList.Contains(mainTypeName))
            {
                var list = maConfig.materialTypeList.Find(x => x.Type == mainTypeName);
                if (list != null)
                {
                    if (list.SubTypeList == null)
                    {
                        return result;
                    }
                    foreach(var subData in list.SubTypeList)
                    {
                        if (result.Contains(subData.SubTypeName))
                        {
                            Debug.LogError("Find Same SubTypeName !  name=" + subData.SubTypeName);
                            continue;
                        }
                        result.Add(subData.Type);
                    }
                }
                else
                {
                    Debug.LogError("Get Sub Type Error!  MainType=" + mainTypeName);
                }
            }
            else
            {
                Debug.LogError("MainType Error,Can not find SubType Data ,MainType=" + mainTypeName);
            }
            return result;
        }

        public List<MaterialConfig.MaterialType.MaterialSubType> GetMaterialSubTypeDataList(string tagName)
        {
            List<MaterialConfig.MaterialType.MaterialSubType> result = new List<MaterialConfig.MaterialType.MaterialSubType>();
            MaterialConfig.MaterialType type = GetMaterialTypeData(tagName);
            List<string> subTag = GetMaterialSubTypeList(tagName);
            if (subTag != null)
            {
                for(int i = 0; i < subTag.Count; i++)
                {
                    result.Add(type.SubTypeList.Find(x => x.Type == subTag[i]));
                }
            }
            return result;
        }
        /// <summary>
        /// 获取主分类
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public MaterialConfig.MaterialType GetMaterialMainType(int materialID)
        {
            string mainType = GetMaterialByMaterialID(materialID).Type;
            if (AllMaterialTypeList.Contains(mainType))
            {
                return maConfig.materialTypeList.Find(x => x.Type == mainType);
            }
            else
            {
                Debug.LogError("Material Main Type Error! type=" + mainType);
                return null;
            }
        }
        public MaterialConfig.MaterialType GetMaterialMainType(Material ma)
        {
            return GetMaterialMainType(ma.MaterialID);
        }

        public string GetMaterialMainTypeName(MaterialConfig.MaterialType type)
        {
            return MultiLanguage.Instance.GetTextValue(type.TypeName);
        }
        public string GetMaterialMainTypeName(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialMainType(ma).TypeName);
        }
        public string GetMaterialMainTypeDesc(MaterialConfig.MaterialType type)
        {
            return MultiLanguage.Instance.GetTextValue(type.TypeDesc);
        }

        public Sprite GetMaterialMainTypeSprite(MaterialConfig.MaterialType type)
        {
            return Utility.LoadSprite(type.TypeIconPath, Utility.SpriteType.png);
        }

        public Sprite GetMaterialMainTypeSprite(Material ma)
        {
            return Utility.LoadSprite(GetMaterialMainType(ma.MaterialID).TypeIconPath, Utility.SpriteType.png);
        }
        /// <summary>
        /// 获取副分类
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public MaterialConfig.MaterialType.MaterialSubType GetMaterialSubType(int materialID)
        {
            MaterialConfig.MaterialType mainType = GetMaterialMainType(materialID);
            string subtype = GetMaterialByMaterialID(materialID).SubType;
            if (GetMaterialSubTypeList(mainType.Type).Contains(subtype))
            {
                return mainType.SubTypeList.Find(x => x.Type == subtype);
            }
            else
            {
                Debug.LogError("GetSubType Error ! SubType=" + subtype);
                return null;
            }
        }
        public MaterialConfig.MaterialType.MaterialSubType GetMaterialSubType(Material ma)
        {
            return GetMaterialSubType(ma.MaterialID);
        }

        public string GetMaterialSubTypeName(MaterialConfig.MaterialType.MaterialSubType subtype)
        {
            return MultiLanguage.Instance.GetTextValue(subtype.SubTypeName);
        }
        public string GetMaterialSubTypeDesc(MaterialConfig.MaterialType.MaterialSubType subtype)
        {
            return MultiLanguage.Instance.GetTextValue(subtype.SubTypeDesc);
        }

        public Sprite GetMaterialSubTypeIcon(MaterialConfig.MaterialType.MaterialSubType subtype)
        {
            return Utility.LoadSprite(subtype.SubTypeIcon, Utility.SpriteType.png);
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
            GameObject MaterialObj = ObjectManager.Instance.InstantiateObject(UIPath.FUNCTIONBLOCK_MATERIAL_PREFAB_PATH);
            MaterialObj.transform.Find("Image").GetComponent<Image>().sprite = GetMaterialSprite(materialID);
            MaterialObj.transform.Find("Name").GetComponent<Text>().text = GetMaterialName(materialID);
            return MaterialObj;
        }
        #endregion
    }

    public class MaterialStorageData
    {
        public Material material;
        public int count;
        public MaterialConfig.MaterialType mainType;
        public MaterialConfig.MaterialType.MaterialSubType subType;

        public MaterialStorageData(Material ma,int count)
        {
            material = ma;
            this.count = count;
            this.mainType = MaterialModule.Instance.GetMaterialMainType(ma.MaterialID);
            this.subType = MaterialModule.Instance.GetMaterialSubType(ma.MaterialID);
        }

    }

    public class MaterialConfig
    {
        //材料稀有度
        public List<MaterialRarityData> rarityDataList;
        //材料分类
        public List<MaterialType> materialTypeList;


        public MaterialConfig LoadConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();

            MaterialConfig config= reader.LoadJsonDataConfig<MaterialConfig>(Config.JsonConfigPath.MaterialConfigJsonPath);
            rarityDataList = config.rarityDataList;
            materialTypeList = config.materialTypeList;

            return config;
        }


        public class MaterialRarityData
        {
            public string RarityLevel;
            public string RarityColor;
            public string RarityName;
        }

        public class MaterialType
        {
            public string Type;
            public string TypeName;
            public string TypeDesc;
            public string TypeIconPath;
            public List<MaterialSubType> SubTypeList;

            public class MaterialSubType
            {
                public string Type;
                public string SubTypeName;
                public string SubTypeDesc;
                public string SubTypeIcon;
            }
        }


    }



}