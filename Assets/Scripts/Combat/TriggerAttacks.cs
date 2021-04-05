using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class TriggerAttacks : MonoBehaviour
    {
        [SerializeField] AudioClip[] attackAudioClips;
        [HideInInspector] public bool isShielded;

        AudioSource audioSource;
        Animator animator;

        void Start()
        {
            audioSource = GetComponentInParent<AudioSource>();
            animator = GetComponentInChildren<Animator>();
        }

        public void TriggerAttack(Attack.Directions dir)
        {
            switch (dir)
            {
                case Attack.Directions.None:
                    break;
                case Attack.Directions.Right:
                    animator.SetTrigger("AttackRight");
                    break;
                case Attack.Directions.Left:
                    animator.SetTrigger("AttackLeft");
                    break;
                case Attack.Directions.Up:
                    animator.SetTrigger("AttackUp");
                    break;
                case Attack.Directions.Down:
                    animator.SetTrigger("AttackDown");
                    break;
                case Attack.Directions.Shield:
                    StartShielded();
                    break;
            }
        }

        void StartShielded()
        {
            isShielded = true;
            animator.SetTrigger("Shield");
        }

        public void StopShielded()
        {
            isShielded = false;
        }
    }
}
