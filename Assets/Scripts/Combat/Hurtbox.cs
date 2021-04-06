using UnityEngine;

namespace Combat
{
    public class Hurtbox : MonoBehaviour
    {
        public SphereCollider hitbox;
        public AudioClip[] isHitAudioClips;
        private static readonly int Hit = Animator.StringToHash("hit");

        private void OnTriggerEnter(Collider other)
        {
            if (GetComponentInParent<TriggerAttacks>().IsShielded) return;
            GetComponentInParent<Animator>().SetTrigger(Hit);
            GetComponentInParent<Health>().Current -= 40;
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