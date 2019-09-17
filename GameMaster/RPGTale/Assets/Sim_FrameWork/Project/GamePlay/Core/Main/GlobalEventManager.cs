﻿using System.Collections;
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
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                UIManager.Instance.SendMessageToWnd(UIPath.Order_Receive_Main_Page, new UIMessage(UIMsgType.Order_Receive_Main, AllOrderDic));
            }
        }

        void Start()
        {
            //For test
            RegisterOrder(new OrderInfo(1));
           
        }

        public void RegisterOrder(OrderInfo order)
        {
            string strGUID = Guid.NewGuid().ToString();
            order.GUID = strGUID;
            AllOrderDic.Add(strGUID, order);
        }



    }
}