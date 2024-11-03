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
    private bool isStartActionEnd = false;
    private bool isEndActionEnd = false;

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += ConversationEnd;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isStartActionEnd)
        {
            if (IngnoreSnowStorm)
                GameManager.Instance.playerIgnoreSnowStorm = true;
            if (IgnorePlayerMove)
                GameManager.Instance.playerCanMove = false;
            action?.Invoke();
            isStartActionEnd = true;
        }
    }

    private void ConversationEnd()
    {
        if (isStartActionEnd && !isEndActionEnd)
        {
            if (IgnorePlayerMove)
                GameManager.Instance.playerCanMove = true;
            endCallback?.Invoke();
            isEndActionEnd = true;
        }
    }
}
