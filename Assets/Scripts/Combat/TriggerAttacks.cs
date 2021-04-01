using Game.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class TriggerAttacks : MonoBehaviour
    {
        [SerializeField] AudioClip[] attackClips;
        [HideInInspector] public bool isShielded;

        PlayerInputHandler playerInputHandler;
        AudioSource source;
        Animator anim;

        void Awake()
        {
            playerInputHandler = GetComponentInParent<PlayerInputHandler>();
            source = GetComponentInParent<AudioSource>();
            anim = GetComponentInChildren<Animator>();
        }

        public void TriggerAttack(Attacks.Direction dir)
        {
            switch (dir)
            {
                case Attacks.Direction.None:
                    break;
                case Attacks.Direction.Right:
                    anim.SetTrigger("AttackRight");
                    source.PlayOneShot(attackClips[0]);
                    break;
                case Attacks.Direction.Left:
                    anim.SetTrigger("AttackLeft");
                    source.PlayOneShot(attackClips[1]);
                    break;
                case Attacks.Direction.Up:
                    anim.SetTrigger("AttackUp");
                    source.PlayOneShot(attackClips[2]);
                    break;
                case Attacks.Direction.Down:
                    anim.SetTrigger("AttackDown");
                    source.PlayOneShot(attackClips[3]);
                    break;
                case Attacks.Direction.Shield:
                    StartShielding();
                    break;
            }
        }

        void StartShielding()
        {
            isShielded = true;
            anim.SetBool("isShielding", true);

            if (tag == "Player")
            {
                playerInputHandler.takeAttacks = false;
                playerInputHandler.takeMovement = false;
            }
        }

        public void StopShielding()
        {
            isShielded = false;
            anim.SetBool("isShielding", false);

            if (tag == "Player" && !GetComponentInChildren<Health>().isDead)
            {
                playerInputHandler.takeAttacks = true;
                playerInputHandler.takeMovement = true;
            }
        }
    }
}
