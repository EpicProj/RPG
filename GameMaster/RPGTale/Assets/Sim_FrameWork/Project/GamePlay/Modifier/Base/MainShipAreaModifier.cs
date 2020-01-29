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
          
        }
    }
}