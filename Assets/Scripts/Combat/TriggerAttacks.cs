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
                case Attack.Directions.Light:
                    animator.SetTrigger("light");
                    break;
                case Attack.Directions.Combo:
                    animator.SetTrigger("combo");
                    break;
                case Attack.Directions.Heavy:
                    animator.SetTrigger("heavy");
                    break;
                case Attack.Directions.Super:
                    animator.SetTrigger("super");
                    break;
                case Attack.Directions.Enrage:
                    animator.SetTrigger("enrage");
                    break;
                case Attack.Directions.Shield:
                    StartShielded();
                    break;
            }
        }

        void StartShielded()
        {
            isShielded = true;
            animator.SetTrigger("shield");
        }

        public void StopShielded()
        {
            isShielded = false;
        }
    }
}
