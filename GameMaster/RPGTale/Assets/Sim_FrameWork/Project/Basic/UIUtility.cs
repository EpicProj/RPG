using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public partial class UIUtility : MonoBehaviour
    {
        public static T SafeGetComponent<T>(Transform trans) where T : class
        {
            T result = null;
            try
            {
                result = trans.GetComponent<T>();
            }
            catch (Exception e)
            {
                if (result == null && trans != null)
                {
                    Debug.LogError("Get Component is null , trans=" + trans.name);
                }
                else if (trans == null)
                {
                    Debug.LogError("Transform is null");
                }
            }
            return result;
         
        }

        /// <summary>
        /// Reset Transform
        /// </summary>
        /// <param name="trans"></param>
        public static void TransIdentity(Transform trans)
        {
            trans.localPosition = Vector3.zero;
            trans.localScale = Vector3.one;
            trans.localRotation = Quaternion.identity;
        }
    }
}