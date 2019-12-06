using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class ConstructMainTabElement : BaseElement {

        public FunctionBlockTypeData typedata;
        public Image Icon;
        public Button Btn;


        public void InitMainTabElement(FunctionBlockTypeData data)
        {
            typedata = data;
            Icon.sprite = FunctionBlockModule.GetMainTypeSprite(data);
            Btn.onClick.AddListener(OnTabClick);

        }

        void OnTabClick()
        {
            if (typedata == null)
                return;
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_Page, new UIMessage(UIMsgType.MenuPage_Update_BuildPanel, new List<object>() { typedata }));
        }
    }
}