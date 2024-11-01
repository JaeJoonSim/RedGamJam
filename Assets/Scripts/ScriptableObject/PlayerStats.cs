using UnityEngine;

namespace BlueRiver.Character
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Character/PlayerStats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("LAYERS")]
        public LayerMask PlayerLayer;

        public LayerMask WallLayer;
        
        [Header("INPUT")]
        public bool SnapInput = true;

        [Range(0.01f, 0.99f)]
        public float VerticalDeadZoneThreshold = 0.3f;

        [Range(0.01f, 0.99f)]
        public float HorizontalDeadZoneThreshold = 0.1f;

        [Header("MOVEMENT")]
        public float MaxSpeed = 14;

        public float InSnowStormMoveSpeed = 7;

        public float Acceleration = 120;

        public float GroundDeceleration = 60;

        public float AirDeceleration = 30;

        public float wallSlidingSpeed = 2f;

        [Range(0f, -10f)]
        public float GroundingForce = -1.5f;

        [Range(0f, 0.5f)]
        public float GrounderDistance = 0.05f;

        [Header("JUMP")]
        public float JumpPower = 36;

        public float MaxFallSpeed = 40;

        public float FallAcceleration = 110;

        public float JumpEndEarlyGravityModifier = 3;

        public float CoyoteTime = .15f;

        public float JumpBuffer = .2f;

        [Header("ITEMS")]
        public float LighterTime = 3f;

        public float RecoverTemperature = 30f;
    }
}