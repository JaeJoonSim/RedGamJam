using BlueRiver.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace BlueRiver.Engine
{
    public class CharacterAbility : MonoBehaviour
    {
        [Header("Permissions")]

        public bool AbilityPermitted = true;
        public CharacterStates.MovementStates[] BlockingMovementStates;
        public CharacterStates.CharacterConditions[] BlockingConditionStates;

        public virtual bool AbilityAuthorized
        {
            get
            {
                if (_character != null)
                {
                    if ((BlockingMovementStates != null) && (BlockingMovementStates.Length > 0))
                    {
                        for (int i = 0; i < BlockingMovementStates.Length; i++)
                        {
                            if (BlockingMovementStates[i] == (_character.MovementState.CurrentState))
                            {
                                return false;
                            }
                        }
                    }

                    if ((BlockingConditionStates != null) && (BlockingConditionStates.Length > 0))
                    {
                        for (int i = 0; i < BlockingConditionStates.Length; i++)
                        {
                            if (BlockingConditionStates[i] == _character.ConditionState.CurrentState)
                            {
                                return false;
                            }
                        }
                    }
                }
                return AbilityPermitted;
            }
        }

        /// true if the ability has already been initialized
        public virtual bool AbilityInitialized { get { return _abilityInitialized; } }

        protected Character _character;
        protected Transform _characterTransform;
        protected Health _health;
        protected CharacterHorizontalMovement _characterHorizontalMovement;
        protected PlayerController _controller;
        protected InputManager _inputManager;
        protected CameraController _sceneCamera;
        protected Animator _animator;
        protected CharacterStates _state;
        protected SpriteRenderer _spriteRenderer;
        protected StateMachine<CharacterStates.MovementStates> _movement;
        protected StateMachine<CharacterStates.CharacterConditions> _condition;
        protected bool _abilityInitialized = false;
        protected CharacterGravity _characterGravity;
        protected float _verticalInput;
        protected float _horizontalInput;
        protected bool _startFeedbackIsPlaying = false;

        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public virtual string HelpBoxText() { return ""; }

        /// <summary>
        /// On Start(), we call the ability's intialization
        /// </summary>
        protected virtual void Start()
        {
            Initialization();
        }

        /// <summary>
        /// Gets and stores components for further use
        /// </summary>
        protected virtual void Initialization()
        {
            _character = this.gameObject.GetComponentInParent<Character>();
            _controller = this.gameObject.GetComponentInParent<PlayerController>();
            _characterHorizontalMovement = _character?.FindAbility<CharacterHorizontalMovement>();
            _characterGravity = _character?.FindAbility<CharacterGravity>();
            _spriteRenderer = this.gameObject.GetComponentInParent<SpriteRenderer>();
            _health = _character.CharacterHealth;
            _handleWeaponList = _character?.FindAbilities<CharacterHandleWeapon>();
            BindAnimator();
            if (_character != null)
            {
                _characterTransform = _character.transform;
                _sceneCamera = _character.SceneCamera;
                _inputManager = _character.LinkedInputManager;
                _state = _character.CharacterState;
                _movement = _character.MovementState;
                _condition = _character.ConditionState;
            }
            _abilityInitialized = true;
        }

        public virtual void SetInputManager(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        /// <summary>
        /// Binds the animator from the character and initializes the animator parameters
        /// </summary>
        public virtual void BindAnimator()
        {
            if (_character != null)
            {
                _animator = _character._animator;
            }
            if (_animator != null)
            {
                InitializeAnimatorParameters();
            }
        }

        protected virtual void InitializeAnimatorParameters()
        {

        }

        protected virtual void InternalHandleInput()
        {
            if (_inputManager == null) { return; }

            _verticalInput = _inputManager.PrimaryMovement.y;
            _horizontalInput = _inputManager.PrimaryMovement.x;

            if (_characterGravity != null)
            {
                if (_characterGravity.ShouldReverseInput())
                {
                    if (_characterGravity.ReverseVerticalInputWhenUpsideDown)
                    {
                        _verticalInput = -_verticalInput;
                    }
                    if (_characterGravity.ReverseHorizontalInputWhenUpsideDown)
                    {
                        _horizontalInput = -_horizontalInput;
                    }
                }
            }
            HandleInput();
        }

        protected virtual void HandleInput()
        {

        }

        public virtual void ResetInput()
        {
            _horizontalInput = 0f;
            _verticalInput = 0f;
        }

        public virtual void EarlyProcessAbility()
        {
            InternalHandleInput();
        }

        public virtual void ProcessAbility()
        {

        }

        public virtual void LateProcessAbility()
        {

        }

        public virtual void UpdateAnimator()
        {

        }

        public virtual void PermitAbility(bool abilityPermitted)
        {
            AbilityPermitted = abilityPermitted;
        }

        public virtual void Flip()
        {

        }

        public virtual void ResetAbility()
        {

        }

        public virtual void RegisterAnimatorParameter(string parameterName, AnimatorControllerParameterType parameterType, out int parameter)
        {
            parameter = Animator.StringToHash(parameterName);

            if (_animator == null)
            {
                return;
            }
            if (_animator.MMHasParameterOfType(parameterName, parameterType))
            {
                _character._animatorParameters.Add(parameter);
            }
        }

        protected virtual void OnRespawn()
        {
        }

        protected virtual void OnDeath()
        {
        }

        protected virtual void OnHit()
        {

        }

        protected virtual void OnEnable()
        {
            if (_health == null)
            {
                _health = this.gameObject.GetComponentInParent<Health>();
            }

            if (_health != null)
            {
                _health.OnRevive += OnRespawn;
                _health.OnDeath += OnDeath;
                _health.OnHit += OnHit;
            }
        }

        protected virtual void OnDisable()
        {
            if (_health != null)
            {
                _health.OnRevive -= OnRespawn;
                _health.OnDeath -= OnDeath;
                _health.OnHit -= OnHit;
            }
        }
    }
}