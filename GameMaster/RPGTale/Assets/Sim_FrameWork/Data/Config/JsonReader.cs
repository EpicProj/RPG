using UnityEngine;
using System.IO;
using LitJson;
using System.Collections.Generic;

namespace Sim_FrameWork.Config
{
    public class JsonReader
    {

        public T LoadJsonDataConfig<T>(string filePath) where T:new()
        {
            T data = new T();
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                data = JsonMapper.ToObject<T>(jsonStr);
                return data;
            }
            else
            {
                Debug.LogError(typeof(T).ToString()+ " Read Fail");
            }
            return default(T);
        }

    }

    public class JsonConfigPath
    {
        //ModifierData
        public static string ModifierDataConfigJsonPath = Application.streamingAssetsPath + "/Data/JsonData/Modifier" + "/GeneralModifier.json";
        //PlayerConfig
        public static string PlayerConfigJsonPath = Application.streamingAssetsPath + "/Data/JsonData/Basic" + "/PlayerConfig.json";
        //GlobalSetting
        public static string GlobalSettingJsonPath = Application.streamingAssetsPath + "/Data/JsonData/Basic" + "/Global_Setting.json";
        //Base Resource
        public static string BaseResourceJsonPath = Application.streamingAssetsPath + "/Data/JsonData/Basic" + "/BaseResourceConfig.json";
        //Material Config
        public static string MaterialConfigJsonPath = Application.streamingAssetsPath + "/Data/JsonData/Material" + "/MaterialBasicConfig.json";


    }
}