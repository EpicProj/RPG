using UnityEngine;
using System;
using System.Collections;

namespace Sim_FrameWork
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