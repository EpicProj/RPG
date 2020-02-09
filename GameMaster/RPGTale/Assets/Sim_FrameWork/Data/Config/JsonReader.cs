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
            string path = "";
            
            if (ResourceManager.m_LoadFormAssetBundle)
            {
                path= JsonConfigPath.ConfigPath_Bundle + filePath;
            }
            else
            {
                path = JsonConfigPath.ConfigPath + filePath;
            }

            T data = new T();
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                data = JsonMapper.ToObject<T>(jsonStr);
                return data;
            }
            else
            {
                DebugPlus.LogError("JsonConfig Read Fail  Path="+ path);
            }
            return default(T);
        }

    }

    public class JsonConfigPath
    {
        public static string ConfigPath = Application.dataPath + "/ConfigData/";
        public static string ConfigPath_Bundle = System.AppDomain.CurrentDomain.BaseDirectory + "/";

        public static string AssembleConfigPath = "Game_Config/Assemble/";
        public static string ModifierConfigPath = "Game_Config/Modifier/";
        public static string BasicConfigPath = "Game_Config/Basic/";
        public static string MaterialConfigPath= "Game_Config/Material/";
        public static string FunctionBlockConfigPath = "Game_Config/FunctionBlock/";
        public static string OrderConfigPath = "Game_Config/Order/";
        public static string TechnologyConfigPath = "Game_Config/Technology/";
        public static string MainShipConfigPath= "Game_Config/MainShipConfig/";
        public static string ExploreConfigPath = "Game_Config/Explore/";
        public static string LeaderConfigPath = "Game_Config/Leader/";

        //ModifierData
        public static string ModifierDataConfigJsonPath = ModifierConfigPath + "GeneralModifier.json";

        //PlayerConfig
        public static string PlayerConfigJsonPath = BasicConfigPath + "PlayerConfig.json";
        //GlobalSetting
        public static string GlobalSettingJsonPath = BasicConfigPath + "Global_Setting.json";
        //Base Resource
        public static string BaseResourceJsonPath = BasicConfigPath + "BaseResourceConfig.json";
        //RewardData
        public static string RewardDataJsonPath = BasicConfigPath + "RewardData.json";

        //Material Config
        public static string MaterialConfigJsonPath = MaterialConfigPath + "MaterialBasicConfig.json";


        //FunctionBlock
        public static string ManufactoryBaseInfoJsonPath= FunctionBlockConfigPath + "/ConfigData/JsonData/FunctionBlock/Manufactory" + "/ManufactoryBaseInfoData.json";
        public static string BlockConfigDataJsonPath = FunctionBlockConfigPath + "BlockConfigData.json";
        public static string BlockLevelConfigJsonPath= FunctionBlockConfigPath + "BlockLevelConfig.json";

        //Order
        public static string OrderConfigJsonPath= OrderConfigPath +  "OrderConfig.json";

        //TechGroup
        public static string TechGroupConfigJsonPath = TechnologyConfigPath + "Group/TechGroupConfig.json";
        public static string TechnologyConfigCommon = TechnologyConfigPath + "TechnologyConfigCommon.json";

        public static string ExploreConfigDataJsonPath = ExploreConfigPath + "ExploreGeneralConfig.json";
        public static string EventConfigDataJsonPath = ExploreConfigPath + "EventConfig.json";

        //Assemble
        public static string AssemblePartsConfigDataJsonPath=  AssembleConfigPath+"AssemblePartsConfigData.json";
        public static string AssembleShipPartConfigDataJsonPath=  AssembleConfigPath+"AssembleShipPartConfigData.json";
        public static string AssembleConfigJsonPath=  AssembleConfigPath+"AssembleConfig.json";

        //MainShipConfig
        public static string MainShipConfigJsonPath = MainShipConfigPath + "MainShipConfig.json";
        public static string MainShipAreaMapConfigJsonPath= MainShipConfigPath + "MainShipMapConfig.json";

        public static string LeaderPortraitConfigJsonPath = LeaderConfigPath + "LeaderPortraitConfig.json";

    }
}