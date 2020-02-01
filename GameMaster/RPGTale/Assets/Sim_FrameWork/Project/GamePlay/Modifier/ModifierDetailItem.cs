using System.Collections;
using System.Collections.Generic;

namespace Sim_FrameWork
{
    public enum ModifierDetailRootType_Simple
    {
        /// <summary>
        /// MainShipArea
        /// </summary>
        PowerArea,
        LivingArea,
        WorkingArea,
        Hangar,
        ControlTower,

    }
    public class ModifierDetailItem_Simple
    {
        /// <summary>
        /// 加成来源
        /// </summary>
        public ModifierDetailRootType_Simple rootType;
        public float value;

        public ModifierDetailItem_Simple(ModifierDetailRootType_Simple rootType, float value)
        {
            this.rootType = rootType;
            this.value = value;
        }
    }
}