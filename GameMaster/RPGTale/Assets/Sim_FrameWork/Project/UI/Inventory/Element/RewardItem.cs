using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class RewardItem : BaseElementSimple
    {
        private Image _icon;
        private Text _count;
        private GeneralRewardItem _item;

        public override void Awake()
        {
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG/Icon"));
            _count = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Value"));
        }

        public void SetUpItem(GeneralRewardItem item) 
        {
            _item = item;
            if (item.type == GeneralRewardItem.RewardType.Material)
            {
                if (MaterialModule.GetMaterialByMaterialID(item.ItemID) != null)
                {
                    var icon = MaterialModule.GetMaterialSprite(item.ItemID);
                    _icon.sprite = icon;
                    _count.text = item.count.ToString();
                }
            }
            else if (item.type == GeneralRewardItem.RewardType.Tech_Unlock)
            {
                if (TechnologyModule.GetTechDataByID(item.ItemID) != null)
                {
                    var icon = TechnologyModule.GetTechIcon(item.ItemID);
                    _icon.sprite = icon;
                }
            }
        }
    }
}