using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class RandomEventDialogContext : WindowBase
    {
        private RandomEventDialog m_dialog;
        private RandomEventDialogItem _item;

        private Animation _anim;
        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _item = (RandomEventDialogItem)paralist[0];
        }

        public override void OnShow(params object[] paralist)
        {
            _item = (RandomEventDialogItem)paralist[0];
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            if (_anim != null)
                _anim.Play();
        }

        public override void OnClose()
        {
            base.OnClose();
        }

        protected override void InitUIRefrence()
        {
            m_dialog = UIUtility.SafeGetComponent<RandomEventDialog>(Transform);
            _anim = UIUtility.SafeGetComponent<Animation>(Transform); 
        }


        #endregion

    }

    public class RandomEventDialogItem
    {
    }

}