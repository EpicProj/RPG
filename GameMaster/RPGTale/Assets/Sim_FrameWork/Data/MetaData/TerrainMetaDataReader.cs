using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class TerrainMetaDataReader
    {
        public static List<BiotaData> BiotaDataList;
        public static Dictionary<int, BiotaData> BiotaDataDic;
        public static List<TerrainData> TerrainDataList;
        public static Dictionary<int, TerrainData> TerrainDataDic;
        public static void LoadData()
        {
            var config = ConfigManager.Instance.LoadData<TerrainMetaData>(ConfigPath.TABLE_TERRIAN_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("MaterialMetaData Read Error");
                return;
            }

            BiotaDataList = config.AllBiotaDataList;
            BiotaDataDic = config.AllBiotaDataDic;
            TerrainDataList = config.AllTerrainDataList;
            TerrainDataDic = config.AllTerrainDataDic;
        }

        public static List<BiotaData> GetBiotaData()
        {
            LoadData();
            return BiotaDataList;
        }
        public static Dictionary<int,BiotaData> GetBiotaDataDic()
        {
            LoadData();
            return BiotaDataDic;
        }
        public static List<TerrainData> GetTerrianData()
        {
            LoadData();
            return TerrainDataList;
        }
        public static Dictionary<int,TerrainData> GetTerrianDataDic()
        {
            LoadData();
            return TerrainDataDic;
        }


    }
}