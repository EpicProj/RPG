using System.Collections;
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

        public AssmeblePartSingleSaveData CreatePartSave()
        {
            AssmeblePartSingleSaveData save = new AssmeblePartSingleSaveData();
            save.createSaveData(partID,UID, customDataInfo.partNameCustomText, customDataInfo.customValueDic);
            return save;
        }

        public AssemblePartInfo LoadSaveData(AssmeblePartSingleSaveData saveData)
        {
            if (saveData != null)
            {
                AssemblePartInfo info = new AssemblePartInfo(saveData.partID);
                return info;
            }
            return null;
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

        public class CustomData
        {
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

            public CustomData(Config.PartsPropertyConfig.ConfigData config, float min, float max,
                Dictionary<string, AssemblePartPropertyDetailInfo> detailDic, Dictionary<string, AssemblePartTimeCostDetialInfo> timeCostDetailInfoDic)
            {
                propertyType = config.PropertyType;
                propertyOriginValue = (float)config.PropertyValue;
                propertyTypeData = AssembleModule.GetAssemblePartPropertyTypeData(config.Name);
                if (propertyTypeData != null)
                {
                    propertyNameText = MultiLanguage.Instance.GetTextValue(propertyTypeData.PropertyName);
                    propertyIcon = Utility.LoadSprite(propertyTypeData.PropertyIcon, Utility.SpriteType.png);
                }
                propertyValueMin = min;
                propertyValueMax = max;
                detailInfoDic = detailDic;
                this.timeCostDetailInfoDic = timeCostDetailInfoDic;
            }

            public float CurrentValueMin
            {
                get
                {
                    float value = propertyOriginValue;
                    foreach (var detailInfo in detailInfoDic.Values)
                        value += detailInfo.modifyValueMin;
                    return value;
                }
            }

            public float CurrentValueMax
            {
                get
                {
                    float value = propertyOriginValue;
                    if (propertyType == 1)
                    {
                        foreach (var detailInfo in detailInfoDic.Values)
                            value += detailInfo.modifyValueFix;
                    }
                    else if (propertyType == 2)
                    {
                        foreach (var detailInfo in detailInfoDic.Values)
                            value += detailInfo.modifyValueMax;
                    }
                    return value;
                }
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

        public AssmeblePartSingleSaveData createSaveData(int partID, ushort UID, string customName, Dictionary<string, float> customValueDic)
        {
            AssmeblePartSingleSaveData data = new AssmeblePartSingleSaveData();
            data.partID = partID;
            data.UID = UID;
            data.customName_Partial = customName;
            data.customValueDic = customValueDic;
            return data;
        }
    }

    #endregion
}