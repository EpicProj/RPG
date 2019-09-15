using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class GeneralTagElement : Slot
    {
        public Text Name;
        public Image Icon;
        public Button btn;
        public GameObject Highlight;
            
       
        public void InitWareHouseMainTag(MaterialConfig.MaterialType type,Transform setParent)
        {
            Highlight.SetActive(false);
            Icon.sprite = MaterialModule.GetMaterialMainTypeSprite(type);
            Name.text = MaterialModule.GetMaterialMainTypeName(type);
            gameObject.transform.SetParent(setParent, false);
        }

        public void InitWareHouseSubTag(MaterialConfig.MaterialType.MaterialSubType subType, Transform setParent)
        {
            Highlight.SetActive(false);
            Icon.sprite = MaterialModule.GetMaterialSubTypeIcon(subType);
            Name.text = MaterialModule.GetMaterialSubTypeName(subType);
            gameObject.transform.SetParent(setParent, false);
        }
        public void HighlightObj()
        {
            Highlight.SetActive(true);
        }
        public void DimObj()
        {
            Highlight.SetActive(false);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            
        }


    }
}