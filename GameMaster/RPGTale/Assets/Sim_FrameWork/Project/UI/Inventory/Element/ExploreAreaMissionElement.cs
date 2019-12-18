using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class ExploreAreaMissionElement : MonoBehaviour
    {
        private Button btn;
        private Text missionName;
        private Text areaLocation;
        private Text missionLevel;

        private ExploreRandomItem _item;
        void Awake()
        {
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


            }
        }

    }
}