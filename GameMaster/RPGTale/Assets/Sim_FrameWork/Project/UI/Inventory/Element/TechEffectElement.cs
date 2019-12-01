using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class TechEffectElement : BaseElementSimple
    {
        private Image Icon;
        private Text Name;
        private Text Unlock;
       

        public override void Awake()
        {
            Icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Icon/Image"));
            Name = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Unlock"));
            Unlock = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));
        }

        public void SetUpElement(Sprite sp,string unlockName,string name)
        {
            Icon.sprite = sp;
            Name.text = unlockName;
            Unlock.text = name;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }

    }
}