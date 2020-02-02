using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MainShipAreaModifier : ModifierGeneral
    {
        public ModifierTarget target;

        public MainShipAreaModifier(ModifierTarget target)
        {
            this.target = target;
            ModifierManager.Instance.RegisterShipAreaModifier(this);   
        }

        public void UnRegisterModifier()
        {
            ModifierManager.Instance.UnRegisterShipAreaModifier(this);
        }
    }
    /*
     * MainShip Modifier
     * only for mainShip
     * Weapon & Shield
     */
    public class MainShipModifier : ModifierGeneral
    {
        public ModifierTarget target;
        public MainShipModifier(ModifierTarget target)
        {
            this.target = target;
            ModifierManager.Instance.RegisterMainShipModifier(this);
        }
    }
}