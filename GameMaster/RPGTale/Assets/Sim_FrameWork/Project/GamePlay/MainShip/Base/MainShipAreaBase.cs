using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MainShipAreaBase : MonoBehaviour
    {

        public MainShipAreaModifier areaModifier;

        public void Awake()
        {
            areaModifier = transform.SafeGetComponent<MainShipAreaModifier>();
        }

    }
}