using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

        protected override void Awake()
        {
            base.Awake();
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
            if (!File.Exists(SaveFilePath))
            {
                Directory.CreateDirectory(SaveFilePath);
            }

            string savePath = SaveFilePath + "/" + gameSave.SaveID;
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
                var dirs = Directory.GetFileSystemEntries(SaveFilePath);
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
        private void LoadSaveByBin()
        {
            string SaveFilePath = Application.persistentDataPath + "Save.sav";
            if (File.Exists(SaveFilePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open(SaveFilePath, FileMode.Open);

                GameSaveData saveData = (GameSaveData)bf.Deserialize(fs);

                fs.Close();

                LoadPlayerSaveData(saveData.playerSaveData);   
            }
        }
        #endregion


        #region  Game Load

        public void LoadPlayerSaveData(PlayerSaveData data)
        {

        }


        #endregion
    }


}