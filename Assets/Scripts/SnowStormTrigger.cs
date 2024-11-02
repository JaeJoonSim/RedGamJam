using BlueRiver.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver
{
    public class SnowStormTrigger : MonoBehaviour
    {
        private PlayerController player;
        private Collider2D playerCol;

        ParticleSystem ps;

        [SerializeField] private ParticleSystem snowStormParticle;

        List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

        private void Start()
        {
            player = GameManager.Instance.player;
            playerCol = player.GetComponent<Collider2D>();
            ps = GetComponent<ParticleSystem>();

            if (player != null)
                ps.trigger.AddCollider(player);
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other == player.gameObject)
            {
                player.SetInSnowStorm(true);
            }
        }
    }
}