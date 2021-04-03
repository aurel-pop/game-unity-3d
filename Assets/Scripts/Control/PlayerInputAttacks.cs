using System;
using Game.Combat;
using UnityEngine;

namespace Game.Control
{
    public class PlayerInputAttacks : MonoBehaviour
    {
        public float minDeltaMouse;
        public float minDeltaController;

        float minDelta;
        Inputs inputs;
        Vector2 inputVector;
        Vector2 inputVectorStarted;
        Vector2 inputVectorDelta;
        Vector2 inputControllerVector;
        bool controllerRTHold;
        bool controllerAttackReset;

        CharacterController characterController;
        PlayerInputHandler playerInputHandler;
        Attacks.Direction queuedAttack;
        bool isShielding;

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerInputHandler = GetComponent<PlayerInputHandler>();
            inputs = new Inputs();

            //Mouse
            inputs.Player.MouseVector.performed += ctx => inputVector = ctx.ReadValue<Vector2>();
            inputs.Player.LMBStart.performed += ctx => inputVectorStarted = inputVector;
            inputs.Player.LMBEnd.performed += ctx =>
            {
                inputVectorDelta = inputVector - inputVectorStarted;
                minDelta = minDeltaMouse;
                AskForAttack();
            };
            inputs.Player.RMBStart.performed += ctx =>
            {
                isShielding = true;
                AskForAttack();
            };
            inputs.Player.RMBEnd.performed += ctx => isShielding = false;

            //Controller
            inputs.Player.ControllerVector.performed += ctx =>
            {
                inputControllerVector = ctx.ReadValue<Vector2>();
                inputVectorDelta = inputControllerVector;
                minDelta = minDeltaController;
            };
            inputs.Player.ControllerRTStart.performed += ctx =>
            {
                controllerRTHold = true;
                AskForAttack();
            };
            inputs.Player.ControllerRTEnd.performed += ctx =>
            {
                controllerRTHold = false;
                controllerAttackReset = true;
            };
            inputs.Player.ControllerLTStart.performed += ctx =>
            {
                isShielding = true;
                AskForAttack();
            };
            inputs.Player.ControllerLTEnd.performed += ctx => isShielding = false;
        }

        void Start()
        {
            queuedAttack = Attacks.Direction.None;
            controllerRTHold = false;
            controllerAttackReset = true;
        }

        void Update()
        {
            if (playerInputHandler.inputMethod == PlayerInputHandler.InputMethods.Gamepad)
            {
                CheckControllerAttack();
            }
        }

        void CheckControllerAttack()
        {
            if (Mathf.Abs(inputControllerVector.x) < minDeltaController && Mathf.Abs(inputControllerVector.y) < minDeltaController)
                controllerAttackReset = true;
            else
            {
                if (controllerRTHold && controllerAttackReset)
                {
                    AskForAttack();
                    controllerAttackReset = false;
                }
            }
        }

        void AskForAttack()
        {
            if (playerInputHandler.takeAttacks)
                GetComponentInChildren<TriggerAttacks>().TriggerAttack(CalculateDirection());
            else
                queuedAttack = CalculateDirection();
        }

        Attacks.Direction CalculateDirection()
        {
            Attacks.Direction attack = Attacks.Direction.Left;

            if (inputVectorDelta.x > minDelta && Mathf.Abs(inputVectorDelta.x) > Mathf.Abs(inputVectorDelta.y))
                attack = Attacks.Direction.Right;
            else if (inputVectorDelta.x < -minDelta && Mathf.Abs(inputVectorDelta.x) > Mathf.Abs(inputVectorDelta.y))
                attack = Attacks.Direction.Left;
            else if (inputVectorDelta.y > minDelta)
                attack = Attacks.Direction.Up;
            else if (inputVectorDelta.y < -minDelta)
                attack = Attacks.Direction.Down;
            else if (isShielding)
                attack = Attacks.Direction.Shield;

            return attack;
        }

        public void PerformQueuedAttack()
        {
            if (queuedAttack != Attacks.Direction.None)
            {
                GetComponentInChildren<TriggerAttacks>().TriggerAttack(queuedAttack);
                queuedAttack = Attacks.Direction.None;
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
