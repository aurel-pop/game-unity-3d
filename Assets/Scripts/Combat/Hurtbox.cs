using UnityEngine;

namespace Combat
{
    public class Hurtbox : MonoBehaviour
    {
        private static readonly int Hit = Animator.StringToHash("hit");
        [SerializeField] private SphereCollider hitbox;

        private void Start()
        {
            hitbox.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            var health = GetComponentInParent<Health>();
            if (health.IsShielded) return;
            GetComponentInParent<Animator>().SetTrigger(Hit);
            health.Current -= 100;
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