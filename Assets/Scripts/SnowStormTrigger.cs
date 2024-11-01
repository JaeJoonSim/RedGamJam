using BlueRiver.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver
{
    public class SnowStormTrigger : MonoBehaviour
    {
        private PlayerController player;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (player == null)
                    player = GameManager.Instance.player;
                    
                if (player != null)
                    player.SetInSnowStorm(true);
            }
        }
    }
}