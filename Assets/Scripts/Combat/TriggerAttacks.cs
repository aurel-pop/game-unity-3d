using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class TriggerAttacks : MonoBehaviour
    {
        [SerializeField] AudioClip[] clips;

        AudioSource source;
        Animator anim;

        void Awake()
        {
            source = GetComponentInParent<AudioSource>();
            anim = GetComponentInChildren<Animator>();
        }

        public void Trigger(Attacks.Direction dir)
        {
            switch (dir)
            {
                case Attacks.Direction.Null:
                    break;
                case Attacks.Direction.Right:
                    anim.SetTrigger("AttackRight");
                    source.PlayOneShot(clips[0]);
                    break;
                case Attacks.Direction.Left:
                    anim.SetTrigger("AttackLeft");
                    source.PlayOneShot(clips[1]);
                    break;
                case Attacks.Direction.Up:
                    anim.SetTrigger("AttackUp");
                    source.PlayOneShot(clips[2]);
                    break;
                case Attacks.Direction.Down:
                    anim.SetTrigger("AttackDown");
                    source.PlayOneShot(clips[3]);
                    break;
            }
        }
    }
}
