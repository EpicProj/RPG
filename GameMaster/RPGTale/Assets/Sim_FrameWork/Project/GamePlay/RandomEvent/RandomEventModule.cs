using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sim_FrameWork
{
    public class RandomEventModule : BaseModule<RandomEventModule>
    {



        public override void Register()
        {

        }

    }

    public class RandomEventItem
    {
        public string name;
        public string Desc;
        public Sprite BG;

        public List<RandomEventChooseItem> itemList = new List<RandomEventChooseItem>();

        public List<GeneralRewardItem> rewardItemList = new List<GeneralRewardItem>();

        public RandomEventItem(int eventID)
        {

        }

    }

    public class RandomEventChooseItem
    {
        public string content;
        public UnityAction action;

        public RandomEventChooseItem(string str,UnityAction action)
        {
            content = str;
            this.action = action;
        }
    }

}