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
            CheckMouseButtonDown(UIPath.FUNCTIONBLOCK_INFO_DIALOG, info,manufactoryInfo);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                info.blockModifier.DoManufactModifier(manufactoryInfo,info,"AddManuSpeed");
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                info.levelInfo.AddCurrentBlockEXP(100);
                UIManager.Instance.SendMessageToWnd(UIPath.FUNCTIONBLOCK_INFO_DIALOG, new UIMessage(UIMsgType.UpdateLevelInfo, info.levelInfo));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddMaterialToInputSlot(100, 2);
                UIManager.Instance.SendMessageToWnd(UIPath.FUNCTIONBLOCK_INFO_DIALOG, new UIMessage(UIMsgType.UpdateManuSlot, manufactoryInfo.formulaInfo));
            }
        }
        public override void InitData()
        {
            base.InitData();
            FunctionBlockManager.Instance.AddFunctionBlockData(info.BlockUID, this);
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