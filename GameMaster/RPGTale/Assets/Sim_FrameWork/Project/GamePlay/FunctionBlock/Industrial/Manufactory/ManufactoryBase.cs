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

        private int _currentFormulaID=-1;
        public int currentFormulaID
        {
            get { return _currentFormulaID; }
            set { _currentFormulaID = value; }
        }

        public List<Dictionary<Material, ushort>> InputMaterialFormulaList = new List<Dictionary<Material, ushort>>();
        public List<Dictionary<Material, ushort>> OutputMaterialFormulaList = new List<Dictionary<Material, ushort>>();
        public List<Dictionary<Material, ushort>> BypruductMaterialFormulaList = new List<Dictionary<Material, ushort>>();

        public Dictionary<int, ushort> InputMaterialDic = new Dictionary<int, ushort>();
        public Dictionary<int, ushort> OutputMaterialDic = new Dictionary<int, ushort>();
        public Dictionary<int, ushort> ByProductMaterialDic = new Dictionary<int, ushort>();


        public override void InitData()
        {
            base.InitData();
            ManufacturingspeedBase = FunctionBlockModule.Instance.GetManufactureSpeed(functionBlockID);
            _currentManuSpeed = ManufacturingspeedBase;

            SetBlockColliderSize(FunctionBlockModule.Instance.InitFunctionBlockBoxCollider<FunctionBlock_Manufacture>(functionBlock));
            GetFormulaData();
        }

        public void GetFormulaData()
        {
            InputMaterialFormulaList = FunctionBlockModule.Instance.GetFunctionBlockFormulaDataList(functionBlock, FormulaModule.MaterialProductType.Input);
            OutputMaterialFormulaList = FunctionBlockModule.Instance.GetFunctionBlockFormulaDataList(functionBlock, FormulaModule.MaterialProductType.Output);
            BypruductMaterialFormulaList = FunctionBlockModule.Instance.GetFunctionBlockFormulaDataList(functionBlock, FormulaModule.MaterialProductType.Byproduct);
        }

     
    }

}