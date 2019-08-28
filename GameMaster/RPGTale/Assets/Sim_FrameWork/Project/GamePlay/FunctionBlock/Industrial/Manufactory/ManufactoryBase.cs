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

        public FunctionBlock_Manufacture manufactoryData;

        public List<Dictionary<Material, ushort>> InputMaterialFormulaList = new List<Dictionary<Material, ushort>>();
        public List<Dictionary<Material, ushort>> OutputMaterialFormulaList = new List<Dictionary<Material, ushort>>();
        public List<Dictionary<Material, ushort>> BypruductMaterialFormulaList = new List<Dictionary<Material, ushort>>();

        public Dictionary<int, ushort> InputMaterialDic = new Dictionary<int, ushort>();
        public Dictionary<int, ushort> OutputMaterialDic = new Dictionary<int, ushort>();
        public Dictionary<int, ushort> ByProductMaterialDic = new Dictionary<int, ushort>();


        public override void InitData()
        {
            base.InitData();
            GetFormulaData();
            InitFunctionBlock_ManuInfo();
        }

        public FunctionBlockInfoData InitFunctionBlock_ManuInfo()
        {
            manufactoryData = FunctionBlockModule.Instance.FetchFunctionBlockTypeIndex<FunctionBlock_Manufacture>(functionBlock.FunctionBlockID);
            info.AddWorkerNum (int.Parse(manufactoryData.MaintenanceBase));
            info.AddEnergyCostNormal (Utility.TryParseIntList(manufactoryData.EnergyConsumptionBase,',')[0]);
            info.AddEnergyCostMagic  (Utility.TryParseIntList(manufactoryData.EnergyConsumptionBase, ',')[1]);


            info.CurrentSpeed = FunctionBlockModule.Instance.GetManufactureSpeed(functionBlock.FunctionBlockID);
            info.districtAreaMax = FunctionBlockModule.Instance.GetFunctionBlockAreaMax<FunctionBlock_Manufacture>(functionBlock);
            info.currentDistrictDataDic = _currentDistrictDataDic;
            info.BlockEXPMap = FunctionBlockModule.Instance.GetManuBlockEXPMapData(info.block.FunctionBlockID);
            return info;
        }

        public FunctionBlockInfoData GenerateFunctionBlock_ManuInfo()
        {
            FunctionBlockInfoData infoData = new FunctionBlockInfoData();
            return infoData;
        }

        public void GetFormulaData()
        {
            InputMaterialFormulaList = FunctionBlockModule.Instance.GetFunctionBlockFormulaDataList(functionBlock, FormulaModule.MaterialProductType.Input);
            OutputMaterialFormulaList = FunctionBlockModule.Instance.GetFunctionBlockFormulaDataList(functionBlock, FormulaModule.MaterialProductType.Output);
            BypruductMaterialFormulaList = FunctionBlockModule.Instance.GetFunctionBlockFormulaDataList(functionBlock, FormulaModule.MaterialProductType.Byproduct);
        }




     
    }

}