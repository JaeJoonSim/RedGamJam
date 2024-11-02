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
        public PlayerStats stats;
        private Rigidbody2D rigid2d;
        private CapsuleCollider2D col;
        private FrameInput frameInput;
        private Vector2 frameVelocity;
        private bool cachedQueryStartInColliders;

        [SerializeField] private Transform treeArea;
        private StartItemType selectedItem;
        private TreeItemType selectedTree;
        private bool itemUsed = false;
        private bool treeSelect = false;
        private bool isSheltered = false;
        private bool ignoreWeightPenalty = false;
        private bool inSnowStorm = false;
        private bool isResistSnowStorm = false;

        [SerializeField]
        private Animator anim;

        [SerializeField]
        private PlayerParticleController particleController;

        private bool isLighterActive = false;

        private float lighterRemainingTime;

        private Coroutine lighterCoroutine;

        private Coroutine snowStormCoroutine = null;

        [SerializeField] private Tree tree;

        #region Interface

        public Vector2 FrameInput => frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float time;

        private bool playerDeath = false;

        private void Awake()
        {
            GameManager.Instance.player = this;

            rigid2d = GetComponent<Rigidbody2D>();
            col = GetComponent<CapsuleCollider2D>();

            cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            PopupManager.ShowPopup<UI_Popup>("Item Selector");
        }

        private void Start()
        {
            lighterRemainingTime = stats.LighterTime;
        }

        private void Update()
        {
            time += Time.deltaTime;
            GatherInput();

            if (!playerDeath)
            {
                if (tree != null && tree.GetTemperature() < 0)
                {
                    playerDeath = true;
                    GameManager.Instance.playerDeath = true;
                }
            }
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
                UseItemEffect(true);

            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                UseItemEffect(false);
            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();

            ApplyMovement();
            SetAnimatorParam();
        }

        private void SetAnimatorParam()
        {
            anim.SetBool("IsMove", frameInput.Move.x != 0);

            if (frameInput.Move.x > 0)
                anim.transform.localScale = new Vector3(-1, 1);
            else if (frameInput.Move.x < 0)
                anim.transform.localScale = new Vector3(1, 1);
            
            anim.SetBool("IsSlip", inSnowStorm);

            if (inSnowStorm && particleController.SearchParticle("SlipEffect").isPlaying == false && frameVelocity.x == 0)
                particleController.PlayParticle("SlipEffect");
            else if (inSnowStorm && frameVelocity.x != 0 || !inSnowStorm)
                particleController.StopParticle("SlipEffect");

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

                anim.SetTrigger("IsLanding");
                particleController.PlayParticle("LandEffect");
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

            if (grounded || CanUseCoyote) anim.SetTrigger("IsJump");

            _jumpToConsume = false;
        }

        public void ExecuteJump()
        {
            if (particleController.SearchParticle("SlipEffect").isPlaying)
                particleController.StopParticle("SlipEffect");
            
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
                float speed = 0; ;

                if (tree == null || ignoreWeightPenalty)
                    speed = stats.MaxSpeed;
                else if (inSnowStorm && !isResistSnowStorm)
                    speed = stats.MaxSpeed - stats.InSnowStormMoveSpeed;
                else
                    speed = tree.GetWeight();

                frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, frameInput.Move.x * speed, stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (grounded && frameVelocity.y <= 0f)
            {
                frameVelocity.y = stats.GroundingForce;
                anim.SetBool("IsFall", false);
            }
            else
            {
                var inAirGravity = stats.FallAcceleration;
                if (_endedJumpEarly && frameVelocity.y > 0) inAirGravity *= stats.JumpEndEarlyGravityModifier;
                frameVelocity.y = Mathf.MoveTowards(frameVelocity.y, -stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);

                if (frameVelocity.y < 0)
                    anim.SetBool("IsFall", true);
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

        public Tree GetTree()
        {
            return tree;
        }

        public void SelectTree(Tree tree)
        {
            if (!treeSelect && tree != null)
            {
                this.tree = Instantiate(tree, treeArea);
                treeSelect = true;
            }
        }

        public void UseItemEffect(bool isHolding)
        {
            if (lighterCoroutine != null)
            {
                isLighterActive = false;
                StopCoroutine(lighterCoroutine);
            }

            if (!isHolding) return;

            if (selectedItem != StartItemType.Lighter)
                itemUsed = true;

            switch (selectedItem)
            {
                case StartItemType.Lighter:
                    HoldLighter(isHolding);
                    break;
                case StartItemType.TemperatureRecovery:
                    RecoverTemperature();
                    break;
                case StartItemType.SnowStormResist:
                    ResistSnowStorm();
                    break;
                case StartItemType.NoWeightPenalty:
                    IgnoreWeightPenalty();
                    break;
            }
        }

        private void HoldLighter(bool isHolding)
        {
            if (isHolding && lighterRemainingTime > 0 && !isLighterActive)
            {
                isLighterActive = true;
                lighterCoroutine = StartCoroutine(UseLighter());
                Debug.Log("Lighter activated");
            }
            else if (!isHolding && isLighterActive)
            {
                if (lighterCoroutine != null)
                {
                    StopCoroutine(lighterCoroutine);
                }

                if (tree != null)
                    tree.PauseDamageTime(false);
                isLighterActive = false;
                Debug.Log("Lighter deactivated");
            }
        }

        private IEnumerator UseLighter()
        {
            if (tree != null)
                tree.PauseDamageTime(true);

            float startTime = Time.time;
            while (lighterRemainingTime > 0 && isLighterActive)
            {
                yield return null;
                lighterRemainingTime -= Time.time - startTime;
                startTime = Time.time;
            }

            if (tree != null)
                tree.PauseDamageTime(false);
            isLighterActive = false;
            Debug.Log("Lighter effect ended");
        }

        private void RecoverTemperature()
        {
            tree.StartRecoverLoop(stats.RecoverTemperature);
        }

        private void ResistSnowStorm()
        {
            isResistSnowStorm = true;
        }

        private void IgnoreWeightPenalty()
        {
            ignoreWeightPenalty = true;
        }

        #endregion

        #region SnowStorm

        public void ApplySnowStormEffect(float pushSpeed, float jumpPushSpeed, float damage)
        {
            if (isSheltered || !inSnowStorm) return;

            if (!isResistSnowStorm)
            {
                float appliedPushSpeed = !grounded ? jumpPushSpeed : pushSpeed;

                rigid2d.AddForce(new Vector2(-appliedPushSpeed, 0), ForceMode2D.Force);
            }
            Debug.Log("Player is pushed by the snowstorm.");
            if (tree != null)
                tree.StartRecoverLoop(-damage * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Shelter"))
            {
                if (IsCollisionAreaAboveThreshold(collision, stats.ShelterThreshold))
                {
                    isSheltered = true;
                    inSnowStorm = false;
                    Debug.Log("Player is sheltered from the blizzard.");
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Shelter"))
            {
                if (IsCollisionAreaAboveThreshold(collision, stats.ShelterThreshold))
                {
                    isSheltered = true;
                    inSnowStorm = false;
                    Debug.Log("Player is sheltered from the blizzard.");
                }
                else
                {
                    isSheltered = false;
                    Debug.Log("Player left the shelter.");
                }
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

        public bool IsMovingTowardsShelter(Vector3 shelterPosition)
        {
            Vector2 directionToShelter = (shelterPosition - transform.position).normalized;
            return Vector2.Dot(frameInput.Move, directionToShelter) > 0.5f;
        }

        public bool IsMovingAwayFromShelter(Vector3 shelterPosition)
        {
            Vector2 directionToShelter = (shelterPosition - transform.position).normalized;
            return Vector2.Dot(frameInput.Move, directionToShelter) < -0.5f;
        }

        public void LeaveShelter()
        {
            isSheltered = false;
        }

        public void SetInSnowStorm(bool value)
        {
            inSnowStorm = value;

            if (value)
            {
                if (snowStormCoroutine != null)
                    StopCoroutine(snowStormCoroutine);

                snowStormCoroutine = StartCoroutine(ResetInSnowStormAfterDelay(0.1f));
            }
        }

        private IEnumerator ResetInSnowStormAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            inSnowStorm = false;
            snowStormCoroutine = null;
        }

        private bool IsCollisionAreaAboveThreshold(Collider2D collision, float threshold)
        {
            Bounds playerBounds = GetComponent<Collider2D>().bounds;
            Bounds shelterBounds = collision.bounds;

            if (playerBounds.Intersects(shelterBounds))
            {
                Bounds intersectionBounds = GetIntersectionBounds(playerBounds, shelterBounds);
                float intersectionArea = intersectionBounds.size.x * intersectionBounds.size.y;
                float playerArea = playerBounds.size.x * playerBounds.size.y;

                return (intersectionArea / playerArea) >= threshold;
            }

            return false;
        }

        private Bounds GetIntersectionBounds(Bounds a, Bounds b)
        {
            Vector3 min = Vector3.Max(a.min, b.min);
            Vector3 max = Vector3.Min(a.max, b.max);

            if (min.x > max.x || min.y > max.y)
            {
                return new Bounds();
            }

            return new Bounds((min + max) / 2, max - min);
        }

        #endregion

        #region Shelter

        public void RecoverTemperatureByShelter()
        {
            if (tree != null)
                tree.RecoverByMaxTemperature();
        }

        #endregion
        public bool GetIsResistSnowStorm()
        {
            return isResistSnowStorm;
        }

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
