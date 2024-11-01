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
                    if (_instance == null)
                    {
                        var founds = FindObjectsOfType<T>();

                        if (founds.Length > 0)
                        {
                            _instance = founds[0];
                            if (founds.Length > 1)
                            {
                                Debug.LogWarning($"Multiple instances of singleton {typeof(T)} found. Extra instances will be destroyed.");

                                for (int i = 1; i < founds.Length; i++)
                                {
                                    Destroy(founds[i].gameObject);
                                }
                            }

                            DontDestroyOnLoad(_instance.gameObject);
                        }
                        else
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(Singleton) " + typeof(T).ToString();

                            DontDestroyOnLoad(singleton);
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
