using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public class AssembleConfig
    {
        public string assemblePartPage_DefaultSelectTab;

        public List<AssembleMainType> assembleMainType;
        public List<AssemblePartMainType> assemblePartMainType;

        public AssembleConfig LoadAssembleConfigData()
        {
            JsonReader config = new JsonReader();
            AssembleConfig settting = config.LoadJsonDataConfig<AssembleConfig>(JsonConfigPath.AssembleConfigJsonPath);
            assemblePartPage_DefaultSelectTab = settting.assemblePartPage_DefaultSelectTab;
            assembleMainType = settting.assembleMainType;
            assemblePartMainType = settting.assemblePartMainType;

            ///DataCheck

            List<string> assembleTypeList = new List<string>();
            for (int i = 0; i < assembleMainType.Count; i++)
            {
                if (!assembleTypeList.Contains(assembleMainType[i].Type))
                {
                    assembleTypeList.Add(assembleMainType[i].Type);
                }
                else
                {
                    Debug.LogError("GlobalConfig: Find Same AssembleMainType  Type=" + assembleMainType[i].Type);
                }
            }

            List<string> assemblePartTypeList = new List<string>();
            for (int i = 0; i < assemblePartMainType.Count; i++)
            {
                if (!assemblePartTypeList.Contains(assemblePartMainType[i].Type))
                {
                    assemblePartTypeList.Add(assemblePartMainType[i].Type);
                }
                else
                {
                    Debug.LogError("GlobalConfig: Find Same assemblePartMainType  Type=" + assemblePartMainType[i].Type);
                }
            }
            return settting;
        }

    }

    public class AssembleMainType
    {
        public string Type;
        public string TypeNameText;
        public string IconPath;
    }

    public class AssemblePartMainType
    {
        public string Type;
        public string TypeName;
        public string IconPath;
        public bool DefaultUnlock;
    }


    /// <summary>
    /// DIY部件配置
    /// </summary>
    public class AssemblePartsConfigData
    {
        public List<PartsPropertyConfig> partsPropertyConfig;
        public List<PartsCustomConfig> partsCustomConfig;

        public AssemblePartsConfigData LoadPartsCustomConfig()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<AssemblePartsConfigData>(JsonConfigPath.AssemblePartsConfigDataJsonPath);
            partsPropertyConfig = data.partsPropertyConfig;
            partsCustomConfig = data.partsCustomConfig;

            List<string> partsPropertyNameList = new List<string>();
            for (int i = 0; i < partsPropertyConfig.Count; i++)
            {
                if (!partsPropertyNameList.Contains(partsPropertyConfig[i].configName))
                {
                    partsPropertyNameList.Add(partsPropertyConfig[i].configName);
                }
                else
                {
                    Debug.LogError("Find Same partsPropertyName , name=" + partsPropertyConfig[i].configName);
                }

                if (partsPropertyConfig[i].configData.Count > GlobalConfigData.AssemblePart_Max_PropertyNum)
                {
                    Debug.LogError("AssemblePart_Max_PropertyNum is 4!   configName=" + partsPropertyConfig[i].configName);
                }

            }
            List<string> partsCustomNameList = new List<string>();
            for (int i = 0; i < partsCustomConfig.Count; i++)
            {
                if (!partsCustomNameList.Contains(partsCustomConfig[i].customName))
                {
                    partsCustomNameList.Add(partsCustomConfig[i].customName);
                }
                else
                {
                    Debug.LogError("Find Same partsCustomName , name=" + partsCustomConfig[i].customName);
                }
            }



            return data;
        }
    }


    public class AssembleShipPartConfigData
    {

        public List<AssembleShipPartConfig> shipPartConfig;

        public AssembleShipPartConfigData LoadAssembleShipPartConfigData()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<AssembleShipPartConfigData>(Config.JsonConfigPath.AssembleShipPartConfigDataJsonPath);
            shipPartConfig = data.shipPartConfig;

            List<string> shipPartConfigList = new List<string>();
            for (int i = 0; i < shipPartConfig.Count; i++)
            {
                if (!shipPartConfigList.Contains(shipPartConfig[i].configName))
                {
                    shipPartConfigList.Add(shipPartConfig[i].configName);
                }
                else
                {
                    Debug.LogError("Find Same shipPartConfig , name=" + shipPartConfig[i].configName);
                }
            }
            return data;
        }


    }



}
