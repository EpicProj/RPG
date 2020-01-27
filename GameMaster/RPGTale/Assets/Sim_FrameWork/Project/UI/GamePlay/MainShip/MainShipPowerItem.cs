using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MainShipPowerItem : MonoBehaviour
    {
        public PowerState currentState;

        public enum PowerState
        {
            Lock,
            Empty,
            Fill
        }

        public void SwitchState(PowerState state)
        {
            transform.SafeSetActiveAllChild(false);
            if(state == PowerState.Empty)
            {
                transform.FindTransfrom("Empty").SafeSetActive(true);
                currentState = PowerState.Empty;
            }
            else if(state == PowerState.Fill)
            {
                transform.FindTransfrom("Fill").SafeSetActive(true);
                currentState = PowerState.Fill;
            }
            else if(state == PowerState.Lock)
            {
                transform.FindTransfrom("Lock").SafeSetActive(true);
                currentState = PowerState.Lock;
            }
        }
    }
}