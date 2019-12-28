using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class GeneralHintDialogContext : WindowBase
    {
        private GeneralHintDialog m_dialog;
        private GeneralHintDialogItem _item;

        private Animation _anim;

        public override void Awake(params object[] paralist)
        {
            m_dialog = UIUtility.SafeGetComponent<GeneralHintDialog>(Transform);
            _anim = UIUtility.SafeGetComponent<Animation>(m_dialog.transform);
            _item = (GeneralHintDialogItem)paralist[0];
            
        }

        public override void OnShow(params object[] paralist)
        {
            _item = (GeneralHintDialogItem)paralist[0];
            _anim.Play();
            InitContent();
            Coroutine_Extend cor = new Coroutine_Extend(CloseHint(), true);
        }

        IEnumerator CloseHint()
        {
            yield return new WaitForSeconds(_item.time);
            UIManager.Instance.HideWnd(UIPath.WindowPath.General_Hint_Dialog);
        }

        private void InitContent()
        {
            m_dialog.content.text = _item.content;
        }
       
    }
}