using System.Collections;
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

        private int _count;


        public void InitOrderDetailElement(MaterialDataModel model, int count)
        {
            _count = count;
            MaterialName.text = model.Name;
            Count.text = count.ToString();
            materialIcon.sprite = model.Icon;
        }

       
    }
}