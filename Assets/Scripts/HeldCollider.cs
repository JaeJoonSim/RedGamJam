using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class HeldCollider : MonoBehaviour
{
    private Collider2D obstacleCollider;
    public float holdDuration = 2f;

    public GameObject arrowUI;
    public Image fillImage;

    private bool isPlayerInTrigger = false;
    private bool isColliderDisabled = false;
    private float holdTime = 0f;
    private bool isOpened = false;

    private void Start()
    {
        obstacleCollider = GetComponent<Collider2D>();
        arrowUI.gameObject.SetActive(false);
    }

    void Update()
    {
        obstacleCollider.enabled = !isColliderDisabled;

        if (isPlayerInTrigger && !isColliderDisabled && !isOpened)
        {
            fillImage.fillAmount = holdTime / holdDuration;

            if (fillImage.fillAmount >= 1)
                arrowUI.gameObject.SetActive(false);

            if (Input.GetKey(KeyCode.RightArrow))
            {
                holdTime += Time.deltaTime;
                if (holdTime >= holdDuration)
                {
                    DisableCollider();
                }
            }
            else
            {
                holdTime = 0f; // 키를 놓으면 타이머를 초기화합니다.
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            if (!isOpened)
                arrowUI.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            arrowUI.gameObject.SetActive(false);
            isPlayerInTrigger = false;
            holdTime = 0f;
            if (isColliderDisabled)
            {
                EnableCollider();
            }
        }
    }

    private void DisableCollider()
    {
        obstacleCollider.isTrigger = false;
        isColliderDisabled = true;
        isOpened = true;
    }

    private void EnableCollider()
    {
        obstacleCollider.isTrigger = true;
        isColliderDisabled = false;
    }
}
