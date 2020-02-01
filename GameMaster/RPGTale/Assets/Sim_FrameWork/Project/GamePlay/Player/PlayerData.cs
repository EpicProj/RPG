using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class PlayerData 
    {

        public GameHardLevel currentHardLevel { get; protected set; }
        public bool SetHardLevel(GameHardLevel hardLevel)
        {
            if (PlayerModule.GetHardlevelData(hardLevel) == null)
            {
                DebugPlus.LogError("[PlayerData] : Change HardLevel Fail, Config is null!  hardLevel=" + hardLevel);
                return false;
            }
            currentHardLevel = hardLevel;
            return true;
        }

        public List<BuildingPanelData> AllBuildingPanelDataList = new List<BuildingPanelData>();
        public List<BuildingPanelData> UnLockBuildingPanelDataList = new List<BuildingPanelData>();
        public PlayerResourceData resourceData;

        public MaterialStorageData materialStorageData;

        public TimeData timeData;

        public PlayerData() { }
        public bool InitData()
        {
            var config = Config.ConfigData.PlayerConfig;
            if (config == null || config.timeConfig == null)
            {
                DebugPlus.LogError("[PlayerData] : playerConfig is null!");
                return false;
            }

            timeData = new TimeData(config.timeConfig);
            resourceData = new PlayerResourceData();
            if (resourceData.InitData(currentHardLevel) == false)
            {
                DebugPlus.LogError("[PlayerData] ResourceData Init Fail CheckHardLevelConfig!");
                return false;
            }

            materialStorageData = new MaterialStorageData();
            //Init BuildPanel
            AllBuildingPanelDataList =PlayerModule.buildPanelDataList;
            UnLockBuildingPanelDataList = PlayerModule.GetUnLockBuildData();
            return true;
        }
        
        /// <summary>
        /// Game Save
        /// </summary>
        /// <param name="saveData"></param>
        public bool LoadPlayerSaveData(PlayerSaveData saveData)
        {
            resourceData = new PlayerResourceData();
            
            materialStorageData = new MaterialStorageData();
            return resourceData.LoadSave(saveData.playerSaveData_Resource) &&
                materialStorageData.LoadSaveData(saveData.materialSaveData);
        }

        
    }

    public class PlayerResourceData
    {
        #region Currency
        //当前货币
        private int _currency;
        public int Currency { get { return _currency; } protected set { } }

        //Add Currenct
        public void AddCurrency(int num)
        {
            _currency += num;
            if (_currency > _currencyMax)
                _currency = _currencyMax;
        }

        //最大货币储量
        private int _currencyMax;
        public int CurrencyMax { get { return _currencyMax; } protected set { } }

        public ModifierDetailPackage currencyMaxDetailPac = new ModifierDetailPackage();

        public void AddCurrencyMax(ModifierDetailRootType_Simple rootType, int num)
        {
            currencyMaxDetailPac.ValueChange(rootType, num);
            _currencyMax += num;
            if (_currencyMax < 0)
                _currencyMax = 0;
        }

        private int _currencyPerDay;
        public int CurrencyPerDay { get { return _currencyPerDay; } protected set { } }

        public ModifierDetailPackage currencyPerDayDetailPac = new ModifierDetailPackage();
        public void AddCurrencyPerDay(ModifierDetailRootType_Simple rootType, int num)
        {
            currencyPerDayDetailPac.ValueChange(rootType, num);
            _currencyPerDay += num;
        }
        #endregion

        #region Research
        //当前研究点
        private float _research;
        public float Research { get { return _research; } }
        //Add research
        public void AddResearch(float num)
        {
            _research += num;
            if (_research > _researchMax)
                _research = _researchMax;
        }

        //研究点最大值
        private float _researchMax;
        public float ResearchMax { get { return _researchMax; } }

        public ModifierDetailPackage researchMaxDetailPac = new ModifierDetailPackage();
        public void AddResearchMax(ModifierDetailRootType_Simple rootType, float num)
        {
            researchMaxDetailPac.ValueChange(rootType, num);
            _researchMax += num;
            if (_researchMax < 0)
                _researchMax = 0;
        }

        private float _researchPerDay;
        public float ResearchPerDay { get { return _researchPerDay; } }

        public ModifierDetailPackage researchPerDayDetailPac = new ModifierDetailPackage();

        public void AddResearchPerMonth(ModifierDetailRootType_Simple rootType, float num)
        {
            researchPerDayDetailPac.ValueChange(rootType, num);
            _researchPerDay += num;
        }
        #endregion

        #region Energy
        //当前能源
        private float _energy;
        public float Energy { get { return _energy; } }
        //Add Energy
        public void AddEnergy(float num)
        {
            _energy += num;
            if (_energy >= _energyMax)
                _energy = _energyMax;
        }

        //能源最大值
        private float _energyMax;
        public float EnergyMax { get { return _energyMax; } }

        public ModifierDetailPackage energyMaxDetailPac = new ModifierDetailPackage();

        public void AddEnergyMax(ModifierDetailRootType_Simple rootType, float num)
        {
            energyMaxDetailPac.ValueChange(rootType, num);
            _energyMax += num;
            if (_energyMax < 0)
                _energyMax = 0;
        }

        //结算
        private float _energyPerDay;
        public float EnergyPerDay { get { return _energyPerDay; } }

        public ModifierDetailPackage energyPerDayDetailPac = new ModifierDetailPackage();

        public void AddEnergyPerDay(ModifierDetailRootType_Simple rootType, float num)
        {
            energyPerDayDetailPac.ValueChange(rootType, num);
            _energyPerDay += num;
        }

        #endregion

        #region Builder
        private ushort _builder;
        public ushort Builder { get { return _builder; } protected set { } }
        public void AddBuilder(ushort num)
        {
            _builder += num;
            if (_builder >= _builderMax)
                _builder = _builderMax;
        }

        private ushort _builderMax;
        public ushort BuilderMax { get { return _builderMax; } protected set { } }

        public ModifierDetailPackage builderMaxDetailPac = new ModifierDetailPackage();
        public void AddBuilderMax(ModifierDetailRootType_Simple rootType, ushort num)
        {
            builderMaxDetailPac.ValueChange(rootType, num);
            _builderMax += num;
            if (_builderMax < 0)
                _builderMax = 0;
        }

        #endregion

        #region RoCore
        private ushort _roCore;
        public ushort RoCore { get { return _roCore; } protected set { } }
        public void AddRoCore(ushort num)
        {
            _roCore += num;
            if (_roCore >= _roCoreMax)
                _roCore = _roCoreMax;
        }

        private ushort _roCoreMax;
        public ushort RoCoreMax { get { return _roCoreMax; } protected set { } }

        public ModifierDetailPackage roCoreMaxDetailPac = new ModifierDetailPackage();
        public void AddRoCoreMax(ModifierDetailRootType_Simple rootType, ushort num)
        {
            roCoreMaxDetailPac.ValueChange(rootType, num);
            _roCoreMax += num;
            if (_roCoreMax < 0)
                _roCoreMax = 0;
        }

        #endregion
        public PlayerResourceData() { }
        public bool InitData(GameHardLevel hardLevel)
        {
            var data = PlayerModule.GetHardlevelData(hardLevel);
            if (data != null)
            {
                AddCurrencyMax(ModifierDetailRootType_Simple.OriginConfig, data.OriginalCurrencyMax);
                AddCurrency(data.OriginalCurrency);

                AddResearch(data.OriginalResearch);
                AddResearchMax(ModifierDetailRootType_Simple.OriginConfig, data.OriginalResearchMax);

                AddBuilder(data.OriginalBuilder);
                AddBuilderMax(ModifierDetailRootType_Simple.OriginConfig, data.OriginalBuilderMax);

                AddRoCore(data.OriginalRoCore);
                AddRoCoreMax(ModifierDetailRootType_Simple.OriginConfig, data.OriginalRoCoreMax);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Game Save
        /// </summary>
        /// <param name="saveData"></param>
        public bool LoadSave(PlayerSaveData_Resource saveData)
        {
            if (saveData != null)
            {
                _currency = saveData.currentCurrency;
                _currencyMax = saveData.currentCurrencyMax;
                _currencyPerDay = saveData.currencyPerDay;
                currencyPerDayDetailPac = saveData.currencyPerDayDetailPac;
                currencyMaxDetailPac = saveData.currencyMaxDetailPac;

                _research = saveData.currentResearch;
                _researchMax = saveData.currentResearchMax;
                _researchPerDay = saveData.researchPerDay;
                researchMaxDetailPac = saveData.researchMaxDetailPac;
                researchPerDayDetailPac = saveData.researchPerDayDetailPac;

                _energy = saveData.currentEnergy;
                _energyMax = saveData.currentEnergyMax;
                _energyPerDay = saveData.energyPerDay;
                energyMaxDetailPac = saveData.energyMaxDetailPac;
                energyPerDayDetailPac = saveData.energyPerDayDetailPac;

                _roCore = saveData.currentRoCore;
                _roCoreMax = saveData.currentRoCoreMax;
                roCoreMaxDetailPac = saveData.roCoreMaxDetailPac;

                _builder = saveData.currentBuilder;
                _builderMax = saveData.currentBuilderMax;
                builderMaxDetailPac = saveData.builderMaxDetailPac;
                return true;
            }
            DebugPlus.LogError("[PlayerSaveData_Resource] : Save is null!");
            return false;
        }
    }


    #region Save Data
    public class PlayerSaveData_Resource
    {
        public int currentCurrency;
        public int currentCurrencyMax;
        public ModifierDetailPackage currencyMaxDetailPac;
        public int currencyPerDay;
        public ModifierDetailPackage currencyPerDayDetailPac;

        public float currentResearch;
        public float currentResearchMax;
        public ModifierDetailPackage researchMaxDetailPac;
        public float researchPerDay;
        public ModifierDetailPackage researchPerDayDetailPac;

        public float currentEnergy;
        public float currentEnergyMax;
        public ModifierDetailPackage energyMaxDetailPac;
        public float energyPerDay;
        public ModifierDetailPackage energyPerDayDetailPac;

        public ushort currentBuilder;
        public ushort currentBuilderMax;
        public ModifierDetailPackage builderMaxDetailPac;

        public ushort currentRoCore;
        public ushort currentRoCoreMax;
        public ModifierDetailPackage roCoreMaxDetailPac;

        public PlayerSaveData_Resource(PlayerData data)
        {
            currentCurrency = data.resourceData.Currency;
            currentCurrencyMax = data.resourceData.CurrencyMax;
            currencyPerDay = data.resourceData.CurrencyPerDay;
            currencyMaxDetailPac = data.resourceData.currencyMaxDetailPac;
            currencyPerDayDetailPac = data.resourceData.currencyPerDayDetailPac;

            currentResearch = data.resourceData.Research;
            currentResearchMax = data.resourceData.ResearchMax;
            researchPerDay = data.resourceData.ResearchPerDay;
            researchMaxDetailPac = data.resourceData.researchMaxDetailPac;
            researchPerDayDetailPac = data.resourceData.researchPerDayDetailPac;

            currentEnergy = data.resourceData.Energy;
            currentEnergyMax = data.resourceData.EnergyMax;
            energyPerDay = data.resourceData.EnergyPerDay;
            energyMaxDetailPac = data.resourceData.energyMaxDetailPac;
            energyPerDayDetailPac = data.resourceData.energyPerDayDetailPac;

            currentBuilder = data.resourceData.Builder;
            currentBuilderMax = data.resourceData.BuilderMax;
            builderMaxDetailPac = data.resourceData.builderMaxDetailPac;

            currentRoCore = data.resourceData.RoCore;
            currentRoCoreMax = data.resourceData.RoCoreMax;
            roCoreMaxDetailPac = data.resourceData.roCoreMaxDetailPac;
        }
    }
    #endregion

}