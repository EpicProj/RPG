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


        //FunctionBlock
        public static string ManufactoryBaseInfoJsonPath= Application.streamingAssetsPath + "/Data/JsonData/FunctionBlock/Manufactory" + "/ManufactoryBaseInfoData.json";
        public static string LaborBaseInfoJsonPath = Application.streamingAssetsPath + "/Data/JsonData/FunctionBlock/Labor" + "/LaborBaseInfoData.json";

        //Camp
        public static string CampBaseConfigJsonPath=Application.streamingAssetsPath+ "/Data/JsonData/Camp" + "/CampConfig.json";
        //Order
        public static string OrderConfigJsonPath= Application.streamingAssetsPath + "/Data/JsonData/Order" + "/OrderConfig.json";

        //TechGroup
        public static string TechGroupConfigJsonPath = Application.streamingAssetsPath + "/Data/JsonData/Technology/Group" + "/TechGroupConfig.json";
        public static string TechnologyConfigCommon = Application.streamingAssetsPath + "/Data/JsonData/Technology/" + "TechnologyConfigCommon.json";

        //RewardData
        public static string RewardDataJsonPath = Application.streamingAssetsPath + "/Data/JsonData/Basic/RewardData.json";

        public static string ExploreConfigDataJsonPath = Application.streamingAssetsPath + "/Data/JsonData/Explore/ExploreGeneralConfig.json";
        public static string EventConfigDataJsonPath = Application.streamingAssetsPath + "/Data/JsonData/Explore/EventConfig.json";

        public static string AssemblePartsConfigDataJsonPath= Application.streamingAssetsPath + "/Data/JsonData/Assemble/AssemblePartsConfigData.json";
        public static string AssembleShipPartConfigDataJsonPath= Application.streamingAssetsPath + "/Data/JsonData/Assemble/AssembleShipPartConfigData.json";


    }
}