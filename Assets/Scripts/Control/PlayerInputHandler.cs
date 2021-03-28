using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [HideInInspector] public bool takeAttacks;
        [HideInInspector] public bool takeMovement;
        [HideInInspector] public bool takeRotation;
        [HideInInspector] public bool takeDamage;

        Inputs inputs;
        public enum InputMethods { Mouse, Gamepad };
        [HideInInspector] public InputMethods inputMethod;

        void Awake()
        {
            inputs = new Inputs();
            inputs.Player.MouseVector.performed += ctx => inputMethod = InputMethods.Mouse;
            inputs.Player.LMBStart.performed += ctx => inputMethod = InputMethods.Mouse;
            inputs.Player.RMBStart.performed += ctx => inputMethod = InputMethods.Mouse;

            inputs.Player.ControllerVector.performed += ctx => inputMethod = InputMethods.Gamepad;
            inputs.Player.ControllerRTStart.performed += ctx => inputMethod = InputMethods.Gamepad;
        }

        void Start()
        {
            takeAttacks = true;
            takeMovement = true;
            takeRotation = true;
            takeDamage = true;
        }
    }
}
