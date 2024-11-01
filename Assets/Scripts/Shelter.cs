using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver
{
    public class Shelter : MonoBehaviour
    {
        private bool isEntered = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isEntered && collision.CompareTag("Player"))
            {
                var player = GameManager.Instance.player;

                if (player != null)
                {
                    player.RecoverTemperatureByShelter();
                    isEntered = true;
                    Debug.Log("Player entered shelter");
                }
            }
        }

    }
}