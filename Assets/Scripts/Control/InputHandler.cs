using System;
using UnityEngine;

namespace Game.Control
{
    public class InputHandler : MonoBehaviour
    {
        static InputHandler _instance;
        public static InputHandler Instance { get => _instance; }

        bool _takeAttacks = true;
        public bool TakeAttacks { get => _takeAttacks; set => _takeAttacks = value; }

        bool _takeMovement = true;
        public bool TakeMovement { get => _takeMovement; set => _takeMovement = value; }

        bool _takeRotation = true;
        public bool TakeRotation { get => _takeRotation; set => _takeRotation = value; }

        Inputs _inputs;
        public enum InputMethods { Mouse, Gamepad };
        public InputMethods name = InputMethods.Mouse;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            _inputs = new Inputs();

            //Mouse
            _inputs.Player.MouseMove.performed += ctx => name = InputMethods.Mouse;
            _inputs.Player.MouseVector.performed += ctx => name = InputMethods.Mouse;
            _inputs.Player.LMBStart.performed += ctx => name = InputMethods.Mouse;
            _inputs.Player.RMBStart.performed += ctx => name = InputMethods.Mouse;

            //Gamepad
            _inputs.Player.GamepadMove.performed += ctx => name = InputMethods.Gamepad;
            _inputs.Player.GamepadVector.performed += ctx => name = InputMethods.Gamepad;
            _inputs.Player.GamepadRTStart.performed += ctx => name = InputMethods.Gamepad;
            _inputs.Player.GamepadLTStart.performed += ctx => name = InputMethods.Gamepad;
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
