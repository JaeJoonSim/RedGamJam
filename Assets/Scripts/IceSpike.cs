using BlueRiver;
using BlueRiver.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpike : MonoBehaviour
{
    private PlayerController player;
    private Rigidbody2D playerRb;

    private void Start()
    {
        player = GameManager.Instance.player;
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            Vector2 center = transform.position;

            if (contactPoint.y > center.y && playerRb.velocity.y < 0)
            {
                if (player.GetTree() != null)
                    player.GetTree().TakeDamage();
            }
        }
    }
}
