using Game.Control;
using System;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerRotate : MonoBehaviour
    {
        [Range(0.0f, 20.0f)] public float rotateSpeed = 4f;
        InputHandler inputHandler;

        Inputs _inputs;
        Vector2 _inputMoveVector;

        void Awake()
        {
            _inputs = new Inputs();
            _inputs.Player.MouseMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
            _inputs.Player.GamepadMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
        }

        void Start()
        {
            inputHandler = InputHandler.Instance;
        }

        void Update()
        {
            if (inputHandler.TakeRotation)
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
