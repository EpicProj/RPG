using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{

    public class AssembleModule : BaseModule<AssembleModule>
    {
        private static Dictionary<int, AssembleWarship> AssembleWarshipDic;
        private static Dictionary<int, AssembleWarshipClass> AssembleWarshipClassDic;

        private static Dictionary<int, AssembleParts> AssemblePartsDic;
        private static Dictionary<int, AssemblePartsType> AssemblePartsTypeDic;

        private const string Assemble_ShipSize_01_Text = "Assemble_ShipSize_01_Text";
        private const string Assemble_ShipSize_02_Text = "Assemble_ShipSize_02_Text";
        private const string Assemble_ShipSize_03_Text = "Assemble_ShipSize_03_Text";

        public override void InitData()
        {
            var config = ConfigManager.Instance.LoadData<AssembleMetaData>(ConfigPath.TABLE_ASSEMBLE_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("BuildingPanelMetaData Read Error");
                return;
            }
            AssembleWarshipDic = config.AllAssembleWarshipDic;
            AssembleWarshipClassDic = config.AllAssembleWarshipClassDic;
            AssemblePartsDic = config.AllAssemblePartsDic;
            AssemblePartsTypeDic = config.AllAssemblePartsTypeDic;
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


        public static Config.PartsCustomConfig GetPartsCustomConfigData(int partID)
        {
            Config.PartsCustomConfig config = null;
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
        public static Config.PartsPropertyConfig GetPartsPropertyConfigData(int typeModelID)
        {
            Config.PartsPropertyConfig config = null;
            var typeMeta = GetAssemblePartTypeByKey(typeModelID);
            if (typeMeta != null)
            {
                config = Config.ConfigData.AssemblePartsConfigData.partsPropertyConfig.Find(x => x.configName == typeMeta.PropertyConfig);
            }
            if (config == null)
                Debug.LogError("GetPartsPropertyConfigData Error!  typeModelID=" + typeModelID);
            return config;
        }

        public static AssemblePartPropertyTypeData GetAssemblePartPropertyTypeData(string typeName)
        {
            AssemblePartPropertyTypeData data = null;
            data = Config.ConfigData.AssembleConfig.assemblePartPropertyType.Find(x => x.Name == typeName);
            return data;
        }

        public static string GetPartName(int modelTypeID)
        {
            var typeMeta = GetAssemblePartTypeByKey(modelTypeID);
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
        public static List<AssembleEquipTarget> GetAssemblePartEquipType(int partID)
        {
            List<AssembleEquipTarget> result = new List<AssembleEquipTarget>();
            var partMeta = GetAssemblePartDataByKey(partID);
            if (partMeta != null)
            {
                var list = Utility.TryParseStringList(partMeta.AssembleType,',');
                for(int i = 0; i < list.Count; i++)
                {
                    if (GetAssembleMainTypeData(list[i]) != null)
                    {
                        AssembleEquipTarget target = AssembleEquipTarget.None;
                        Enum.TryParse<AssembleEquipTarget>(list[i], out target);
                        if(target== AssembleEquipTarget.None)
                        {
                            DebugPlus.LogError("GetAssemblePartEquipType Error! type=" + list[i]);
                        }
                        else
                        {
                            result.Add(target);
                        }
                    }
                }

                if(list.Count> Config.GlobalConfigData.AssemblePart_Target_MaxNum)
                {
                    DebugPlus.LogError("AssemblePartTarget can not largerThan " + Config.GlobalConfigData.AssemblePart_Target_MaxNum + "  Current partID is " + partID);
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
            foreach(var item in AssemblePartsDic.Values)
            {
                if (item.Unlock == true)
                {
                    result.Add(item.PartID);
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

        public static ModifierDetailRootType_Mix FetchAssemblePartModifieRootType(string TypeID)
        {
            ModifierDetailRootType_Mix type =  ModifierDetailRootType_Mix.None;
            Enum.TryParse<ModifierDetailRootType_Mix>(TypeID, out type);
            if(type == ModifierDetailRootType_Mix.None)
            {
                DebugPlus.LogError(" FetchAssemblePartModifieRootType Error! typeID=" + TypeID);
            }
            return type;
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

        public static Config.AssembleShipPartConfig GetShipPartConfigData(int shipID)
        {
            Config.AssembleShipPartConfig configData = null;
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
            foreach(var item in AssembleWarshipDic.Values)
            {
                if (item.Unlock == true)
                {
                    result.Add(item.WarShipID);
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

}