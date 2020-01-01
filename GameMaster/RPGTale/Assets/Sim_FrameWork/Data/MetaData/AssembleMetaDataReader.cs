using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class AssembleMetaDataReader 
    {
        private static List<AssembleWarship> AssembleWarshipList;
        private static Dictionary<int, AssembleWarship> AssembleWarshipDic;

        private static List<AssembleWarshipClass> AssembleWarshipClassList;
        private static Dictionary<int, AssembleWarshipClass> AssembleWarshipClassDic;

        private static List<AssembleParts> AssemblePartsList;
        private static Dictionary<int, AssembleParts> AssemblePartsDic;

        private static List<AssemblePartsType> AssemblePartsTypeList;
        private static Dictionary<int, AssemblePartsType> AssemblePartsTypeDic;


        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<AssembleMetaData>(ConfigPath.TABLE_ASSEMBLE_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("BuildingPanelMetaData Read Error");
                return;
            }
            AssembleWarshipList = config.AllAssembleWarshipList;
            AssembleWarshipDic = config.AllAssembleWarshipDic;
            AssembleWarshipClassList = config.AllAssembleWarshipClassList;
            AssembleWarshipClassDic = config.AllAssembleWarshipClassDic;
            AssemblePartsList = config.AllAssemblePartsList;
            AssemblePartsDic = config.AllAssemblePartsDic;
            AssemblePartsTypeList = config.AllAssemblePartsTypeList;
            AssemblePartsTypeDic = config.AllAssemblePartsTypeDic;
        }


        public static List<AssembleWarship> GetAssembleWarshipList()
        {
            LoadData();
            return AssembleWarshipList;
        }

        public static Dictionary<int, AssembleWarship> GetAssembleWarshipDic()
        {
            LoadData();
            return AssembleWarshipDic;
        }

        public static List<AssembleWarshipClass> GetAssembleWarshipClassList()
        {
            LoadData();
            return AssembleWarshipClassList;
        }
        public static Dictionary<int, AssembleWarshipClass> GetAssembleWarshipClassDic()
        {
            LoadData();
            return AssembleWarshipClassDic;
        }

        public static List<AssembleParts> GetAssemblePartsList()
        {
            LoadData();
            return AssemblePartsList;
        }
        public static Dictionary<int, AssembleParts> GetAssemblePartsDic()
        {
            LoadData();
            return AssemblePartsDic;
        }

        public static List<AssemblePartsType> GetAssemblePartsTypeList()
        {
            LoadData();
            return AssemblePartsTypeList;
        }
        public static Dictionary<int, AssemblePartsType> GetAssemblePartsTypeDic()
        {
            LoadData();
            return AssemblePartsTypeDic;
        }


    }
}