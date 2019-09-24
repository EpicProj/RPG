using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public  class MonoSingleton<T> : MonoBehaviour  where T:MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                    Debug.LogError("MonoSingleton Not instantiate ");
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Debug.LogWarning("Get a second instance of this class" + this.GetType());
            }
        }



    }
}
