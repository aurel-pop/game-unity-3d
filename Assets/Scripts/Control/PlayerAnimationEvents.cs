using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        public void AnimationStart()
        {
            GetComponent<PlayerInputHandler>().takeAttacks = false;
        }

        public void AnimationDelayedStart()
        {
            GetComponent<PlayerInputHandler>().takeMovement = false;
            GetComponent<PlayerInputHandler>().takeRotation = false;
        }

        public void AnimationEnd()
        {
            GetComponent<PlayerInputHandler>().takeAttacks = true;
            GetComponent<PlayerInputHandler>().takeMovement = true;
            GetComponent<PlayerInputHandler>().takeRotation = true;
            GetComponent<PlayerInputAttacks>().PerformQueuedAttack();
        }
    }
}
