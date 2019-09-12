using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class PlayerData 
    {
        public class WareHouseInfo
        {
            //存储物资
            public List<MaterialStorageData> materialStorageDataList =new List<MaterialStorageData> ();
            //材料分类
            public List<MaterialConfig.MaterialType> materialTagList =new List<MaterialConfig.MaterialType> ();

            public Dictionary<MaterialConfig.MaterialType, List<MaterialConfig.MaterialType.MaterialSubType>> materialSubTagDic = new Dictionary<MaterialConfig.MaterialType, List<MaterialConfig.MaterialType.MaterialSubType>>();

            public MaterialConfig.MaterialType currentSelectMainType;
            public MaterialConfig.MaterialType.MaterialSubType currentSelectSubType;
        }
       

        public List<BuildingPanelData> AllBuildingPanelDataList = new List<BuildingPanelData>();
        public List<BuildingPanelData> UnLockBuildingPanelDataList = new List<BuildingPanelData>();
        public List<BuildMainTag> buildTagList = new List<BuildMainTag>();
        public WareHouseInfo wareHouseInfo = new WareHouseInfo();

        //当前货币
        private float _currency;
        public float Currency { get { return _currency; } protected set { } }
        //最大货币储量
        private float _currencyMax;
        public float CurrencyMax { get { return _currencyMax; } protected set {  } }

        //当前食物
        private int _food;
        public int Food { get { return _food; } protected set { } }
        //食物最大储量
        private int _foodMax;
        public int FoodMax { get { return _foodMax; } protected set { } } 

        //当前劳动力
        private int _labor;
        public int Labor { get { return _labor; } }
        //劳动力最大值
        private int _laborMax;
        public int LaborMax { get { return _laborMax; } }

        //当前信誉
        private int _reputation;
        public int Reputation { get { return _reputation; } }
        //信誉最大值
        private int _reputationMax;
        public int ReputationMax { get { return _reputationMax; } protected set { } }

        //当前科技转换率
        private float _technologyConversionRate;
        public float TechnologyConversionRate { get { return _technologyConversionRate; } protected set {  } }

        //Add Currenct
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

        //Add Food
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

        //Add Labor
        public void AddLabor(int num)
        {
            _labor += num;
            if (_labor > _laborMax)
                _labor = _laborMax;
        }
        public void AddLaborMax(int num)
        {
            _laborMax += num;
            if (_laborMax < 0)
                _laborMax = 0;
        }


        //Add Reputation
        public void AddReputation(int num)
        {
            _reputation += num;
            if (_reputation > _reputationMax)
                _reputation = _reputationMax;
        }
        public void AddReputationMax(int num)
        {
            _reputationMax += num;
            if (_reputationMax < 0)
                _reputationMax = 0;
        }



        public void AddMaterialStoreData(int materialID,ushort count)
        {
            Material ma = MaterialModule.GetMaterialByMaterialID(materialID);
            if (ma == null)
                return;

            var material = wareHouseInfo.materialStorageDataList.Find(x => x.material == ma);
            if (material != null)
            {
                material.count += count;
                if (material.count > ma.BlockCapacity)
                {
                    //超出上限 ,TODO
                    UIManager.Instance.SendMessageToWnd(UIPath.WAREHOURSE_DIALOG,new UIMessage (UIMsgType.UpdateWarehouseData,material));
                }
                else if(material.count <= 0)
                {
                    wareHouseInfo.materialStorageDataList.Remove(material);
                }
                else
                {
                    UIManager.Instance.SendMessageToWnd(UIPath.WAREHOURSE_DIALOG, new UIMessage(UIMsgType.UpdateWarehouseData,material));
                }
               
            }
            else
            {
                MaterialStorageData data = new MaterialStorageData(ma, count);
                wareHouseInfo.materialStorageDataList.Add(data);
                UIManager.Instance.SendMessageToWnd(UIPath.WAREHOURSE_DIALOG, new UIMessage (UIMsgType.UpdateWarehouseData,data));
            }
        }

        public void InitMaterialType()
        {
            List<string> mainTag = MaterialModule.GetAllMainMaterialTypeList();
            if (mainTag != null)
            {
                for(int i = 0; i < mainTag.Count; i++)
                {
                    MaterialConfig.MaterialType type = MaterialModule.GetMaterialTypeData(mainTag[i]);
                    if (type != null)
                    {
                        wareHouseInfo.materialTagList.Add(type);
                    }
                }
            }
        }

        public void InitSubTagType()
        {
            if (wareHouseInfo.materialTagList != null)
            {
                for(int i = 0; i < wareHouseInfo.materialTagList.Count; i++)
                {
                    wareHouseInfo.materialSubTagDic.Add(wareHouseInfo.materialTagList[i], MaterialModule.GetMaterialSubTypeDataList(wareHouseInfo.materialTagList[i].Type));
                }
            }
        }

    }

    public class PlayerDataConfig
    {

    }




}