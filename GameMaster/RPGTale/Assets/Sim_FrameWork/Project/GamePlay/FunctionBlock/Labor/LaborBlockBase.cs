using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class LaborBlockBase : FunctionBlockBase {

        public LaborBlockInfo laborInfo;

        public override void InitData()
        {
            base.InitData();
        }

    }


    public class LaborBlockInfo
    {

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




        public void AddPopulation(float num)
        {
            _population += num;
            if (_population <= 0)
                _population = 0;
            if (_population > _populationMax)
                _population = _populationMax;
        }


        public LaborBlockInfo()
        {

        }


    }

}