using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class BuildingPanelMetaDataReader 
    {

        public static List<BuildingPanelData> BuildingPanelDataList = new List<BuildingPanelData>();
        public static Dictionary<int, BuildingPanelData> BuildingPanelDataDic = new Dictionary<int, BuildingPanelData>();

        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<BuildingPanelMetaData>(ConfigPath.TABLE_BUILDPANEL_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("BuildingPanelMetaData Read Error");
                return;
            }
            BuildingPanelDataList = config.AllBuildingPanelDataList;
            BuildingPanelDataDic = config.AllBuildingPanelDataDic;
 
        }


        public static List<BuildingPanelData> GetBuildingPanelDataList()
        {
            LoadData();
            return BuildingPanelDataList;
        }

        public static Dictionary<int,BuildingPanelData> GetBuildingPanelDataDic()
        {
            LoadData();
            return BuildingPanelDataDic;
        }

        public static BuildingPanelData GetDistrictDataByKey(int key)
        {
            LoadData();
            BuildingPanelData data = null;
            if (BuildingPanelDataDic == null)
                return null;
            BuildingPanelDataDic.TryGetValue(key, out data);
            if (data == null)
            {
                Debug.LogError("Can not Find BuildingPanelData , Key = " + key);
                return null;
            }
            return data;
        }


    }
}