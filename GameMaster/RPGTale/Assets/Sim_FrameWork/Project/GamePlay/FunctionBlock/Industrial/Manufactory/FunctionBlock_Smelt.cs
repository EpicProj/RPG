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



        public void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _blockBase.info.blockModifier.DoManufactModifier(manufactoryInfo, _blockBase.info, "AddManuSpeed");
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                _blockBase.info.levelInfo.AddCurrentBlockEXP(100);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.FUNCTIONBLOCK_INFO_DIALOG, new UIMessage(UIMsgType.UpdateLevelInfo, new List<object>(1) { _blockBase.info.levelInfo }));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddMaterialToInputSlot(100, 2);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.FUNCTIONBLOCK_INFO_DIALOG, new UIMessage(UIMsgType.UpdateManuSlot, new List<object>(1) { manufactoryInfo.formulaInfo }));
            }
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