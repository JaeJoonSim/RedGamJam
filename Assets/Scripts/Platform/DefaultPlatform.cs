using BlueRiver;
using BlueRiver.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlatform : MonoBehaviour
{
    protected PlayerController player;
    [SerializeField] bool CanFalling;

    private bool IsFalling;

    private void Start()
    {
        player = GameManager.Instance.player;
    }

    protected virtual void Update()
    {
        if(IsFalling && CanFalling)
        {
            transform.position -= new Vector3(0.0f, player.stats.MaxFallSpeed * Time.deltaTime, 0.0f);
        }        
    }

    protected virtual void BeginEvent()
    {
        player.GetTree().PauseDamageTime(false);
    }

    protected virtual void EndEvent()
    {

    }

    void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            BeginEvent();
        }        
    }

    void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            EndEvent();
        }
    }

    protected void SetFalling(bool _value)
    {
        IsFalling = _value;
    }
}
