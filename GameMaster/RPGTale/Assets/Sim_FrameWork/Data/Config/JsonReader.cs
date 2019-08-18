using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;

namespace Sim_FrameWork.Config
{
    public class JsonReader
    {
        public MaterialRarityData LoadMaterialRarityDataConfig()
        {
            MaterialRarityData rarityData = new MaterialRarityData();
            string filePath = Application.streamingAssetsPath + "/Data/JsonData/Material" + "/MaterialBasicConfig.json";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                rarityData = JsonMapper.ToObject<MaterialRarityData>(jsonStr);
                return rarityData;
            }
            else
            {
                Debug.LogError("Material RarityData Read Fail");
            }
            return null;
        }

        public BaseResourcesConfig LoadBaseResourcesConfig()
        {
            BaseResourcesConfig config = new BaseResourcesConfig();
            string filePath= Application.streamingAssetsPath + "/Data/JsonData/Basic" + "/BaseResourceConfig.json";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                config = JsonMapper.ToObject<BaseResourcesConfig>(jsonStr);
                return config;
            }
            else
            {
                Debug.LogError("BaseResourcesConfig Read Fail");
            }
            return null;
        }

        //PlayerConfig
        public PlayerConfig LoadPlayerConfig()
        {
            PlayerConfig config = new PlayerConfig();
            string filePath = Application.streamingAssetsPath + "/Data/JsonData/Basic" + "/PlayerConfig.json";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                config = JsonMapper.ToObject<PlayerConfig>(jsonStr);
                return config;
            }
            else
            {
                Debug.LogError("PlayerConfig Read Fail");
            }
            return null;
        }


        //ModifierData
        public GeneralModifier LoadModifierData()
        {
            GeneralModifier data = new GeneralModifier();
            string filePath = Application.streamingAssetsPath + "/Data/JsonData/Modifier" + "/GeneralModifier.json";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                data = JsonMapper.ToObject<GeneralModifier>(jsonStr);
                return data;
            }
            else
            {
                Debug.LogError("ModifierData Read Fail");
            }
            return null;
        }

    }
}