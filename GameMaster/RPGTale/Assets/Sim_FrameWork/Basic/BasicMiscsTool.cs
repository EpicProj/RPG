using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_FrameWork
{
    public class BasicMiscsTool : MonoBehaviour
    {

        //PercentTool Random
        public static bool Percent(int percent)
        {
            return Random.Range(0, 100) <= percent;
        }
        
        
    }
}