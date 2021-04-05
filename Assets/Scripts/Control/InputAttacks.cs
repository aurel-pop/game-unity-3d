using System;
using Game.Combat;
using UnityEngine;

namespace Game.Control
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputHandler))]
    public class InputAttacks : MonoBehaviour
    {
        public float minDeltaMouse;
        public float minDeltaGamepad;
        float _minDelta;
        Inputs Inputs;
        Vector2 _inputVector;
        Vector2 _inputVectorStarted;
        Vector2 _inputVectorDelta;
        Vector2 _inputGamepadVector;
        bool _controllerRTHold;
        bool _controllerAttackReset = true;
        CharacterController characterController;
        Attack.Directions _queuedAttack = Attack.Directions.None;
        bool _isShielding;

        void Start()
        {
            Inputs = InputHandler.Instance.Inputs;
            characterController = GetComponent<CharacterController>();

            //Mouse
            Inputs.Player.MouseVector.performed += ctx => _inputVector = ctx.ReadValue<Vector2>();
            Inputs.Player.LMBStart.performed += ctx => _inputVectorStarted = _inputVector;
            Inputs.Player.LMBEnd.performed += ctx =>
            {
                _inputVectorDelta = _inputVector - _inputVectorStarted;
                _minDelta = minDeltaMouse;
                AskForAttack();
            };
            Inputs.Player.RMBStart.performed += ctx =>
            {
                _isShielding = true;
                AskForAttack();
            };
            Inputs.Player.RMBEnd.performed += ctx => _isShielding = false;

            //Gamepad
            Inputs.Player.GamepadVector.performed += ctx =>
            {
                _inputGamepadVector = ctx.ReadValue<Vector2>();
                _inputVectorDelta = _inputGamepadVector;
                _minDelta = minDeltaGamepad;
            };
            Inputs.Player.GamepadRTStart.performed += ctx =>
            {
                _controllerRTHold = true;
                AskForAttack();
            };
            Inputs.Player.GamepadRTEnd.performed += ctx =>
            {
                _controllerRTHold = false;
                _controllerAttackReset = true;
            };
            Inputs.Player.GamepadLTStart.performed += ctx =>
            {
                _isShielding = true;
                AskForAttack();
            };
            Inputs.Player.GamepadLTEnd.performed += ctx => _isShielding = false;
        }

        void Update()
        {
            if (InputHandler.Instance.Method == InputHandler.InputMethods.Gamepad)
            {
                CheckControllerAttack();
            }
        }

        void CheckControllerAttack()
        {
            if (!_controllerAttackReset && Mathf.Abs(_inputGamepadVector.x) < minDeltaGamepad && Mathf.Abs(_inputGamepadVector.y) < minDeltaGamepad)
                _controllerAttackReset = true;
            else
            {
                if (_controllerRTHold && _controllerAttackReset)
                {
                    AskForAttack();
                    _controllerAttackReset = false;
                }
            }
        }

        void AskForAttack()
        {
            if (InputHandler.Instance.TakeAttacks)
                GetComponentInChildren<TriggerAttacks>().TriggerAttack(CalculateDirection());
            else
                _queuedAttack = CalculateDirection();
        }

        Attack.Directions CalculateDirection()
        {
            Attack.Directions attack = Attack.Directions.Light;

            if (_isShielding)
                attack = Attack.Directions.Shield;
            else if (_inputVectorDelta.x > _minDelta && Mathf.Abs(_inputVectorDelta.x) > Mathf.Abs(_inputVectorDelta.y))
                attack = Attack.Directions.Combo;
            else if (_inputVectorDelta.x < -_minDelta && Mathf.Abs(_inputVectorDelta.x) > Mathf.Abs(_inputVectorDelta.y))
                attack = Attack.Directions.Light;
            else if (_inputVectorDelta.y > _minDelta)
                attack = Attack.Directions.Super;
            else if (_inputVectorDelta.y < -_minDelta)
                attack = Attack.Directions.Heavy;

            return attack;
        }

        public void PerformQueuedAttack()
        {
            if (_queuedAttack != Attack.Directions.None)
            {
                GetComponentInChildren<TriggerAttacks>().TriggerAttack(_queuedAttack);
                _queuedAttack = Attack.Directions.None;
            }
        }

        #region Enable / Disable
        void OnEnable()
        {
            Inputs.Enable();
        }

        void OnDisable()
        {
            Inputs.Disable();
        }
        #endregion
    }
}
