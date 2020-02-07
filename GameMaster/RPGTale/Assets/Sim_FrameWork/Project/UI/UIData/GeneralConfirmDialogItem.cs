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

        public List<GeneralConfrimBtnItem> btnList;

        public GeneralConfirmDialogItem(string title,string content, List<GeneralConfrimBtnItem> btnList)
        {
            TitleText = title;
            ContentText = content;
            this.btnList = btnList;
        }
    }
    public class GeneralConfrimBtnItem
    {
        public enum btnColor
        {
            Red,
            Blue
        }
        public string _btnText;
        public UnityEngine.Events.UnityAction _action;
        public btnColor _btnColor;

        public GeneralConfrimBtnItem(string text, UnityEngine.Events.UnityAction action,btnColor color= btnColor.Blue)
        {
            this._btnText = text;
            this._action = action;
            this._btnColor = color;
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