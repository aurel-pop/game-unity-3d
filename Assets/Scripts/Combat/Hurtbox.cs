using UnityEngine;

namespace Combat
{
    public class Hurtbox : MonoBehaviour
    {
        private static readonly int Hit = Animator.StringToHash("hit");
        [SerializeField] private SphereCollider hitbox;
        private Health _health;
        private Attacks _attacks;

        private void Start()
        {
            hitbox.enabled = false;
            _health = GetComponentInParent<Health>();
            _attacks = GetComponentInParent<Attacks>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Attacks otherAttacks = other.GetComponentInParent<Attacks>();
            if (_health.IsInvulnerable) return;
            if (_health.IsShielded && !otherAttacks.IsPenetrating)
            {
                _attacks.Rage += _attacks.RageGain;
                return;
            }
            ApplyDamage(otherAttacks);
        }
        private void ApplyDamage(Attacks otherAttacks)
        {
            GetComponentInParent<Animator>().SetTrigger(Hit);
            _health.Current -= otherAttacks.Damage;
            otherAttacks.Rage += otherAttacks.RageGain;
            _health.IsStunned = otherAttacks.IsStunning;
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