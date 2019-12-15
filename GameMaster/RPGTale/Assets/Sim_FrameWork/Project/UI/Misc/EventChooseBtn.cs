using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class EventChooseBtn : BaseElementSimple
    {
        private Text _content;
        private Button _btn;
        private Animator _anim;

        private ExploreChooseItem exploreItem;
        public override void Awake()
        {
            _content = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Text"));
            _btn = UIUtility.SafeGetComponent<Button>(transform);
            _anim = UIUtility.SafeGetComponent<Animator>(transform);
        }

        public void InitBtn(ExploreChooseItem item)
        {
            exploreItem = item;
            _content.text = exploreItem.content;
            _btn.onClick.RemoveAllListeners();
            _btn.onClick.AddListener(() =>
            {
                GlobalEventManager.Instance.HandleRewardDataItem(item.rewardID);
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
                if (item.nextEvent != 0 )
                {
                    if (ExploreModule.GetExploreEventDataByKey(item.nextEvent) != null)
                    {
                        UIGuide.Instance.ShowRandomEventDialog(item.nextEvent);
                    }
                }
                else
                {
                    UIManager.Instance.HideWnd(UIPath.WindowPath.RandomEvent_Dialog);
                }
            });
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (exploreItem != null)
            {
                UIManager.Instance.SendMessage(new UIMessage(UIMsgType.RandomEventDialog_Update_Effect, new List<object>(1) { exploreItem.rewardID }));
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            
        }



    }
}