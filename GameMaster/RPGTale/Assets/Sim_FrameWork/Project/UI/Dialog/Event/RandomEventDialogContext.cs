using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class RandomEventDialogContext : WindowBase
    {
        
        private RandomEventDialogItem _item;

  
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
            SetUpDialog();
            if (_anim != null)
                _anim.Play();
            if (_typeEffect != null)
                _typeEffect.StartEffect();
        }

        public override void OnClose()
        {
            base.OnClose();
        }

     


        void SetUpDialog()
        {
            if (_item == null)
                return;
            foreach(Transform trans in m_dialog.ChooseContent)
            {
                trans.gameObject.SetActive(false);
            }

            _titleName.text = _item.eventName;
            _titleText.text = _item.eventTitleName;
            _eventDesc.text = _item.eventDesc;
            _eventBG.sprite = _item.eventBG;

            for(int i = 0; i < _item.itemList.Count; i++)
            {
                var eventCmpt = UIUtility.SafeGetComponent<EventChooseBtn>( m_dialog.ChooseContent.GetChild(i));
                if (eventCmpt != null)
                {
                    eventCmpt.InitBtn(_item.itemList[i]);
                    eventCmpt.gameObject.SetActive(true);
                }
            }
        }

        #endregion

    }

    public partial class RandomEventDialogContext : WindowBase
    {
        private RandomEventDialog m_dialog;
        private Animation _anim;

        private Text _titleText;
        private Text _titleName;
        private Image _eventBG;
        private Text _eventDesc;

        private Transform _eventEffectContent;
        private TypeWriterEffect _typeEffect;


        protected override void InitUIRefrence()
        {
            m_dialog = UIUtility.SafeGetComponent<RandomEventDialog>(Transform);
            _anim = UIUtility.SafeGetComponent<Animation>(Transform);

            _titleText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.transform, "Content/Title/Text"));
            _titleName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.DetailContent, "Name"));
            _eventBG = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_dialog.DetailContent, "Pic"));
            _eventDesc = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.DetailContent, "Pic/Desc"));

            _eventEffectContent = UIUtility.FindTransfrom(m_dialog.DetailContent, "EffectContent");
            _typeEffect = UIUtility.SafeGetComponent<TypeWriterEffect>(_eventDesc.transform);
        }


    }

    public class RandomEventDialogItem
    {
        public List<ExploreChooseItem> itemList;
        public string eventName;
        public string eventTitleName;
        public string eventDesc;
        public Sprite eventBG;

        public RandomEventDialogItem(string name,string eventTitleName, string desc,Sprite BG,List<ExploreChooseItem> itemList)
        {
            eventName = name;
            this.eventTitleName = eventTitleName;
            eventDesc = desc;
            eventBG = BG;
            this.itemList = itemList;
        }

    }

}