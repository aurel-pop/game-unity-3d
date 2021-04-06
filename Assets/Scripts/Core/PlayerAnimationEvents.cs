using Combat;
using Control;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationEvents : MonoBehaviour
    {
        private Health _health;

        private void Start()
        {
            _health = GetComponentInParent<Health>();
        }

        private void AnimationStart()
        {
            InputHandler.Instance.TakeAttacks = false;
        }

        private void AnimationDelayedStart()
        {
            InputHandler.Instance.TakeMovement = false;
            InputHandler.Instance.TakeRotation = false;
        }

        private void AnimationEnd()
        {
            if (_health.IsDead) return;
            _health.IsShielded = false;
            InputHandler.Instance.TakeAttacks = true;
            InputHandler.Instance.TakeMovement = true;
            InputHandler.Instance.TakeRotation = true;
            GetComponentInParent<InputAttacks>().PerformQueuedAttack();
        }

        private void AnimationAttackHitStart()
        {
            GetComponentInChildren<Hurtbox>().EnableHitbox();
        }

        private void AnimationAttackHitEnd()
        {
            GetComponentInChildren<Hurtbox>().DisableHitbox();
        }

        private void AnimationIsHitStart()
        {
            InputHandler.Instance.TakeMovement = false;
        }

        private void AnimationIsHitEnd()
        {
            if (_health.IsDead) return;
            InputHandler.Instance.TakeMovement = true;
        }
    }
}