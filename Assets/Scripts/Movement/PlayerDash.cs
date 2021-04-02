using System.Collections;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerDash : MonoBehaviour
    {
        public float dashDistance;
        public Vector3 drag;
        public float dashDuration;

        float gravity;
        Vector3 velocity;
        Inputs inputs;
        Vector2 inputMoveVector;
        Vector3 movement;
        CharacterController characterController;
        Animator anim;

        void Awake()
        {
            inputs = new Inputs();
            inputs.Player.Dash.performed += ctx => Dash();
        }

        void Start()
        {
            characterController = GetComponentInParent<CharacterController>();
            anim = GetComponentInParent<Animator>();
            gravity = -9.81f;
        }

        void Update()
        {
            // Read Inputs
            inputs.Player.MouseMove.performed += ctx => inputMoveVector = ctx.ReadValue<Vector2>();
            inputs.Player.ControllerMove.performed += ctx => inputMoveVector = ctx.ReadValue<Vector2>();
            movement = (inputMoveVector.y * transform.forward) + (inputMoveVector.x * transform.right);

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;

            // Velocity drag for Dash
            velocity.x /= 1 + drag.x * Time.deltaTime;
            velocity.y /= 1 + drag.y * Time.deltaTime;
            velocity.z /= 1 + drag.z * Time.deltaTime;

            characterController.Move(velocity * Time.deltaTime);
        }

        private void Dash()
        {
            StartCoroutine(AnimateDash());
            velocity += Vector3.Scale(movement, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
        }

        IEnumerator AnimateDash()
        {
            anim.SetBool("isDashing", true);
            yield return new WaitForSeconds(this.dashDuration);
            anim.SetBool("isDashing", false);
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
