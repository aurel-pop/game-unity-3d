using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        PlayerInputHandler PlayerInputHandler;
        PlayerAttacks PlayerAttacks;

        void Start()
        {
            PlayerInputHandler = GetComponent<PlayerInputHandler>();
            PlayerAttacks = GetComponent<PlayerAttacks>();
        }

        public void AnimationStart()
        {
            PlayerInputHandler.takeAttacks = false;
        }

        public void AnimationDelayedStart()
        {
            PlayerInputHandler.takeMovement = false;
            PlayerInputHandler.takeRotation = false;
        }

        public void AnimationEnd()
        {
            PlayerInputHandler.takeAttacks = true;
            PlayerInputHandler.takeMovement = true;
            PlayerInputHandler.takeRotation = true;
            PlayerAttacks.PerformQueuedAttack();
        }
    }
}
