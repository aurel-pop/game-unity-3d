using Combat;
using UnityEngine;

namespace Control
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputHandler))]
    public class InputAttacks : MonoBehaviour
    {
        public float minDeltaMouse;
        public float minDeltaGamepad;
        private bool _controllerAttackReset = true;
        private bool _controllerRTHold;
        private Vector2 _inputGamepadVector;
        private Inputs _inputs;
        private Vector2 _inputVector;
        private Vector2 _inputVectorDelta;
        private Vector2 _inputVectorStarted;
        private bool _isShielding;
        private float _minDelta;
        private Attack.Directions _queuedAttack = Attack.Directions.None;
        private TriggerAttacks _triggerAttacks;

        private void Start()
        {
            _inputs = InputHandler.Instance.Inputs;
            _triggerAttacks = GetComponentInChildren<TriggerAttacks>();

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

        private void Update()
        {
            if (InputHandler.Instance.Method == InputHandler.InputMethods.Gamepad) CheckControllerAttack();
        }

        private void CheckControllerAttack()
        {
            if (!_controllerAttackReset && Mathf.Abs(_inputGamepadVector.x) < minDeltaGamepad &&
                Mathf.Abs(_inputGamepadVector.y) < minDeltaGamepad)
            {
                _controllerAttackReset = true;
            }
            else
            {
                if (!_controllerRTHold || !_controllerAttackReset) return;
                AskForAttack();
                _controllerAttackReset = false;
            }
        }

        private void AskForAttack()
        {
            if (InputHandler.Instance.TakeAttacks)
                _triggerAttacks.TriggerAttack(CalculateDirection());
            else
                _queuedAttack = CalculateDirection();
        }

        private Attack.Directions CalculateDirection()
        {
            var attack = Attack.Directions.Light;

            if (_isShielding)
                attack = Attack.Directions.Shield;
            else if (_inputVectorDelta.x > _minDelta && Mathf.Abs(_inputVectorDelta.x) > Mathf.Abs(_inputVectorDelta.y))
                attack = Attack.Directions.Combo;
            else if (_inputVectorDelta.x < -_minDelta &&
                     Mathf.Abs(_inputVectorDelta.x) > Mathf.Abs(_inputVectorDelta.y))
                attack = Attack.Directions.Light;
            else if (_inputVectorDelta.y > _minDelta)
                attack = Attack.Directions.Super;
            else if (_inputVectorDelta.y < -_minDelta)
                attack = Attack.Directions.Heavy;

            return attack;
        }

        public void PerformQueuedAttack()
        {
            if (_queuedAttack == Attack.Directions.None) return;
            GetComponentInChildren<TriggerAttacks>().TriggerAttack(_queuedAttack);
            _queuedAttack = Attack.Directions.None;
        }
    }
}