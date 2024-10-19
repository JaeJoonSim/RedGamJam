using BlueRiver.Managers;
using BlueRiver.Tool;
using BlueRiver.Engine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

namespace BlueRiver.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        public enum UpdateModes
        {
            Update,
            FixedUpdate,
            LateUpdate
        }

        public PlayerControllerState State { get; private set; }

        [Tooltip("Initial Parameters")]
        public PlayerControllerParameters defaultParameters;
        public PlayerControllerParameters Parameters { get { return _overrideParameters ?? defaultParameters; } }

        [Header("Collisions")]
        public LayerMask platformMask = LayerManager.PlatformsLayerMask | LayerManager.PushableLayerMask;
        public LayerMask stairsMask = LayerManager.StairsLayerMask;

        public enum RaycastDirections { up, down, left, right }
        public enum DetachmentMethods { Layer, Object, LayerChange }
        public DetachmentMethods detachmentMethod = DetachmentMethods.Layer;

        

        [ReadOnly]
        public GameObject StandingOn;
        public GameObject StandingOnLastFrame { get; protected set; }
        public Collider2D StandingOnCollider { get; protected set; }
        public GameObject LastStandingOn { get; protected set; }
        public Vector2 Speed { get { return _speed; } }
        public virtual Vector2 WorldSpeed { get { return _worldSpeed; } }
        public virtual Vector2 ForcesApplied { get; protected set; }
        public virtual GameObject CurrentWallCollider { get; protected set; }

        [Header("Raycasting")]
        public UpdateModes UpdateMode = UpdateModes.Update;
        public int NumberOfHorizontalRays = 8;
        public int NumberOfVerticalRays = 8;
        [FormerlySerializedAs("RayOffset")]
        public float RayOffsetHorizontal = 0.05f;
        public float RayOffsetVertical = 0.05f;
        public float RayExtraLengthHorizontal = 0f;
        public float RayExtraLengthVertical = 0f;

        public float CrouchedRaycastLengthMultiplier = 1f;
        public bool CastRaysOnBothSides = false;
        public float DistanceToTheGroundRayMaximumLength = 100f;
        public float OnMovingPlatformRaycastLengthMultiplier = 2f;
        public float ObstacleHeightTolerance = 0.05f;

        [Header("Stickiness")]
        public bool StickToSlopes = false;
        public float StickyRaycastLength = 0f;
        public float StickToSlopesOffsetY = 0.2f;
        [ReadOnly]
        public float TimeAirborne = 0f;

        [Header("Safety")]
        public bool SafeSetTransform = false;
        public bool AutomaticallySetPhysicsSettings = true;

        public bool AutomaticGravitySettings = true;

        public bool PerformSafetyBoxcast = false;
        public bool SafetyBoxcastInAirOnly = true;
        public Vector2 SafetyBoxcastSizeRatio = new Vector2(0.8f, 0.8f);
        public float ExtractionIncrement = 0.05f;
        public int MaxExtractionsIterations = 10;

        public Vector3 ColliderSize => Vector3.Scale(transform.localScale, _boxCollider.size);
        public Vector2 ColliderOffset => _boxCollider.offset;
        public Vector3 ColliderCenterPosition => _boxCollider.bounds.center;

        public virtual Vector3 ColliderBottomPosition
        {
            get
            {
                _colliderBottomCenterPosition.x = _boxCollider.bounds.center.x;
                _colliderBottomCenterPosition.y = _boxCollider.bounds.min.y;
                _colliderBottomCenterPosition.z = 0;
                return _colliderBottomCenterPosition;
            }
        }

        public virtual Vector3 ColliderLeftPosition
        {
            get
            {
                _colliderLeftCenterPosition.x = _boxCollider.bounds.min.x;
                _colliderLeftCenterPosition.y = _boxCollider.bounds.center.y;
                _colliderLeftCenterPosition.z = 0;
                return _colliderLeftCenterPosition;
            }
        }

        public virtual Vector3 ColliderTopPosition
        {
            get
            {
                _colliderTopCenterPosition.x = _boxCollider.bounds.center.x;
                _colliderTopCenterPosition.y = _boxCollider.bounds.max.y;
                _colliderTopCenterPosition.z = 0;
                return _colliderTopCenterPosition;
            }
        }

        public virtual Vector3 ColliderRightPosition
        {
            get
            {
                _colliderRightCenterPosition.x = _boxCollider.bounds.max.x;
                _colliderRightCenterPosition.y = _boxCollider.bounds.center.y;
                _colliderRightCenterPosition.z = 0;
                return _colliderRightCenterPosition;
            }
        }

        public bool isGravityActive => _gravityActive;
        public virtual float deltaTime => _update ? Time.deltaTime : Time.fixedDeltaTime;
        public float friction => _friction;
        public SurfaceModifier currentSurfaceModifier { get; set; }
        public GameObject[] standingOnArray { get; set; }

        public virtual float Width()
        {
            return _boundsWidth;
        }

        public virtual float Height()
        {
            return _boundsHeight;
        }

        public virtual Vector2 Bounds
        {
            get
            {
                _bounds.x = _boundsWidth;
                _bounds.y = _boundsHeight;
                return _bounds;
            }
        }

        public virtual Vector3 BoundsTopLeftCorner
        {
            get
            {
                return _boundsTopLeftCorner;
            }
        }

        public virtual Vector3 BoundsBottomLeftCorner
        {
            get
            {
                return _boundsBottomLeftCorner;
            }
        }

        public virtual Vector3 BoundsTopRightCorner
        {
            get
            {
                return _boundsTopRightCorner;
            }
        }

        public virtual Vector3 BoundsBottomRightCorner
        {
            get
            {
                return _boundsBottomRightCorner;
            }
        }

        public virtual Vector3 BoundsTop
        {
            get
            {
                return (_boundsTopLeftCorner + _boundsTopRightCorner) / 2;
            }
        }

        public virtual Vector3 BoundsBottom
        {
            get
            {
                return (_boundsBottomLeftCorner + _boundsBottomRightCorner) / 2;
            }
        }

        public virtual Vector3 BoundsRight
        {
            get
            {
                return (_boundsTopRightCorner + _boundsBottomRightCorner) / 2;
            }
        }

        public virtual Vector3 BoundsLeft
        {
            get
            {
                return (_boundsTopLeftCorner + _boundsBottomLeftCorner) / 2;
            }
        }

        public virtual Vector3 BoundsCenter
        {
            get
            {
                return _boundsCenter;
            }
        }

        public virtual float DistanceToTheGround
        {
            get
            {
                return _distanceToTheGround;
            }
        }

        public virtual Vector2 ExternalForce
        {
            get
            {
                return _externalForce;
            }
        }

        protected PlayerControllerParameters _overrideParameters;
        protected Vector2 _speed;
        protected float _friction = 0;
        protected float _fallSlowFactor;
        protected float _currentGravity = 0;
        protected Vector2 _externalForce;
        protected Vector2 _newPosition;
        protected Transform _transform;
        protected BoxCollider2D _boxCollider;
        protected LayerMask _platformMaskSave;
        protected LayerMask _raysBelowLayerMaskPlatforms;
        protected int _savedBelowLayer;
        protected bool _gravityActive = true;
        protected Collider2D _ignoredCollider = null;
        protected bool _collisionsOnWithStairs = false;

        protected const float _smallValue = 0.0001f;
        protected const float _movingPlatformsGravity = -500;

        protected Vector2 _originalColliderSize;
        protected Vector2 _originalColliderOffset;
        protected Vector2 _originalSizeRaycastOrigin;

        protected Vector3 _crossBelowSlopeAngle;

        protected RaycastHit2D[] _sideHitsStorage;
        protected RaycastHit2D[] _belowHitsStorage;
        protected RaycastHit2D[] _aboveHitsStorage;
        protected RaycastHit2D _stickRaycastLeft;
        protected RaycastHit2D _stickRaycastRight;
        protected RaycastHit2D _stickRaycast;
        protected RaycastHit2D _distanceToTheGroundRaycast;
        protected float _movementDirection;
        protected float _storedMovementDirection = 1;
        protected const float _movementDirectionThreshold = 0.0001f;

        protected Vector2 _horizontalRayCastFromBottom = Vector2.zero;
        protected Vector2 _horizontalRayCastToTop = Vector2.zero;
        protected Vector2 _verticalRayCastFromLeft = Vector2.zero;
        protected Vector2 _verticalRayCastToRight = Vector2.zero;
        protected Vector2 _aboveRayCastStart = Vector2.zero;
        protected Vector2 _aboveRayCastEnd = Vector2.zero;
        protected Vector2 _rayCastOrigin = Vector2.zero;

        protected Vector3 _colliderBottomCenterPosition;
        protected Vector3 _colliderLeftCenterPosition;
        protected Vector3 _colliderRightCenterPosition;
        protected Vector3 _colliderTopCenterPosition;

        protected bool _update;

        protected RaycastHit2D[] _raycastNonAlloc = new RaycastHit2D[0];

        protected Vector2 _boundsTopLeftCorner;
        protected Vector2 _boundsBottomLeftCorner;
        protected Vector2 _boundsTopRightCorner;
        protected Vector2 _boundsBottomRightCorner;
        protected Vector2 _boundsCenter;
        protected Vector2 _bounds;
        protected float _boundsWidth;
        protected float _boundsHeight;
        protected float _distanceToTheGround;
        protected Vector2 _worldSpeed;

        protected List<RaycastHit2D> _contactList;
        protected bool _shouldComputeNewSpeed = false;
        protected Vector2 _safetyExtractionSize = Vector2.zero;
        
        protected virtual void Awake()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            _transform = transform;
            _boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
            _originalColliderSize = _boxCollider.size;
            _originalColliderOffset = _boxCollider.offset;
            currentSurfaceModifier = null;

            if ((_boxCollider.offset.x != 0) && (Parameters.displayWarnings))
            {
                Debug.LogWarning("The boxcollider for " + gameObject.name + " should have an x offset set to zero. Right now this may cause issues when you change direction close to a wall.");
            }

            _contactList = new List<RaycastHit2D>();
            State = new PlayerControllerState();

            CachePlatformMask();
            _sideHitsStorage = new RaycastHit2D[NumberOfHorizontalRays];
            _belowHitsStorage = new RaycastHit2D[NumberOfVerticalRays];
            _aboveHitsStorage = new RaycastHit2D[NumberOfVerticalRays];
            _update = (UpdateMode == UpdateModes.Update);

            standingOnArray = new GameObject[NumberOfVerticalRays];

            CurrentWallCollider = null;
            State.Reset();
            SetRaysParameters();

            ApplyGravitySettings();
            ApplyPhysicsSettings();
        }

        protected virtual void ApplyPhysicsSettings()
        {
            if (AutomaticallySetPhysicsSettings)
            {
                Physics2D.queriesHitTriggers = true;
                Physics2D.queriesStartInColliders = true;
                Physics2D.callbacksOnDisable = true;
                Physics2D.reuseCollisionCallbacks = true;
                Physics2D.autoSyncTransforms = false;

                if (platformMask.Contains(this.gameObject.layer))
                {
                    Physics2D.queriesStartInColliders = false;
                }
            }
        }

        protected virtual void ApplyGravitySettings()
        {
            if (AutomaticGravitySettings)
            {
                _characterGravity = this.gameObject.GetComponentNoAlloc<Character>()?.FindAbility<CharacterGravity>();
                if (_characterGravity == null)
                {
                    this.transform.rotation = Quaternion.identity;
                }
            }
        }
    }
}