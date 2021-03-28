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
        bool ControllerRTHold;
        bool ControllerRSReset;

        CharacterController characterController;
        PlayerInputHandler PlayerInputHandler;
        Attacks.Direction queuedAttack;

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            PlayerInputHandler = GetComponent<PlayerInputHandler>();

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
            queuedAttack = Attacks.Direction.Null;
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
                GetComponent<TriggerAttacks>().Trigger(CalculateDirection());
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

            return attack;
        }

        public void PerformQueuedAttack()
        {
            if (queuedAttack != Attacks.Direction.Null)
            {
                GetComponent<TriggerAttacks>().Trigger(queuedAttack);
                queuedAttack = Attacks.Direction.Null;
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
