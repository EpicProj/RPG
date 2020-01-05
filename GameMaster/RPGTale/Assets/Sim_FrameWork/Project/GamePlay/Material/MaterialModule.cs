using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class MaterialModule : BaseModule<MaterialModule> {

        public static List<Material> MaterialList;
        public static Dictionary<int, Material> MaterialDic;

        public static List<MaterialType> MaterialTypeList;
        public static Dictionary<string, MaterialType> MaterialTypeDic;

        public static List<MaterialSubType> MaterialSubTypeList;
        public static Dictionary<string, MaterialSubType> MaterialSubTypeDic;

        public static MaterialConfig maConfig;


        public override void InitData()
        {
            MaterialList = MaterialMetaDataReader.GetMaterialListData();
            MaterialDic = MaterialMetaDataReader.GetMaterialDic();
            MaterialTypeList = MaterialMetaDataReader.GetMaterialTypeListData();
            MaterialTypeDic = MaterialMetaDataReader.GetMaterialTypeDic();
            MaterialSubTypeList = MaterialMetaDataReader.GetMaterialSubTypeListData();
            MaterialSubTypeDic = MaterialMetaDataReader.GetMaterialSubTypeDic();

            maConfig = new MaterialConfig();
            maConfig.LoadConfigData();
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


        /// <summary>
        /// 获取稀有度信息
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public static GeneralRarity GetMaterialRarityData(int materialID)
        {
            return GeneralModule.Instance.GetRarity(GetMaterialByMaterialID(materialID).Rarity);
        }



        /// <summary>
        /// 获取主分类
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public static MaterialType GetMaterialType(int materialID)
        {
            MaterialType type = null;
            string mainType = GetMaterialByMaterialID(materialID).Type;
            MaterialTypeDic.TryGetValue(mainType, out type);
            if (type == null)
            {
                Debug.LogError("Material Type Error! type=" + mainType);
            }
            return type;
        }
        public static MaterialType GetMaterialType(Material ma)
        {
            return GetMaterialType(ma.MaterialID);
        }

        public static string GetMaterialTypeName(MaterialType type)
        {
            return MultiLanguage.Instance.GetTextValue(type.TypeName);
        }
        public static string GetMaterialTypeName(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialType(ma).TypeName);
        }
        public static string GetMaterialTypeDesc(MaterialType type)
        {
            return MultiLanguage.Instance.GetTextValue(type.TypeDesc);
        }

        public static Sprite GetMaterialTypeSprite(MaterialType type)
        {
            return Utility.LoadSprite(type.TypeIcon, Utility.SpriteType.png);
        }

        public static Sprite GetMaterialTypeSprite(Material ma)
        {
            return Utility.LoadSprite(GetMaterialType(ma.MaterialID).TypeIcon, Utility.SpriteType.png);
        }

        /// <summary>
        /// 获取副分类
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public static MaterialSubType GetMaterialSubType(int materialID)
        {
            var mainType = GetMaterialType(materialID);
            string subtype = GetMaterialByMaterialID(materialID).SubType;
            //Check Value
            var result = GetMaterialSubTypeList(mainType).Find(x => x.SubType == subtype);
            if (result != null)
                return result;
            return null;
        }

        public static List<MaterialSubType> GetMaterialSubTypeList(MaterialType type)
        {
            List<MaterialSubType> result = new List<MaterialSubType>();
            var list= Utility.TryParseStringList(type.SubTypeList, ',');
            for(int i = 0; i < list.Count; i++)
            {
                var subType = MaterialSubTypeList.Find(x => x.SubType == list[i]);
                if (subType != null)
                {
                    result.Add(subType);
                }
                else
                {
                    Debug.LogError("illegal Material Sub Type, subTypeName=" + list[i]);
                }
            }
            return result;
        }



        public static MaterialSubType GetMaterialSubType(Material ma)
        {
            return GetMaterialSubType(ma.MaterialID);
        }

        public static string GetMaterialSubTypeName(MaterialSubType subtype)
        {
            return MultiLanguage.Instance.GetTextValue(subtype.TypeName);
        }
        public static string GetMaterialSubTypeName(Material ma)
        {
            return MultiLanguage.Instance.GetTextValue(GetMaterialSubType(ma.MaterialID).TypeName);
        }
        public static string GetMaterialSubTypeDesc(MaterialSubType subtype)
        {
            return MultiLanguage.Instance.GetTextValue(subtype.TypeDesc);
        }

        public static Sprite GetMaterialSubTypeIcon(MaterialSubType subtype)
        {
            return Utility.LoadSprite(subtype.TypeIcon, Utility.SpriteType.png);
        }
        public static Sprite GetMaterialSubTypeIcon(Material ma)
        {
            return Utility.LoadSprite(GetMaterialSubType(ma.MaterialID).TypeIcon, Utility.SpriteType.png);
        }
        #endregion


        #region Method
        /// <summary>
        /// 材料存储数据
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<List<BaseDataModel>> InitMaterialStorageModel(List<MaterialStorageItem> items)
        {
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();
            for(int i = 0; i < items.Count; i++)
            {
                List<BaseDataModel> list = new List<BaseDataModel>();
                MaterialStorageModel model = new MaterialStorageModel();
                model.Create(items[i].info.ID);
                list.Add((BaseDataModel)model);
                result.Add(list);
            }
            return result;
        }
    


        #endregion
    }

    public class MaterialCostItem
    {
        public MaterialDataModel model;
        public int count;

        public bool InitSuccess = true;

        public MaterialCostItem(int materialID,int count)
        {
            model = new MaterialDataModel();
            if (!model.Create(materialID))
                InitSuccess=false;
            this.count = count;
        }
    }


    public class MaterialConfig
    {

        public MaterialConfig LoadConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();

            MaterialConfig config= reader.LoadJsonDataConfig<MaterialConfig>(Config.JsonConfigPath.MaterialConfigJsonPath);

            return config;
        }


    }




}