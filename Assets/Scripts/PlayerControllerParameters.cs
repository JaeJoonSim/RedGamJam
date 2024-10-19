using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.Player
{
    [RequireComponent(typeof(Collider2D))]
    [Serializable]
    public class PlayerControllerParameters
    {
        [Header("Gravity")]
        public float gravity = -30f;
        public float fallmultiplier = 1f;
        public float ascentMultiplier = 1f;

        [Header("Speed")]
        public Vector2 maxVelocity = new Vector2(100f, 100f);
        [Tooltip("지상 속도")]
        public float speedAccelerationOnGround = 20f;
        [Tooltip("if(true) : 지상에서 감속될 때 별도의 감속값 사용. if(false) : 해당 값 사용")]
        public bool useDefaultSpeedAcceleration = false;
        [Tooltip("입력값이 없을 때 적용할 속도 수정값")]
        public float SpeedDecelerationOnGround = 20f;
        [Tooltip("공중 속도")]
        public float speedAccelerationInAir = 5f;
        [Tooltip("if(true) : 공중에서 감속될 때 별도의 감속값 사용. if(false) : 해당 값 사용")]
        public bool useDefaultSpeedDeceleration = false;
        [Tooltip("입력값이 없을 때 적용할 속도 수정값")]
        public float speedDecelerationInAir = 5f;
        [Tooltip("공통 속도")]
        public float speedFactor = 1f;

        [Header("Slopes")]
        [Range(0, 90)]
        public float maxSlopeAngle = 30f;
        [Tooltip("경사면을 걸을 때 적용할 속도 승수")]
        public AnimationCurve slopAngleSpeedFactor = new AnimationCurve(new Keyframe(-90f, 1f), new Keyframe(0f, 1f), new Keyframe(90f, 1f));

        [Header("Physics2D Interaction")]
        [Tooltip("if (true) : 캐릭터가 수평으로 충돌하는 모든 강체에 힘을 전달")]
        public bool physics2DInteraction = true;
        [Tooltip("캐릭터가 만나는 물체에 가해지는 힘")]
        public float physics2DPushForce = 2f;

        [Header("Gizmos")]
        [Tooltip("충돌을 감지하는데 사용할 레이캐스트 기즈모")]
        public bool drawRaycastsGizmos = true;
        [Tooltip("Warning Debug Gizmo")]
        public bool displayWarnings = true;
    }
}