using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Sim_FrameWork
{
    public class GameDataSaveManager : MonoSingleton<GameDataSaveManager>
    {


        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            //For test
            if (Input.GetKeyDown(KeyCode.O))
            {
                SaveByBin();
            }
        }


        #region Data Save
        private GameSaveData Create_gameSaveData()
        {
            GameSaveData gameSave = new GameSaveData();
            
            gameSave.playerSaveData= Create_playerSaveData();

            return gameSave;
        }




        private PlayerSaveData Create_playerSaveData()
        {
            PlayerSaveData saveData = new PlayerSaveData();
            PlayerSaveData.PlayerSaveData_Resource Resdata = new PlayerSaveData.PlayerSaveData_Resource(PlayerManager.Instance.playerData);

            saveData.playerSaveData_Resource = Resdata;

            return saveData;
        }


        #endregion

        #region Save Func

        private void SaveByBin()
        {
            //For Test
            GameSaveData gameSave = Create_gameSaveData();

            string SaveFilePath = Application.persistentDataPath + "Save.sav";

            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Create(SaveFilePath);
            bf.Serialize(stream, gameSave);

            stream.Close();

            if (File.Exists(SaveFilePath))
            {
                Debug.Log("存档成功");
            }
        }


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