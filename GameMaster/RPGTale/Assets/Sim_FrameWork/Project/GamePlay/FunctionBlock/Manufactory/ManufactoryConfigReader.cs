using UnityEngine;
using System.IO;
using LitJson;

namespace Sim_FrameWork.Config
{
    public class ManufactoryConfigReader 
    {

        public FunctionBlock_Smelt_Config LoadMaterialRarityDataConfig()
        {
            FunctionBlock_Smelt_Config config = new FunctionBlock_Smelt_Config();
            string filePath = Application.streamingAssetsPath + "/Data/JsonData/Factory/Manufactory" + "/FunctionBlock_Smelt_Config.json";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                config = JsonMapper.ToObject<FunctionBlock_Smelt_Config>(jsonStr);
                return config;
            }
            else
            {
                Debug.LogError("FunctionBlock_Smelt_Config  Read Fail");
            }
            return null;
        }


    }
}