﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class OrderDetailElement : BaseElement
    {
        [Header("Content")]
        public Text MaterialName;
        public Text MaterialName_En;
        public Text Count;
        public Image materialIcon;
        public Slider slider;
        public Text Progress;
        public Text StorageValue;

        private string OrderDetail_Storage_Text = "OrderDetail_Storage_Count_Text";

        private int _count;
        private MaterialDataModel _model;

        public void InitOrderDetailElement(MaterialDataModel model, int count)
        {
            _count = count;
            _model = model;
            MaterialName.text = model.Name;
            Count.text = count.ToString();
            materialIcon.sprite = model.Icon;
            RefreshProgress();
        }

        public void RefreshProgress()
        {
            var count = PlayerManager.Instance.GetMaterialStoreCount(_model.ID);
            StorageValue.text = Utility.ParseStringParams(MultiLanguage.Instance.GetTextValue(OrderDetail_Storage_Text),new string[1] {count.ToString()});

        }
       
    }
}