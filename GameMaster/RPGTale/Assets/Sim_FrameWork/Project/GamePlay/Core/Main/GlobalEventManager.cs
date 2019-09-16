using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork {

    /// <summary>
    /// Manager Events && Order && Organization
    /// </summary>
    public class GlobalEventManager : MonoSingleton<GlobalEventManager>{

        public Dictionary<string, OrderInfo> AllOrderDic = new Dictionary<string, OrderInfo>();


        protected override void Awake()
        {
            base.Awake();
        }

        public void RegisterOrder(OrderInfo order)
        {
            string strGUID = Guid.NewGuid().ToString();
            order.GUID = strGUID;
            AllOrderDic.Add(strGUID, order);
        }



    }
}