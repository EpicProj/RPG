﻿using System.Collections;
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

        public override void ChangeAction(BaseDataModel model)
        {
            _model = (MaterialStorageModel)model;
            InitMaterialElement();
        }


        public void InitMaterialElement()
        {
            Icon.sprite = _model.MaModel.Icon;
            Num.text = _model.Count.ToString();
            RareLine.color = _model.MaModel.Rarity.color;
            RareLight.color = _model.MaModel.Rarity.color;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.WareHouse_Page, new UIMessage(UIMsgType.WareHouse_Refresh_Detail, new List<object>() { _model.MaModel }));
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.WareHouse_Page, new UIMessage(UIMsgType.WareHouse_Hide_Detail));
        }


        

    }
}