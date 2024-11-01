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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BeginEvent();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EndEvent();
        }
    }

    protected void SetFalling(bool _value)
    {
        IsFalling = _value;
    }
}
