using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine;

namespace Sim_FrameWork
{

    public class ManufactoryBaseInfoData
    {
        public List<BlockLevelData> BlockLevelDatas;
        public List<BlockDistrictUnlockData> DistrictUnlockDatas;
        public List<InherentLevelData> InherentLevelDatas;
        public List<ManufactureType> ManufactureTypes;

        public void LoadData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            ManufactoryBaseInfoData info= reader.LoadJsonDataConfig<ManufactoryBaseInfoData>(Config.JsonConfigPath.ManufactoryBaseInfoJsonPath);
            BlockLevelDatas = info.BlockLevelDatas;
            DistrictUnlockDatas = info.DistrictUnlockDatas;
            InherentLevelDatas = info.InherentLevelDatas;
            ManufactureTypes = info.ManufactureTypes;
        }

        public class ManufactureType
        {
            public string Type;
            public string TypeName;
            public string TypeDesc;
            public string TypeIconPath;
        }
    }


    public class InherentLevelData
    {
        public string Name;
        public string LevelName;
        public string LevelDesc;
        public string IconPath;
    }

    /// <summary>
    /// EXP Data
    /// </summary>

    public class BlockLevelData
    {
        public string ID;
        public List<int> EXPMap;
    }

    public class BlockDistrictUnlockData
    {
        public string ID;
        public List<DistrictUnlockData> UnlockData;

        public class DistrictUnlockData
        {
            public int DistrictID;
            public bool UnlockDefault;
        }
    }

   

}