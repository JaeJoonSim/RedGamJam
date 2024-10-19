using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.Player
{
    public class PlayerControllerState
    {
        public bool isCollidingRight { get; set; }
        public bool isCollidingLeft { get; set; }
        public bool isCollidingAbove { get; set; }
        public bool isCollidingBelow { get; set; }
        public virtual bool HasCollisions { get { return isCollidingRight || isCollidingLeft || isCollidingAbove || isCollidingBelow; } }

        public float distanceToLeftCollider;
        public float distanceToRightCollider;

        public float lateralSlopeAngle { get; set; }
        public float belowSlopeAngle { get; set; }
        public Vector2 blowSlopeNormal { get; set; }
        public float belowSlopeAngleAbsolute { get; set; }
        public bool slopeAngleOK { get; set; }
        public bool onAMovingPlatform { get; set; }

        public virtual bool isGrounded { get { return isCollidingBelow; } }
        public bool isFalling { get; set; }
        public bool isJumping { get; set; }
        public bool wasGroundedLastFrame { get; set; }
        public bool wasTouchingTheCeilingLastFrame { get; set; }
        public bool justGotGrounded { get; set; }
        public bool colliderResized { get; set; }
        public bool touchingLevelBounds { get; set; }
        
        public virtual void Reset()
        {
            isCollidingLeft = false;
            isCollidingRight = false;
            isCollidingAbove = false;
            distanceToLeftCollider = -1;
            distanceToRightCollider = -1;
            slopeAngleOK = false;
            justGotGrounded = false;
            isFalling = true;
            lateralSlopeAngle = 0;
        }

        public override string ToString()
        {
            return string.Format("(controller: collidingRight:{0} collidingLeft:{1} collidingAbove:{2} collidingBelow:{3} lateralSlopeAngle:{4} belowSlopeAngle:{5} isGrounded: {6}",
                isCollidingRight,
                isCollidingLeft,
                isCollidingAbove,
                isCollidingBelow,
                lateralSlopeAngle,
                belowSlopeAngle,
                isGrounded);
        }
    }
}