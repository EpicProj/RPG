using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class WarehouseBase : FunctionBlockBase
    {
        public int WidthMax;
        public int HeightMax;
        public int GridNum;
        public int CountPerGrid;



        public bool AddMaterialToWarehouse(int materialID, int count)
        {
            //TODO
            return true;
        }

    }
}