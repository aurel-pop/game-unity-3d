using UnityEngine;

namespace Combat
{
    public class Hurtbox : MonoBehaviour
    {
        [SerializeField] private SphereCollider hitbox;
        private static readonly int Hit = Animator.StringToHash("hit");

        private void OnTriggerEnter(Collider other)
        {
            Health health = GetComponentInParent<Health>();
            if (health.IsShielded) return;
            GetComponentInParent<Animator>().SetTrigger(Hit);
            health.Current -= 100;
        }

        private void Start()
        {
            hitbox.enabled = false;
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