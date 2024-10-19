using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BlueRiver.Tool
{
    public struct GameEvent
    {
        static GameEvent e;

        public string EventName;
        public int IntParameter;
        public Vector2 Vector2Parameter;
        public Vector3 Vector3Parameter;
        public bool BoolParameter;
        public string StringParameter;

        public static void Trigger(string eventName, int intParameter = 0, Vector2 vector2Parameter = default(Vector2), Vector3 vector3Parameter = default(Vector3), bool boolParameter = false, string stringParameter = "")
        {
            e.EventName = eventName;
            e.IntParameter = intParameter;
            e.Vector2Parameter = vector2Parameter;
            e.Vector3Parameter = vector3Parameter;
            e.BoolParameter = boolParameter;
            e.StringParameter = stringParameter;
            EventManager.TriggerEvent(e);
        }
    }

    public class EventManager : MonoBehaviour
    {
        private static Dictionary<Type, List<EventListenerBase>> _subscribersList;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitializeStatics()
        {
            _subscribersList = new Dictionary<Type, List<EventListenerBase>>();
        }

        static EventManager()
        {
            _subscribersList = new Dictionary<Type, List<EventListenerBase>>();
        }

        public static void AddListener<BREvent>(EventListener<BREvent> listener) where BREvent : struct
        {
            Type eventType = typeof(BREvent);

            if (!_subscribersList.ContainsKey(eventType))
            {
                _subscribersList[eventType] = new List<EventListenerBase>();
            }

            if (!SubscriptionExists(eventType, listener))
            {
                _subscribersList[eventType].Add(listener);
            }
        }

        public static void RemoveListener<BREvent>(EventListener<BREvent> listener) where BREvent : struct
        {
            Type eventType = typeof(BREvent);

            if (!_subscribersList.ContainsKey(eventType))
            {
#if EVENTROUTER_THROWEXCEPTIONS
					throw new ArgumentException( string.Format( "Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString() ) );
#else
                return;
#endif
            }

            List<EventListenerBase> subscriberList = _subscribersList[eventType];

#if EVENTROUTER_THROWEXCEPTIONS
	            bool listenerFound = false;
#endif

            for (int i = subscriberList.Count - 1; i >= 0; i--)
            {
                if (subscriberList[i] == listener)
                {
                    subscriberList.Remove(subscriberList[i]);
#if EVENTROUTER_THROWEXCEPTIONS
					    listenerFound = true;
#endif

                    if (subscriberList.Count == 0)
                    {
                        _subscribersList.Remove(eventType);
                    }

                    return;
                }
            }
        }

        public static void TriggerEvent<BREvent>(BREvent newEvent) where BREvent : struct
        {
            List<EventListenerBase> list;
            if (!_subscribersList.TryGetValue(typeof(BREvent), out list))

                return;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                (list[i] as EventListener<BREvent>).OnBREvent(newEvent);
            }
        }

        private static bool SubscriptionExists(Type type, EventListenerBase receiver)
        {
            List<EventListenerBase> receivers;

            if (!_subscribersList.TryGetValue(type, out receivers)) return false;

            bool exists = false;

            for (int i = receivers.Count - 1; i >= 0; i--)
            {
                if (receivers[i] == receiver)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }
    }

    public static class EventRegister
    {
        public delegate void Delegate<T>(T eventType);

        public static void EventStartListening<EventType>(this EventListener<EventType> caller) where EventType : struct
        {
            EventManager.AddListener<EventType>(caller);
        }

        public static void EventStopListening<EventType>(this EventListener<EventType> caller) where EventType : struct
        {
            EventManager.RemoveListener<EventType>(caller);
        }
    }

    public interface EventListenerBase { };

    public interface EventListener<T> : EventListenerBase
    {
        void OnBREvent(T eventType);
    }

    public class EventListenerWrapper<TOwner, TTarget, TEvent> : EventListener<TEvent>, IDisposable
        where TEvent : struct
    {
        private Action<TTarget> _callback;

        private TOwner _owner;
        public EventListenerWrapper(TOwner owner, Action<TTarget> callback)
        {
            _owner = owner;
            _callback = callback;
            RegisterCallbacks(true);
        }

        public void Dispose()
        {
            RegisterCallbacks(false);
            _callback = null;
        }

        protected virtual TTarget OnEvent(TEvent eventType) => default;
        public void OnBREvent(TEvent eventType)
        {
            var item = OnEvent(eventType);
            _callback?.Invoke(item);
        }

        private void RegisterCallbacks(bool b)
        {
            if (b)
            {
                this.EventStartListening<TEvent>();
            }
            else
            {
                this.EventStopListening<TEvent>();
            }
        }
    }
}