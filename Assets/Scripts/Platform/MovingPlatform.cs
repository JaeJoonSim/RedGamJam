using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : DefaultPlatform
{
    [SerializeField]
    private Transform positionA;

    [SerializeField]
    private Transform positionB;

    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float delayTime = 2.0f;

    private Transform targetPoint;
    private bool isMoving = true;

    private void Awake()
    {
        if (positionA == null || positionB == null)
        {
            Debug.LogError("Positions A and B must be assigned.");
            enabled = false;
            return;
        }

        targetPoint = positionB;
    }

    protected override void Update()
    {
        base.Update();

        if (isMoving)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            StartCoroutine(DelayBeforeMoving());
        }
    }

    private IEnumerator DelayBeforeMoving()
    {
        isMoving = false;
        yield return new WaitForSeconds(delayTime);

        targetPoint = targetPoint == positionA ? positionB : positionA;
        isMoving = true;
    }

    protected override void BeginEvent()
    {
        if (player != null)
        {
            player.transform.SetParent(transform);
        }
    }

    protected override void EndEvent()
    {
        if (player != null)
        {
            player.transform.SetParent(null);
        }
    }
}
