using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class AssembleModule : BaseModule<AssembleModule>
    {
        private static List<AssembleWarship> AssembleWarshipList;
        private static Dictionary<int,AssembleWarship> AssembleWarshipDic;
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
            public float PropertyRangeMin;
            public float PropertyRangeMax;
        }
    }

    public class PartsCustomConfig
    {
        public string customName;

        public class ConfigData
        {
            public string CustomDataName;
            public float CustomDataRangeMin;
            public float CustomDataRangeMax;
            public float CustomDataDefaultValue;
            public float ValueChangeMinUnit;
            public List<PropertyLinkData> propertyLinkData;
            public List<MaterialCostLinkData> materialCostLinkData;

            public class PropertyLinkData
            {
                public string Name;
                public string LinkDesc;
                public float PropertyChangePerUnitMin;
                public float PropertyChangePerUnitMax;
            }

            public class MaterialCostLinkData
            {

            }

        }
    }


}