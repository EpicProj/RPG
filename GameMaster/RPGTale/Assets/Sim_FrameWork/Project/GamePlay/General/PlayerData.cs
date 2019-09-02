using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class PlayerData 
    {

        public List<MaterialStorageData> materialStorageDataList =new List<MaterialStorageData> ();

        //当前货币
        private float _currency;
        public float Currency { get { return _currency; } protected set { } }
        //最大货币储量
        private float _currencyMax;
        public float CurrencyMax { get { return _currency; } set { _currencyMax = value; } }

        //当前食物
        private int _food;
        public int Food { get { return _food; } protected set { } }
        //食物最大储量
        private int _foodMax;
        public int FoodMax { get { return _foodMax; } set { _foodMax = value; } }

        //当前劳动力
        private int _labor;
        public int Labor { get { return _labor; } }

        //当前信誉
        private int _reputation;
        public int Reputation { get { return _reputation; } }
        //信誉最大值
        private int _reputationMax;
        public int ReputationMax { get { return _reputationMax; } set { _reputationMax = value; } }

        //当前科技转换率
        private float technologyConversionRate;
        public float TechnologyConversionRate { get { return technologyConversionRate; } set { technologyConversionRate = value; } }

        public void AddCurrency(float num)
        {
            _currency += num;
            if (_currency > _currencyMax)
                _currency = _currencyMax;
        }
        public void AddCurrencyMax(float num)
        {
            _currencyMax += num;
            if (_currencyMax < 0)
                _currencyMax = 0;
        }

        public void AddFood(int num)
        {
            _food += num;
            if (_food > _foodMax)
                _food = _foodMax;
        }
        public void AddFoodMax(int num)
        {
            _foodMax += num;
            if (_foodMax < 0)
                _foodMax = 0;
        }

        public void AddReputation(int num)
        {
            _reputation += num;
            if (_reputation > _reputationMax)
                _reputation = _reputationMax;
        }

        public void AddMaterialStoreData(int materialID,ushort count)
        {
            Material ma = MaterialModule.Instance.GetMaterialByMaterialID(materialID);
            if (ma == null)
                return;
            if (materialStorageDataList.Count == 0 && count>0)
            {
                materialStorageDataList.Add(new MaterialStorageData(ma, count));
            }
            for (int i = 0; i < materialStorageDataList.Count; i++)
            {
                if (materialStorageDataList[i].material == ma)
                {
                    materialStorageDataList[i].count += count;
                    if (materialStorageDataList[i].count <= 0)
                    {
                        materialStorageDataList.Remove(materialStorageDataList[i]);
                    }
                }
                else
                {
                    materialStorageDataList.Add(new MaterialStorageData(ma, count));
                }
            }

        }


    }

    public class PlayerDataConfig
    {

    }
}