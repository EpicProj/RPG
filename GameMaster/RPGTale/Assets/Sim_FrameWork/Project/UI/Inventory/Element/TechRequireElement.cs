using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class TechRequireElement : BaseElementSimple
    {
        private Transform titleWaringTrans;
        private Image rarityBG;
        private Image icon;
        private Text nameText;
        private Transform lockIconTrans;

        private Transform SelectEffectTrans;


        public override void Awake()
        {
            titleWaringTrans = UIUtility.FindTransfrom(transform, "Title");
            rarityBG = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG/Circle"));
            icon= UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG/Icon")); ;
            nameText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));
            lockIconTrans = UIUtility.FindTransfrom(transform, "Name/Icon");
            SelectEffectTrans = UIUtility.FindTransfrom(transform, "Select");
        }

        public void SetUpElement(Sprite sp, Color rarityColor, string name,bool showWarning)
        {
            icon.sprite = sp;
            nameText.text = name;
            nameText.color = new Color(rarityColor.r, rarityColor.g, rarityColor.b, 0.8f);
            rarityBG.color=new Color(rarityColor.r, rarityColor.g, rarityColor.b, 0.3f);
            ShowLockWaring(showWarning);
        }


        public void ShowLockWaring(bool active)
        {
            titleWaringTrans.gameObject.SetActive(active);
            lockIconTrans.gameObject.SetActive(active);
        }




        public override void OnPointerEnter(PointerEventData eventData)
        {
            SelectEffectTrans.gameObject.SetActive(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            SelectEffectTrans.gameObject.SetActive(false);
        }


    }
}