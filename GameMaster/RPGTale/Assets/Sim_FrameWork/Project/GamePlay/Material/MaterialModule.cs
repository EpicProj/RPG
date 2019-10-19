using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class MaterialModule : BaseModule<MaterialModule> {


        public static List<Material> MaterialList;
        public static Dictionary<int, Material> MaterialDic;

        public static MaterialConfig maConfig;
        public static List<string> AllMaterialRarityList;
        public static List<string> AllMaterialTypeList;


        public override void InitData()
        {
            MaterialList = MaterialMetaDataReader.GetMaterialListData();
            MaterialDic = MaterialMetaDataReader.GetMaterialDic();
            maConfig = new MaterialConfig();
            maConfig.LoadConfigData();
            AllMaterialRarityList = GetAllMaterialRarityList();
            AllMaterialTypeList = GetAllMainMaterialTypeList();
        }

        public override void Register()
        {
           
        }

        public MaterialModule()
        {
            InitData();
        }
        #region data


        public static Material GetMaterialByMaterialID(int materialID)
        {
            Material ma = null;
            MaterialDic.TryGetValue(materialID, out ma);
            if (ma == null)
                Debug.LogError("GetMaterial Error  materialID=" + materialID);
            return ma;
        }

        public static string GetMaterialName(int materialID)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialByMaterialID(materialID).MaterialName);
        }
        public static string GetMaterialName(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(ma.MaterialName);
        }
        public static string GetMaterialNameEn(int materialID)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialByMaterialID(materialID).NameEn);
        }
        public static string GetMaterialNameEn(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(ma.NameEn);
        }

        public static string GetMaterialDesc(int materialID)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialByMaterialID(materialID).MaterialDesc);
        }
        public static string GetMaterialDesc(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(ma.MaterialDesc);
        }

        public static string GetMaterialUnitName(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(ma.UnitName);
        }
        public static string GetMaterialUnitName(int materialID)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialByMaterialID(materialID).UnitName);
        }

        public static Sprite GetMaterialSprite(int materialID)
        {
            string path = GetMaterialByMaterialID(materialID).MaterialIcon;
            return Utility.LoadSprite(path, Utility.SpriteType.png);
        }
        public static Sprite GetMaterialBG(int materialID)
        {
            string path = GetMaterialByMaterialID(materialID).BG;
            return Utility.LoadSprite(path, Utility.SpriteType.png);
        }


        //Rarity
        public static List<string> GetAllMaterialRarityList()
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

        public static string GetMaterialRarity(int materialID)
        {
            return GetMaterialRarityData(materialID).RarityLevel;
        }
        /// <summary>
        /// 获取稀有度信息
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public static MaterialConfig.MaterialRarityData GetMaterialRarityData(int materialID)
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
        public static string GetMaterialRarityName(MaterialConfig.MaterialRarityData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.RarityName);
        }
        public string GetMaterialRarityName(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialRarityData(ma.MaterialID).RarityName);
        }

        public Color TryParseRarityColor(Material ma)
        {
            Color result = new Color();
            ColorUtility.TryParseHtmlString(GetMaterialRarityData(ma.MaterialID).RarityColor, out result);
            if (result == null)
            {
                Debug.LogError("Parse Color Error! color=" + GetMaterialRarityData(ma.MaterialID).RarityColor);
            }
            return result;
        }
        //Type

        /// <summary>
        /// 获取所有主要材料类型
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllMainMaterialTypeList()
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

        public static MaterialConfig.MaterialType GetMaterialTypeData(string tagName)
        {
            return maConfig.materialTypeList.Find(x => x.Type == tagName);
        }
        /// <summary>
        /// 获取所有副类型
        /// </summary>
        /// <param name="mainTypeName"></param>
        /// <returns></returns>
        public static List<string> GetMaterialSubTypeList(string mainTypeName)
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

        public static List<MaterialConfig.MaterialType.MaterialSubType> GetMaterialSubTypeDataList(string tagName)
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
        public static MaterialConfig.MaterialType GetMaterialMainType(int materialID)
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
        public static MaterialConfig.MaterialType GetMaterialMainType(Material ma)
        {
            return GetMaterialMainType(ma.MaterialID);
        }

        public static string GetMaterialMainTypeName(MaterialConfig.MaterialType type)
        {
            return MultiLanguage.Instance.GetTextValue(type.TypeName);
        }
        public static string GetMaterialMainTypeName(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialMainType(ma).TypeName);
        }
        public static string GetMaterialMainTypeDesc(MaterialConfig.MaterialType type)
        {
            return MultiLanguage.Instance.GetTextValue(type.TypeDesc);
        }

        public static Sprite GetMaterialMainTypeSprite(MaterialConfig.MaterialType type)
        {
            return Utility.LoadSprite(type.TypeIconPath, Utility.SpriteType.png);
        }

        public static Sprite GetMaterialMainTypeSprite(Material ma)
        {
            return Utility.LoadSprite(GetMaterialMainType(ma.MaterialID).TypeIconPath, Utility.SpriteType.png);
        }
        /// <summary>
        /// 获取副分类
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public static MaterialConfig.MaterialType.MaterialSubType GetMaterialSubType(int materialID)
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
        public static MaterialConfig.MaterialType.MaterialSubType GetMaterialSubType(Material ma)
        {
            return GetMaterialSubType(ma.MaterialID);
        }

        public static string GetMaterialSubTypeName(MaterialConfig.MaterialType.MaterialSubType subtype)
        {
            return MultiLanguage.Instance.GetTextValue(subtype.SubTypeName);
        }
        public static string GetMaterialSubTypeName(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialSubType(ma.MaterialID).SubTypeName);
        }
        public static string GetMaterialSubTypeDesc(MaterialConfig.MaterialType.MaterialSubType subtype)
        {
            return MultiLanguage.Instance.GetTextValue(subtype.SubTypeDesc);
        }

        public static Sprite GetMaterialSubTypeIcon(MaterialConfig.MaterialType.MaterialSubType subtype)
        {
            return Utility.LoadSprite(subtype.SubTypeIcon, Utility.SpriteType.png);
        }
        public static Sprite GetMaterialSubTypeIcon(Material ma)
        {
            return Utility.LoadSprite(GetMaterialSubType(ma.MaterialID).SubTypeIcon, Utility.SpriteType.png);
        }
        #endregion


        #region Method

    


        #endregion
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