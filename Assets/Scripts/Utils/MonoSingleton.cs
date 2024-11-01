using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _mutex = new object();
        
        public static T Instance
        {
            get
            {
                lock (_mutex)
                {
                    var founds = FindObjectsOfType<T>();
                    if (_instance == null)
                    {
                        if (founds.Length > 1)
                            return null;
                    }
                    else if (founds.Length == 0)
                    {
                        _instance = (T)founds[0];

                        DontDestroyOnLoad(_instance.gameObject);
                    }
                    else
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(Singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);
                    }
                    return _instance;
                }
            }
        }
    }
}