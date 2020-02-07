using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Game Save Data
 * SOMA
 */
namespace Sim_FrameWork
{
    /// <summary>
    /// Main Game Save Data
    /// </summary>
    public class GameSaveData
    {
        public int GroupID;
        public int SaveIndex;
        /// <summary>
        /// PlayerSave
        /// Resource
        /// </summary>
        public PlayerSaveData playerSaveData;
        public GameStatisticsSaveData gameStatisticsData;

        public MainShipSaveData mainShipSaveData;
        /// <summary>
        /// AssembleSave  preset & currentParts
        /// </summary>
        public AssembleSaveData assembleSaveData;
        /// <summary>
        /// Technology Save
        /// </summary>
        public TechnologySaveData technologySaveData;

        public GameSaveData() { }
        public static GameSaveData CreateSave(int groupID,int saveIndex)
        {
            GameSaveData saveData = new GameSaveData();
            saveData.GroupID = groupID;
            saveData.SaveIndex = saveIndex;
            saveData.playerSaveData = PlayerSaveData.CreateSave();

            saveData.mainShipSaveData =  MainShipSaveData.CreateSave();

            saveData.assembleSaveData = AssembleSaveData.CreateSave();
            saveData.technologySaveData = TechnologySaveData.CreateSave();
            return saveData;
        }
    }

    /// <summary>
    /// GeneralData , not Link specific data
    /// </summary>
    public class GameSaveDataItem
    {
        public int groupID;
        public string saveName;
        public int Index;
        public string SaveDate;
        public float GameTime;

        public GameSaveDataItem(int groupID, string saveName, int index, string saveData, float gameTime)
        {
            this.groupID = groupID;
            this.saveName = saveName;
            this.Index = index;
            this.SaveDate = saveData;
            this.GameTime = gameTime;
        }
    }

    public class PlayerSaveData
    {
        public PlayerSaveData_Resource playerSaveData_Resource;
        public MaterialStorageSaveData materialSaveData;
        public TimeDataSave timeSave;

        public static PlayerSaveData CreateSave()
        {
            PlayerSaveData data = new PlayerSaveData();
            data.playerSaveData_Resource = PlayerSaveData_Resource.CreateSaveData();

            data.materialSaveData = MaterialStorageSaveData.CreateSave();
            ///Save Game Time
            data.timeSave = TimeDataSave.CreateSave();
            return data;
        }
    }

    public class AssembleSaveData
    {
        public AssemblePartGeneralSaveData partSaveData;
        public AssembleShipGeneralSaveData shipSaveData;

        public static AssembleSaveData CreateSave()
        {
            AssembleSaveData data = new AssembleSaveData();
            data.partSaveData = AssemblePartGeneralSaveData.CreateSave();
            data.shipSaveData = AssembleShipGeneralSaveData.CreateSave();
            return data;
        }
    }
}