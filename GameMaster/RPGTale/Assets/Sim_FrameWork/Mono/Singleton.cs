using System.Reflection;
using System;

namespace Sim_FrameWork
{
    public abstract class Singleton<T> where T : new()
    {
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
    }
}
