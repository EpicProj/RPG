using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class GeneralConfirmDialogItem
    {
        public string TitleText;
        public string ContentText;

        public byte ButtonNum = 1;

        public Action _action1;
        public Action _action2;
        public Action _action3;

        public string _btnText1;
        public string _btnText2;
        public string _btnText3;

        public GeneralConfirmDialogItem(string title,string content,byte buttonNum, Action action1 = null, string btnText1=null, Action action2 = null, string btnText2=null, Action action3 = null, string btnText3=null)
        {
            TitleText = title;
            ContentText = content;
            ButtonNum = buttonNum;
            _btnText1 = btnText1;
            _btnText2 = btnText2;
            _btnText3 = btnText3;

            _action1 = action1;
            _action2 = action2;
            _action3 = action3;
        }
    }

    public class GeneralHintDialogItem
    {
        public string content;
        public float time;

        public GeneralHintDialogItem(string content,float time)
        {
            this.content = content;
            this.time = time;
        }
    }
}