﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Assemble Info
 * SOMA
 */

namespace Sim_FrameWork
{

    public class AssemblePartInfo
    {
        public int partID;

        /// <summary>
        /// For Custom
        /// </summary>
        public ushort UID;
        public string customName
        {
            get { return typePresetData.partName + "·" + customDataInfo.partNameCustomText; }
        }

        public List<MaterialCostItem> materialCostItem = new List<MaterialCostItem>();
        public List<string> partEquipType = new List<string>();

        /// <summary>
        /// 基础时间花费
        /// </summary>
        public float baseTimeCost;
        public float realTimeCost
        {
            get { return (ushort)(baseTimeCost + customDataInfo.addTimeCost); }
        }

        public Config.PartsCustomConfig partsConfig;

        public AssembleParts _partsMeta;
        public AssemblePartCustomDataInfo customDataInfo;
        public AssemblePartTypePresetData typePresetData;

        public AssemblePartInfo() { }
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

        public AssemblePartInfo LoadSaveData(AssmeblePartSingleSaveData saveData)
        {
            if (saveData != null)
            {
                AssemblePartInfo info = new AssemblePartInfo(saveData.partID);
                this.UID = saveData.UID;
                this.customDataInfo = new AssemblePartCustomDataInfo();
                this.customDataInfo.LoadSaveData(saveData);

                return info;
            }
            return null;
        }

        /// <summary>
        /// Game Save
        /// </summary>
        /// <returns></returns>
        public AssmeblePartSingleSaveData CreatePartSave()
        {
            AssmeblePartSingleSaveData save = new AssmeblePartSingleSaveData();

            Dictionary<string, AssmeblePartCustomSaveData> proDic = new Dictionary<string, AssmeblePartCustomSaveData>();
            foreach(var customData in customDataInfo.propertyDic.Values)
            {
                AssmeblePartCustomSaveData saveData = new AssmeblePartCustomSaveData(
                    partID,
                    customData.propertyName,
                    customData.propertyValueMin,
                    customData.propertyValueMax,
                    customData.detailInfoDic,
                    customData.timeCostDetailInfoDic);
                proDic.Add(saveData.propertyName, saveData);
            }
            return save.createSaveData(partID, UID, customDataInfo.partNameCustomText, customDataInfo.customValueDic, proDic);
        }

   

    }


    /// <summary>
    /// 部件预设信息
    /// </summary>
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
        public Config.PartsPropertyConfig partsPropertyConfig;


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
        public float addTimeCost;

        /// <summary>
        /// key = curstomName
        /// </summary>
        public Dictionary<string, CustomData> propertyDic=new Dictionary<string, CustomData> ();
        public Dictionary<string, float> customValueDic=new Dictionary<string, float> ();

        public AssemblePartCustomDataInfo() { }
        public AssemblePartCustomDataInfo(int partID, string partNameCustomText, Dictionary<string, CustomData> propertyDic, Dictionary<string, float> customValueDic)
        {
            this.partID = partID;
            this.partNameCustomText = partNameCustomText;
            this.propertyDic = propertyDic;
            this.customValueDic = customValueDic;
        }

        public AssemblePartCustomDataInfo(int partID, string partNameCustomText)
        {
            this.partID = partID;
            this.partNameCustomText = partNameCustomText;
        }

        /// <summary>
        /// Game Save
        /// </summary>
        /// <param name="saveData"></param>
        public AssemblePartCustomDataInfo LoadSaveData(AssmeblePartSingleSaveData saveData)
        {
            AssemblePartCustomDataInfo info = new AssemblePartCustomDataInfo(saveData.partID, saveData.customName_Partial);

            Dictionary<string, CustomData> propertyDic = new Dictionary<string, CustomData>();
            foreach(var custom in saveData.propertyDic.Values)
            {
                CustomData customData = new CustomData();
                customData.LoadCustomDataSave(custom);
                propertyDic.Add(customData.propertyName, customData);
            }
            info.propertyDic = propertyDic;
            info.customValueDic = saveData.customValueDic;

            return info;
        }


        public class CustomData
        {
            public string propertyName;

            public string propertyNameText;
            public Sprite propertyIcon;
            public float propertyValueMin;
            public float propertyValueMax;

            public float propertyOriginValue;

            public int propertyType;

            public AssemblePartPropertyTypeData propertyTypeData;
            public Dictionary<string, AssemblePartPropertyDetailInfo> detailInfoDic;

            public Dictionary<string, AssemblePartTimeCostDetialInfo> timeCostDetailInfoDic;

            /// <summary>
            /// 实际成品数值
            /// </summary>
            public float realValue;

