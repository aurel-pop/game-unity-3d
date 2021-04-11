using Combat;
using UnityEngine;

namespace Control
{
    [RequireComponent(typeof(InputHandler))]
    public class InputCombat : MonoBehaviour
    {
        public static InputCombat Instance { get; private set; }
        
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
        private Actions.Type _queuedAttack = Actions.Type.None;
        private Actions _actions;

        private void Awake()
        {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _inputs = InputHandler.Instance.Inputs;
            _actions = GetComponentInChildren<Actions>();

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
            if (InputHandler.Instance.Current == InputHandler.InputMethod.Gamepad) CheckControllerAttack();
        }

        private void CheckControllerAttack()
        {
            if (!_controllerAttackReset && Mathf.Abs(_inputGamepadVector.x) < minDeltaGamepad && Mathf.Abs(_inputGamepadVector.y) < minDeltaGamepad)
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
                _actions.PerformAction(CalculateDirection());
            else
                _queuedAttack = CalculateDirection();
        }

        private Actions.Type CalculateDirection()
        {
            Actions.Type currentAttack = Actions.Type.Light;

            if (_isShielding)
                currentAttack = Actions.Type.Shield;
            else if (_inputVectorDelta.x > _minDelta && Mathf.Abs(_inputVectorDelta.x) > Mathf.Abs(_inputVectorDelta.y))
                currentAttack = Actions.Type.Combo;
            else if (_inputVectorDelta.x < -_minDelta && Mathf.Abs(_inputVectorDelta.x) > Mathf.Abs(_inputVectorDelta.y))
                currentAttack = Actions.Type.Light;
            else if (_inputVectorDelta.y > _minDelta)
                currentAttack = Actions.Type.Super;
            else if (_inputVectorDelta.y < -_minDelta)
                currentAttack = Actions.Type.Heavy;

            return currentAttack;
        }

        public void PerformQueuedAttack()
        {
            if (_queuedAttack == Actions.Type.None) return;
            GetComponentInChildren<Actions>().PerformAction(_queuedAttack);
            _queuedAttack = Actions.Type.None;
        }
    }
}