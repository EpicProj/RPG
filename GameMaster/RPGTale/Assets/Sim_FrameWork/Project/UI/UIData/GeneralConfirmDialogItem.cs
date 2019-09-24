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
        public Action OnCancelClick;

        public GeneralConfirmDialogItem(string title,string content,Action confirm,Action cancel)
        {
            TitleText = title;
            ContentText = content;
            OnConfirmClick = confirm;
            OnCancelClick = cancel;
        }
    }
}