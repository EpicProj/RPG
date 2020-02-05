using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

namespace Sim_FrameWork
{
    public class GameDataSaveManager : Singleton<GameDataSaveManager>
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
        //最大存档组数量
        private int max_save_group_Num;
        //单组最大存档数量
        private int max_saveIndex_perGroup_num;

        private List<GameSaveDataItem> AllSaveDataList;

        public int currentSaveIndex;
        public int currentSaveGroupID;

        public string currentSaveName = "Savasdafiujkw";

        public void InitData()
        {
            max_save_group_Num = Config.ConfigData.GlobalSetting.GameSaveData_MaxGroup_Num;
            max_saveIndex_perGroup_num = Config.ConfigData.GlobalSetting.GameSaveData_MaxSaveNum_PerGroup;

            AllSaveDataList = new List<GameSaveDataItem>();
        }


        public void ClearSaveCache()
        {
            AllSaveDataList.Clear();
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
        private GameSaveDataItem Create_gameSaveData_GroupNav(int groupID,string saveName)
        {
            GameSaveDataItem groupData = new GameSaveDataItem(
                groupID,
                saveName,
                GetGameDataCountInGroup(saveName) + 1,
                GetCurrentTime(),
                1000);
            return groupData;
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
            sav = SaveEncrypt.Encrypt(sav);
            StreamWriter sw = File.CreateText(fileName);
            sw.Write(sav);
            sw.Close();
            //TODO 密钥生成写外部方法，用机器码
        }

        private static object GetData(string fileName,Type type)
        {
            StreamReader sr = File.OpenText(fileName);
            string data = sr.ReadToEnd();

            data = SaveEncrypt.Decrypt(data);
            sr.Close();
            if (data != string.Empty)
            {
                return JsonConvert.DeserializeObject(data, type);
            }
            else
            {
                DebugPlus.LogError("[GameSaveData] : GetData Error!");
                return null;
            }
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

        public void SaveGameFile()
        {
            SaveGameFile(GetGameDataGroupCount() + 1,
                currentSaveName,
                GetGameDataCountInGroup(currentSaveName) + 1);
        }

        /// <summary>
        /// Save Data 
        /// </summary>
        public void SaveGameFile(int groupID,string saveName, int indexID)
        {
            GameSaveData gameSave = new GameSaveData(groupID, indexID);

            GameSaveDataItem saveItem = Create_gameSaveData_GroupNav(groupID,saveName);

            string SaveFilePath = Application.persistentDataPath + "/SaveData";
            if (!Directory.Exists(SaveFilePath))
            {
                Directory.CreateDirectory(SaveFilePath);
            }

            ///Crete Save Group
            DirectoryInfo info = new DirectoryInfo(SaveFilePath);
            string saveGroupFilePath = SaveFilePath + "/Sav_" + saveName;
            if (!Directory.Exists(saveGroupFilePath))
            {
                Directory.CreateDirectory(saveGroupFilePath);
            }

            string savePath = saveGroupFilePath + "/" + saveName + "_" + indexID + ".sav";
            string savePathNav = saveGroupFilePath + "/" + saveName + "_" + indexID + ".nav";

            SaveData(savePath, gameSave);
            SaveData(savePathNav, saveItem);

            if (File.Exists(savePath) && File.Exists(savePathNav))
            {
                Debug.Log("Save Success");
            }
        }

        /// <summary>
        /// 获取存档数量
        /// </summary>
        /// <returns></returns>
        int GetGameDataCountInGroup(string groupName)
        {
            string SaveFilePath = Application.persistentDataPath + "/SaveData/"+groupName;
            if (File.Exists(SaveFilePath))
            {
                DirectoryInfo info = new DirectoryInfo(SaveFilePath);
                var files = info.GetDirectories();
                return files.Length;
            }
            else
            {
                DebugPlus.Log("Save GroupFile not Exist! path=" + SaveFilePath);
            }
            return 0;
        }

        int GetGameDataGroupCount()
        {
            string saveFilePath= Application.persistentDataPath + "/SaveData";
            if (File.Exists(saveFilePath))
            {
                DirectoryInfo info = new DirectoryInfo(saveFilePath);
                var file = info.GetDirectories();
                return file.Length;
            }
            else
            {
                DebugPlus.Log("Save File not Exist! path=" + saveFilePath);
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

        private GameSaveDataItem LoadSaveNavigator(string savePath)
        {
            GameSaveDataItem result = null;
            if (File.Exists(savePath))
            {
                try
                {
                    result = (GameSaveDataItem)GetData(savePath, typeof(GameSaveDataItem));
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
        public List<GameSaveDataItem> RefreshCurrentSaveDataList()
        {
            AllSaveDataList.Clear();
            string SaveFilePath = Application.persistentDataPath + "/SaveData";
            if (Directory.Exists(SaveFilePath))
            {
                DirectoryInfo dirs = new DirectoryInfo(SaveFilePath);
                var saves = dirs.GetFiles("*.nav", SearchOption.AllDirectories);

                DebugPlus.Log("[GameSaveData] : Save File Length=" + saves.Length);

                for(int i = 0; i < saves.Length; i++)
                {
                    try
                    {
                        var savePath = saves[i].FullName;
                        var data = LoadSaveNavigator(savePath);
                        if (data != null)
                        {
                            AllSaveDataList.Add(data);
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
            return AllSaveDataList;
        }

        public GameSaveDataItem GetSaveNavigatorData(int groupID, int saveIndex)
        {
            for(int i = 0; i < AllSaveDataList.Count; i++)
            {
                if (AllSaveDataList[i].groupID == groupID && AllSaveDataList[i].Index == saveIndex)
                    return AllSaveDataList[i];
            }
            return null;
        }
        /// <summary>
        /// 获取同组所有存档引导
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<GameSaveDataItem> GetSaveNavigatorDataList(int groupID)
        {
            List<GameSaveDataItem> result = new List<GameSaveDataItem>();
            for (int i = 0; i < AllSaveDataList.Count; i++)
            {
                if (AllSaveDataList[i].groupID == groupID)
                    result.Add(AllSaveDataList[i]);
            }
            return result;
        }
        /// <summary>
        /// 获取同组最新存档
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public GameSaveDataItem GetLatestSaveData(int groupID)
        {

            var saveList = GetSaveNavigatorDataList(groupID);
            if (saveList.Count == 0)
                return null;

            for(int i = 0; i < saveList.Count -1; i++)
            {
                for(int j = 0; j < saveList.Count - 1 - i; j++)
                {
                     if(Utility.CompareDate(saveList[j].SaveDate, saveList[j + 1].SaveDate) > 0)
                    {
                        // t1>t2
                        GameSaveDataItem temp = saveList[j];
                        saveList[j] = saveList[j + 1];
                        saveList[j + 1] = temp;
                    }
                }
            }
            return saveList[0];
        }

        /// <summary>
        /// 获取存档
        /// </summary>
        /// <param name="saveID"></param>
        /// <returns></returns>
        private GameSaveData GetSaveDataBySaveID(string groupName, int groupID , int saveIndex)
        {
            GameSaveDataItem data = GetSaveNavigatorData(groupID, saveIndex);
            if (data == null)
            {
                DebugPlus.LogError("Save Not Exists! SaveID=  " + data.saveName+"_"+ data.Index);
                return null;
            }
            else
            {
                string SaveFilePath = Application.persistentDataPath + "/SaveData/Sav_"+groupName;
                if (Directory.Exists(SaveFilePath))
                {
                    DirectoryInfo dirs = new DirectoryInfo(SaveFilePath);
                    var files = dirs.GetDirectories();
                    DebugPlus.Log("Save num=" + files.Length);

                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i].FullName == groupName + "_" + saveIndex +  ".sav")
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

        private GameSaveData GetSaveData(string groupName,int groupID, int saveIndex)
        {
            var savedata = GetSaveDataBySaveID(groupName,groupID,saveIndex);
            if (savedata == null)
            {
                Debug.LogError("Save Error!");
            }
            return savedata;
        }

        public List<List<BaseDataModel>> GetSaveGroupModel()
        {
            RefreshCurrentSaveDataList();
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();
            List<int> groupID = new List<int>();
            for (int i = 0; i < AllSaveDataList.Count; i++)
            {
                if (!groupID.Contains(AllSaveDataList[i].groupID))
                {
                    groupID.Add(AllSaveDataList[i].groupID);
                }
            }

            for(int i = 0; i < groupID.Count; i++)
            {
                SaveDataModel model = new SaveDataModel();
                if (model.Create(AllSaveDataList[i].groupID))
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