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
        void AnimationStart()
        {
            InputHandler.Instance.TakeAttacks = false;
        }

        void AnimationDelayedStart()
        {
            InputHandler.Instance.TakeMovement = false;
            InputHandler.Instance.TakeRotation = false;
        }

        void AnimationEnd()
        {
            if(!GetComponentInParent<Health>().isDead)
            {
                InputHandler.Instance.TakeAttacks = true;
                InputHandler.Instance.TakeMovement = true;
                InputHandler.Instance.TakeRotation = true;
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
            InputHandler.Instance.TakeMovement = false;
        }

        void AnimationIsHitEnd()
        {
            if (!GetComponentInParent<Health>().isDead)
            {
                InputHandler.Instance.TakeMovement = true;
            }
        }
    }
}
