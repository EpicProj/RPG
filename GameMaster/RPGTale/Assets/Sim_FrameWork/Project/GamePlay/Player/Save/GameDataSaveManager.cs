using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

namespace Sim_FrameWork
{
    public class GameDataSaveManager : MonoSingleton<GameDataSaveManager>
    {
        public enum SaveState
        {
            /// <summary>
            /// 新存档
            /// </summary>
            NewSave,
            /// <summary>
            /// 覆盖存档
            /// </summary>
            CoverSave
        }

        private int currentSaveNum;
        private int maxSaveNum;
        private List<GameSaveGeneralData> AllSaveDataGeneralList;

        public int currentSaveID;
        private GameSaveData _currentSaveData;
        public GameSaveData currentSaveData
        {
            get
            {
                if (currentSaveID == 0)
                    return null;
                if (_currentSaveData != null)
                {
                    if(_currentSaveData.SaveID != currentSaveID)
                        _currentSaveData = GetSaveData(currentSaveID);
                }
                else
                {
                    _currentSaveData = GetSaveData(currentSaveID);
                }
                return _currentSaveData;
            }
        }

        public void InitCurrentSaveData(int saveID)
        {
            currentSaveID = saveID;
        }

        public void ClearSaveCache()
        {
            _currentSaveData = null;
            AllSaveDataGeneralList.Clear();
        }


        protected override void Awake()
        {
            base.Awake();
            AllSaveDataGeneralList = new List<GameSaveGeneralData>();
        }

        void Start()
        {
            currentSaveNum = GetFileCount();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                //For Test
                SaveGameFile();
            }
        }

        /// <summary>
        /// Load Save Data
        /// </summary>
        public void LoadAllSave()
        {
            PlayerManager.Instance.LoadGameSaveData();
            TechnologyDataManager.Instance.LoadTechSaveData();
        }



        #region Data Save
        private GameSaveGeneralData Create_gameSaveData_Nav()
        {
            GameSaveGeneralData generalData = new GameSaveGeneralData(
                currentSaveNum + 1,
                "Test",
                GetCurrentTime(),
                1000);
            return generalData;
        }


        #endregion

        #region Save Func

