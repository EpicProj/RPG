using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class BaseElementModel 
    {

    }

    public class OrderReceiveElementModel :BaseElementModel
    {
        public string TitleName;
        public string TitleDesc;
        public Sprite OrderBG;


        public OrderReceiveElementModel(OrderItemBase item)
        {
            TitleName = item.Name;
            TitleDesc = item.Desc;
            OrderBG = item.BG;
        }

    }


}