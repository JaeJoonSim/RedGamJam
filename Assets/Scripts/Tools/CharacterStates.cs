using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.Engine
{
    public class CharacterStates
    {
        public enum CharacterConditions
        {
            Normal,
            ControlledMovement,
            Paused,
            Dead,
        }

        public enum MovementStates
        {
            Null,
            Idle,
            Walking,
            Falling,
            Running,
            Crouching,
            Dashing,
            WallClinging,
            Diving,
            Jumping,
            Pushing,
            DoubleJumping,
            WallJumping,
            Flying,
            FollowingPath,
        }
    }
}