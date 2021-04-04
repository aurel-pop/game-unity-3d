using Game.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class Hurtbox : MonoBehaviour
    {
        public SphereCollider hitbox;
        public AudioClip[] isHitClips;

        void OnTriggerEnter(Collider other)
        {
            if (!GetComponentInParent<TriggerAttacks>().isShielded)
            {
                GetComponentInParent<AudioSource>().PlayOneShot(isHitClips[0]);
                GetComponentInParent<Animator>().SetTrigger("Hit");
                GetComponentInParent<Health>().Value -= 40;
            } else
            {
                GetComponentInParent<AudioSource>().PlayOneShot(isHitClips[1]);
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