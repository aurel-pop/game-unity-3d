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
                GetComponentInParent<Animator>().SetTrigger("Hit");
                GetComponentInParent<Health>().Value -= 40;
                GetComponentInParent<AudioSource>().PlayOneShot(isHitClips[0]);
            } else
            {
                int rng = Random.Range(0, 100);
                if (rng < 50 )
                    GetComponentInParent<AudioSource>().PlayOneShot(isHitClips[1]);
                else
                    GetComponentInParent<AudioSource>().PlayOneShot(isHitClips[2]);
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