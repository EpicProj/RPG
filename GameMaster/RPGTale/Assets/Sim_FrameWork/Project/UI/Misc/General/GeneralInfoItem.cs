using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public enum GeneralInfoItemType
    {
        Camp_Creed,
        Camp_Attribute,
        Leader_Skill,
        Leader_Attribute,
    }
    public class GeneralInfoItem : BaseElementSimple
    {

        public GeneralInfoItemType type;
        public int keyParam = -1;

        private Image iconTrans;
        private Text textTrans;

        public override void Awake()
        {
            iconTrans = transform.FindTransfrom("Name/Icon").SafeGetComponent<Image>();
            textTrans = transform.FindTransfrom("Name").SafeGetComponent<Text>();
        }

        public void SetUpItem(GeneralInfoItemType type,object param,bool showText=true)
        {
            this.type = type;
            if(type== GeneralInfoItemType.Camp_Creed)
            {
                try
                {
                    CampCreedInfo info = (CampCreedInfo)param;
                    keyParam = info.creedID;
                    iconTrans.sprite = Utility.LoadSprite(info.creedIconPath);
                    textTrans.text = showText ? info.creedName : "";
                }
                catch{ return;}
            }
            else if(type == GeneralInfoItemType.Camp_Attribute)
            {
                try
                {
                    CampAttributeInfo info = (CampAttributeInfo)param;
                    keyParam = info.attributeID;
                    iconTrans.sprite = Utility.LoadSprite(info.iconPath);
                    textTrans.text = showText ? info.attributeName : "";
                }
                catch { return; }
            }
            else if(type == GeneralInfoItemType.Leader_Skill)
            {
                try
                {
                    LeaderSkillInfo info = (LeaderSkillInfo)param;
                    keyParam = info.skillID;
                    iconTrans.sprite = Utility.LoadSprite(info.skillIconPath);
                    textTrans.text = showText ? info.skillName : "";
                }
                catch { return; }
            }
            else if(type== GeneralInfoItemType.Leader_Attribute)
            {
                try
                {
                    LeaderAttributeInfo info = (LeaderAttributeInfo)param;
                    keyParam = info.attributeID;
                    iconTrans.sprite = Utility.LoadSprite(info.attributeIconPath);
                    textTrans.text = showText ? info.attributeName : "";
                }
                catch { return; }
            }

            RefrehsContentSize();
            //FadeAnim
            var canvas = transform.SafeGetComponent<CanvasGroup>();
            canvas.alpha = 0;
            canvas.DoCanvasFade(1, 0.8f);
        }

        void RefrehsContentSize()
        {
            var rect = transform.SafeGetComponent<RectTransform>();
            var imageWidth = iconTrans.transform.SafeGetComponent<RectTransform>().rect.width;
            var fitter = textTrans.transform.SafeGetComponent<ContentSizeFitter>();
            var textWidth = textTrans.transform.SafeGetComponent<RectTransform>().GetContentSizeFitterPreferredSize(fitter).x;
            rect.sizeDelta = new Vector2(imageWidth + textWidth, rect.sizeDelta.y);
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