using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class GeneralHintDialogContext : WindowBase
    {
        private GeneralHintDialogItem _item;

        private Animation _anim;

        public override void Awake(params object[] paralist)
        {
            _anim = Transform.SafeGetComponent<Animation>();
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
            Transform.FindTransfrom("Content/Text").SafeGetComponent<Text>().text = _item.content;
        }
       
    }
}