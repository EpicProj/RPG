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
            var chooseContent = Transform.FindTransfrom("Content/Choose");
            chooseContent.SafeSetActiveAllChild(false);

            _titleName.text = _item.eventName;
            _titleText.text = _item.eventTitleName;
            _eventDesc.text = _item.eventDesc;
            _eventBG.sprite = _item.eventBG;

            for(int i = 0; i < _item.itemList.Count; i++)
            {
                var eventCmpt = chooseContent.GetChild(i).SafeGetComponent<EventChooseBtn>();
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
            _eventEffectContent.SafeSetActiveAllChild(false);

            if (rewardItem.Count == 0)
            {
                _effectLine.SafeSetActive(false);
            }
            else
            {
                _effectLine.SafeSetActive(true);
                for (int i = 0; i < rewardItem.Count; i++)
                {
                    var rewardCmpt = _eventEffectContent.GetChild(i).SafeGetComponent<RewardItem>();
                    if (rewardCmpt != null)
                    {
                        rewardCmpt.SetUpItem(rewardItem[i]);
                        rewardCmpt.transform.SafeSetActive(true);
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
            _anim = Transform.SafeGetComponent<Animation>();

            _titleText = Transform.FindTransfrom("Content/Title/Text").SafeGetComponent<Text>();
            _titleName = Transform.FindTransfrom("Content/Detail/Name").SafeGetComponent<Text>();
            _eventBG = Transform.FindTransfrom("Content/Detail/Pic").SafeGetComponent<Image>();
            _eventDesc = Transform.FindTransfrom("Content/Detail/Pic/Desc").SafeGetComponent<Text>();

            _eventEffectContent = Transform.FindTransfrom("Content/Detail/EffectContent");
            _effectLine = Transform.FindTransfrom("Content/Detail/Line2");
            _typeEffect = _eventDesc.transform.SafeGetComponent<TypeWriterEffect>();

            _effectAnim = _eventEffectContent.SafeGetComponent<Animation>();
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