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

        public static Transform FindTransfrom(Transform transfrom, string name)
        {
            var trans = transfrom.Find(name);
            if (trans == null)
            {
                Debug.LogError("Find Transfrom is null !  name= " + name);
            }
            return trans;
        }

        public static bool SafeSetActive(Transform trans,bool active)
        {
            if (trans != null)
            {
                trans.gameObject.SetActive(active);
                return true;
            }
            return false;
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

        public static void ActiveCanvasGroup(CanvasGroup group,bool active)
        {
            if (active)
            {
                group.alpha = 1;
            }
            else
            {
                group.alpha = 0;
            }
            group.interactable = active;
            group.blocksRaycasts = active;
        }

    }
}