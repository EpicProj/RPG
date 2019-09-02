using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MainMenuPageContext : WindowBase
    {
        public PlayerData playerData;
        public MainMenuPage m_page;

        public override void Awake(params object[] paralist)
        {
            playerData = PlayerModule.Instance.InitPlayerData();
            m_page = GameObject.GetComponent<MainMenuPage>();
            AddBtnListener();
            UpdatePlayerBaseData();
        }

        public override void OnShow(params object[] paralist)
        {


        }


        public override bool OnMessage(string msgID, params object[] paralist)
        {
            switch (msgID)
            {
                case "UpdateResourceData":
                    //更新资源面板
                    playerData = (PlayerData)paralist[0];
                    UpdatePlayerBaseData();
                    return true;
                default:
                    return false;
            }

          
            
        }


        public void UpdatePlayerBaseData()
        {
            m_page.CurrencyNum.text = playerData.Currency.ToString();
            m_page.FoodNum.text = playerData.Food.ToString();
        }



        //Button
        private void AddBtnListener()
        {
            AddButtonClickListener(m_page.MaterialBtn, delegate () 
            {
                UIManager.Instance.PopUpWnd(UIPath.WAREHOURSE_DIALOG, true ,playerData.materialStorageDataList);
            });
        }



    }
}