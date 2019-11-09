﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlock_Smelt : ManufactoryBase,IFunctionBlockAction
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
            CheckMouseButtonDown(delegate()
            {
                UIManager.Instance.PopUpWnd(UIPath.WindowPath.FUNCTIONBLOCK_INFO_DIALOG, WindowType.Dialog, true, info, manufactoryInfo);
            });
            if (Input.GetKeyDown(KeyCode.Space))
            {
                info.blockModifier.DoManufactModifier(manufactoryInfo,info,"AddManuSpeed");
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                info.levelInfo.AddCurrentBlockEXP(100);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.FUNCTIONBLOCK_INFO_DIALOG, new UIMessage(UIMsgType.UpdateLevelInfo, new List<object>(1) {info.levelInfo }));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddMaterialToInputSlot(100, 2);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.FUNCTIONBLOCK_INFO_DIALOG, new UIMessage(UIMsgType.UpdateManuSlot, new List<object>(1) { manufactoryInfo.formulaInfo }));
            }
        }
        public override void InitData()
        {
            base.InitData();
        }

        public void Product()
        {

        }

        public void OnPlaceFunctionBlock()
        {
           
        }

        public void OnHoldFunctionBlock()
        {
            
        }

        public void OnDestoryFunctionBlock()
        {
            
        }

        public void OnSelectFunctionBlock()
        {
           
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