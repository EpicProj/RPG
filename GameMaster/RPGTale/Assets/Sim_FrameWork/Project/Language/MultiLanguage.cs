using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Text.RegularExpressions;

namespace Sim_FrameWork
{
    public class MultiLanguage :Singleton<MultiLanguage>
    {
        public string CurrentLanguage = "Language_CN";

        public static string LanguagePath = Application.dataPath + "/ConfigData/Localization";
        public static string LanguagePath_Bundle = System.AppDomain.CurrentDomain.BaseDirectory + "/Localization";
        private Dictionary<string, string> TextData = new Dictionary<string, string>();
        private List<string> textID;

        public LanguageConfig config = new LanguageConfig();

        public MultiLanguage()
        {
            LoadLanguageConfig();
            if (string.IsNullOrEmpty(GetCurLanguageFilePath(CurrentLanguage)))
                return;

            string filePath = "";
            if (ResourceManager.m_LoadFormAssetBundle)
            {
                filePath = LanguagePath_Bundle + "/" + GetCurLanguageFilePath(CurrentLanguage) + "/";
            }
            else
            {
                filePath= LanguagePath+"/" + GetCurLanguageFilePath(CurrentLanguage) + "/";
            }
           
            for (int i = 0; i < config.txtFile.Count;i++)
            {
                string languageDataPath = filePath + config.txtFile[i]+".txt";
                if (File.Exists(languageDataPath))
                {
                    StreamReader sr = new StreamReader(languageDataPath);
                    string txt = sr.ReadToEnd();
                    sr.Close();
                    if (string.IsNullOrEmpty(txt))
                        continue;
                    string[] lines = txt.Replace("\r\n", "\r").Split('\r');
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        string[] value = Regex.Split(line, "==", RegexOptions.IgnoreCase);
                        if (TextData.ContainsKey(value[0]))
                        {
                            Debug.LogError("FindSame TextID  TextID=" + line+"   txt="+ languageDataPath);
                            continue;
                        }else if (value.Length != 2)
                        {
                            Debug.LogError("Text Format Error  string=" + line + "   txt=" + languageDataPath);
                            continue;
                        }
                        var replaceStr= value[1].Replace("\\n", "\n");
                        TextData.Add(value[0], replaceStr);
                    }
                }
                else
                {
                    DebugPlus.LogError("TextData Read Fail, path="+languageDataPath);
                }
            }
           
        }

        public LanguageConfig LoadLanguageConfig()
        {
            string configPath = "";
            if (ResourceManager.m_LoadFormAssetBundle)
            {
                configPath = LanguagePath_Bundle + "/LanguageConfig.json";
            }
            else
            {
                configPath= LanguagePath + "/LanguageConfig.json";
            }

            if (File.Exists(configPath))
            {
                StreamReader sr = new StreamReader(configPath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                config = JsonMapper.ToObject<LanguageConfig>(jsonStr);
                return config;
            }
            else
            {
                DebugPlus.LogError("LanguageConfig Read Fail,Path="+configPath);
            }
            return null;
        }

        private string GetCurLanguageFilePath(string currentLanguage)
        {
            if (config.LanguageList.Contains(currentLanguage))
            {
                return currentLanguage;
            }
            else
            {
                DebugPlus.LogError("CurrentLanguage Data Error, currentLanguage=" + currentLanguage);
                return string.Empty;
            }
        }

        public string GetTextValue(string key)
        {
            var result = string.Empty;
            TextData.TryGetValue(key, out result);
            if (string.IsNullOrEmpty(result))
            {
                DebugPlus.LogError("Text not Found! ID=" + key);
            }       
            return result;
        }

    }
    public class LanguageConfig
    {
        public string LanguageFilePath;
        public List<string> txtFile;
        public List<string> LanguageList;
    }
}