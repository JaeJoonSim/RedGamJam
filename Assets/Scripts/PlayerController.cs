using BlueRiver.Items;
using BlueRiver.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.Character
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private PlayerStats stats;
        private Rigidbody2D rigid2d;
        private CapsuleCollider2D col;
        private FrameInput frameInput;
        private Vector2 frameVelocity;
        private bool cachedQueryStartInColliders;

        private StartItemType selectedItem;
        private bool itemUsed = false;
        private bool isSheltered = false;

        [SerializeField] private Temperature tree;

        #region Interface

        public Vector2 FrameInput => frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float time;

        private void Awake()
        {
            GameManager.Instance.player = this;

            rigid2d = GetComponent<Rigidbody2D>();
            col = GetComponent<CapsuleCollider2D>();

            cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            PopupManager.ShowPopup<UI_Popup>("Item Selector");
        }

        private void Update()
        {
            time += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput()
        {
            frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump"),
                JumpHeld = Input.GetButton("Jump"),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (stats.SnapInput)
            {
                frameInput.Move.x = Mathf.Abs(frameInput.Move.x) < stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.x);
                frameInput.Move.y = Mathf.Abs(frameInput.Move.y) < stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.y);
            }

            if (frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = time;
            }

            if (Input.GetKeyDown(KeyCode.C) && !itemUsed)
            {
                UseItemEffect();
                itemUsed = true;
            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();

            ApplyMovement();
        }

        #region Collisions

        private float frameLeftGrounded = float.MinValue;
        private bool grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            bool groundHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.down, stats.GrounderDistance, ~stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.up, stats.GrounderDistance, ~stats.PlayerLayer);

            if (ceilingHit) frameVelocity.y = Mathf.Min(0, frameVelocity.y);

            if (!grounded && groundHit)
            {
                grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(frameVelocity.y));
            }
            else if (grounded && !groundHit)
            {
                grounded = false;
                frameLeftGrounded = time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && time < _timeJumpWasPressed + stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !grounded && time < frameLeftGrounded + stats.CoyoteTime;

        private void HandleJump()
        {
            if (!_endedJumpEarly && !grounded && !frameInput.JumpHeld && rigid2d.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (grounded || CanUseCoyote) ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            frameVelocity.y = stats.JumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (frameInput.Move.x == 0)
            {
                var deceleration = grounded ? stats.GroundDeceleration : stats.AirDeceleration;
                frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, frameInput.Move.x * stats.MaxSpeed, stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (grounded && frameVelocity.y <= 0f)
            {
                frameVelocity.y = stats.GroundingForce;
            }
            else
            {
                var inAirGravity = stats.FallAcceleration;
                if (_endedJumpEarly && frameVelocity.y > 0) inAirGravity *= stats.JumpEndEarlyGravityModifier;
                frameVelocity.y = Mathf.MoveTowards(frameVelocity.y, -stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement() => rigid2d.velocity = frameVelocity;

        #region Select Start Items
        public void SelectItem(StartItemType item)
        {
            selectedItem = item;
            itemUsed = false;
        }

        public void UseItemEffect()
        {
            switch (selectedItem)
            {
                case StartItemType.Lighter:
                    StartCoroutine(UseLighter());
                    break;
                case StartItemType.TemperatureRecovery:
                    RecoverTemperature();
                    break;
                case StartItemType.BlizzardResist:
                    ResistBlizzard();
                    break;
                case StartItemType.NoWeightPenalty:
                    IgnoreWeightPenalty();
                    break;
            }
        }

        private IEnumerator UseLighter()
        {
            tree.SetPause(true);

            yield return new WaitForSeconds(stats.LighterTime);

            tree.SetPause(false);
        }

        private void RecoverTemperature()
        {
            tree.SetValue(stats.RecoverTemperature);
        }

        private void ResistBlizzard()
        {
            Debug.Log("Blizzard resistance: No backward push or speed reduction");
        }

        private void IgnoreWeightPenalty()
        {
            tree.GetComponent<Tree>().SetWeight(0);
        }

        #endregion

        #region SnowStorm

        public void ApplySnowStormEffect(float pushSpeed, float jumpPushSpeed, float damage)
        {
            if (isSheltered) return;

            float appliedPushSpeed = !grounded ? jumpPushSpeed : pushSpeed;

            rigid2d.AddForce(new Vector2(-appliedPushSpeed, 0), ForceMode2D.Force);
            Debug.LogError("Player is pushed by the snowstorm.");
            tree.SetValue(-damage * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Shelter"))
            {
                isSheltered = true;
                Debug.Log("Player is sheltered from the blizzard.");
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Shelter"))
            {
                isSheltered = false;
                Debug.Log("Player left the shelter.");
            }
        }

        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (stats == null) Debug.LogWarning("Please assign a PlayerStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}
