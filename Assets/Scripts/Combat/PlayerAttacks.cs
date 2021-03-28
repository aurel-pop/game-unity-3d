using Game.Control;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Combat
{
    public class PlayerAttacks : MonoBehaviour
    {
        public float minDeltaMouse;
        public float minDeltaController;

        public AudioClip[] clips;
        AudioSource audio;

        float minDelta;
        Inputs inputs;
        Vector2 inputVector;
        Vector2 inputVectorStarted;
        Vector2 inputVectorDelta;
        Vector2 inputControllerVector;
        bool ControllerRTHold;
        bool ControllerRSReset;

        CharacterController characterController;

        PlayerInputHandler PlayerInputHandler;
        Animator anim;

        public enum Attacks { Null, Right, Left, Up, Down };
        Attacks queuedAttack;

        ObjectHealth playerHealth;

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            PlayerInputHandler = GetComponent<PlayerInputHandler>();
            anim = GetComponent<Animator>();
            audio = GetComponent<AudioSource>();
            playerHealth = GetComponent<ObjectHealth>();

            inputs = new Inputs();
            inputs.Player.MouseVector.performed += ctx => inputVector = ctx.ReadValue<Vector2>();
            inputs.Player.LMBStart.performed += ctx => inputVectorStarted = inputVector;
            inputs.Player.LMBEnd.performed += ctx =>
            {
                inputVectorDelta = inputVector - inputVectorStarted;
                minDelta = minDeltaMouse;
                AskForAttack();
            };

            inputs.Player.ControllerVector.performed += ctx =>
            {
                inputControllerVector = ctx.ReadValue<Vector2>();
                inputVectorDelta = inputControllerVector;
                minDelta = minDeltaController;
            };
            inputs.Player.ControllerRTStart.performed += ctx => ControllerRTHold = true;
            inputs.Player.ControllerRTEnd.performed += ctx =>
            {
                ControllerRTHold = false;
                ControllerRSReset = true;
            };
        }

        void Start()
        {
            queuedAttack = Attacks.Null;
            ControllerRTHold = false;
            ControllerRSReset = true;
        }

        void Update()
        {
            if (PlayerInputHandler.inputMethod == PlayerInputHandler.InputMethods.Gamepad)
            {
                if (Mathf.Abs(inputControllerVector.x) < minDeltaController && Mathf.Abs(inputControllerVector.y) < minDeltaController)
                    ControllerRSReset = true;
                else
                {
                    if (ControllerRTHold && ControllerRSReset)
                    {
                        AskForAttack();
                        ControllerRSReset = false;
                    }
                }
            }

        }

        void AskForAttack()
        {
            if (PlayerInputHandler.takeAttacks)
                PerformAttack(CalculateDirection());
            else
                queuedAttack = CalculateDirection();
        }

        Attacks CalculateDirection()
        {
            Attacks attack = Attacks.Left;

            if (inputVectorDelta.x > minDelta && Mathf.Abs(inputVectorDelta.x) > Mathf.Abs(inputVectorDelta.y))
                attack = Attacks.Right;
            else if (inputVectorDelta.x < -minDelta && Mathf.Abs(inputVectorDelta.x) > Mathf.Abs(inputVectorDelta.y))
                attack = Attacks.Left;
            else if (inputVectorDelta.y > minDelta)
                attack = Attacks.Up;
            else if (inputVectorDelta.y < -minDelta)
                attack = Attacks.Down;

            return attack;
        }

        void PerformAttack(Attacks dir)
        {
            switch (dir)
            {
                case Attacks.Null:
                    break;
                case Attacks.Right:
                    anim.SetTrigger("AttackRight");
                    audio.clip = clips[0];
                    audio.Play();
                    break;
                case Attacks.Left:
                    anim.SetTrigger("AttackLeft");
                    audio.clip = clips[1];
                    audio.Play();
                    break;
                case Attacks.Up:
                    anim.SetTrigger("AttackUp");
                    audio.clip = clips[2];
                    audio.Play();
                    playerHealth.Value -= 20;
                    break;
                case Attacks.Down:
                    anim.SetTrigger("AttackDown");
                    audio.clip = clips[3];
                    audio.Play();
                    break;
            }
        }

        public void PerformQueuedAttack()
        {
            if (queuedAttack != Attacks.Null)
            {
                PerformAttack(queuedAttack);
                queuedAttack = Attacks.Null;
            }
        }

        #region Enable / Disable
        void OnEnable()
        {
            inputs.Enable();
        }

        void OnDisable()
        {
            inputs.Disable();
        }
        #endregion
    }
}
