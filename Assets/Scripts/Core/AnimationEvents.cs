using Combat;
using Control;
using Enemy;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    public class AnimationEvents : MonoBehaviour
    {
        private Health _health;
        private Actions _actions;
        private Hurtbox _hurtbox;
        private AI _ai;
        [SerializeField] private GameObject[] vfx;

        private void Start()
        {
            _health = GetComponentInParent<Health>();
            _actions = GetComponentInParent<Actions>();
            _hurtbox = GetComponentInChildren<Hurtbox>();
            if (!CompareTag("Player")) _ai = GetComponentInParent<AI>();
        }

        private void AnimationStart()
        {
            if (CompareTag("Player")) InputHandler.Instance.TakeAttacks = false;
        }

        private void AnimationStartShield()
        {
            if(CompareTag("Player")) InputHandler.Instance.TakeAttacks = false;
            _health.IsShielded = true;
            SpawnVFX(2);
        }
        
        private void AnimationStartShieldrun()
        {
            if(CompareTag("Player")) InputHandler.Instance.TakeAttacks = false;
            _health.IsShielded = true;
            SpawnVFX(3);
        }
        
        private void AnimationStartInvulnerable()
        {
            if(CompareTag("Player")) InputHandler.Instance.TakeAttacks = false;
            _health.IsInvulnerable = true;
            SpawnVFX(4);
        }

        private void AnimationDelayedStart()
        {
            if (!CompareTag("Player")) return;
            InputHandler.Instance.TakeMovement = false;
            InputHandler.Instance.TakeRotation = false;
        }

        private void AnimationAttackHitStart()
        {
            if (_actions.IsPenetrating) SpawnVFX(0);
            if (_actions.IsStunning) SpawnVFX(1);
            _hurtbox.EnableHitbox();
        }

        private void AnimationAttackHitEnd()
        {
            _hurtbox.DisableHitbox();
        }

        private void AnimationIsHitStart()
        {
            if (CompareTag("Player")) InputHandler.Instance.TakeMovement = false;
            else _ai.GotHit();
        }

        private void AnimationIsHitEnd()
        {
            if (_health.IsDead) return;
            if (CompareTag("Player")) InputHandler.Instance.TakeMovement = true;
        }
        
        private void AnimationEnd()
        {
            if (_health.IsDead) return;
            ResetActions();
            if (CompareTag("Player"))
            {
                ResetInputs();
                InputCombat.Instance.PerformQueuedAttack();
            }
            else
            {
               _ai.ExitState();
            }
        }
        
        private void ResetActions()
        {
            _health.IsShielded = false;
            _health.IsInvulnerable = false;
            _health.IsStunned = false;
            _actions.IsPenetrating = false;
            _actions.IsStunning = false;
            _actions.RageGain = 0;
            _actions.Damage = 0;
        }
        private static void ResetInputs()
        {
            InputHandler.Instance.TakeAttacks = true;
            InputHandler.Instance.TakeMovement = true;
            InputHandler.Instance.TakeRotation = true;
        }
        
        private void SpawnVFX(int index)
        {
            Instantiate(vfx[index], transform);
        }
    }
}