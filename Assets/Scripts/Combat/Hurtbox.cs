using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class Hurtbox : MonoBehaviour
    {
        public SphereCollider hitbox;
        public AudioClip[] isHitAudioClips;

        void OnTriggerEnter(Collider other)
        {
            if (!GetComponentInParent<TriggerAttacks>().isShielded)
            {
                GetComponentInParent<Animator>().SetTrigger("hit");
                GetComponentInParent<Health>().Current -= 40;
            }
        }

        void Start()
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