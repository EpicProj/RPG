using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public static partial  class UIUtility 
    {
        public static T SafeGetComponent<T>(this Transform trans) where T : class
        {
            T result = null;
            try
            {
                result = trans.GetComponent<T>();
            }
            catch 
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
        /// Release All Child, child Must Init By ObjManager
        /// </summary>
        /// <param name="trans"></param>
        public static void ReleaseAllChildObj(this Transform trans)
        {
            foreach(Transform t in trans)
            {
                ObjectManager.Instance.ReleaseObject(t.gameObject,0);
            }
        }

        public static Transform FindTransfrom(this Transform transfrom, string name)
        {
            var trans = transfrom.Find(name);
            if (trans == null)
            {
                DebugPlus.LogError("Find Transfrom is null !  name= " + name);
            }
            return trans;
        }

        public static T SafeAddCmpt<T>(this Transform transform) where T: Component
        {
            T result = null;
            if(transform != null)
            {
                if (transform.SafeGetComponent<T>() == null)
                {
                    result= transform.gameObject.AddComponent<T>();
                    return result;
                }
                else
                {
                    return transform.SafeGetComponent<T>();
                }
            }
            return null;
        }

        public static bool SafeSetActive(this Transform trans,bool active)
        {
            if (trans != null)
            {
                if (trans.gameObject.activeSelf != active)
                {
                    trans.gameObject.SetActive(active);
                }
                return true;
            }
            return false;
        }

        public static void SafeSetActiveAllChild(this Transform trans ,bool active)
        {
            if (trans != null)
            {
                foreach(Transform t in trans)
                {
                    t.SafeSetActive(active);
                }
            }
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

        public static void InitObj(this Transform parent,string objPath,int count,int maxcount=-1)
        {
            int currentCount = parent.childCount;
            if (count < currentCount)
            {
                ///生成数量小于目前数量
                for(int i = currentCount - 1; i> count - 1; i--)
                {
                    ObjectManager.Instance.ReleaseObject(parent.GetChild(i).gameObject, 0);
                }
            }
            else if (count > currentCount)
            {
                for(int i=currentCount-1; i < count - 1; i++)
                {
                    if (maxcount != -1 && i + 1 >= maxcount)
                        return;
                    var obj = ObjectManager.Instance.InstantiateObject(objPath);
                    if (obj != null)
                    {
                        obj.transform.SetParent(parent,false);
                    }
                }
            }
        }

        public static Vector2 GetContentSizeFitterPreferredSize(this RectTransform rect,ContentSizeFitter fitter)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            return new Vector2(HandleSelfFittingAlongAxis(0, rect, fitter), HandleSelfFittingAlongAxis(1, rect, fitter));
        }

        private static float HandleSelfFittingAlongAxis(int axis,RectTransform rect,ContentSizeFitter fitter)
        {
            ContentSizeFitter.FitMode fitting = (axis == 0 ? fitter.horizontalFit : fitter.verticalFit);
            if (fitting == ContentSizeFitter.FitMode.MinSize)
            {
                return LayoutUtility.GetMinSize(rect, axis);
            }
            else
            {
                return LayoutUtility.GetPreferredSize(rect, axis);
            }
        }

        public static void ActiveCanvasGroup(this CanvasGroup group,bool active)
        {
            if (group == null)
            {
                DebugPlus.LogError("CanvasGroup is Null!");
                return;
            }
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
        public static void DoCanvasFade(this CanvasGroup group, float targetAlpha,float time)
        {
            if (group == null)
            {
                DebugPlus.LogError("CanvasGroup is Null!");
                return;
            }
            group.DOFade(targetAlpha, time);
        }
    }

    public class GeneralMaterial
    {
        /// <summary>
        /// 通用颜色变化材质
        /// </summary>
        public const string ImageColor_Mat_Path = "Assets/Material/UI/ImageColor.mat";
    }
}