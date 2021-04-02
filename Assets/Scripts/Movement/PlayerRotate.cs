using Game.Control;
using System;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerRotate : MonoBehaviour
    {
        [Range(0.0f, 20.0f)] public float rotateSpeed = 4f;
        PlayerInputHandler playerInputHandler;
        Inputs inputs;
        Vector2 inputMoveVector;

        void Awake()
        {
            playerInputHandler = GetComponentInParent<PlayerInputHandler>();
            inputs = new Inputs();
            inputs.Player.MouseMove.performed += ctx => inputMoveVector = ctx.ReadValue<Vector2>();
            inputs.Player.ControllerMove.performed += ctx => inputMoveVector = ctx.ReadValue<Vector2>();
        }

        void Update()
        {
            if (playerInputHandler.takeRotation)
            {
                RotateCharacter();
            }
        }

        void RotateCharacter()
        {
            Vector3 direction = new Vector3(inputMoveVector.x, 0f, inputMoveVector.y);
            if (inputMoveVector.magnitude > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSpeed);
            }
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
