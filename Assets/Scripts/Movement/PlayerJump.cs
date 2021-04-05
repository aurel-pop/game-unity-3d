using System;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerJump : MonoBehaviour
    {
        public float jumpHeight;
        bool _isGrounded;
        Inputs _inputs;

        void Awake()
        {
            _inputs = new Inputs();
        }

        void Update()
        {
            //isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, ground, QueryTriggerInteraction.Ignore);
            //if (isGrounded && velocity.y < 0)
            //    velocity.y = 0f;

            //if (Input.GetButtonDown("Jump") && isGrounded)
            //    velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
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
