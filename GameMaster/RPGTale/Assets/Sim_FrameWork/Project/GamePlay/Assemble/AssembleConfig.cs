using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public class AssembleConfig
    {
        public string assemblePartPage_DefaultSelectTab;
        public string assembleShipPage_DefaultSelectTab;

        /// <summary>
        /// 耐久度关联属性名
        /// </summary>
        public string assembleShip_Durability_Property_Link;
        /// <summary>
        /// 货仓储量关联属性名
        /// </summary>
        public string assembleShip_Storage_Property_Link;
        /// <summary>
        /// 最大成员属性关联
        /// </summary>
        public string assembleShip_Member_Property_Link;

        /// <summary>
        /// 母舰护盾开启初始值
        /// </summary>
        public string mainShip_Shield_OpenInit_Property_Link;
        /// <summary>
        /// 母舰护盾最大值
        /// </summary>
        public string mainShip_Shield_Max_Property_Link;
        /// <summary>
        /// 母舰护盾回复速度
        /// </summary>
        public string mainShip_Shield_ChargeSpeed_Property_Link;
        /// <summary>
        /// 母舰护盾能耗
        /// </summary>
        public string mainShip_Shield_EnergyCost_Property_Link;
        /// <summary>
        /// 减伤数值
        /// </summary>
        public string mainShip_Shield_DamageReduce_Property_Link;
        /// <summary>
        /// 护盾等级，用于权重计算
        /// </summary>
        public string mainShip_Shield_ShieldLevel_Property_Link;
        /// <summary>
        /// 减伤概率
        /// </summary>
        public string mainShip_Shield_ReduceProbability_Property_Link;
        /// <summary>
        /// 最大可开启护盾层数
        /// </summary>
        public string mainShip_Shield_MaxLayer_Property_Link;

        public List<AssembleMainType> assembleMainType;
        public List<AssemblePartPropertyTypeData> assemblePartPropertyType;
        public List<AssembleShipMainType> assembleShipMainType;
        public List<AssemblePartMainType> assemblePartMainType;

        public AssembleConfig LoadAssembleConfigData()
        {
            JsonReader config = new JsonReader();
            AssembleConfig settting = config.LoadJsonDataConfig<AssembleConfig>(JsonConfigPath.AssembleConfigJsonPath);
            assemblePartPage_DefaultSelectTab = settting.assemblePartPage_DefaultSelectTab;
            assembleShipPage_DefaultSelectTab = settting.assembleShipPage_DefaultSelectTab;

            assembleShip_Durability_Property_Link = settting.assembleShip_Durability_Property_Link;
            assembleShip_Member_Property_Link = settting.assembleShip_Member_Property_Link;
            assembleShip_Storage_Property_Link = settting.assembleShip_Storage_Property_Link;

            mainShip_Shield_OpenInit_Property_Link = settting.mainShip_Shield_OpenInit_Property_Link;
            mainShip_Shield_Max_Property_Link = settting.mainShip_Shield_Max_Property_Link;
            mainShip_Shield_ChargeSpeed_Property_Link = settting.mainShip_Shield_ChargeSpeed_Property_Link;
            mainShip_Shield_EnergyCost_Property_Link = settting.mainShip_Shield_EnergyCost_Property_Link;
            mainShip_Shield_ReduceProbability_Property_Link = settting.mainShip_Shield_ReduceProbability_Property_Link;
            mainShip_Shield_ShieldLevel_Property_Link = settting.mainShip_Shield_ShieldLevel_Property_Link;
            mainShip_Shield_DamageReduce_Property_Link = settting.mainShip_Shield_DamageReduce_Property_Link;
            mainShip_Shield_MaxLayer_Property_Link = settting.mainShip_Shield_MaxLayer_Property_Link;

            assembleMainType = settting.assembleMainType;
            assemblePartPropertyType = settting.assemblePartPropertyType;
            assembleShipMainType = settting.assembleShipMainType;
            assemblePartMainType = settting.assemblePartMainType;

            ///DataCheck

            List<string> assembleTypeList = new List<string>();
            for (int i = 0; i < assembleMainType.Count; i++)
            {
                if (!assembleTypeList.Contains(assembleMainType[i].Type))
                {
                    assembleTypeList.Add(assembleMainType[i].Type);
                }
                else
                {
                    Debug.LogError("AssembleConfig: Find Same AssembleMainType  Type=" + assembleMainType[i].Type);
                }
            }

            List<string> assemblePartPropertyList = new List<string>();
            for (int i = 0; i < assemblePartPropertyType.Count; i++)
            {
                if (!assemblePartPropertyList.Contains(assemblePartPropertyType[i].Name))
                {
                    assemblePartPropertyList.Add(assemblePartPropertyType[i].Name);
                }
                else
                {
                    Debug.LogError("AssembleConfig: Find Same assembleShipMainType  Type=" + assembleShipMainType[i].Type);
                }
            }

            List<string> assembleShipTypeList = new List<string>();
            for(int i = 0; i < assembleShipMainType.Count; i++)
            {
                if (!assembleShipTypeList.Contains(assembleShipMainType[i].Type))
                {
                    assembleShipTypeList.Add(assembleShipMainType[i].Type);
                }
                else
                {
                    Debug.LogError("AssembleConfig: Find Same assembleShipMainType  Type=" + assembleShipMainType[i].Type);
                }
            }

            List<string> assemblePartTypeList = new List<string>();
            for (int i = 0; i < assemblePartMainType.Count; i++)
            {
                if (!assemblePartTypeList.Contains(assemblePartMainType[i].Type))
                {
                    assemblePartTypeList.Add(assemblePartMainType[i].Type);
                }
                else
                {
                    Debug.LogError("AssembleConfig: Find Same assemblePartMainType  Type=" + assemblePartMainType[i].Type);
                }
            }
            return settting;
        }

    }

    public class AssembleMainType
    {
        public string Type;
        public string TypeNameText;
        public string IconPath;
    }

    public class AssemblePartMainType
    {
        public string Type;
        public string TypeName;
        public string IconPath;
        public bool DefaultUnlock;
    }

    public class AssembleShipMainType
    {
        public string Type;
        public string TypeName;
        public string IconPath;
        public bool DefaultUnlock;
    }


    /// <summary>
    /// DIY部件配置
    /// </summary>
    public class AssemblePartsConfigData
    {
        public List<PartsPropertyConfig> partsPropertyConfig;
        public List<PartsCustomConfig> partsCustomConfig;

        public AssemblePartsConfigData LoadPartsCustomConfig()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<AssemblePartsConfigData>(JsonConfigPath.AssemblePartsConfigDataJsonPath);
            partsPropertyConfig = data.partsPropertyConfig;
            partsCustomConfig = data.partsCustomConfig;

            List<string> partsPropertyNameList = new List<string>();
            for (int i = 0; i < partsPropertyConfig.Count; i++)
            {
                if (!partsPropertyNameList.Contains(partsPropertyConfig[i].configName))
                {
                    partsPropertyNameList.Add(partsPropertyConfig[i].configName);
                }
                else
                {
                    Debug.LogError("Find Same partsPropertyName , name=" + partsPropertyConfig[i].configName);
                }

                if (partsPropertyConfig[i].configData.Count > GlobalConfigData.AssemblePart_Max_PropertyNum)
                {
                    Debug.LogError("AssemblePart_Max_PropertyNum is 4!   configName=" + partsPropertyConfig[i].configName);
                }

            }
            List<string> partsCustomNameList = new List<string>();
            for (int i = 0; i < partsCustomConfig.Count; i++)
            {
                if (!partsCustomNameList.Contains(partsCustomConfig[i].customName))
                {
                    partsCustomNameList.Add(partsCustomConfig[i].customName);
                }
                else
                {
                    Debug.LogError("Find Same partsCustomName , name=" + partsCustomConfig[i].customName);
                }
            }



            return data;
        }
    }


    public class AssembleShipPartConfigData
    {

        public List<AssembleShipPartConfig> shipPartConfig;

        public AssembleShipPartConfigData LoadAssembleShipPartConfigData()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<AssembleShipPartConfigData>(Config.JsonConfigPath.AssembleShipPartConfigDataJsonPath);
            shipPartConfig = data.shipPartConfig;

            List<string> shipPartConfigList = new List<string>();
            for (int i = 0; i < shipPartConfig.Count; i++)
            {
                if (!shipPartConfigList.Contains(shipPartConfig[i].configName))
                {
                    shipPartConfigList.Add(shipPartConfig[i].configName);
                }
                else
                {
                    Debug.LogError("Find Same shipPartConfig , name=" + shipPartConfig[i].configName);
                }
            }
            return data;
        }
    }

    #region Config Data

    public class PartsPropertyConfig
    {
        public string configName;
        public List<ConfigData> configData;

        public class ConfigData
        {
            public string Name;
            /// <summary>
            /// 1 = 固定值
            /// 2 = 浮动值
            /// </summary>
            public int PropertyType;

            public double PropertyValue;
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
            public string LinkDesc;

            public double CustomDataRangeMin;
            public double CustomDataRangeMax;
            public double CustomDataDefaultValue;

            public double TimeCostPerUnit;
            public List<PropertyLinkData> propertyLinkData;
            public List<MaterialCostLinkData> materialCostLinkData;

            public class PropertyLinkData
            {
                public string Name;
                /// <summary>
                /// 1 = 固定值
                /// 2 = 浮动值
                /// </summary>
                public int PropertyChangeType;

                public double PropertyChangePerUnitMin;
                public double PropertyChangePerUnitMax;
                public double PropertyChangePerUnitValue;
            }

            public class MaterialCostLinkData
            {

            }

        }
    }


    public class AssembleShipPartConfig
    {
        public string configName;
        public List<ConfigData> configData;

        public class ConfigData
        {
            public int configID;
            public string PosType;
            public double PosX;
            public double PosY;
            public double LineWidth;
            public List<string> EquipPartType;
        }


    }



    #endregion


}
