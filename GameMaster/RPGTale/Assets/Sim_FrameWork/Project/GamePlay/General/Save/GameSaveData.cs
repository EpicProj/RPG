using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    [System.Serializable]
    public class GameSaveData
    {
        public int SaveID;
        public string SaveName;
        public string SaveDate;
        public string OrganizatioName;
        public float GameTime;
        public PlayerSaveData playerSaveData;
        public GameStatisticsSaveData gameStatisticsData;

    }

    [System.Serializable]
    public class PlayerSaveData
    {
        public PlayerSaveData_Resource playerSaveData_Resource;


        public class PlayerSaveData_Resource
        {
            public float _currency;
            public float _currency_max;

            public float _labor;
            public float _labor_max;
            public float _labor_per_month;

            public PlayerSaveData_Resource(PlayerData data)
            {
                _currency = data.resourceData.Currency;
                _currency_max = data.resourceData.CurrencyMax;

                _labor = data.resourceData.Labor;
                _labor_max = data.resourceData.LaborMax;
                _labor_per_month = data.resourceData.LaborPerMonth;

            }

        }




    }
}