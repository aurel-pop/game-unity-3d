using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class Hurtbox : MonoBehaviour
    {
        public SphereCollider hitbox;

        void OnTriggerEnter(Collider other)
        {
            GetComponentInParent<Animator>().SetTrigger("Hit");
            GetComponentInParent<Health>().Value -= 45;
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