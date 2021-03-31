using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class TriggerAttacks : MonoBehaviour
    {
        [SerializeField] AudioClip[] clips;
        Health Health;

        void Awake()
        {
            Health = GetComponent<Health>();
        }

        public void Trigger(Attacks.Direction dir)
        {
            switch (dir)
            {
                case Attacks.Direction.Null:
                    break;
                case Attacks.Direction.Right:
                    GetComponent<Animator>().SetTrigger("AttackRight");
                    GetComponent<AudioSource>().PlayOneShot(clips[0]);
                    break;
                case Attacks.Direction.Left:
                    GetComponent<Animator>().SetTrigger("AttackLeft");
                    GetComponent<AudioSource>().PlayOneShot(clips[1]);
                    break;
                case Attacks.Direction.Up:
                    GetComponent<Animator>().SetTrigger("AttackUp");
                    GetComponent<AudioSource>().PlayOneShot(clips[2]);
                    break;
                case Attacks.Direction.Down:
                    GetComponent<Animator>().SetTrigger("AttackDown");
                    GetComponent<AudioSource>().PlayOneShot(clips[3]);
                    break;
            }
        }
    }
}
