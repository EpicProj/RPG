using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public abstract class MonoSingleton<T> : MonoBehaviour  where T:MonoSingleton<T>
    {
        protected static T m_instance = null;
        
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogWarning("MonoInstance More than 1");
                        return m_instance;
                    }
                    if (m_instance == null)
                    {
                        var instanceName = typeof(T).Name;
                        var instanceObj = GameObject.Find(instanceName);
                        if (!instanceObj)
                            instanceObj = new GameObject(instanceName);

                        m_instance = instanceObj.AddComponent<T>();
                        DontDestroyOnLoad(instanceObj);
                    }
                }
                return m_instance;
            }
        }

        protected virtual void OnDestory()
        {
            m_instance = null;
        }



    }
}
