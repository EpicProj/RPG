using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class EventChooseBtn : MonoBehaviour
    {
        private Text _content;
        private Button _btn;

        private ExploreChooseItem exploreItem;
        void Awake()
        {
            _content = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Text"));
            _btn = UIUtility.SafeGetComponent<Button>(transform);
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
            });
        }


       
    }
}