using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class BaseResourcesData
    {
        //货币
        public string CurrencyName;
        public Sprite CurrencyIcon;
        //食物
        public string FoodName;
        public Sprite FoodIconPath;
        //能源
        public string EnergyName;
        public Sprite EnergyIcon;
        //劳动力
        public string LaborName;
        public Sprite LaborIcon;

        public void ReadData()
        {
            Config.JsonReader reader= new Config.JsonReader();
            BaseResourcesConfig config = reader.LoadBaseResourcesConfig();
            CurrencyName = config.CurrencyName;
            CurrencyIcon = Utility.LoadSprite(config.CurrencyIconPath);
            
        }


    }
    public class BaseResourcesConfig
    {
        //货币
        public string CurrencyName;
        public string CurrencyIconPath;
        //食物
        public string FoodName;
        public string FoodIconPath;
        //能源
        public string EnergyName;
        public string EnergyIconPath;
        //劳动力
        public string LaborName;
        public string LaborIconPath;
        //信誉值
        public string ReputationName;
        public string ReputationIconPath;

    }
}