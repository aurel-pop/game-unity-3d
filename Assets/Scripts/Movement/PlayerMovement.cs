using Game.Control;
using System;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(0.0f, 20.0f)] public float moveSpeed;
        [Range(0.0f, 20.0f)] public float animationSpeed;
        [Range(0.0f, 3.0f)] public float groundDistance;
        public Transform groundChecker;
        public LayerMask ground;

        PlayerInputHandler playerInputHandler;
        CharacterController characterController;
        Animator anim;
        Inputs inputs;
        Vector2 inputMoveVector;

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
            playerInputHandler = GetComponent<PlayerInputHandler>();
            inputs = new Inputs();
            inputs.Player.MouseMove.performed += ctx => inputMoveVector = ctx.ReadValue<Vector2>();
            inputs.Player.ControllerMove.performed += ctx => inputMoveVector = ctx.ReadValue<Vector2>();
        }

        void Update()
        {
            if (playerInputHandler.takeMovement)
            {
                MoveCharacter();
            }

            UpdateAnimator();
        }

        void MoveCharacter()
        {
            Vector3 velocity = (inputMoveVector.y * transform.forward) + (inputMoveVector.x * transform.right);
            characterController.Move(velocity * (moveSpeed * Time.deltaTime));
        }

        void UpdateAnimator()
        {
            Vector3 velocity = characterController.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            anim.SetFloat("forwardSpeed", Mathf.Lerp(anim.GetFloat("forwardSpeed"), localVelocity.magnitude, animationSpeed * Time.deltaTime));
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
