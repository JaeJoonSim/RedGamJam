using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlueRiver.Character
{
    public class PlayerParticleController : MonoBehaviour
    {
        public List<PlayerParticle> particles = new List<PlayerParticle>();

        private void Start()
        {
            var childParticles = GetComponentsInChildren<ParticleSystem>();

            foreach (var childParticle in childParticles)
            {
                if (particles.Contains(particles.FirstOrDefault(p => p.name == childParticle.name))) continue;

                particles.Add(new PlayerParticle
                {
                    name = childParticle.name,
                    particle = childParticle
                });
            }
        }

        public ParticleSystem SearchParticle(string name)
        {
            var ps = particles.FirstOrDefault(p => p.name == name);

            return ps?.particle;
        }

        public void playParticle(string name)
        {
            var ps = particles.FirstOrDefault(particles => particles.name == name);

            if (ps == null) return;

            ps.particle.Play();
        }

        [Serializable]
        public class PlayerParticle
        {
            public string name;
            public ParticleSystem particle;
        }

    }
}