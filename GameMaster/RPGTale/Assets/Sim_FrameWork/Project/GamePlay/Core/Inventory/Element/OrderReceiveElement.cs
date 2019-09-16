using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class OrderReceiveElement : BaseElement
    {
        [Header("Base Info")]
        public Text TitleName;
        public Text TitleDesc;
        public Image OrderBG;


        private OrderInfo orderInfo;


        public void InitOrderReceiveElement(OrderInfo info)
        {
            orderInfo = info;
            TitleName.text = info.Name;
            TitleDesc.text = info.Desc;
            OrderBG.sprite = info.BG;
        }


    }
}