using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlatform : DefaultPlatform
{
    [SerializeField]
    private float destroyTime;

    [SerializeField]
    private float fallSpeed;

    [SerializeField]
    private bool useBeginEvent = true;

    protected override void BeginEvent()
    {
        if (useBeginEvent)
            StartCoroutine(SetDestroyPlatform(destroyTime));
    }

    protected override void EndEvent()
    {
        
    }

    public void FallPlaform()
    {
        StartCoroutine(SetDestroyPlatform());
    }

    public IEnumerator SetDestroyPlatform(float value = 0)
    {
        Camera mainCamera = Camera.main;

        while (true)
        {
            yield return new WaitForSeconds(value);

            while (true)
            {
                transform.position -= new Vector3(0.0f, fallSpeed * Time.deltaTime, 0.0f);

                Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

                if (viewportPosition.y < 0)
                {
                    Destroy(gameObject);
                    yield break;
                }

                yield return null;
            }
        }
    }
}
