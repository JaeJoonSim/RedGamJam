using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.Engine
{
    public static class GameObjectExtensions
    {
        static List<Component> m_ComponentCache = new List<Component>();

        public static Component GetComponentNoAlloc(this GameObject @this, System.Type componentType)
        {
            @this.GetComponents(componentType, m_ComponentCache);
            Component component = m_ComponentCache.Count > 0 ? m_ComponentCache[0] : null;
            m_ComponentCache.Clear();
            return component;
        }

        public static T GetComponentNoAlloc<T>(this GameObject @this) where T : Component
        {
            @this.GetComponents(typeof(T), m_ComponentCache);
            Component component = m_ComponentCache.Count > 0 ? m_ComponentCache[0] : null;
            m_ComponentCache.Clear();
            return component as T;
        }

        public static T GetComponentAroundOrAdd<T>(this GameObject @this) where T : Component
        {
            T component = @this.GetComponentInChildren<T>(true);
            if (component == null)
            {
                component = @this.GetComponentInParent<T>();
            }
            if (component == null)
            {
                component = @this.AddComponent<T>();
            }
            return component;
        }

        public static T GetOrAddComponent<T>(this GameObject @this) where T : Component
        {
            T component = @this.GetComponent<T>();
            if (component == null)
            {
                component = @this.AddComponent<T>();
            }
            return component;
        }

        public static (T newComponent, bool createdNew) FindOrCreateObjectOfType<T>(this GameObject @this, string newObjectName, Transform parent, bool forceNewCreation = false) where T : Component
        {
            T searchedObject = (T)Object.FindObjectOfType(typeof(T));
            if ((searchedObject == null) || forceNewCreation)
            {
                GameObject newGo = new GameObject(newObjectName);
                newGo.transform.SetParent(parent);
                return (newGo.AddComponent<T>(), true);
            }
            else
            {
                return (searchedObject, false);
            }
        }
    }
}