using Game.Control;
using System;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerRotate : MonoBehaviour
    {
        [Range(0.0f, 20.0f)] public float rotateSpeed = 4f;
        Inputs Inputs;
        Vector2 _inputMoveVector;

        void Start()
        {
            Inputs = InputHandler.Instance.Inputs;
            Inputs.Player.MouseMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
            Inputs.Player.GamepadMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
        }

        void Update()
        {
            if (InputHandler.Instance.TakeRotation)
            {
                RotateCharacter();
            }
        }

        void RotateCharacter()
        {
            Vector3 direction = new Vector3(_inputMoveVector.x, 0f, _inputMoveVector.y);
            if (_inputMoveVector.magnitude > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSpeed);
            }
        }
    }
}
