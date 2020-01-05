using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{

    public class AssembleModule : BaseModule<AssembleModule>
    {
        private static List<AssembleWarship> AssembleWarshipList;
        private static Dictionary<int, AssembleWarship> AssembleWarshipDic;
        private static List<AssembleWarShipType> AssembleWarShipTypeList;
        private static Dictionary<int, AssembleWarShipType> AssembleWarShipTypeDic;
        private static List<AssembleWarshipClass> AssembleWarshipClassList;
        private static Dictionary<int, AssembleWarshipClass> AssembleWarshipClassDic;

        private static List<AssembleParts> AssemblePartsList;
        private static Dictionary<int, AssembleParts> AssemblePartsDic;
        private static List<AssemblePartsType> AssemblePartsTypeList;
        private static Dictionary<int, AssemblePartsType> AssemblePartsTypeDic;

        private const string Assemble_ShipSize_01_Text = "Assemble_ShipSize_01_Text";
        private const string Assemble_ShipSize_02_Text = "Assemble_ShipSize_02_Text";
        private const string Assemble_ShipSize_03_Text = "Assemble_ShipSize_03_Text";

        public override void InitData()
        {
            AssembleWarshipList = AssembleMetaDataReader.GetAssembleWarshipList();
            AssembleWarshipDic = AssembleMetaDataReader.GetAssembleWarshipDic();
            AssembleWarShipTypeList = AssembleMetaDataReader.GetAssembleWarShipTypeList();
            AssembleWarShipTypeDic = AssembleMetaDataReader.GetAssembleWarShipTypeDic();
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

        /// <summary>
        /// 获取部件可装配类型
        /// </summary>
        /// <param name="partID"></param>
        /// <returns></returns>
        public static List<string> GetAssemblePartEquipType(int partID)
        {
            List<string> result = new List<string>();
            var partMeta = GetAssemblePartDataByKey(partID);
            if (partMeta != null)
            {
                var list = Utility.TryParseStringList(partMeta.AssembleType,',');
                for(int i = 0; i < list.Count; i++)
                {
                    if (GetAssembleMainTypeData(list[i]) != null)
                        result.Add(list[i]);
                }

                if(list.Count> Config.GlobalConfigData.AssemblePart_Target_MaxNum)
                {
                    Debug.LogError("AssemblePartTarget can not largerThan " + Config.GlobalConfigData.AssemblePart_Target_MaxNum + "  Current partID is " + partID);
                }
            }
            return result;
        }

        public static List<MaterialCostItem> GetPartMaterialCost(int partID)
        {
            List<MaterialCostItem> costList = new List<MaterialCostItem>();
            var meta = GetAssemblePartDataByKey(partID);
            if (meta != null)
            {
                var list = Utility.TryParseStringList(meta.BaseMaterialCost, ',');
                if (list.Count > Config.GlobalConfigData.AssemblePart_MaterialCost_MaxNum)
                    Debug.LogError("Assemble Parts MaterialCost Num can not be larger than " + Config.GlobalConfigData.AssemblePart_MaterialCost_MaxNum + "  PartID=" + partID);
                for(int i = 0; i < list.Count; i++)
                {
                    var maData = Utility.TryParseStringList(list[i], ':');
                    if (maData.Count == 2)
                    {
                        int materialID = Utility.TryParseInt(maData[0]);
                        MaterialCostItem item = new MaterialCostItem(materialID, Utility.TryParseInt(maData[1]));
                        if (item.InitSuccess)
                        {
                            costList.Add(item);
                        }
                    }
                    else
                    {
                        Debug.LogError("Assemble Parts MaterialCost FormatError!  PartID=" + partID);
                    }
                }
            }
            return costList;
        }

        #endregion

        #region Assemble Ship

        public static AssembleWarship GetWarshipDataByKey(int shipID)
        {
            AssembleWarship ship = null;
            AssembleWarshipDic.TryGetValue(shipID, out ship);
            if (ship == null)
            {
                Debug.LogError("GetWarshipData Error! shipID=" + shipID);
            }
            return ship;
        }

        public static AssembleWarshipClass GetWarshipClassDataByKey(int classID)
        {
            AssembleWarshipClass result = null;
            AssembleWarshipClassDic.TryGetValue(classID, out result);
            if (result == null)
            {
                Debug.LogError("GetWarshipClassData Error! shipID=" + classID);
            }
            return result;
        }

        public static AssembleWarShipType GetWarshipTypeDataByKey(int typeID)
        {
            AssembleWarShipType type = null;
            AssembleWarShipTypeDic.TryGetValue(typeID, out type);
            if (type == null)
            {
                Debug.LogError("GetWarshipTypeData Error! typeID=" + typeID);
            }
            return type;
        }

        public static AssembleShipPartConfig GetShipPartConfigData(int shipID)
        {
            AssembleShipPartConfig configData = null;
            var metaData = GetWarshipDataByKey(shipID);
            if (metaData != null)
            {
                configData= Config.ConfigData.AssembleShipPartConfigData.shipPartConfig.Find(x => x.configName == metaData.ConfigData);
                if (configData == null)
                    Debug.LogError("Find ShipPartConfig null configName=" + metaData.ConfigData);
            }
            return configData;
        }

        public static string GetShipSizeText(int scale)
        {
            if (scale == 1)
                return MultiLanguage.Instance.GetTextValue(Assemble_ShipSize_01_Text);
            else if (scale == 2)
                return MultiLanguage.Instance.GetTextValue(Assemble_ShipSize_02_Text);
            else if (scale == 3)
                return MultiLanguage.Instance.GetTextValue(Assemble_ShipSize_03_Text);
            else
                return string.Empty;
        }

        #endregion

        #region Misc

        public static Config.GlobalSetting.AssembleMainType GetAssembleMainTypeData(string type)
        {
            var config = Config.ConfigData.GlobalSetting.assembleMainType;
            return config.Find(x => x.Type == type);
        }

     

        #endregion

    }

    #region Assemble Part Data

    public class AssemblePartInfo
    {
        public int partID;

        /// <summary>
        /// For Custom
        /// </summary>
        public ushort UID;

        public string partName;
        public string partDesc;
        public Sprite partSprite;
        public Sprite partIconSmall;
        /// <summary>
        /// Prefab Path
        /// </summary>
        public string ModelPath;

        public string TypeID;
        public Sprite TypeIcon;
        public string TypeName;

        public List<MaterialCostItem> materialCostItem = new List<MaterialCostItem>();
        public List<string> partEquipType = new List<string>();

        /// <summary>
        /// 基础时间花费
        /// </summary>
        public ushort baseTimeCost;
        public PartsCustomConfig partsConfig;
        public PartsPropertyConfig partsPropertyConfig;

        public AssemblePartCustomDataInfo customDataInfo;

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
                partSprite = Utility.LoadSprite(_partsMeta.PartSprite, Utility.SpriteType.png);
                partIconSmall = Utility.LoadSprite(_partsMeta.PartIconSmall, Utility.SpriteType.png);
                materialCostItem = AssembleModule.GetPartMaterialCost(partID);

                TypeID = _partsTypeMeta.TypeID;
                TypeIcon = AssembleModule.GetPartTypeIcon(_partsTypeMeta.ModelTypeID);
                TypeName = AssembleModule.GetPartTypeName(_partsTypeMeta.ModelTypeID);

                partsConfig = AssembleModule.GetPartsCustomConfigData(partID);
                partsPropertyConfig = AssembleModule.GetPartsPropertyConfigData(partID);

                partEquipType = AssembleModule.GetAssemblePartEquipType(partID);
            }
        }

    }

    /// <summary>
    /// 部件自定义数据
    /// </summary>
    public class AssemblePartCustomDataInfo
    {
        public int partID;
        public string partNameCustomText;
        public Dictionary<string, CustomData> propertyDic;
        public Dictionary<string, float> customValueDic;


        public AssemblePartCustomDataInfo(int partID,string partNameCustomText, Dictionary<string, CustomData> propertyDic, Dictionary<string, float> customValueDic)
        {
            this.partID = partID;
            this.partNameCustomText = partNameCustomText;
            this.propertyDic = propertyDic;
            this.customValueDic = customValueDic;
        }

        public class CustomData
        {
            public string propertyNameText;
            public Sprite propertyIcon;
            public float propertyValueMin;
            public float propertyValueMax;

            public CustomData(PartsPropertyConfig.ConfigData config ,float min,float max)
            {
                propertyNameText = MultiLanguage.Instance.GetTextValue(config.PropertyName);
                propertyIcon = Utility.LoadSprite(config.PropertyIcon, Utility.SpriteType.png);
                propertyValueMin = min;
                propertyValueMax = max;
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

            public string PosType;
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
    #endregion

    #region Assemble Ship Data

    public class AssembleShipInfo
    {
        public int warShipID;
        public string shipName;

        public Config.GlobalSetting.AssembleMainType mainTypeData;

        public string className;
        public string classDesc;
        public string modelPath;
        public string shipSizeText;

        public string typeName;
        public Sprite typeIcon;

        /// <summary>
        /// 建造时长
        /// </summary>
        private ushort _timeCost;
        public ushort timeCost { get { return _timeCost; } }
        public void AddTimeCost(ushort value)
        {
            _timeCost += value;
            if (_timeCost <= 0)
                _timeCost = 0;
        }

        /// <summary>
        /// 船体耐久
        /// </summary>
        private int _shipDurability;
        public int shipDurability { get { return _shipDurability; } }
        public void AddShipDurability(int value)
        {
            _shipDurability += value;
            if (_shipDurability <= 0)
                _shipDurability = 0;
        }

        /// <summary>
        /// 速度
        /// </summary>
        private float _shipSpeed;
        public float shipSpeed { get { return (float)Math.Round(_shipSpeed, 2); } }
        public void AddShipSpeed(float value)
        {
            _shipSpeed += value;
            if (_shipSpeed <= 0)
                _shipSpeed = 0;
        }

        /// <summary>
        /// 火力
        /// </summary>
        private float _shipFirePower;
        public float shipFirePower { get { return (float)Math.Round(_shipFirePower, 2); } }
        public void AddShipFirePower(float value)
        {
            _shipFirePower += value;
            if (_shipFirePower <= 0)
                _shipFirePower = 0;
        }

        /// <summary>
        /// 探索
        /// </summary>
        private float _shipDetect;
        public float shipDetect { get { return (float)Math.Round(_shipDetect, 2); } }
        public void AddShipDetect (float value)
        {
            _shipDetect += value;
            if (_shipDetect <= 0)
                _shipDetect = 0;
        }

        /// <summary>
        /// 最大成员数
        /// </summary>
        public float shipCrewMax;


        /// <summary>
        /// 最大货仓储量
        /// </summary>
        private ushort _shipStorage;
        public ushort shipStorage { get { return _shipStorage; } }
        public void AddShipStorage(ushort value)
        {
            _shipStorage += value;
            if (_shipStorage <= 0)
                _shipStorage = 0;
        }

        public AssembleShipPartConfig partConfig;

        public AssembleWarship _meta;
        public AssembleWarshipClass _metaClass;
        public AssembleWarShipType _metaType;


        public AssembleShipInfo(int shipID)
        {
            _meta = AssembleModule.GetWarshipDataByKey(shipID);
            _metaClass = AssembleModule.GetWarshipClassDataByKey(_meta.Class);
            _metaType = AssembleModule.GetWarshipTypeDataByKey(_meta.Type);
            if (_meta == null || _metaClass == null || _metaType == null)
                return;

            mainTypeData = AssembleModule.GetAssembleMainTypeData(_meta.MainType);

            warShipID = _meta.WarShipID;
            className = MultiLanguage.Instance.GetTextValue(_metaClass.ClassName);
            classDesc = MultiLanguage.Instance.GetTextValue(_metaClass.ClassDesc);
            typeName = MultiLanguage.Instance.GetTextValue(_metaType.Name);
            typeIcon = Utility.LoadSprite(_metaType.IconPath, Utility.SpriteType.png);
            shipSizeText = AssembleModule.GetShipSizeText(_meta.ShipScale);
            modelPath = _metaClass.ModelPath;

            AddShipDurability(_meta.HPBase);
            AddTimeCost(_meta.BaseTimeCost);
            AddShipFirePower(_meta.FirePowerBase);
            AddShipSpeed(_meta.SpeedBase);
            AddShipDetect(_meta.DetectBase);
            AddShipStorage(_meta.StorageBase);
            shipCrewMax = _meta.CrewMax;

            partConfig = AssembleModule.GetShipPartConfigData(shipID);

        }



    }

    public class AssembleShipPartConfig
    {
        public string configName;
        public List<ConfigData> configData;

        public class ConfigData
        {
            public string PosType;
            public double PosX;
            public double PosY;
            public double LineWidth;
            public List<string> EquipPartType;
        }


    }


    #endregion

}