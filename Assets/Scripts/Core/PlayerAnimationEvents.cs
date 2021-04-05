using Game.Combat;
using Game.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationEvents : MonoBehaviour
    {
        InputHandler inputHandler;

        void Start()
        {
            inputHandler = InputHandler.Instance;
        }

        void AnimationStart()
        {
            inputHandler.TakeAttacks = false;
        }

        void AnimationDelayedStart()
        {
            inputHandler.TakeMovement = false;
            inputHandler.TakeRotation = false;
        }

        void AnimationEnd()
        {
            if(!GetComponentInParent<Health>().isDead)
            {
                inputHandler.TakeAttacks = true;
                inputHandler.TakeMovement = true;
                inputHandler.TakeRotation = true;
                GetComponentInParent<InputAttacks>().PerformQueuedAttack();
            }

            GetComponentInParent<TriggerAttacks>().StopShielded();
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
            inputHandler.TakeMovement = false;
        }

        void AnimationIsHitEnd()
        {
            if (!GetComponentInParent<Health>().isDead)
            {
                inputHandler.TakeMovement = true;
            }
        }
    }
}
