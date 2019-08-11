using System.Reflection;
using System;

namespace Sim_FrameWork
{
    public abstract class Singleton<T> where T:Singleton<T>
    {
        private static T m_instance=null;
        public static T Instance
        {
            get
            {
                if (Instance == null)
                {
                    var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                    if (ctor == null)
                    {
                        throw new Exception("Non Public Ctor not found");
                    }
                    m_instance = ctor.Invoke(null) as T;
                }
                return m_instance;
            }
        }

        protected Singleton() { }
    }
}
