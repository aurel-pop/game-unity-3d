using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        void AnimationStart()
        {
            GetComponent<PlayerInputHandler>().takeAttacks = false;
        }

        void AnimationDelayedStart()
        {
            GetComponent<PlayerInputHandler>().takeMovement = false;
            GetComponent<PlayerInputHandler>().takeRotation = false;
        }

        void AnimationAttackHit()
        {
            GetComponentInParent<Transform>().GetComponentInChildren<Hurtbox>().EnableHitbox();
        }

        void AnimationAttackHitEnd()
        {
            GetComponentInParent<Transform>().GetComponentInChildren<Hurtbox>().DisableHitbox();
        }

        void AnimationEnd()
        {
            GetComponent<PlayerInputHandler>().takeAttacks = true;
            GetComponent<PlayerInputHandler>().takeMovement = true;
            GetComponent<PlayerInputHandler>().takeRotation = true;

            GetComponent<PlayerInputAttacks>().PerformQueuedAttack();
        }
    }
}
