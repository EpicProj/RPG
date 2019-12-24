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

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.RandomEventDialog_Update_Effect:
                    int rewardID = (int)msg.content[0];
                    var list = GeneralModule.GetRewardItem(rewardID);
                    return SetUpReward(list);
                default:
                    return false;
            }
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
                    eventCmpt.InitBtn(_item,_item.itemList[i].ChooseID);
                    eventCmpt.gameObject.SetActive(true);
                }
            }
        }

        void InitRewardElement()
        {
            for(int i = 0; i < Config.GlobalConfigData.RandomEvent_Dialog_Reward_Max; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Reward_Item);
                if (obj != null)
                {
                    obj.transform.SetParent(_eventEffectContent,false);
                }
            }
        }

        bool SetUpReward(List<GeneralRewardItem> rewardItem)
        {
            if (_eventEffectContent.childCount != Config.GlobalConfigData.RandomEvent_Dialog_Reward_Max)
            {
                InitRewardElement();
            }
            foreach(Transform trans in _eventEffectContent)
            {
                trans.gameObject.SetActive(false);
            }

            if (rewardItem.Count == 0)
            {
                _effectLine.gameObject.SetActive(false);
            }
            else
            {
                _effectLine.gameObject.SetActive(true);
                for (int i = 0; i < rewardItem.Count; i++)
                {
                    var rewardCmpt = UIUtility.SafeGetComponent<RewardItem>(_eventEffectContent.GetChild(i));
                    if (rewardCmpt != null)
                    {
                        rewardCmpt.SetUpItem(rewardItem[i]);
                        rewardCmpt.gameObject.SetActive(true);
                    }
                    else
                    {
                        return false;
                    }
                }
                if (_effectAnim != null)
                    _effectAnim.Play();
            }
            return true;
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
        private Transform _effectLine;

        private TypeWriterEffect _typeEffect;
        private Animation _effectAnim;


        protected override void InitUIRefrence()
        {
            m_dialog = UIUtility.SafeGetComponent<RandomEventDialog>(Transform);
            _anim = UIUtility.SafeGetComponent<Animation>(Transform);

            _titleText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.transform, "Content/Title/Text"));
            _titleName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.DetailContent, "Name"));
            _eventBG = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_dialog.DetailContent, "Pic"));
            _eventDesc = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.DetailContent, "Pic/Desc"));

            _eventEffectContent = UIUtility.FindTransfrom(m_dialog.DetailContent, "EffectContent");
            _effectLine = UIUtility.FindTransfrom(m_dialog.DetailContent, "Line2");
            _typeEffect = UIUtility.SafeGetComponent<TypeWriterEffect>(_eventDesc.transform);

            _effectAnim = UIUtility.SafeGetComponent<Animation>(_eventEffectContent);
        }


    }

    public class RandomEventDialogItem
    {
        public int AreaID;
        public int ExploreID;
        public int PointID;

        public int EventID;
        public List<ExploreChooseItem> itemList;
        public string eventName;
        public string eventTitleName;
        public string eventDesc;
        public Sprite eventBG;

        public RandomEventDialogItem(int eventID,int areaid,int exploreID,int pointID)
        {
            AreaID = areaid;
            ExploreID = exploreID;
            PointID = pointID;

            EventID = eventID;
            eventName = ExploreModule.GetEventName(eventID);
            eventTitleName = ExploreModule.GetEventTitleName(eventID);
            eventDesc = ExploreModule.GetEventDesc(eventID);
            eventBG = ExploreModule.GetEventBG(eventID);
            itemList = ExploreModule.GetChooseItem(eventID);
        }

    }

}