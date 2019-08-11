using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

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




    }
}