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
        Inputs _inputs;
        Vector2 _inputVector;
        Vector2 _inputVectorStarted;
        Vector2 _inputVectorDelta;
        Vector2 _inputGamepadVector;
        bool _controllerRTHold;
        bool _controllerAttackReset = true;

        CharacterController characterController;
        InputHandler inputHandler;

        Attack.Directions _queuedAttack = Attack.Directions.None;
        bool _isShielding;

        void Awake()
        {
            _inputs = new Inputs();

            //Mouse
            _inputs.Player.MouseVector.performed += ctx => _inputVector = ctx.ReadValue<Vector2>();
            _inputs.Player.LMBStart.performed += ctx => _inputVectorStarted = _inputVector;
            _inputs.Player.LMBEnd.performed += ctx =>
            {
                _inputVectorDelta = _inputVector - _inputVectorStarted;
                _minDelta = minDeltaMouse;
                AskForAttack();
            };
            _inputs.Player.RMBStart.performed += ctx =>
            {
                _isShielding = true;
                AskForAttack();
            };
            _inputs.Player.RMBEnd.performed += ctx => _isShielding = false;

            //Gamepad
            _inputs.Player.GamepadVector.performed += ctx =>
            {
                _inputGamepadVector = ctx.ReadValue<Vector2>();
                _inputVectorDelta = _inputGamepadVector;
                _minDelta = minDeltaGamepad;
            };
            _inputs.Player.GamepadRTStart.performed += ctx =>
            {
                _controllerRTHold = true;
                AskForAttack();
            };
            _inputs.Player.GamepadRTEnd.performed += ctx =>
            {
                _controllerRTHold = false;
                _controllerAttackReset = true;
            };
            _inputs.Player.GamepadLTStart.performed += ctx =>
            {
                _isShielding = true;
                AskForAttack();
            };
            _inputs.Player.GamepadLTEnd.performed += ctx => _isShielding = false;
        }

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            inputHandler = InputHandler.Instance;
        }

        void Update()
        {
            if (inputHandler.name == InputHandler.InputMethods.Gamepad)
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
            if (inputHandler.TakeAttacks)
                GetComponentInChildren<TriggerAttacks>().TriggerAttack(CalculateDirection());
            else
                _queuedAttack = CalculateDirection();
        }

        Attack.Directions CalculateDirection()
        {
            Attack.Directions attack = Attack.Directions.Left;

            if (_isShielding)
                attack = Attack.Directions.Shield;
            else if (_inputVectorDelta.x > _minDelta && Mathf.Abs(_inputVectorDelta.x) > Mathf.Abs(_inputVectorDelta.y))
                attack = Attack.Directions.Right;
            else if (_inputVectorDelta.x < -_minDelta && Mathf.Abs(_inputVectorDelta.x) > Mathf.Abs(_inputVectorDelta.y))
                attack = Attack.Directions.Left;
            else if (_inputVectorDelta.y > _minDelta)
                attack = Attack.Directions.Up;
            else if (_inputVectorDelta.y < -_minDelta)
                attack = Attack.Directions.Down;

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
            _inputs.Enable();
        }

        void OnDisable()
        {
            _inputs.Disable();
        }
        #endregion
    }
}
