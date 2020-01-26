using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class ExploreAreaMissionElement : BaseElementSimple
    {
        private Animator anim;
        private ExploreRandomItem _item;
        public override void Awake()
        {
            anim = transform.SafeGetComponent<Animator>();
        }

        public void SetUpElement(ExploreRandomItem item)
        {
            if (item != null)
            {
                _item = item;
                var btn = transform.SafeGetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OnBtnClick);

                transform.FindTransfrom("Content/Text").SafeGetComponent<Text>().text = item.missionName;
                transform.FindTransfrom("Area/Text").SafeGetComponent<Text>().text = item.missionAreaName;
                transform.FindTransfrom("Content/Level/Value").SafeGetComponent<Text>().text = item.areaHardLevel.ToString();
            }
        }

        public void ShowMission()
        {
            if (anim != null)
            {
                anim.Play("ExploreMission_Show", 0, 0);
            }
        }

        void OnBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Explore_Main_Page, new UIMessage(UIMsgType.ExplorePage_Show_MissionDetail ,new List<object>(1) { _item }));
            SolarSystemManager.Instance.MoveToExploreMissionPoint(_item.exploreID);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_General);
        }

    }
}