        /// <summary>
        ///  Data Save
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="obj"></param>
        private static void SaveData(string fileName,object obj)
        {
            string sav = JsonConvert.SerializeObject(obj);
            ///32 Encrypt
            sav = SaveEncrypt.Encrypt(sav, "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            StreamWriter sw = File.CreateText(fileName);
            sw.Write(sav);
            sw.Close();
            //TODO 密钥生成写外部方法，用机器码
        }

        private static object GetData(string fileName,Type type)
        {
            StreamReader sr = File.OpenText(fileName);
            string data = sr.ReadToEnd();

            data = SaveEncrypt.Decrypt(data, "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            sr.Close();
            return JsonConvert.DeserializeObject(data,type);
        }

        private static string SerialieObject(object obj)
        {
            string sString = string.Empty;
            sString = JsonConvert.SerializeObject(obj);
            return sString;
        }

        private static object DeserializeObject(string str,Type type)
        {
            object dObj = null;
            dObj = JsonConvert.DeserializeObject(str,type);
            return dObj;
        }


        /// <summary>
        /// Save Data By Bin
        /// </summary>
        public void SaveGameFile()
        {
            //For Test
            GameSaveData gameSave = new GameSaveData(currentSaveNum + 1);
            GameSaveGeneralData gameSaveNav = Create_gameSaveData_Nav();


            string SaveFilePath = Application.persistentDataPath + "/SaveData";
            if (!Directory.Exists(SaveFilePath))
            {
                Directory.CreateDirectory(SaveFilePath);
            }

            string savePath = SaveFilePath + "/" + gameSave.SaveID+".sav";
            string saveNavigatorPath=SaveFilePath+"/"+gameSave.SaveID+"nav";

            SaveData(savePath, gameSave);
            SaveData(saveNavigatorPath, gameSaveNav);

            if (File.Exists(savePath) && File.Exists(saveNavigatorPath))
            {
                Debug.Log("Save Success");
            }
        }

        /// <summary>
        /// 获取存档数量
        /// </summary>
        /// <returns></returns>
        int GetFileCount()
        {
            string SaveFilePath = Application.persistentDataPath + "/SaveData";
            if (File.Exists(SaveFilePath))
            {
                var dirs = Directory.GetFileSystemEntries(SaveFilePath,"*.sav");
                return dirs.Length;
            }
            return 0;
        }

        string GetCurrentTime()
        {
            ///Local Time
            return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// Load Save 
        /// </summary>
        private GameSaveData LoadSave(string savePath)
        {
            GameSaveData result = null;
            if (File.Exists(savePath))
            {
                try
                {
                    result =(GameSaveData)GetData(savePath, typeof(GameSaveData));
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            return result;
        }

        private GameSaveGeneralData LoadSaveNavigator(string savePath)
        {
            GameSaveGeneralData result = null;
            if (File.Exists(savePath))
            {
                try
                {
                    result = (GameSaveGeneralData)GetData(savePath, typeof(GameSaveGeneralData));
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            return result;
        }
        #endregion


        #region  Game Load
        public List<GameSaveGeneralData> RefreshCurrentSaveDataList()
        {
            AllSaveDataGeneralList.Clear();
            string SaveFilePath = Application.persistentDataPath + "/SaveData";
            if (Directory.Exists(SaveFilePath))
            {
                DirectoryInfo dirs = new DirectoryInfo(SaveFilePath);
                var files = dirs.GetFiles("*.nav", SearchOption.AllDirectories);

                Debug.Log("Save File Length=" + files.Length);

                for(int i = 0; i < files.Length; i++)
                {
                    try
                    {
                        var savePath = files[i].FullName;
                        var data = LoadSaveNavigator(savePath);
                        if (data != null)
                        {
                            AllSaveDataGeneralList.Add(data);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        continue;
                    }
                }
            }
            else
            {
                Debug.Log("Save Path not Exits!  Path=" + SaveFilePath);
            }
            return AllSaveDataGeneralList;
        }

        public GameSaveGeneralData GetSaveNavigatorData(int saveID)
        {
            var data = AllSaveDataGeneralList.Find(x => x.SaveID == saveID);
            if (data == null)
            {
                Debug.LogError("Save Not exist!");
            }
            return data;
        }

        /// <summary>
        /// 获取存档
        /// </summary>
        /// <param name="saveID"></param>
        /// <returns></returns>
        private GameSaveData GetSaveDataBySaveID(int saveID)
        {
            GameSaveGeneralData data = AllSaveDataGeneralList.Find(x => x.SaveID == saveID);
            if (data == null)
            {
                Debug.LogError("Save Not Exists! SaveID=  " + saveID);
                return null;
            }
            else
            {
                string SaveFilePath = Application.persistentDataPath + "/SaveData";
                if (Directory.Exists(SaveFilePath))
                {
                    DirectoryInfo dirs = new DirectoryInfo(SaveFilePath);
                    var files = dirs.GetFiles("*.sav", SearchOption.AllDirectories);

                    Debug.Log("Save File Length=" + files.Length);

                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i].FullName == saveID + ".sav")
                        {
                            try
                            {
                                var savePath = files[i].FullName;
                                var saveData = LoadSave(savePath);
                                if (saveData != null)
                                    return saveData;
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e);
                                return null;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private GameSaveData GetSaveData(int saveID)
        {
            var savedata = GetSaveDataBySaveID(saveID);
            if (savedata == null)
            {
                Debug.LogError("Save Error!");
            }
            return savedata;
        }

        public List<List<BaseDataModel>> GetSaveModel()
        {
            RefreshCurrentSaveDataList();
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();
            for(int i = 0; i < AllSaveDataGeneralList.Count; i++)
            {
                SaveDataModel model = new SaveDataModel();
                if(model.Create(AllSaveDataGeneralList[i].SaveID))
                {
                    List<BaseDataModel> list = new List<BaseDataModel>();
                    list.Add((BaseDataModel)model);
                    result.Add(list);
                }    
            }
            return result;
        }

        #endregion
    }


}