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

        /// <summary>
        /// AssembleSave  preset & currentParts
        /// </summary>
        public AssembleSaveData assembleSaveData;
        /// <summary>
        /// Technology Save
        /// </summary>
        public TechnologySaveData technologySaveData;

        public GameSaveData(int groupID,int saveIndex)
        {
            this.GroupID = groupID;
            this.SaveIndex = saveIndex;
            playerSaveData = new PlayerSaveData();
            assembleSaveData = new AssembleSaveData();
            technologySaveData = new TechnologySaveData();
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

        public PlayerSaveData()
        {
            playerSaveData_Resource = new PlayerSaveData_Resource(PlayerManager.Instance.playerData);
            materialSaveData = new MaterialStorageSaveData();
        }
    }

    public class AssembleSaveData
    {
        public AssemblePartGeneralSaveData partSaveData;
        public AssembleShipGeneralSaveData shipSaveData;

        public AssembleSaveData()
        {
            partSaveData = new AssemblePartGeneralSaveData();
            shipSaveData = new AssembleShipGeneralSaveData();
        }
    }
}