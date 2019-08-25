using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlock_Smelt : ManufactoryBase
    {
        public List<BlockLevelData> leveldata;
        private string InputDesc;
        private string InputIconPath;
        private string OutputDesc;
        private string OutputIconPath;
        private string ByproductDesc;
        private string ByproductIconPath;

        //Factory

        public override void Awake()
        {
            functionBlockID = 100;
            base.Awake();
           

        }

        public override void Update()
        {
            CheckMouseButtonDown(UIPath.FUCNTIONBLOCK_INFO_DIALOG, GenerateFunctionBlock_SmeltInfo());
        }
        public override void InitData()
        {
            base.InitData();
            Config.ManufactoryConfigReader reader= new Config.ManufactoryConfigReader();
            FunctionBlock_Smelt_Config config = new FunctionBlock_Smelt_Config();
            leveldata = config.leveldata;
            InputDesc = config.InputDesc;
            InputIconPath = config.InputIconPath;
            OutputDesc = config.OutputDesc;
            OutputIconPath = config.OutputIconPath;
            ByproductDesc = config.ByproductDesc;
            ByproductIconPath = config.ByproductIconPath;

        }

        public void Product()
        {

        }


        public override void OnPlaceFunctionBlock()
        {
            base.OnPlaceFunctionBlock();
        }

        private FunctionBlock_Info GenerateFunctionBlock_SmeltInfo()
        {
            FunctionBlock_Info info = new FunctionBlock_Info();
            info.block = functionBlock;
            info.districtAreaMax = FunctionBlockModule.Instance.GetFunctionBlockAreaMax<FunctionBlock_Manufacture>(functionBlock);
            info.currentDistrictDataDic = _currentDistrictDataDic;
            return info;
        }


    }

    public class FunctionBlock_Smelt_Config
    {
        public List<BlockLevelData> leveldata;
        public string InputDesc;
        public string InputIconPath;
        public string OutputDesc;
        public string OutputIconPath;
        public string ByproductDesc;
        public string ByproductIconPath;
    }

    public class FunctionBlock_Info
    {
        public FunctionBlock block;
        public Vector2 districtAreaMax;
        public Dictionary<Vector2, DistrictAreaInfo> currentDistrictDataDic;
    }
}