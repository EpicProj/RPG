using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public static class BasicMiscsTool 
    {

        //PercentTool Random
        public static bool Percent(int percent)
        {
            return UnityEngine.Random.Range(0, 100) <= percent;
        }
        
    }
}