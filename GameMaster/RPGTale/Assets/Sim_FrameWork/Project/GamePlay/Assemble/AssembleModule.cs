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


        public static Config.AssemblePartMainType GetAssemblePartMainType(string type)
        {
            var configData = Config.ConfigData.AssembleConfig.assemblePartMainType;
            return configData.Find(x => x.Type == type);
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

        /// <summary>
        /// 获取部件属性配置信息
        /// </summary>
        /// <param name="partID"></param>
        /// <returns></returns>
        public static PartsPropertyConfig GetPartsPropertyConfigData(int typeModelID)
        {
            PartsPropertyConfig config = null;
            var typeMeta = GetAssemblePartTypeByKey(typeModelID);
            if (typeMeta != null)
            {
                config = Config.ConfigData.AssemblePartsConfigData.partsPropertyConfig.Find(x => x.configName == typeMeta.PropertyConfig);
            }
            if (config == null)
                Debug.LogError("GetPartsPropertyConfigData Error!  typeModelID=" + typeModelID);
            return config;
        }

        public static string GetPartName(int modelTypeName)
        {
            var typeMeta = GetAssemblePartTypeByKey(modelTypeName);
            if (typeMeta != null)
            {
                return MultiLanguage.Instance.GetTextValue(typeMeta.ModelTypeName);
            }
            return string.Empty;
        }
        public static string GetPartDesc(int modelTypeID)
        {
            var typeMeta = GetAssemblePartTypeByKey(modelTypeID);
            if (typeMeta != null)
            {
                return MultiLanguage.Instance.GetTextValue(typeMeta.ModelTypeDesc);
            }
            return string.Empty;
        }

        public static Sprite GetPartTypeIcon(int ModelTypeID)
        {
            var meta = GetAssemblePartTypeByKey(ModelTypeID);
            if (meta != null)
            {
                var typeData = GetAssemblePartMainType(meta.TypeID);
                if (typeData != null)
                    return Utility.LoadSprite(typeData.IconPath, Utility.SpriteType.png);
            }
            return null;
        }
        public static string GetPartTypeName(int ModelTypeID)
        {
            var meta = GetAssemblePartTypeByKey(ModelTypeID);
            if (meta != null)
            {
                var typeData = GetAssemblePartMainType(meta.TypeID);
                if (typeData != null)
                    return MultiLanguage.Instance.GetTextValue(typeData.TypeName);
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

        /// <summary>
        /// 获取所有初始解锁状态的部件模板ID
        /// </summary>
        /// <returns></returns>
        public static List<int> GetAllUnlockPartTypeID()
        {
            List<int> result = new List<int>();
            for(int i = 0; i < AssemblePartsList.Count; i++)
            {
                if (AssemblePartsList[i].Unlock == true)
                {
                    result.Add(AssemblePartsList[i].PartID);
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
                if (list.Count > Config.GlobalConfigData.Assemble_MaterialCost_MaxNum)
                    Debug.LogError("Assemble Parts MaterialCost Num can not be larger than " + Config.GlobalConfigData.Assemble_MaterialCost_MaxNum + "  PartID=" + partID);
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

        public static Config.AssembleShipMainType GetShipPresetMainTypeData(string type)
        {
            var config = Config.ConfigData.AssembleConfig.assembleShipMainType;
            return config.Find(x => x.Type == type);
        }

        public static string GetShipPresetMainTypeName(int warShipID)
        {
            var meta = GetWarshipDataByKey(warShipID);
            if (meta != null)
            {
                var mainType = GetShipPresetMainTypeData(meta.MainType);
                if (mainType != null)
                    return MultiLanguage.Instance.GetTextValue(mainType.TypeName);
            }
            return string.Empty;
        }

        public static Sprite GetShipPresetMainTypeIcon(int warShipID)
        {
            var meta = GetWarshipDataByKey(warShipID);
            if (meta != null)
            {
                var mainType = GetShipPresetMainTypeData(meta.MainType);
                if (mainType != null)
                    return Utility.LoadSprite(mainType.IconPath, Utility.SpriteType.png);
            }
            return null;
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

        /// <summary>
        /// 获取所有初始解锁状态的舰船模板ID
        /// </summary>
        /// <returns></returns>
        public static List<int> GetAllUnlockShipPresetID()
        {
            List<int> result = new List<int>();
            for (int i = 0; i < AssembleWarshipList.Count; i++)
            {
                if (AssembleWarshipList[i].Unlock == true)
                {
                    result.Add(AssembleWarshipList[i].WarShipID);
                }
            }
            return result;
        }

        public static List<MaterialCostItem> GetShipMaterialCost(int shipID)
        {
            List<MaterialCostItem> costList = new List<MaterialCostItem>();
            var meta = GetWarshipDataByKey(shipID);
            if (meta != null)
            {
                var list = Utility.TryParseStringList(meta.MaterialCost, ',');
                if (list.Count > Config.GlobalConfigData.Assemble_MaterialCost_MaxNum)
                    Debug.LogError("Assemble ship MaterialCost Num can not be larger than " + Config.GlobalConfigData.Assemble_MaterialCost_MaxNum + "  shipID=" + shipID);
                for (int i = 0; i < list.Count; i++)
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
                        Debug.LogError("Assemble ship MaterialCost FormatError!  PartID=" + shipID);
                    }
                }
            }
            return costList;
        }

        #endregion

        #region Misc

        public static Config.AssembleMainType GetAssembleMainTypeData(string type)
        {
            var config = Config.ConfigData.AssembleConfig.assembleMainType;
            return config.Find(x => x.Type == type);
        }

        public static Config.AssembleShipMainType GetAssembleShipMianTypeData(string type)
        {
            var config = Config.ConfigData.AssembleConfig.assembleShipMainType;
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
        public string customName
        {
            get { return typePresetData.partName +"·"+ customDataInfo.partNameCustomText; }
        }


        public List<MaterialCostItem> materialCostItem = new List<MaterialCostItem>();
        public List<string> partEquipType = new List<string>();

        /// <summary>
        /// 基础时间花费
        /// </summary>
        public ushort baseTimeCost;
        public ushort realTimeCost
        {
            get { return (ushort)(baseTimeCost + customDataInfo.addTimeCost); }
        }

        public PartsCustomConfig partsConfig;

        public AssembleParts _partsMeta;
        public AssemblePartCustomDataInfo customDataInfo;
        public AssemblePartTypePresetData typePresetData;

        public AssemblePartInfo(int partID)
        {
            this.partID = partID;
            _partsMeta = AssembleModule.GetAssemblePartDataByKey(partID);
           
            if (_partsMeta != null)
            {
                baseTimeCost = _partsMeta.BaseTimeCost;
                materialCostItem = AssembleModule.GetPartMaterialCost(partID);
                partsConfig = AssembleModule.GetPartsCustomConfigData(partID);
                partEquipType = AssembleModule.GetAssemblePartEquipType(partID);
                typePresetData = new AssemblePartTypePresetData(_partsMeta.ModelTypeID);
            }
        }

    }

    public class AssemblePartTypePresetData
    {
        public string TypeID;
        public Sprite TypeIcon
        {
            get { return AssembleModule.GetPartTypeIcon(_partsTypeMeta.ModelTypeID); }
        }
        public string TypeName
        {
            get { return AssembleModule.GetPartTypeName(_partsTypeMeta.ModelTypeID); }
        }

        public Sprite partSprite
        {
            get { return Utility.LoadSprite(_partsTypeMeta.PartSprite, Utility.SpriteType.png); }
        }
        public Sprite partIconSmall
        {
            get { return Utility.LoadSprite(_partsTypeMeta.PartIconSmall, Utility.SpriteType.png); }
        }

        public string partDesc
        {
            get { return AssembleModule.GetPartDesc(_partsTypeMeta.ModelTypeID); }
        }
        public string partName
        {
            get { return AssembleModule.GetPartName(_partsTypeMeta.ModelTypeID); }
        }

        /// <summary>
        /// Prefab Path
        /// </summary>
        public string ModelPath;

        public AssemblePartsType _partsTypeMeta;
        public PartsPropertyConfig partsPropertyConfig;
      

        public AssemblePartTypePresetData(int typeModelID)
        {
            _partsTypeMeta = AssembleModule.GetAssemblePartTypeByKey(typeModelID);

            if (_partsTypeMeta != null)
            {
                ModelPath = _partsTypeMeta.ModelPath;
                TypeID = _partsTypeMeta.TypeID;
            }
            partsPropertyConfig = AssembleModule.GetPartsPropertyConfigData(typeModelID);
        }
    }

    /// <summary>
    /// 部件自定义数据
    /// </summary>
    public class AssemblePartCustomDataInfo
    {
        public int partID;
        public string partNameCustomText;

        /// <summary>
        /// 额外制造时长
        /// </summary>
        public ushort addTimeCost;

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

            public ushort TimeCostPerUnit;
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
        public ushort UID;

        public Config.AssembleMainType mainTypeData;

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

   

        public AssembleShipTypePresetData presetData;



        public AssembleShipInfo(int shipID)
        {
            presetData = new AssembleShipTypePresetData(shipID);
            warShipID = presetData.WarshipID;

            AddShipDurability(presetData._metaData.HPBase);
            AddTimeCost(presetData._metaData.BaseTimeCost);
            AddShipFirePower(presetData._metaData.FirePowerBase);
            AddShipSpeed(presetData._metaData.SpeedBase);
            AddShipDetect(presetData._metaData.DetectBase);
            AddShipStorage(presetData._metaData.StorageBase);
            shipCrewMax = presetData._metaData.CrewMax;

           
        }
    }

    public class AssembleShipTypePresetData
    {
        public int WarshipID;

        public string TypeID { get { return _metaData.MainType; } }
        public Sprite TypeIcon { get { return AssembleModule.GetShipPresetMainTypeIcon(WarshipID); } }
        public string TypeName { get {return AssembleModule.GetShipPresetMainTypeName(WarshipID); } }

        public string shipSizeText { get { return AssembleModule.GetShipSizeText(_metaData.ShipScale); } }

        /// <summary>
        /// 模块数量
        /// </summary>
        public ushort ModuleNum
        {
            get
            {
                if (partConfig != null)
                    return (ushort)partConfig.configData.Count;
                return 0;
            }
        }

        public Sprite ShipSprite
        {
            get { return Utility.LoadSprite(_metaData.ShipSpritePath, Utility.SpriteType.png); }
        }
           
        public string shipClassDesc
        {
            get
            {
                if(_metaClass!=null)
                    return MultiLanguage.Instance.GetTextValue(_metaClass.ClassDesc);
                return string.Empty;
            }
        }
        public string shipClassName
        {
            get
            {
                if(_metaClass!=null)
                    return MultiLanguage.Instance.GetTextValue(_metaClass.ClassName);
                return string.Empty;
            }
        }

        public List<MaterialCostItem> shipCostBase = new List<MaterialCostItem>();

        public AssembleWarship _metaData;
        public AssembleWarshipClass _metaClass;
        public AssembleShipPartConfig partConfig;

        public AssembleShipTypePresetData(int warShipID)
        {
            _metaData = AssembleModule.GetWarshipDataByKey(warShipID);
            
            if (_metaData != null)
            {
                WarshipID = _metaData.WarShipID;
                shipCostBase = AssembleModule.GetShipMaterialCost(_metaData.WarShipID);
                _metaClass = AssembleModule.GetWarshipClassDataByKey(_metaData.Class);
                partConfig = AssembleModule.GetShipPartConfigData(warShipID);
            }
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