            public CustomData() { }
            public CustomData InitData(Config.PartsPropertyConfig.ConfigData config)
            {
                CustomData data = new CustomData();
                propertyName = config.Name;
                propertyType = config.PropertyType;
                propertyOriginValue = (float)config.PropertyValue;
                propertyTypeData = AssembleModule.GetAssemblePartPropertyTypeData(config.Name);
                if (propertyTypeData != null)
                {
                    propertyNameText = MultiLanguage.Instance.GetTextValue(propertyTypeData.PropertyName);
                    propertyIcon = Utility.LoadSprite(propertyTypeData.PropertyIcon, Utility.SpriteType.png);
                }
                return data;
            }

            /// <summary>
            /// Data Create
            /// </summary>
            public CustomData CrateData(Config.PartsPropertyConfig.ConfigData config, float min, float max,
                Dictionary<string, AssemblePartPropertyDetailInfo> detailDic, Dictionary<string, AssemblePartTimeCostDetialInfo> timeCostDetailInfoDic)
            {
                CustomData data = new CustomData();
                data.InitData(config);
                data.propertyValueMin = min;
                data.propertyValueMax = max;
                data.detailInfoDic = detailDic;
                data.timeCostDetailInfoDic = timeCostDetailInfoDic;
                return data;
            }

            /// <summary>
            /// GameSave
            /// </summary>
            /// <param name="saveData"></param>
            public CustomData LoadCustomDataSave(AssmeblePartCustomSaveData saveData)
            {
                var config = AssembleModule.GetPartsPropertyConfigData(saveData.partID);
                if (config != null)
                {
                    var proConfig = config.configData.Find(x => x.Name == saveData.propertyName);
                    if (proConfig != null)
                    {
                        CustomData data = new CustomData();
                        data.InitData(proConfig);
                        propertyValueMin = saveData.propertyValueMin;
                        propertyValueMax = saveData.propertyValueMax;
                        detailInfoDic = saveData.detailInfoDic;
                        timeCostDetailInfoDic = saveData.timeCostDetailInfoDic;
                        return data;
                    }
                }
                return null;
            }

        }
    }

    /// <summary>
    /// 部件自定义数据详情加成
    /// </summary>
    public class AssemblePartPropertyDetailInfo
    {
        public string customDataName;
        /// <summary>
        /// 自定义部件关联名
        /// </summary>
        public string propertyLinkName;
        /// <summary>
        /// 数据修正
        /// 1 = FixData
        /// 2 = RangeData
        /// </summary>
        public float modifyType;

        public float modifyValueMax;
        public float modifyValueMin;

        public float modifyValueFix;
    }
    
    public class AssemblePartTimeCostDetialInfo
    {
        public string customDataName;

        public float modifyTimeValue;

    }

    public class AssemblePartPropertyTypeData
    {
        public string Name;
        public ushort Type;
        public string PropertyName;
        public string PropertyIcon;
    }

    #region SaveData

    public class AssmeblePartSingleSaveData
    {
        public int partID;
        public ushort UID;
        //Only Save Custom Part
        public string customName_Partial;
        /// <summary>
        /// Custom Value Data
        /// </summary>
        public Dictionary<string, float> customValueDic;

        public Dictionary<string, AssmeblePartCustomSaveData> propertyDic;

        public AssmeblePartSingleSaveData createSaveData(int partID, ushort UID, string customName, Dictionary<string, float> customValueDic, Dictionary<string, AssmeblePartCustomSaveData> propertyDic)
        {
            AssmeblePartSingleSaveData data = new AssmeblePartSingleSaveData();
            this.partID = partID;
            this.UID = UID;
            this.customName_Partial = customName;
            this.customValueDic = customValueDic;
            this.propertyDic = propertyDic;
            return data;
        }
    }

    public class AssmeblePartCustomSaveData
    {
        public int partID;
        public string propertyName;
        public float propertyValueMin;
        public float propertyValueMax;

        public Dictionary<string, AssemblePartPropertyDetailInfo> detailInfoDic;
        public Dictionary<string, AssemblePartTimeCostDetialInfo> timeCostDetailInfoDic;

        public AssmeblePartCustomSaveData(int partID,string propertyName,float min,float max, Dictionary<string, AssemblePartPropertyDetailInfo> detailInfoDic, Dictionary<string, AssemblePartTimeCostDetialInfo> timeCostDetailInfoDic)
        {
            this.partID = partID;
            this.propertyName = propertyName;
            this.propertyValueMax = max;
            this.propertyValueMin = min;
            this.detailInfoDic = detailInfoDic;
            this.timeCostDetailInfoDic = timeCostDetailInfoDic;
        }

    }

    #endregion
}