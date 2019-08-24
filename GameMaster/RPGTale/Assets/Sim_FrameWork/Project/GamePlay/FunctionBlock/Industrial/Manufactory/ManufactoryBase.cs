using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ManufactoryBase : FunctionBlockBase
    {

        public enum ManufactoryType
        {
            Iron,
            Electronic
        }

        public enum ManufactoryInherentLevel
        {
            Elementary,
            Secondary,
            Advanced
        }

        //基础制造速度
        public float ManufacturingspeedBase = 0f;
        //当前制造速度
        private float _currentManuSpeed = 0f;
        public float currentManuSpeed
        {
            get { return _currentManuSpeed; }
            set { _currentManuSpeed = value; }
        }

        public int _currentFormulaID=-1;
        public FormulaData _currentFormulaData = null;


        public Dictionary<int, ushort> InputMaterialDic = new Dictionary<int, ushort>();
        public Dictionary<int, ushort> OutputMaterialDic = new Dictionary<int, ushort>();
        public Dictionary<int, ushort> ByProductMaterialDic = new Dictionary<int, ushort>();


        public override void InitData()
        {
            base.InitData();
            ManufacturingspeedBase = FunctionBlockModule.Instance.GetManufactureSpeed(functionBlockID);
            _currentManuSpeed = ManufacturingspeedBase;
        }

        public void GetCurrentFormulaData()
        {
            _currentFormulaData = FormulaModule.Instance.GetFormulaDataByID(_currentFormulaID);
        }

     
    }

    [System.Serializable]
    public class ManufactoryBaseInfoData
    {
        //BaseInfo
        public Vector3 BlockPos;

        public string BlockUID;
        public int BlockID;

        public int CurrentFormulaID;


    }
}