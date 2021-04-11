using UnityEngine;

namespace Combat
{
    public class Hurtbox : MonoBehaviour
    {
        private static readonly int Hit = Animator.StringToHash("hit");
        [SerializeField] private SphereCollider hitbox;
        private Health _health;
        private Actions _actions;

        private void Start()
        {
            hitbox.enabled = false;
            _health = GetComponentInParent<Health>();
            _actions = GetComponentInParent<Actions>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Actions enemyActions = other.GetComponentInParent<Actions>();
            if (_health.IsInvulnerable) return;
            if (_health.IsShielded && !enemyActions.IsPenetrating)
            {
                _actions.Rage += _actions.RageGain;
                return;
            }
            if (enemyActions.IsStunning) _actions.PerformAction(Actions.Type.Stunned);
            ApplyDamage(enemyActions);
        }
        private void ApplyDamage(Actions enemyActions)
        {
            GetComponentInParent<Animator>().SetTrigger(Hit);
            _health.Current -= enemyActions.Damage;
            enemyActions.Rage += enemyActions.RageGain;
        }

        public void EnableHitbox()
        {
            hitbox.enabled = true;
        }

        public void DisableHitbox()
        {
            hitbox.enabled = false;
        }

        public void DisableHurtbox()
        {
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}