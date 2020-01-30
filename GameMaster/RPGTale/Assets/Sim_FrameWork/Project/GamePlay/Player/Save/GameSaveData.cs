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
        public PlayerSaveData playerSaveData;
        public GameStatisticsSaveData gameStatisticsData;

        public AssembleSaveData assembleSaveData;

        public GameSaveData(int saveID)
        {
            this.SaveID = saveID;
            playerSaveData = new PlayerSaveData();
            assembleSaveData = new AssembleSaveData();
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


        public PlayerSaveData()
        {
            playerSaveData_Resource = new PlayerSaveData.PlayerSaveData_Resource(PlayerManager.Instance.playerData);
        }


        public class PlayerSaveData_Resource
        {
            public int _currency;
            public int _currency_max;

            public float _research;
            public float _research_max;
            public float _research_per_month;

            public PlayerSaveData_Resource(PlayerData data)
            {
                _currency = data.resourceData.Currency;
                _currency_max = data.resourceData.CurrencyMax;

                _research = data.resourceData.Research;
                _research_max = data.resourceData.ResearchMax;
                _research_per_month = data.resourceData.ResearchPerMonth;
            }
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