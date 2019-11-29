using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class ConfigManager : Singleton<ConfigManager> {

        /// <summary>
        /// 储存所有已经加载的配置表
        /// </summary>
        protected Dictionary<string, ExcelBase> m_AllExcelData = new Dictionary<string, ExcelBase>();

        /// <summary>
        /// 加载数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">二进制路径</param>
        /// <returns></returns>
        public T LoadData<T>(string path) where T : ExcelBase
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (m_AllExcelData.ContainsKey(path))
            {
                return m_AllExcelData[path] as T;
            }

            T data = BinarySerializeOpt.BinaryDeserilize<T>(path);

#if UNITY_EDITOR
            if (data == null)
            {
                Debug.Log(path + "不存在，从xml加载数据了！");
                string xmlPath = path.Replace("Binary", "Xml").Replace(".bytes", ".xml");
                data = BinarySerializeOpt.XmlDeserialize<T>(xmlPath);
            }
#endif

            if (data != null)
            {
                data.Init();
            }

            m_AllExcelData.Add(path, data);

            return data;
        }

        /// <summary>
        /// 根据路径查找数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T FindData<T>(string path) where T : ExcelBase
        {
            if (string.IsNullOrEmpty(path))
                return null;

            ExcelBase excelBase = null;
            if (m_AllExcelData.TryGetValue(path, out excelBase))
            {
                return excelBase as T;
            }
            else
            {
                excelBase = LoadData<T>(path);
            }

            return (T)excelBase;
        }
    }

    public class ConfigPath
    {
        public const string TABLE_FUNCTIONBLOCK_METADATA_PATH = "Assets/Resources/Data/DataFormat/Binary/FunctionBlockMetaData.bytes";
        public const string TABLE_MATERIAL_METADATA_PATH= "Assets/Resources/Data/DataFormat/Binary/MaterialMetaData.bytes";
        public const string TABLE_FORMULA_METADATA_PATH= "Assets/Resources/Data/DataFormat/Binary/FunctionBlockFormulaMetaData.bytes";
        public const string TABLE_DISTRICT_METADATA_PATH= "Assets/Resources/Data/DataFormat/Binary/DistrictMetaData.bytes";
        public const string TABLE_BUILDPANEL_METADATA_PATH= "Assets/Resources/Data/DataFormat/Binary/BuildingPanelMetaData.bytes";
        public const string TABLE_TERRIAN_METADATA_PATH= "Assets/Resources/Data/DataFormat/Binary/TerrianMetaData.bytes";
        public const string TABLE_CAMP_METADATA_PATH= "Assets/Resources/Data/DataFormat/Binary/CampMetaData.bytes";
        public const string TABLE_ORGANIZATION_METADATA_PATH= "Assets/Resources/Data/DataFormat/Binary/OrganizationMetaData.bytes";
        public const string TABLE_ORDER_METADATA_PATH= "Assets/Resources/Data/DataFormat/Binary/OrderMetaData.bytes";
        public const string TABLE_TECHNOLOGY_METADATA_PATH= "Assets/Resources/Data/DataFormat/Binary/TechnologyMetaData.bytes";

    }
}