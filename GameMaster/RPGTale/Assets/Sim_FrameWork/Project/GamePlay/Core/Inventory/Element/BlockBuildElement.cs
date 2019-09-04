using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class BlockBuildElement : Slot
    {
        public Text Name;
        public Image BlockIcon;


        public void InitBuildElement(BuildingPanelData data)
        {
            FunctionBlock block = PlayerModule.Instance.GetBuildFunctionBlock(data);
            Name.text = FunctionBlockModule.Instance.GetFunctionBlockName(block);
            BlockIcon.sprite = FunctionBlockModule.Instance.GetFunctionBlockIcon(block.FunctionBlockID);
        }

    }
}