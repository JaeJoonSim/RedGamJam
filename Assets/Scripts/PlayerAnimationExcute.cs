using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.Character
{
    public class PlayerAnimationExcute : MonoBehaviour
    {
        [SerializeField] private PlayerController player;

        public void JumpExcute()
        {
            if (player == null)
                player = GameManager.Instance.player;

            player.ExecuteJump();
        }
    }
}