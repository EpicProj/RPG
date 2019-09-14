using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class ConstructMainTabElement : BaseElement {

        public FunctionBlockTypeData typedata;
        public Image Icon;
        public Text Name;
        public GameObject SelectObj;
        public Button Btn;


        public void InitMainTabElement(FunctionBlockTypeData data)
        {
            typedata = data;
            Icon.sprite = FunctionBlockModule.GetMainTypeSprite(data);
            Name.text = FunctionBlockModule.GetMainTypeName(data);
        }
       

      
    }
}