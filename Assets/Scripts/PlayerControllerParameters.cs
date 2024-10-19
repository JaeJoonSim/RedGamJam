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
        [Tooltip("���� �ӵ�")]
        public float speedAccelerationOnGround = 20f;
        [Tooltip("if(true) : ���󿡼� ���ӵ� �� ������ ���Ӱ� ���. if(false) : �ش� �� ���")]
        public bool useDefaultSpeedAcceleration = false;
        [Tooltip("�Է°��� ���� �� ������ �ӵ� ������")]
        public float SpeedDecelerationOnGround = 20f;
        [Tooltip("���� �ӵ�")]
        public float speedAccelerationInAir = 5f;
        [Tooltip("if(true) : ���߿��� ���ӵ� �� ������ ���Ӱ� ���. if(false) : �ش� �� ���")]
        public bool useDefaultSpeedDeceleration = false;
        [Tooltip("�Է°��� ���� �� ������ �ӵ� ������")]
        public float speedDecelerationInAir = 5f;
        [Tooltip("���� �ӵ�")]
        public float speedFactor = 1f;

        [Header("Slopes")]
        [Range(0, 90)]
        public float maxSlopeAngle = 30f;
        [Tooltip("������ ���� �� ������ �ӵ� �¼�")]
        public AnimationCurve slopAngleSpeedFactor = new AnimationCurve(new Keyframe(-90f, 1f), new Keyframe(0f, 1f), new Keyframe(90f, 1f));

        [Header("Physics2D Interaction")]
        [Tooltip("if (true) : ĳ���Ͱ� �������� �浹�ϴ� ��� ��ü�� ���� ����")]
        public bool physics2DInteraction = true;
        [Tooltip("ĳ���Ͱ� ������ ��ü�� �������� ��")]
        public float physics2DPushForce = 2f;

        [Header("Gizmos")]
        [Tooltip("�浹�� �����ϴµ� ����� ����ĳ��Ʈ �����")]
        public bool drawRaycastsGizmos = true;
        [Tooltip("Warning Debug Gizmo")]
        public bool displayWarnings = true;
    }
}