using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public static class AnimUtility 
    {

        public static void SafePlayAnim(this Animation anim)
        {
            if (anim != null)
            {
                anim.Play();
            }
        }
    }
}