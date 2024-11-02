using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartTriggerEvent : MonoBehaviour
{
    public UnityEvent action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            action?.Invoke();
            Destroy(gameObject);
        }
    }
}
