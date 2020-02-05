using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MainShipShiledDetailItem : MonoBehaviour
    {
        public MainShip_ShieldDirection direction;
        private MainShipShieldInfo _info;

        private const string energyItemPrefabPath = "Assets/Prefabs/Object/MainShip/MainShipPowerItem.prefab";

        public void SetUpItem(MainShipShieldInfo info)
        {
            _info = info;
            direction = info.direction;

            UpdateShieldLevelMax();
        }


        void UpdateShieldLevelMax()
        {
            var layerMax = MainShipManager.Instance.mainShipInfo.shieldEnergy_Max_current;
            var content = transform.FindTransfrom("Right/Energy/Content");
            content.InitObj(energyItemPrefabPath, layerMax);
        }

    }
}