using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlockType
    {
        public enum Type
        {
            None,
            Industry,
            Agriculture,
            Science,
            Alchemy,
            Energy,
            Labor,
            Unique
        }

        public enum SubType_Industry
        {
            None,
            Manufacture,
            Raw
        }
    }
}