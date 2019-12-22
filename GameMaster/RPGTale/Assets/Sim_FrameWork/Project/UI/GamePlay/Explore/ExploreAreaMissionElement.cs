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

        private Button btn;
        private Text missionName;
        private Text areaLocation;
        private Text missionLevel;

        private ExploreRandomItem _item;
        void Awake()
        {
            anim = UIUtility.SafeGetComponent<Animator>(transform);
            btn = UIUtility.SafeGetComponent<Button>(transform);
            missionName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/Text"));
            areaLocation = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Area/Text"));
            missionLevel= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/Level/Value"));
        }

        public void SetUpElement(ExploreRandomItem item)
        {
            btn.onClick.RemoveAllListeners();
            if (item != null)
            {
                _item = item;
                missionName.text = item.missionName;
                areaLocation.text = item.missionAreaName;
                missionLevel.text = item.areaHardLevel.ToString();
                btn.onClick.AddListener(OnBtnClick);
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