using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlatform : DefaultPlatform
{
    [SerializeField]
    private float destroyTime;

    [SerializeField]
    private float fallSpeed;

    protected override void BeginEvent()
    {
        StartCoroutine(SetDestroyPlatform());
    }

    protected override void EndEvent()
    {
        
    }

    private IEnumerator SetDestroyPlatform()
    {
        Camera mainCamera = Camera.main;

        while (true)
        {
            yield return new WaitForSeconds(destroyTime);

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
