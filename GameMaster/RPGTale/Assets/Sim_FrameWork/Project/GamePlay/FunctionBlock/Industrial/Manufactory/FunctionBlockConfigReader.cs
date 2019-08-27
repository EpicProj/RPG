using UnityEngine;
using System.IO;
using LitJson;

namespace Sim_FrameWork.Config
{
    public class FunctionBlockConfigReader 
    {

        public BlockLevelData LoadEXPConfig()
        {
            BlockLevelData config = new BlockLevelData();
            string filePath = Application.streamingAssetsPath + "/Data/JsonData/Factory" + "/FunctionBlockLevelData.json";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                config = JsonMapper.ToObject<BlockLevelData>(jsonStr);
                return config;
            }
            else
            {
                Debug.LogError("FunctionBlockLevelData  Read Fail");
            }
            return null;
        }


    }
}