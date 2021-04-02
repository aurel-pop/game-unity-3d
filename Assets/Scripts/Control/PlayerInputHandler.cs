using System;
using UnityEngine;

namespace Game.Control
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public bool takeAttacks;
        public bool takeMovement;
        public bool takeRotation;

        Inputs inputs;
        public enum InputMethods { Mouse, Gamepad };
        public InputMethods inputMethod;

        void Awake()
        {
            inputs = new Inputs();

            //Mouse
            inputs.Player.MouseMove.performed += ctx => inputMethod = InputMethods.Mouse;
            inputs.Player.MouseVector.performed += ctx => inputMethod = InputMethods.Mouse;
            inputs.Player.LMBStart.performed += ctx => inputMethod = InputMethods.Mouse;
            inputs.Player.RMBStart.performed += ctx => inputMethod = InputMethods.Mouse;

            //Controller
            inputs.Player.ControllerMove.performed += ctx => inputMethod = InputMethods.Gamepad;
            inputs.Player.ControllerVector.performed += ctx => inputMethod = InputMethods.Gamepad;
            inputs.Player.ControllerRTStart.performed += ctx => inputMethod = InputMethods.Gamepad;
            inputs.Player.ControllerLTStart.performed += ctx => inputMethod = InputMethods.Gamepad;
        }

        void Start()
        {
            takeAttacks = true;
            takeMovement = true;
            takeRotation = true;
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
