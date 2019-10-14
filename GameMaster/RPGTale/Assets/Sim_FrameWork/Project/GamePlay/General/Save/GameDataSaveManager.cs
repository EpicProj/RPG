using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

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
        private List<GameSaveData> AllSaveDataList;

        protected override void Awake()
        {
            base.Awake();
            AllSaveDataList = new List<GameSaveData>();
        }

        void Start()
        {
            currentSaveNum = GetFileCount();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.O))
            {
                //For Test
                SaveByBin();
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache()
        {
            AllSaveDataList = null;
        }

        #region Data Save
        private GameSaveData Create_gameSaveData()
        {
            GameSaveData gameSave = new GameSaveData();
            gameSave.SaveID = currentSaveNum + 1;
            gameSave.SaveName = "Test";
            gameSave.SaveDate = GetCurrentTime();

            //gameSave.playerSaveData= Create_playerSaveData();
            //gameSave.gameStatisticsData = Create_GameStatisticsSaveData();

            return gameSave;
        }

        private PlayerSaveData Create_playerSaveData()
        {
            PlayerSaveData saveData = new PlayerSaveData();
            PlayerSaveData.PlayerSaveData_Resource Resdata = new PlayerSaveData.PlayerSaveData_Resource(PlayerManager.Instance.playerData);

            saveData.playerSaveData_Resource = Resdata;

            return saveData;
        }

        private GameStatisticsSaveData Create_GameStatisticsSaveData()
        {
            GameStatisticsSaveData saveData = new GameStatisticsSaveData();
            return saveData;
        }

        #endregion

        #region Save Func
        /// <summary>
        /// Save Data By Bin
        /// </summary>
        public void SaveByBin()
        {
            //For Test
            GameSaveData gameSave = Create_gameSaveData();

            string SaveFilePath = Application.persistentDataPath + "/SaveData";
            if (!Directory.Exists(SaveFilePath))
            {
                Directory.CreateDirectory(SaveFilePath);
            }

            string savePath = SaveFilePath + "/" + gameSave.SaveID+".sav";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Create(savePath);
            bf.Serialize(stream, gameSave);
            stream.Close();

            if (File.Exists(savePath))
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
        /// Load Save By Bin
        /// </summary>
        private GameSaveData LoadSaveByBin(string savePath)
        {
            GameSaveData result = null;
            if (File.Exists(savePath))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = File.Open(savePath, FileMode.Open);
                    result = (GameSaveData)bf.Deserialize(fs);

                    fs.Close();

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
        public List<GameSaveData> RefreshCurrentSaveDataList()
        {
            AllSaveDataList.Clear();
            string SaveFilePath = Application.persistentDataPath + "/SaveData";
            if (Directory.Exists(SaveFilePath))
            {
                DirectoryInfo dirs = new DirectoryInfo(SaveFilePath);
                var files = dirs.GetFiles("*.sav", SearchOption.AllDirectories);

                Debug.Log("Save File Length=" + files.Length);
                for(int i = 0; i < files.Length; i++)
                {
                    try
                    {
                        var savePath = files[i].FullName;
                        var data = LoadSaveByBin(savePath);
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

        /// <summary>
        /// 获取存档
        /// </summary>
        /// <param name="saveID"></param>
        /// <returns></returns>
        public GameSaveData GetSaveDataBySaveID(int saveID)
        {
            GameSaveData data = AllSaveDataList.Find(x => x.SaveID == saveID);
            if (data == null)
                Debug.LogError("Save Not Exists! SaveID=  " + saveID);
            return data;
        }

        void LoadPlayerSaveData(PlayerSaveData data)
        {

        }

        public List<List<BaseDataModel>> GetSaveModel()
        {
            RefreshCurrentSaveDataList();
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();
            for(int i = 0; i < AllSaveDataList.Count; i++)
            {
                SaveDataModel model = new SaveDataModel();
                if(model.Create(AllSaveDataList[i].SaveID))
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