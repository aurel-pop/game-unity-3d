using Game.Control;
using System;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(0.0f, 20.0f)] public float moveSpeed;
        public Transform groundChecker;
        public LayerMask ground;
        public float groundDistance;

        CharacterController characterController;
        Animator anim;
        Inputs inputs;
        Vector2 inputMoveVector;
        Vector3 velocity;

        void Awake()
        {
            inputs = new Inputs();
            characterController = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            inputMoveVector = inputs.Player.Move.ReadValue<Vector2>();

            if (GetComponent<PlayerInputHandler>().takeMovement)
            {
                velocity = (inputMoveVector.y * transform.forward) + (inputMoveVector.x * transform.right);
                characterController.Move(velocity * (moveSpeed * Time.deltaTime));
            }

            anim.SetFloat("forwardSpeed", Mathf.Lerp(anim.GetFloat("forwardSpeed"), characterController.velocity.magnitude, Time.deltaTime * 10f));
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
