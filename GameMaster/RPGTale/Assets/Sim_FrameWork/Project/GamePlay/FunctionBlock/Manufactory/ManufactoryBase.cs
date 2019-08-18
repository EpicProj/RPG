using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ManufactoryBase : FunctionBlockBase
    {
        //基础制造速度
        public float ManufacturingspeedBase = 0f;
        //当前制造速度
        public float _currentManuSpeed = 0f;

        public int _currentFormulaID=-1;
        public FormulaData _currentFormulaData = null;


        public Dictionary<int, ushort> InputMaterialDic = new Dictionary<int, ushort>();
        public Dictionary<int, ushort> OutputMaterialDic = new Dictionary<int, ushort>();
        public Dictionary<int, ushort> ByProductMaterialDic = new Dictionary<int, ushort>();


        public override void InitData()
        {
            ManufacturingspeedBase = FunctionBlockModule.Instance.GetManufactureSpeed(functionBlockID);
            //InputMaterialDic = FunctionBlockModule.Instance.GetManufactureMaterialList(functionBlockID, FunctionBlockModule.FunctionBlockManuMaterialType.Input);
            //OutputMaterialDic = FunctionBlockModule.Instance.GetManufactureMaterialList(functionBlockID, FunctionBlockModule.FunctionBlockManuMaterialType.Output);
            //ByProductMaterialDic = FunctionBlockModule.Instance.GetManufactureMaterialList(functionBlockID, FunctionBlockModule.FunctionBlockManuMaterialType.Byproduct);
        }

        public void GetCurrentFormulaData()
        {
            _currentFormulaData = FormulaModule.Instance.GetFormulaDataByID(_currentFormulaID);
        }


    }
}