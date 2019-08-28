using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine;

namespace Sim_FrameWork
{
    public class BlockBaseInfoData
    {

        public List<BlockLevelData> ManufactoryBlockLevelDataList;

        public BlockBaseInfoData()
        {
            LoadData();
        }

        public void LoadData()
        {
            ManufactoryBaseInfoData manuData = new ManufactoryBaseInfoData();
            manuData.LoadData();
            ManufactoryBlockLevelDataList = manuData.ManufactoryBlockLevelDatas;
        }


    }


    public class ManufactoryBaseInfoData
    {
        public List<BlockLevelData> ManufactoryBlockLevelDatas;


        public void LoadData()
        {
            BlockBaseInfoDataReader reader = new BlockBaseInfoDataReader();
            ManufactoryBaseInfoData info= reader.LoadManufactoryBaseInfoData();
            ManufactoryBlockLevelDatas = info.ManufactoryBlockLevelDatas;
        }
     

    }


    public class BlockLevelData
    {
        public string ID;
        public List<int> EXPMap;
    }



    public class BlockBaseInfoDataReader
    {
        public ManufactoryBaseInfoData LoadManufactoryBaseInfoData()
        {
            ManufactoryBaseInfoData data = new ManufactoryBaseInfoData();
            string filePath = Application.streamingAssetsPath + "/Data/JsonData/FunctionBlock/Manufactory" + "/ManufactoryBaseInfoData.json";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                data = JsonMapper.ToObject<ManufactoryBaseInfoData>(jsonStr);
                return data;
            }
            else
            {
                Debug.LogError("ManufactoryBaseInfoData Read Fail");
            }
            return null;
        }
    }
}