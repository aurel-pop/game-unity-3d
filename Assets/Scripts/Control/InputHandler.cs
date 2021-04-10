using UnityEngine;

namespace Control
{
    public class InputHandler : MonoBehaviour
    {
        public enum InputMethod
        {
            Mouse,
            Gamepad
        }

        public static InputHandler Instance { get; private set; }
        public bool TakeAttacks { get; set; } = true;
        public bool TakeMovement { get; set; } = true;
        public bool TakeRotation { get; set; } = true;
        public Inputs Inputs { get; private set; }
        public InputMethod Current { get; private set; } = InputMethod.Mouse;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Inputs = new Inputs();

            //Mouse
            Inputs.Player.MouseMove.performed += ctx => Current = InputMethod.Mouse;
            Inputs.Player.MouseVector.performed += ctx => Current = InputMethod.Mouse;
            Inputs.Player.LMBStart.performed += ctx => Current = InputMethod.Mouse;
            Inputs.Player.RMBStart.performed += ctx => Current = InputMethod.Mouse;

            //Gamepad
            Inputs.Player.GamepadMove.performed += ctx => Current = InputMethod.Gamepad;
            Inputs.Player.GamepadVector.performed += ctx => Current = InputMethod.Gamepad;
            Inputs.Player.GamepadRTStart.performed += ctx => Current = InputMethod.Gamepad;
            Inputs.Player.GamepadLTStart.performed += ctx => Current = InputMethod.Gamepad;
        }

        private void OnEnable()
        {
            Inputs.Enable();
        }

        private void OnDisable()
        {
            Inputs.Disable();
        }
    }
}