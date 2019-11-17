using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class ManuSlotElement : BaseElementSimple
    {
        public FormulaModule.MaterialProductType SlotType;
        private Image materialIcon;
        private Text materialCountText;
        private Text TitleText;

        private const string ManuSlotElement_Enhance_Title = "ManuSlotElement_Enhance_Title";

        public override void Awake()
        {
            materialIcon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Icon"));
            materialCountText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Value"));
            TitleText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Title/Text"));
        }


        public void SetUpElement(MaterialDataModel model, int needAmount,int currentAmount)
        {
            if(SlotType== FormulaModule.MaterialProductType.Enhance)
            {
                TitleText.text = MultiLanguage.Instance.GetTextValue(ManuSlotElement_Enhance_Title);
            }
            else if( SlotType == FormulaModule.MaterialProductType.Input)
            {
                TitleText.text = string.Format("{0} : {1}", model.Name, needAmount.ToString());
            }

            if (currentAmount <= 0)
            {
                SetElementState(false);
            }
            else
            {
                SetElementState(true);
                materialIcon.sprite = model.Icon;
                materialCountText.text = currentAmount.ToString();
            }
        }

        public void RefreshCount(MaterialDataModel model,int currentAmount)
        {
            if (currentAmount <= 0)
            {
                SetElementState(false);
            }
            else if (currentAmount == 1)
            {
                SetElementState(true);
                materialCountText.gameObject.SetActive(false);
            }
            else
            {
                SetElementState(true);
                materialCountText.text = currentAmount.ToString();
            }
        }



        void SetElementState(bool active)
        {
            materialIcon.gameObject.SetActive(active);
            materialCountText.gameObject.SetActive(active);
            if (SlotType == FormulaModule.MaterialProductType.Enhance)
            {
                UIUtility.FindTransfrom(transform, "Bottom").gameObject.SetActive(active);
            }
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            InventoryManager.Instance.HideMaterialInfoTip();
        }

    }
}