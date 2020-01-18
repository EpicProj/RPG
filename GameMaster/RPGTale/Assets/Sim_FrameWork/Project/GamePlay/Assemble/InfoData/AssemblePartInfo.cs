using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public Dictionary<string, CustomData> propertyDic;
        public Dictionary<string, float> customValueDic;


        public AssemblePartCustomDataInfo(int partID, string partNameCustomText, Dictionary<string, CustomData> propertyDic, Dictionary<string, float> customValueDic)
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
        public string PropertyName;
        public string PropertyIcon;
    }

}