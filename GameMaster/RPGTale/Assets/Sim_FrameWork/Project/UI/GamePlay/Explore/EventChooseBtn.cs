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

        private UI.RandomEventDialogItem eventItem;
        private ExploreChooseItem chooseItem;

        public override void Awake()
        {
            _content = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Text"));
            _btn = UIUtility.SafeGetComponent<Button>(transform);
            _anim = UIUtility.SafeGetComponent<Animator>(transform);
        }

        public void InitBtn(UI.RandomEventDialogItem item,int chooseID)
        {
            eventItem = item;
            var exploreChooseItem = item.itemList.Find(x => x.ChooseID == chooseID);
            if (exploreChooseItem != null)
            {
                chooseItem = exploreChooseItem;
                _content.text = exploreChooseItem.content;
                _btn.onClick.RemoveAllListeners();
                _btn.onClick.AddListener(() =>
                {
                    GlobalEventManager.Instance.HandleRewardDataItem(exploreChooseItem.rewardID);
                    AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);

                    ExploreEventManager.Instance.OnRandomEventFinish(item);

                    if (exploreChooseItem.nextEvent != 0)
                    {
                        if (ExploreModule.GetExploreEventDataByKey(exploreChooseItem.nextEvent) != null)
                        {
                            UIGuide.Instance.ShowRandomEventDialog(exploreChooseItem.nextEvent,item.AreaID,item.ExploreID, item.PointID);
                        }
                    }
                    else
                    {
                        ExploreEventManager.Instance.FinishExplorePoint(item.AreaID,item.ExploreID,item.PointID);
                        UIManager.Instance.HideWnd(UIPath.WindowPath.RandomEvent_Dialog);
                    }
                });
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (chooseItem != null)
            {
                UIManager.Instance.SendMessage(new UIMessage(UIMsgType.RandomEventDialog_Update_Effect, new List<object>(1) { chooseItem.rewardID }));
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            
        }



    }
}