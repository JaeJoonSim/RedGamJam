using BlueRiver.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace BlueRiver.Engine
{
	[System.Serializable]
    public class SurfaceModifierTarget
    {
        public PlayerController TargetController;
        public Character TargetCharacter;
        public float LastForceAppliedAtX;
        public float LastForceAppliedAtY;
        public bool TargetAffectedBySurfaceModifier;
    }

    public class SurfaceModifier : MonoBehaviour
    {
        public enum ForceApplicationModes { AddForce, SetForce }

        [Header("Friction")]
        public float Friction;

        [Header("Force")]
        public ForceApplicationModes ForceApplicationMode = ForceApplicationModes.AddForce;
        public Vector2 AddedForce = Vector2.zero;
        public bool OnlyApplyForceIfGrounded = false;
        public Vector2 ForceApplicationCooldownDuration = new Vector2(0f, 0.25f);
        public bool ResetForcesOnExit = false;
        public List<SurfaceModifierTarget> _targets { get; set; }

        protected PlayerController _controller;
        protected Character _character;

        protected virtual void Awake()
        {
            _targets = new List<SurfaceModifierTarget>();
        }

        public virtual void OnTriggerStay2D(Collider2D collider)
        {
            _controller = collider.gameObject.GetComponent<PlayerController>();
            _character = collider.gameObject.GetComponent<Character>();
            
            if (_controller == null)
            {
                return;
            }
            _controller.CurrentSurfaceModifier = this;

            bool found = false;
            foreach (SurfaceModifierTarget target in _targets)
            {
                if (target.TargetController == _controller)
                {
                    found = true;
                    target.TargetAffectedBySurfaceModifier = true;
                }
            }
            if (!found)
            {
                SurfaceModifierTarget newSurfaceModifierTarget = new SurfaceModifierTarget();
                newSurfaceModifierTarget.TargetController = _controller;
                newSurfaceModifierTarget.TargetCharacter = _character;
                newSurfaceModifierTarget.TargetAffectedBySurfaceModifier = true;
                _targets.Add(newSurfaceModifierTarget);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collider)
        {
            _controller = collider.gameObject.GetComponent<PlayerController>();
            _character = collider.gameObject.GetComponent<Character>();

            if (_controller == null)
            {
                return;
            }

            bool found = false;
            int index = 0;
            int counter = 0;
            foreach (SurfaceModifierTarget target in _targets)
            {
                if (target.TargetController == _controller)
                {
                    index = counter;
                    found = true;
                }
                counter++;
            }
            if (found)
            {
                if (ResetForcesOnExit)
                {
                    _controller.SetForce(Vector2.zero);
                }
                _targets[index].TargetAffectedBySurfaceModifier = false;
            }

            _controller.CurrentSurfaceModifier = null;
        }

        /// <summary>
        /// On Update, we make sure we have a controller and a live character, and if we do, we apply a force to it
        /// </summary>
        protected virtual void Update()
        {
            ProcessSurface();
        }

        /// <summary>
        /// A method used to go through all targets and apply force if needed
        /// </summary>
        protected virtual void ProcessSurface()
        {
            if (_targets.Count == 0)
            {
                return;
            }

            bool removeNeeded = false;
            int counter = 0;
            int removeIndex = 0;
            foreach (SurfaceModifierTarget target in _targets)
            {
                if (!target.TargetAffectedBySurfaceModifier)
                {
                    continue;
                }

                _controller = target.TargetController;
                _character = target.TargetCharacter;

                if ((_character != null) && (_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead))
                {
                    removeNeeded = true;
                    removeIndex = counter;
                    _character = null;
                    _controller = null;
                    continue;
                }

                if (ForceApplicationConditionsMet())
                {
                    ApplyHorizontalForce(target);
                    ApplyVerticalForce(target);
                }
            }

            if (removeNeeded)
            {
                _targets.RemoveAt(removeIndex);
            }
        }

        /// <summary>
        /// This method returns true if conditions to apply force are met
        /// </summary>
        /// <returns></returns>
        protected virtual bool ForceApplicationConditionsMet()
        {
            if (_controller == null)
            {
                return false;
            }

            if (OnlyApplyForceIfGrounded && !_controller.State.IsGrounded)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This method applies horizontal forces if needed
        /// </summary>
        /// <param name="target"></param>
        protected virtual void ApplyHorizontalForce(SurfaceModifierTarget target)
        {
            if (Time.time - target.LastForceAppliedAtX > ForceApplicationCooldownDuration.x)
            {
                if (ForceApplicationMode == ForceApplicationModes.AddForce)
                {
                    _controller.AddHorizontalForce(AddedForce.x * 10f * Time.deltaTime);
                }
                else
                {
                    _controller.SetHorizontalForce(AddedForce.x);
                }
                target.LastForceAppliedAtX = Time.time;
            }
        }

        /// <summary>
        /// This method applies vertical forces if needed
        /// </summary>
        /// <param name="target"></param>
        protected virtual void ApplyVerticalForce(SurfaceModifierTarget target)
        {
            if (Time.time - target.LastForceAppliedAtY > ForceApplicationCooldownDuration.y)
            {
                if (ForceApplicationMode == ForceApplicationModes.AddForce)
                {
                    float verticalForce = Mathf.Sqrt(2f * AddedForce.y * -_controller.Parameters.Gravity);
                    _controller.AddVerticalForce(verticalForce);
                }
                else
                {
                    _controller.SetVerticalForce(AddedForce.y);
                }
                target.LastForceAppliedAtY = Time.time;
            }
        }
    }
}