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
        Camp_Attribute
    }
    public class GeneralInfoItem : BaseElementSimple
    {

        public GeneralInfoItemType type;
        public int keyParam = -1;

        private Image iconTrans;
        private Text textTrans;

        public override void Awake()
        {
            iconTrans = transform.FindTransfrom("Icon").SafeGetComponent<Image>();
            textTrans = transform.FindTransfrom("Name").SafeGetComponent<Text>();
        }

        public void SetUpItem(GeneralInfoItemType type,object param)
        {
            this.type = type;
            if(type== GeneralInfoItemType.Camp_Creed)
            {
                try
                {
                    CampCreedInfo info = (CampCreedInfo)param;
                    keyParam = info.creedID;
                    iconTrans.sprite = Utility.LoadSprite(info.creedIconPath);
                    textTrans.text = info.creedName;
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
                    textTrans.text = info.attributeName;
                }
                catch { return; }
            }

            //FadeAnim
            var canvas = transform.SafeGetComponent<CanvasGroup>();
            canvas.alpha = 0;
            canvas.DoCanvasFade(1, 0.8f);
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