using BlueRiver.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver
{
    public class SnowStorm : MonoBehaviour
    {
        [SerializeField] private SnowStormStats snowStats;
        [SerializeField] private ParticleSystem snowStormParticle;
        [SerializeField] private PlayerController player;
        [SerializeField] private bool startAwake = false;


        private bool isBlizzardActive = false;
        private float blizzardTimer;

        private void Start()
        {
            if (startAwake) 
                StartCoroutine(SnowStormCycle());
        }

        private void Update()
        {
            if (isBlizzardActive)
            {
                ApplySnowStormEffect();
                SetParticlePosition();
            }
        }

        public void SnowStormOneShot()
        {
            StartCoroutine(SnowStormOneCycle());
        }

        private IEnumerator SnowStormOneCycle()
        {
            snowStormParticle.Play();
            StartSnowStorm();
            yield return new WaitForSeconds(snowStats.Event_Snow_Time);
            StopSnowStorm();
        }

        private IEnumerator SnowStormCycle()
        {
            while (true)
            {
                yield return new WaitForSeconds(snowStats.Event_Snow_Start + Random.Range(0, snowStats.Event_Snow_Start));
                snowStormParticle.Play();
                StartSnowStorm();
                yield return new WaitForSeconds(snowStats.Event_Snow_Time);
                StopSnowStorm();
                yield return new WaitForSeconds(snowStats.Event_Snow_Cool);
            }
        }

        private void StartSnowStorm()
        {
            isBlizzardActive = true;
            blizzardTimer = snowStats.Event_Snow_Time;
            
            Debug.Log("Snowstorm started!");
        }

        private void StopSnowStorm()
        {
            isBlizzardActive = false;
            snowStormParticle.Stop();
            Debug.Log("Snowstorm ended!");
        }

        private void ApplySnowStormEffect()
        {
            blizzardTimer -= Time.deltaTime;
            if (blizzardTimer <= 0)
            {
                StopSnowStorm();
                return;
            }

            if (player == null)
                player = GameManager.Instance.player;

            // 눈보라에 노출된 플레이어 효과 적용
            if (player != null)
            {
                player.ApplySnowStormEffect(snowStats.Event_Snow_Spd, snowStats.Event_Snow_Jump_Spd, snowStats.Event_Snow_Damage);
            }
        }

        private void SetParticlePosition()
        {
            Vector3 cameraTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
            Vector3 particlePosition = new Vector3(cameraTopRight.x, cameraTopRight.y, snowStormParticle.transform.position.z);
            snowStormParticle.transform.position = particlePosition;
        }
    }
}