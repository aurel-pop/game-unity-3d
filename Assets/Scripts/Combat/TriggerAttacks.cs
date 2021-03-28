using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class TriggerAttacks : MonoBehaviour
    {
        Animator anim;
        AudioSource source;
        [SerializeField] AudioClip[] clips;
        Health playerHealth;

        void Awake()
        {
            anim = GetComponent<Animator>();
            source = GetComponent<AudioSource>();
            playerHealth = GetComponent<Health>();
        }

        public void Trigger(Attacks.Direction dir)
        {
            switch (dir)
            {
                case Attacks.Direction.Null:
                    break;
                case Attacks.Direction.Right:
                    anim.SetTrigger("AttackRight");
                    source.clip = clips[0];
                    source.Play();
                    break;
                case Attacks.Direction.Left:
                    anim.SetTrigger("AttackLeft");
                    source.clip = clips[1];
                    source.Play();
                    break;
                case Attacks.Direction.Up:
                    anim.SetTrigger("AttackUp");
                    source.clip = clips[2];
                    source.Play();
                    playerHealth.Value -= 20;
                    break;
                case Attacks.Direction.Down:
                    anim.SetTrigger("AttackDown");
                    source.clip = clips[3];
                    source.Play();
                    break;
            }
        }
    }
}
