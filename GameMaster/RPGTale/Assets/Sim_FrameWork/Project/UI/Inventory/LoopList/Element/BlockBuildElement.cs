using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class BlockBuildElement : Slot
    {
        public Text Name;
        public Image BlockIcon;

        public FunctionBlockDataModel model; 

        public void InitBuildElement(FunctionBlockDataModel _model)
        {
            model = _model;
            Name.text = _model.Name;
            BlockIcon.sprite = _model.Icon;
        }

    }
}