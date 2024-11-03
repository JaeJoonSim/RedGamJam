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

        public void SnowStormShot()
        {
            StartCoroutine(SnowStormCycle());
        }

        private IEnumerator SnowStormOneCycle()
        {
            StartSnowStorm();
            yield return new WaitForSeconds(snowStats.Event_Snow_Time);
            StopSnowStorm();
        }

        private IEnumerator SnowStormCycle()
        {
            while (true)
            {
                yield return new WaitForSeconds(snowStats.Event_Snow_Start + Random.Range(0, snowStats.Event_Snow_Start));
                StartSnowStorm();
                yield return new WaitForSeconds(snowStats.Event_Snow_Time);
                StopSnowStorm();
                yield return new WaitForSeconds(snowStats.Event_Snow_Cool);
            }
        }
        
        public void StartStartSnowStormUnlimitTime()
        {
            SoundManager.Instance.PlaySound("SnowWind", 3.0f);
            snowStormParticle.Play();
            isBlizzardActive = true;
            blizzardTimer = 999999;

            Debug.Log("Snowstorm started!");
        }

        public void StartSnowStorm()
        {
            snowStormParticle.Play();
            isBlizzardActive = true;
            blizzardTimer = snowStats.Event_Snow_Time;
            
            Debug.Log("Snowstorm started!");
        }

        public void StopSnowStorm()
        {
            SoundManager.Instance.StopSound();
            snowStormParticle.Stop();
            isBlizzardActive = false;
            blizzardTimer = 0;
            Debug.Log("Snowstorm ended!");
        }

        private void ApplySnowStormEffect()
        {
            if (blizzardTimer > 0)
                blizzardTimer -= Time.deltaTime;
            else
            {
                StopSnowStorm();
                return;
            }

            if (player == null)
                player = GameManager.Instance.player;

            // ������ ����� �÷��̾� ȿ�� ����
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