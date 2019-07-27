﻿using UnityEngine;
using System;
using System.Collections;

namespace RPG_FrameWork
{
    public partial class MonoBehaviourSimplify : MonoBehaviour
    {
        //Transform Simpfiy
        public void SetLocalPosX(Transform trans,float x)
        {
            var localPos = trans.localPosition;
            localPos.x = x;
            trans.localPosition = localPos;
        }

        public void SetLocalPosY(Transform trans,float y)
        {
            var localPos = trans.localPosition;
            localPos.y = y;
            trans.localPosition = localPos;
        }

        public void SetLocalPosZ(Transform trans, float z)
        {
            var localPos = trans.localPosition;
            localPos.z = z;
            trans.localPosition = localPos;
        }

        public void SetLocalPosXY(Transform trans,float x, float y)
        {
            var localPos = trans.localPosition;
            localPos.x = x;
            localPos.y = y;
            trans.localPosition = localPos;
        }

        public void SetLocalPosXZ(Transform trans, float x, float z)
        {
            var localPos = trans.localPosition;
            localPos.x = x;
            localPos.z = z;
            trans.localPosition = localPos;
        }
        public void SetLocalPosYZ(Transform trans, float y, float z)
        {
            var localPos = trans.localPosition;    
            localPos.y = y;
            localPos.z = z;
            trans.localPosition = localPos;
        }

        public void SetLocalPosXYZ(Transform trans, float x, float y, float z)
        {
            var localPos = trans.localPosition;
            localPos.x = x;
            localPos.y = y;
            localPos.z = z;
            trans.localPosition = localPos;
        }

        //Reset Transform
        public void TransIdentity(Transform trans)
        {
            trans.localPosition = Vector3.zero;
            trans.localScale = Vector3.one;
            trans.localRotation = Quaternion.identity;
        }

        //Timer
        public void Delay(float time,Action onFinish)
        {
            StartCoroutine(DelayCoroutine(time, onFinish));
        }

        private IEnumerator DelayCoroutine(float time,Action onFinish)
        {
            yield return new WaitForSeconds(time);
            onFinish();
        }
        
    }

}