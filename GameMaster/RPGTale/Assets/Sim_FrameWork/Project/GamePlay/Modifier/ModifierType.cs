using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public enum ModifierType
    {

    }
    public enum ModifierFunctionBlockType
    {
        /// <summary>
        /// 建造速度
        /// </summary>
        ManuSpeed,
        /// <summary>
        /// 维护成本
        /// </summary>
        Maintain,
        /// <summary>
        /// 能源消耗
        /// </summary>
        EnergyCostNormal,
        /// <summary>
        /// 所需工人
        /// </summary>
        Worker,
        EnergyStorageMax,
        /// <summary>
        /// Error
        /// </summary>
        None,
    }

    /// <summary>
    /// Modifier Sub Type  
    /// </summary>
    public enum ModifierBaseResourceType
    {
        Currency,
        Food,
        Energy,
        Laber,
        Reputation
    }

}