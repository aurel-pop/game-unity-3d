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

        InputHandler inputHandler;
        CharacterController characterController;
        Animator animator;

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
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            inputHandler = InputHandler.Instance;
        }

        void Update()
        {
            if (inputHandler.TakeMovement)
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
