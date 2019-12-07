using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class ConstructMainTabElement : BaseElement {

        public FunctionBlockTypeData typedata;
        public Image Icon;
        public Toggle toggle;


        public void InitMainTabElement(FunctionBlockTypeData data)
        {
            typedata = data;
            Icon.sprite = FunctionBlockModule.GetMainTypeSprite(data);
            toggle.onValueChanged.AddListener((bool isOn) =>
            {
                OnTabClick(isOn);
            });
        }

        void OnTabClick(bool isOn)
        {
            if (isOn)
            {
                if (typedata == null)
                    return;
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_Page, new UIMessage(UIMsgType.MenuPage_Update_BuildPanel, new List<object>() { typedata }));
                Debug.Log("OnBuildClick" );
            }
        }
    }
}