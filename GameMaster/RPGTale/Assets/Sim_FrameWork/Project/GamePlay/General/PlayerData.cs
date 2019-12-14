using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class PlayerData 
    {

        public List<BuildingPanelData> AllBuildingPanelDataList = new List<BuildingPanelData>();
        public List<BuildingPanelData> UnLockBuildingPanelDataList = new List<BuildingPanelData>();
        public PlayerResourceData resourceData = new PlayerResourceData();

        public TimeData timeData;
   

        //当前科技转换率
        private float _technologyConversionRate;
        public float TechnologyConversionRate { get { return _technologyConversionRate; } protected set { } }


        public class PlayerResourceData
        {
            #region Currency
            //当前货币
            private int _currency;
            public int Currency { get { return _currency; } protected set { } }
            //最大货币储量
            private int _currencyMax;
            public int CurrencyMax { get { return _currencyMax; } protected set { } }

            private int _currencyPerMonth;
            public int CurrencyPerMonth { get { return _currencyPerMonth; } protected set { } }

            //Add Currenct
            public void AddCurrency(int num)
            {
                _currency += num;
                if (_currency > _currencyMax)
                    _currency = _currencyMax;
            }
            public void AddCurrencyMax(int num)
            {
                _currencyMax += num;
                if (_currencyMax < 0)
                    _currencyMax = 0;
            }
            public void AddCurrencyPerMonth(int num)
            {
                _currencyPerMonth += num;
            }
            #endregion

            #region Research
            //当前研究点
            private float _research;
            public float Research { get { return _research; } }
            //研究点最大值
            private float _researchMax;
            public float ResearchMax { get { return _researchMax; } }
            private float _researchPerMonth;
            public float ResearchPerMonth { get { return _researchMax; } }
            //Add research
            public void AddResearch(float num)
            {
                _research += num;
                if (_research > _researchMax)
                    _research = _researchMax;
            }
            public void AddResearchMax(float num)
            {
                _researchMax += num;
                if (_researchMax < 0)
                    _researchMax = 0;
            }
            public void AddResearchPerMonth(float num)
            {
                _researchPerMonth += num;
            }
            #endregion

            #region Energy
            //当前能源
            private float _energy;
            public float Energy { get { return _energy; } }
            //能源最大值
            private float _energyMax;
            public float EnergyMax { get { return _energyMax; } }
            //月结算
            private float _energyPerMonth;
            public float EnergyPerMonth { get { return _energyPerMonth; } }
            //Add Energy
            public void AddEnergy(float num)
            {
                _energy += num;
                if (_energy >= _energyMax)
                    _energy = _energyMax;
            }
            public void AddEnergyMax(float num)
            {
                _energyMax += num;
                if (_energyMax < 0)
                    _energyMax = 0;
            }
            public void AddEnergyPerMonth(float num)
            {
                _energyPerMonth += num;
            }
            #endregion

            #region Reputation
            //当前信誉
            private int _reputation;
            public int Reputation { get { return _reputation; } }
            //信誉最大值
            private int _reputationMax;
            public int ReputationMax { get { return _reputationMax; } protected set { } }
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
            #endregion

            #region Builder
            private ushort _builder;
            public ushort Builder { get { return _builder; } protected set { } }

            private ushort _builderMax;
            public ushort BuilderMax { get { return _builderMax; } protected set { } }
            public void AddBuilder(ushort num)
            {
                _builder += num;
                if (_builder >= _builderMax)
                    _builder = _builderMax;
            }
            public void AddBuilderMax(ushort num)
            {
                _builderMax += num;
                if (_builderMax < 0)
                    _builderMax = 0;
            }

            #endregion

            #region RoCore
            private ushort _roCore;
            public ushort RoCore { get { return _roCore; } protected set { } }

            private ushort _roCoreMax;
            public ushort RoCoreMax { get { return _roCoreMax; } protected set { } }
            public void AddRoCore(ushort num)
            {
                _roCore += num;
                if (_roCore >= _roCoreMax)
                    _roCore = _roCoreMax;
            }
            public void AddRoCoreMax(ushort num)
            {
                _roCoreMax += num;
                if (_roCoreMax < 0)
                    _roCoreMax = 0;
            }

            #endregion
        }

    }
}