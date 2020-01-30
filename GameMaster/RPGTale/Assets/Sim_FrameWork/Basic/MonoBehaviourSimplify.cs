using UnityEngine;
using System;
using System.Collections;

namespace Sim_FrameWork
{
    public static partial class MonoBehaviourSimplify 
    {
        //Transform Simpfiy
        public static void SetLocalPosX(this Transform trans,float x)
        {
            var localPos = trans.localPosition;
            localPos.x = x;
            trans.localPosition = localPos;
        }

        public static void SetLocalPosY(this Transform trans,float y)
        {
            var localPos = trans.localPosition;
            localPos.y = y;
            trans.localPosition = localPos;
        }

        public static void SetLocalPosZ(this Transform trans, float z)
        {
            var localPos = trans.localPosition;
            localPos.z = z;
            trans.localPosition = localPos;
        }

        public static void SetLocalPosXY(this Transform trans,float x, float y)
        {
            var localPos = trans.localPosition;
            localPos.x = x;
            localPos.y = y;
            trans.localPosition = localPos;
        }

        public static void SetLocalPosXZ(this Transform trans, float x, float z)
        {
            var localPos = trans.localPosition;
            localPos.x = x;
            localPos.z = z;
            trans.localPosition = localPos;
        }
        public static void SetLocalPosYZ(this Transform trans, float y, float z)
        {
            var localPos = trans.localPosition;    
            localPos.y = y;
            localPos.z = z;
            trans.localPosition = localPos;
        }

        public static void SetLocalPosXYZ(this Transform trans, float x, float y, float z)
        {
            var localPos = trans.localPosition;
            localPos.x = x;
            localPos.y = y;
            localPos.z = z;
            trans.localPosition = localPos;
        }

 
    }

}