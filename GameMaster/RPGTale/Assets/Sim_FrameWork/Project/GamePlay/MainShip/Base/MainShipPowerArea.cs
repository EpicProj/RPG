using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class MainShipPowerArea : MainShipAreaBase
    {
        public void Start()
        {
            MainShipManager.Instance.Register<MainShipPowerArea>(MainShipAreaType.PowerArea);
        }
    }
}