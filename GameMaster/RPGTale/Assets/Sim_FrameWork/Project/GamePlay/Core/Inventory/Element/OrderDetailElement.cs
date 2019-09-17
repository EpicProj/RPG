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

        private MaterialInfo _info;
        private int _count;


        public void InitOrderDetailElement(MaterialInfo info, int count)
        {
            _info = info;
            _count = count;

            MaterialName.text = info.Name;
            Count.text = count.ToString();
            materialIcon.sprite = info.Icon;

            
        }

       
    }
}