using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver
{
    public class SnowParticlePosition : MonoBehaviour
    {
        [SerializeField] private ParticleSystem snowParticle;

        private void Update()
        {
            SetParticlePosition();
        }

        private void SetParticlePosition()
        {
            Vector3 cameraTopRight = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0));
            Vector3 particlePosition = new Vector3(cameraTopRight.x, cameraTopRight.y, snowParticle.transform.position.z);
            snowParticle.transform.position = particlePosition;
        }
    }
}