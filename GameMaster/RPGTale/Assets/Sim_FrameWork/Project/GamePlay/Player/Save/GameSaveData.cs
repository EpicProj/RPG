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
        public int SaveID;
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

        public GameSaveData(int saveID)
        {
            this.SaveID = saveID;
            playerSaveData = new PlayerSaveData();
            assembleSaveData = new AssembleSaveData();
            technologySaveData = new TechnologySaveData();
        }
    }

    /// <summary>
    /// GeneralData , not Link specific data
    /// </summary>
    public class GameSaveGeneralData
    {
        public int SaveID;
        public string SaveName;
        public string SaveDate;
        public float GameTime;

        public GameSaveGeneralData(int saveID, string saveName, string saveData, float gameTime)
        {
            this.SaveID = saveID;
            this.SaveName = saveName;
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