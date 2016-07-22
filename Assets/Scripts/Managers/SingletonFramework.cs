using System;
using System.Collections;
using System.Text;
using System.Reflection;

namespace Framework
{
    public abstract class SingletonFramework<T> where T : SingletonFramework<T>
    {
        protected static T m_instance = null;
        
        protected SingletonFramework()
        {

        }

        public static T GetInstance()
        {
            if (m_instance == null)
            {
                ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                if (ctor == null)
                    throw new Exception("Non-public ctor() not found!");
                m_instance = ctor.Invoke(null) as T;
            }

            return m_instance;
        }
    }
}

