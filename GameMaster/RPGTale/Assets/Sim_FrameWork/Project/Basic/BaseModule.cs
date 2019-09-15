using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class BaseModule<T> where T:new ()  {

        private static T m_Instance;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new T();
                }

                return m_Instance;
            }
        }

        public virtual void InitData() { }

        public virtual void Register() { }




    }
}