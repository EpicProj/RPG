using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class GeneralHintDialogContent : WindowBase
    {
        private GeneralHintDialog m_dialog;
        private GeneralHintDialogItem _item;

        private float timer;

        public override void Awake(params object[] paralist)
        {
            m_dialog = UIUtility.SafeGetComponent<GeneralHintDialog>(Transform);
            _item = (GeneralHintDialogItem)paralist[0];
            
        }

        public override void OnShow(params object[] paralist)
        {
            _item = (GeneralHintDialogItem)paralist[0];
            InitContent();
        }
        public override void OnUpdate()
        {
            if (_item == null)
                return;
            timer += Time.deltaTime;
            if (timer >= _item.time)
            {
                //TODO
                timer = 0;
                UIManager.Instance.HideWnd(UIPath.General_Hint_Dialog);
            }
        }

        private void InitContent()
        {
            m_dialog.content.text = MultiLanguage.Instance.GetTextValue(_item.content);
        }
       
    }
}