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
            base.Awake();
            
           

        }

        public override void Update()
        {
            CheckMouseButtonDown(UIPath.FUCNTIONBLOCK_INFO_DIALOG, info);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                info.blockModifier.DoModifier(info,"AddManuSpeed");
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                info.AddCurrentBlockEXP(100);
            }
        }
        public override void InitData()
        {
            base.InitData();
        }

        public void Product()
        {

        }


        public override void OnPlaceFunctionBlock()
        {
            base.OnPlaceFunctionBlock();
        }

     


    }

    public class FunctionBlock_Smelt_Config
    {
       
        public string InputDesc;
        public string InputIconPath;
        public string OutputDesc;
        public string OutputIconPath;
        public string ByproductDesc;
        public string ByproductIconPath;
    }

}