using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ConstructSlotPanel : InventoryBase
    {
        public override void Start()
        {
            base.Start();
            InitConstructSlot();
        }



        private void InitConstructSlot()
        {
            InitFunctionBlock(100);
        }
    }
}