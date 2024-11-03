using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnStartEvent : MonoBehaviour
{
    public UnityEvent action;

    private void Start()
    {
        action?.Invoke();
    }
}
