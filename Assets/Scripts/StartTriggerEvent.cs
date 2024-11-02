using BlueRiver;
using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartTriggerEvent : MonoBehaviour
{
    public UnityEvent action;
    public UnityEvent endCallback;

    private bool isEnd = false;

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += ConversationEnd;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.playerIgnoreSnowStorm = true;
            action?.Invoke();
            isEnd = true;
        }
    }

    private void ConversationEnd()
    {
        if (isEnd)
        {
            endCallback?.Invoke();
            Destroy(gameObject);
        }
    }
}
