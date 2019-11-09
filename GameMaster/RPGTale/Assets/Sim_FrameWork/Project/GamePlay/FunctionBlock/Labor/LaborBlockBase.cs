using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class LaborBlockBase : FunctionBlockBase {

        public LaborBlockInfo laborInfo;


    }


    public class LaborBlockInfo
    {
        public FunctionBlock_Labor laborData;
        public LaborBaseInfoData.LaborInherentLevelData inherentLevelData;


        /// <summary>
        /// 当前人口
        /// </summary>
        private float _population=0;
        public float Population { get { return _population; } }
        /// <summary>
        /// 人口上限
        /// </summary>
        private float _populationMax=0;
        public float PopulationMax { get { return _populationMax; } }

        /// <summary>
        /// 食物消耗
        /// </summary>
        private float _foodConsum;
        public float FoodConsum { get { return _foodConsum; } }


        public void InitPopulationMax()
        {
            if (laborData == null)
                return;
        }

        public void AddPopulation(float num)
        {
            _population += num;
            if (_population <= 0)
                _population = 0;
            if (_population > _populationMax)
                _population = _populationMax;
        }
        
        /// <summary>
        /// Refresh Food Consum
        /// </summary>
        public void AddFoodConsum()
        {

        }

        public LaborBlockInfo(FunctionBlock block)
        {
            laborData = FunctionBlockModule.FetchFunctionBlockTypeIndex<FunctionBlock_Labor>(block.FunctionBlockID);
            inherentLevelData = FunctionBlockModule.GetLaborInherentLevelData(laborData);
            AddPopulation(laborData.BasePopulation);
        }


    }

}