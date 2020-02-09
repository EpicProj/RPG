using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class LeaderPrepareCard : MonoBehaviour
    {
        public LeaderInfo _info;

        public void SetUpItem(LeaderInfo info)
        {
            _info = info;
            transform.FindTransfrom("LeaderPortrait").SafeGetComponent<LeaderPortraitUI>().SetUpItem(info.portraitInfo);
            transform.FindTransfrom("NameBG/Text").SafeGetComponent<Text>().text = info.leaderName;
        }
    }
}