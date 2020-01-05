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

        private MaterialDataModel _model;
        private ushort _count = 0;

        private const string ManuSlotElement_Enhance_Title = "ManuSlotElement_Enhance_Title";

        public override void Awake()
        {
            materialIcon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Icon"));
            materialCountText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Value"));
            TitleText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Title/Text"));
        }


        public void SetUpElement(MaterialDataModel model, ushort needAmount,ushort currentAmount)
        {
            _model = model;
            _count = currentAmount;
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

        /// <summary>
        /// 初始化更换生产线
        /// </summary>
        /// <param name="model"></param>
        /// <param name="count"></param>
        public void SetUpFormulaChangeElement(MaterialDataModel model,ushort count)
        {
            _model = model;
            _count = count;
            if(SlotType == FormulaModule.MaterialProductType.Enhance)
            {
                TitleText.text = MultiLanguage.Instance.GetTextValue(ManuSlotElement_Enhance_Title);
                UIUtility.FindTransfrom(transform, "Bottom").gameObject.SetActive(false);
            }
            else if (SlotType== FormulaModule.MaterialProductType.Input)
            {
                TitleText.text = model.Name;
            }
            materialIcon.sprite = model.Icon;
            materialCountText.text = count.ToString();
        }

        public void RefreshCount(MaterialDataModel model,ushort currentAmount)
        {
            if (currentAmount <= 0)
            {
                SetElementState(false);
            }
            else if (currentAmount == 1)
            {
                SetElementState(true);
                materialCountText.gameObject.SetActive(false);
                materialIcon.sprite = model.Icon;
            }
            else
            {
                SetElementState(true);
                materialCountText.text = currentAmount.ToString();
                materialIcon.sprite = model.Icon;
            }
            _count = currentAmount;
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
            if (_model.ID != 0 && _count!=0)
            {
                UIGuide.Instance.ShowMaterialDetailInfo(_model);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.HideWnd(UIPath.WindowPath.Material_Info_UI);
        }

    }
}