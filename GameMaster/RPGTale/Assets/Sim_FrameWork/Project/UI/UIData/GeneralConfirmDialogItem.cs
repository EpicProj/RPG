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


        public Action OnConfirmClick;
        public Action OnCancelClick ;

        public GeneralConfirmDialogItem(string title,string content,Action confirm=null ,Action cancel=null)
        {
            TitleText = title;
            ContentText = content;
            OnConfirmClick = confirm;
            if (cancel == null)
            {
                OnCancelClick = () =>
                {
                    UIManager.Instance.HideWnd(UIPath.WindowPath.General_Confirm_Dialog);
                };
            }
            else
            {
                OnCancelClick = cancel;
            }
           
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