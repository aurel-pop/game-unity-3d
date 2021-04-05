using Game.Control;
using System;
using UnityEngine;

namespace Game.Movement
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputHandler))]
    public class PlayerMovement : MonoBehaviour
    {
        [Range(0.0f, 20.0f)] public float moveSpeed;
        [Range(0.0f, 20.0f)] public float animationSpeed;
        [Range(0.0f, 3.0f)] public float groundDistance;
        public Transform groundChecker;
        public LayerMask ground;
        CharacterController characterController;
        Animator animator;
        Inputs Inputs;
        Vector2 _inputMoveVector;

        void Start()
        {
            Inputs = InputHandler.Instance.Inputs;
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            Inputs.Player.MouseMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
            Inputs.Player.GamepadMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
        }

        void Update()
        {
            if (InputHandler.Instance.TakeMovement)
            {
                MoveCharacter();
            }

            UpdateAnimator();
        }

        void MoveCharacter()
        {
            Vector3 velocity = (_inputMoveVector.y * transform.forward) + (_inputMoveVector.x * transform.right);
            characterController.Move(velocity * (moveSpeed * Time.deltaTime));
        }

        void UpdateAnimator()
        {
            Vector3 velocity = characterController.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            animator.SetFloat("forwardSpeed", Mathf.Lerp(animator.GetFloat("forwardSpeed"), localVelocity.magnitude, animationSpeed * Time.deltaTime));
        }
    }
}
