using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlatform : MonoBehaviour
{
    [SerializeField] protected float FallingSpeed;

    void Start()
    {
        
    }

    protected virtual void Update()
    {
        transform.position -= new Vector3(0.0f, FallingSpeed * Time.deltaTime, 0.0f);
    }

    protected virtual void BeginEvent()
    {

    }

    protected virtual void EndEvent()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        BeginEvent();
    }

    void OnTriggerExit(Collider other)
    {
        EndEvent();
    }
}
