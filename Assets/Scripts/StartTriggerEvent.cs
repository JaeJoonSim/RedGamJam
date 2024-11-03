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
    
    public bool IngnoreSnowStorm = true;
    public bool IgnorePlayerMove = false;    
    private bool isEnd = false;

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += ConversationEnd;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isEnd)
        {
            if (IngnoreSnowStorm)
                GameManager.Instance.playerIgnoreSnowStorm = true;
            if (IgnorePlayerMove)
                GameManager.Instance.playerCanMove = false;
            action?.Invoke();
            isEnd = true;
        }
    }

    private void ConversationEnd()
    {
        if (isEnd)
        {
            if (IgnorePlayerMove)
                GameManager.Instance.playerCanMove = true;
            endCallback?.Invoke();
        }
    }
}
