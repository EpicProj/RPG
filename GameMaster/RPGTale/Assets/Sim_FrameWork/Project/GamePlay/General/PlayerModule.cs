using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sim_FrameWork
{
    public class PlayerModule :Singleton<PlayerModule>
    {
        public enum HardLevel
        {
            easy=1<<0,
            normal=1<<1,
            hard=1<<2
        }


        public BaseResourcesData resourceData;
        public PlayerConfig config;
        public PlayerData playerData;

        public HardLevel currentHardLevel = HardLevel.easy;

        private bool HasInit = false;

        public void InitData()
        {
            if(HasInit)
                return;
            //resourceData = new BaseResourcesData();
            //resourceData.ReadData();
            config = new PlayerConfig();
            config.ReadPlayerConfigData();
            HasInit = true;
        }


        
        public PlayerData InitPlayerData()
        {
            HardLevelData data = GetHardlevelData(HardLevel.easy);
            playerData = new PlayerData();
            //Init Food
            playerData.AddFoodMax(data.OriginalFoodMax);
            playerData.AddFood(data.OriginalFood);
            //Init Currency
            playerData.AddCurrencyMax(data.OriginalCurrencyMax);
            playerData.AddCurrency(data.OriginalCurrency);

            playerData.AddReputation(data.OriginalReputation);
            return playerData;

        }

        public void AddCurrency(float num)
        {
            playerData.AddCurrency(num);
            UIManager.Instance.SendMessageToWnd(UIPath.MAINMENU_PAGE, "UpdateResourceData", playerData);
        }


        public void AddMaterialData(int id,ushort count)
        {
            playerData.AddMaterialStoreData(id, count);
            UIManager.Instance.SendMessageToWnd(UIPath.WAREHOURSE_DIALOG, "UpdateWarehouseData", playerData.materialStorageDataList);
        }



        public HardLevelData GetHardlevelData(HardLevel level)
        {
            if (config.hardlevelData.Count == 0)
            {
                Debug.LogError("HardlevelData is null");
                return null;
            }
            switch (level)
            {
                case HardLevel.easy:
                    return config.hardlevelData[0];
                case HardLevel.normal:
                    return config.hardlevelData[1];
                case HardLevel.hard:
                    return config.hardlevelData[2];
                default:
                    Debug.LogError("HardLevelMode Error");
                    return config.hardlevelData[0];
            }
        }

    }


    public class PlayerConfig
    {
        public List<HardLevelData> hardlevelData;
        public TimeDataConfig timeConfig;

        public void ReadPlayerConfigData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            PlayerConfig config = reader.LoadPlayerConfig();
            hardlevelData = config.hardlevelData;
            timeConfig = config.timeConfig;
        }
    }


    public class HardLevelData
    {
        public string HardName;
        //初始货币
        public float OriginalCurrency;
        public float OriginalCurrencyMax;
        //初始食物
        public int OriginalFood;
        public int OriginalFoodMax;
        //初始能量
        public int OriginalEnergy;
        //初始劳动力
        public int OriginalLaber;
        //初始信誉
        public int OriginalReputation;

        //初始科技转化率
        public float TechnologyConversionRate;
    }
}