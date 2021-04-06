using UnityEngine;

namespace Control
{
    public class InputHandler : MonoBehaviour
    {
        public enum InputMethods
        {
            Mouse,
            Gamepad
        }

        public static InputHandler Instance { get; private set; }
        public bool TakeAttacks { get; set; } = true;
        public bool TakeMovement { get; set; } = true;
        public bool TakeRotation { get; set; } = true;
        public Inputs Inputs { get; private set; }
        public InputMethods Method { get; private set; } = InputMethods.Mouse;

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
            Inputs.Player.MouseMove.performed += ctx => Method = InputMethods.Mouse;
            Inputs.Player.MouseVector.performed += ctx => Method = InputMethods.Mouse;
            Inputs.Player.LMBStart.performed += ctx => Method = InputMethods.Mouse;
            Inputs.Player.RMBStart.performed += ctx => Method = InputMethods.Mouse;

            //Gamepad
            Inputs.Player.GamepadMove.performed += ctx => Method = InputMethods.Gamepad;
            Inputs.Player.GamepadVector.performed += ctx => Method = InputMethods.Gamepad;
            Inputs.Player.GamepadRTStart.performed += ctx => Method = InputMethods.Gamepad;
            Inputs.Player.GamepadLTStart.performed += ctx => Method = InputMethods.Gamepad;
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