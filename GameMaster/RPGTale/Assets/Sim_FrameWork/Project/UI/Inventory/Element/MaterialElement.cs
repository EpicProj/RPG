using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class MaterialElement : BaseElement
    {
        [Header("Base Info")]
        public Image Icon;
        public Text Num;
        public Image TypeIcon;
        public Image RareLine;
        public Image RareLight;

        public MaterialStorageModel _model;

        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (MaterialStorageModel)model[0];
            InitMaterialElement();
        }


        public void InitMaterialElement()
        {
            Icon.sprite = _model.MaModel.Icon;

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