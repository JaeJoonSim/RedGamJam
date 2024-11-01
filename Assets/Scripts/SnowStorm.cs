using BlueRiver.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver
{
    public class SnowStorm : MonoBehaviour
    {
        [SerializeField] private SnowStormStats snowStats;
        [SerializeField] private PlayerController player;

        private BoxCollider2D stormArea;
        private bool isBlizzardActive = false;
        private float blizzardTimer;

        private void Start()
        {
            stormArea = GetComponent<BoxCollider2D>();

            StartCoroutine(SnowStormCycle());
        }

        private void Update()
        {
            if (isBlizzardActive)
            {
                ApplySnowStormEffect();
                MoveSnowStorm();
                AdjustStormAreaSize();
            }
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

        private void StartSnowStorm()
        {
            isBlizzardActive = true;
            blizzardTimer = snowStats.Event_Snow_Time;
            SetInitialPosition();
            Debug.Log("Snowstorm started!");
        }

        private void StopSnowStorm()
        {
            isBlizzardActive = false;
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
            if (player != null && PlayerInSnowStorm(player.transform))
            {
                player.ApplySnowStormEffect(snowStats.Event_Snow_Spd, snowStats.Event_Snow_Jump_Spd, snowStats.Event_Snow_Damage);
            }
        }

        private void MoveSnowStorm()
        {
            transform.Translate(Vector3.left * snowStats.Event_Snow_Move_Spd * Time.deltaTime);
        }

        private void SetInitialPosition()
        {
            Vector3 cameraRightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));
            var stormWidth = stormArea.size.x;
            transform.position = new Vector3(cameraRightEdge.x + stormWidth / 2, transform.position.y, transform.position.z);
        }

        private void AdjustStormAreaSize()
        {
            float newWidth = snowStats.Event_Snow_Move_Spd * snowStats.Event_Snow_Time;
            float newHeight = Camera.main.orthographicSize * 2;
            stormArea.size = new Vector2(newWidth, newHeight);
        }

        private bool PlayerInSnowStorm(Transform player)
        {
            // 여기에 눈보라 범위 내에 있는지 확인하는 로직을 추가하세요 (예: 범위 내 트리거 등)
            return stormArea.bounds.Contains(player.position);
        }

        private void OnDrawGizmos()
        {
            if (stormArea != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(stormArea.bounds.center, stormArea.bounds.size);
            }
        }
    }
}