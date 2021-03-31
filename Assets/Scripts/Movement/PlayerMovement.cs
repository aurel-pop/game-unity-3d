using Game.Control;
using System;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed;
        public float groundDistance;
        public Transform groundChecker;
        public LayerMask ground;
        PlayerInputHandler PlayerInputHandler;
        Inputs inputs;
        Vector2 inputMoveVector;
        Vector3 velocity;
        CharacterController characterController;
        Animator anim;

        void Awake()
        {
            inputs = new Inputs();
        }

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            PlayerInputHandler = GetComponent<PlayerInputHandler>();
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            // Read Inputs (normalized)
            inputMoveVector = inputs.Player.Move.ReadValue<Vector2>();

            // Move
            if (PlayerInputHandler.takeMovement)
            {
                velocity = (inputMoveVector.y * transform.forward) + (inputMoveVector.x * transform.right);
                characterController.Move(velocity * (moveSpeed * Time.deltaTime));
            }

            if (characterController.velocity.magnitude > 0)
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isDashing", false);
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
