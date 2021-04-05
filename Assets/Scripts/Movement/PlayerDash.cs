using Game.Control;
using System.Collections;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerDash : MonoBehaviour
    {
        public float dashDistance;
        public Vector3 drag;
        public float dashDuration;
        Inputs Inputs;
        CharacterController characterController;
        Animator animator;
        const float _GRAVITY = -9.81f;
        Vector3 _velocity;
        Vector2 _inputMoveVector;
        Vector3 _movement;

        void Start()
        {
            Inputs = InputHandler.Instance.Inputs;
            characterController = GetComponentInParent<CharacterController>();
            animator = GetComponentInParent<Animator>();
            Inputs.Player.Dash.performed += ctx => Dash();
            Inputs.Player.MouseMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
            Inputs.Player.GamepadMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
        }

        void Update()
        {
            CalculateDrag();
        }

        void CalculateDrag()
        {
            _movement = (_inputMoveVector.y * transform.forward) + (_inputMoveVector.x * transform.right);
            Vector3 currentDrag = drag * Time.deltaTime;
            _velocity.y += _GRAVITY * Time.deltaTime;
            _velocity.x /= 1 + currentDrag.x;
            _velocity.y /= 1 + currentDrag.y;
            _velocity.z /= 1 + currentDrag.z;
            characterController.Move(_velocity * Time.deltaTime);
        }

        void Dash()
        {
            StartCoroutine(AnimateDash());
            _velocity += Vector3.Scale(_movement, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
        }

        IEnumerator AnimateDash()
        {
            animator.SetBool("isDashing", true);
            yield return new WaitForSeconds(this.dashDuration);
            animator.SetBool("isDashing", false);
        }
    }
}
