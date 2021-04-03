using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        PlayerInputHandler playerInputHandler;

        void Awake()
        {
            playerInputHandler = GetComponentInParent<PlayerInputHandler>();
        }

        void AnimationStart()
        {
            playerInputHandler.takeAttacks = false;
        }

        void AnimationDelayedStart()
        {
            playerInputHandler.takeMovement = false;
            playerInputHandler.takeRotation = false;
        }

        void AnimationEnd()
        {
            if(!GetComponentInParent<Health>().isDead)
            {
                playerInputHandler.takeAttacks = true;
                playerInputHandler.takeMovement = true;
                playerInputHandler.takeRotation = true;
                GetComponentInParent<PlayerInputAttacks>().PerformQueuedAttack();
            } else
            {
                playerInputHandler.takeAttacks = false;
                playerInputHandler.takeMovement = false;
                playerInputHandler.takeRotation = false;
            }
        }

        void AnimationAttackHitStart()
        {
            GetComponentInChildren<Hurtbox>().EnableHitbox();
        }

        void AnimationAttackHitEnd()
        {
            GetComponentInChildren<Hurtbox>().DisableHitbox();
        }

        void AnimationIsHitStart()
        {
            playerInputHandler.takeMovement = false;
        }

        void AnimationIsHitEnd()
        {
            if (!GetComponentInParent<Health>().isDead)
            {
                playerInputHandler.takeMovement = true;
            }
        }
    }
}
