using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlatform : MonoBehaviour
{
    [SerializeField] protected float FallingSpeed;

    protected Tree MyCuteTree;

    private bool IsFalling;

    protected virtual void Update()
    {
        if(IsFalling)
        {
            transform.position -= new Vector3(0.0f, FallingSpeed * Time.deltaTime, 0.0f);
        }        
    }

    protected virtual void BeginEvent()
    {
        MyCuteTree.PauseDamageTime(false);
    }

    protected virtual void EndEvent()
    {

    }

    void OnTriggerEnter(Collider _other)
    {
        MyCuteTree = _other.GetComponent<Tree>();
        if (MyCuteTree != null)
        {
            BeginEvent();
        }        
    }

    void OnTriggerExit(Collider _other)
    {
        if (MyCuteTree != null)
        {
            EndEvent();
        }
    }

    protected void SetFalling(bool _value)
    {
        IsFalling = _value;
    }
}
