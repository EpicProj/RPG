using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class DistrictMetaDataReader
    {

        public static List<DistrictData> DistrictDataList = new List<DistrictData>();
        public static Dictionary<int, DistrictData> DistrictDataDic = new Dictionary<int, DistrictData>();


        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<DistrictMetaData>(ConfigPath.TABLE_DISTRICT_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("DistrictMetaData Read Error");
                return;
            }
            DistrictDataList = config.AllDistrictDataList;
            DistrictDataDic = config.AllDistrictDataDic;
        }



        public static List<DistrictData> GetDistrictData()
        {
            LoadData();
            return DistrictDataList;
        }

        public static Dictionary<int,DistrictData> GetDistrictDic()
        {
            LoadData();
            return DistrictDataDic;
        }

        public static DistrictData GetDistrictDataByKey(int key)
        {
            LoadData();
            DistrictData data = null;
            if (DistrictDataDic == null)
                return null;
            DistrictDataDic.TryGetValue(key, out data);
            if (data == null)
            {
                Debug.LogError("Can not Find DistrictData , Key = " + key);
                return null;
            }
            return data;
        }

    }
}