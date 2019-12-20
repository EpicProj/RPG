using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class ExploreAreaMissionElement : MonoBehaviour
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
            if (item != null)
            {
                _item = item;
                missionName.text = item.missionName;
                areaLocation.text = item.missionAreaName;
                missionLevel.text = item.areaHardLevel.ToString();
            }
        }

        public void ShowMission()
        {
            if (anim != null)
            {
                anim.Play("ExploreMission_Show", 0, 0);
            }
        }

    }
}