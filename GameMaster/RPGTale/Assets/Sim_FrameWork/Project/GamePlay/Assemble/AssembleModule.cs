using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class AssembleModule : BaseModule<AssembleModule>
    {
        private static List<AssembleWarship> AssembleWarshipList;
        private static Dictionary<int, AssembleWarship> AssembleWarshipDic;
        private static List<AssembleWarshipClass> AssembleWarshipClassList;
        private static Dictionary<int, AssembleWarshipClass> AssembleWarshipClassDic;
        private static List<AssembleParts> AssemblePartsList;
        private static Dictionary<int, AssembleParts> AssemblePartsDic;
        private static List<AssemblePartsType> AssemblePartsTypeList;
        private static Dictionary<int, AssemblePartsType> AssemblePartsTypeDic;

        public override void InitData()
        {
            AssembleWarshipList = AssembleMetaDataReader.GetAssembleWarshipList();
            AssembleWarshipDic = AssembleMetaDataReader.GetAssembleWarshipDic();
            AssembleWarshipClassList = AssembleMetaDataReader.GetAssembleWarshipClassList();
            AssembleWarshipClassDic = AssembleMetaDataReader.GetAssembleWarshipClassDic();
            AssemblePartsList = AssembleMetaDataReader.GetAssemblePartsList();
            AssemblePartsDic = AssembleMetaDataReader.GetAssemblePartsDic();
            AssemblePartsTypeList = AssembleMetaDataReader.GetAssemblePartsTypeList();
            AssemblePartsTypeDic = AssembleMetaDataReader.GetAssemblePartsTypeDic();
        }


        public override void Register()
        {
        }

        public AssembleModule()
        {
            InitData();
        }
        #region Assemble Part
        public static AssembleParts GetAssemblePartDataByKey(int partID)
        {
            AssembleParts result = null;
            AssemblePartsDic.TryGetValue(partID, out result);
            if (result == null)
            {
                Debug.LogError("GetAssemblePartData Error! partID=" + partID);
            }
            return result;
        }
        public static AssemblePartsType GetAssemblePartTypeByKey(int modelTypeID)
        {
            AssemblePartsType type = null;
            AssemblePartsTypeDic.TryGetValue(modelTypeID, out type);
            if (type == null)
            {
                Debug.LogError("GetAssemblePartType Error! modelTypeID=" + modelTypeID);
            }
            return type;
        }


        public static PartsCustomConfig GetPartsCustomConfigData(int partID)
        {
            PartsCustomConfig config = null;
            var meta = GetAssemblePartDataByKey(partID);
            if (meta != null)
            {
                config = Config.ConfigData.AssemblePartsConfigData.partsCustomConfig.Find(x => x.customName == meta.CustomData);
            }
            if (config == null)
                Debug.LogError("GetPartsCustomConfigData Error! partID=" + partID);

            return config;
        }

        public static PartsPropertyConfig GetPartsPropertyConfigData(int partID)
        {
            PartsPropertyConfig config = null;
            var meta = GetAssemblePartDataByKey(partID);
            if (meta != null)
            {
                var typeMeta = GetAssemblePartTypeByKey(meta.ModelTypeID);
                if (typeMeta != null)
                {
                    config = Config.ConfigData.AssemblePartsConfigData.partsPropertyConfig.Find(x => x.configName == typeMeta.PropertyConfig);
                }
            }
            if (config == null)
                Debug.LogError("GetPartsPropertyConfigData Error!  partID=" + partID);
            return config;
        }

        public static string GetPartName(int partID)
        {
            var partMeta = GetAssemblePartDataByKey(partID);
            if (partMeta != null)
            {
                var typeMeta = GetAssemblePartTypeByKey(partMeta.ModelTypeID);
                if (typeMeta != null)
                {
                    return MultiLanguage.Instance.GetTextValue(typeMeta.ModelTypeName);
                }
            }
            return string.Empty;
        }
        public static string GetPartDesc(int partID)
        {
            var partMeta = GetAssemblePartDataByKey(partID);
            if (partMeta != null)
            {
                var typeMeta = GetAssemblePartTypeByKey(partMeta.ModelTypeID);
                if (typeMeta != null)
                {
                    return MultiLanguage.Instance.GetTextValue(typeMeta.ModelTypeDesc);
                }
            }
            return string.Empty;
        }

        public static Sprite GetPartTypeIcon(int ModelTypeID)
        {
            var meta = GetAssemblePartTypeByKey(ModelTypeID);
            if (meta != null)
            {
                return Utility.LoadSprite(meta.IconPath, Utility.SpriteType.png);
            }
            return null;
        }
        public static string GetPartTypeName(int ModelTypeID)
        {
            var meta = GetAssemblePartTypeByKey(ModelTypeID);
            if (meta != null)
            {
                return MultiLanguage.Instance.GetTextValue(meta.TypeName);
            }
            return string.Empty;
        }

        #endregion

    }

    public class AssemblePartInfo
    {
        public int partID;

        public string partName;
        public string partDesc;
        /// <summary>
        /// Prefab Path
        /// </summary>
        public string ModelPath;

        public string TypeID;
        public Sprite TypeIcon;
        public string TypeName;

        public Dictionary<int, ushort> materialCostDic = new Dictionary<int, ushort>();

        /// <summary>
        /// 基础时间花费
        /// </summary>
        public ushort baseTimeCost;
        public PartsCustomConfig partsConfig;
        public PartsPropertyConfig partsPropertyConfig;


        public AssembleParts _partsMeta;
        public AssemblePartsType _partsTypeMeta;

        public AssemblePartInfo(int partID)
        {
            this.partID = partID;
            _partsMeta = AssembleModule.GetAssemblePartDataByKey(partID);
            _partsTypeMeta = AssembleModule.GetAssemblePartTypeByKey(_partsMeta.ModelTypeID);
            if (_partsMeta != null && _partsTypeMeta!=null)
            {
                baseTimeCost = _partsMeta.BaseTimeCost;
                partName = AssembleModule.GetPartName(partID);
                partDesc = AssembleModule.GetPartDesc(partID);
                ModelPath = _partsTypeMeta.ModelPath;

                TypeID = _partsTypeMeta.TypeID;
                TypeIcon = AssembleModule.GetPartTypeIcon(_partsTypeMeta.ModelTypeID);
                TypeName = AssembleModule.GetPartTypeName(_partsTypeMeta.ModelTypeID);

                partsConfig = AssembleModule.GetPartsCustomConfigData(partID);
                partsPropertyConfig = AssembleModule.GetPartsPropertyConfigData(partID);
            }
        }

    }





    public class PartsPropertyConfig
    {
        public string configName;
        public List<ConfigData> configData;

        public class ConfigData
        {
            public string Name;
            public string PropertyName;
            public string PropertyIcon;
            public double PropertyRangeMin;
            public double PropertyRangeMax;
        }
    }

    public class PartsCustomConfig
    {
        public string customName;
        public List<ConfigData> configData;

        public class ConfigData
        {
            public string CustomDataName;

            public double PosX;
            public double PosY;
            public double LineWidth;

            public double CustomDataRangeMin;
            public double CustomDataRangeMax;
            public double CustomDataDefaultValue;
            public List<PropertyLinkData> propertyLinkData;
            public List<MaterialCostLinkData> materialCostLinkData;

            public class PropertyLinkData
            {
                public string Name;
                public string LinkDesc;
                public double PropertyChangePerUnitMin;
                public double PropertyChangePerUnitMax;
            }

            public class MaterialCostLinkData
            {

            }

        }
    }


}