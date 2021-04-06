using System.Collections;
using Control;
using UnityEngine;

namespace Movement
{
    public class PlayerDash : MonoBehaviour
    {
        public float dashDistance;
        public Vector3 drag;
        public float dashDuration;
        private Inputs _inputs;
        private CharacterController _characterController;
        private Animator _animator;
        private const float Gravity = -9.81f;
        private Vector3 _velocity;
        private Vector2 _inputMoveVector;
        private Vector3 _movement;
        private static readonly int IsDashing = Animator.StringToHash("isDashing");

        private void Start()
        {
            _inputs = InputHandler.Instance.Inputs;
            _characterController = GetComponentInParent<CharacterController>();
            _animator = GetComponentInParent<Animator>();
            _inputs.Player.Dash.performed += ctx => Dash();
            _inputs.Player.MouseMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
            _inputs.Player.GamepadMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
        }

        private void Update()
        {
            CalculateDrag();
        }

        private void CalculateDrag()
        {
            _movement = (_inputMoveVector.y * transform.forward) + (_inputMoveVector.x * transform.right);
            Vector3 currentDrag = drag * Time.deltaTime;
            _velocity.y += Gravity * Time.deltaTime;
            _velocity.x /= 1 + currentDrag.x;
            _velocity.y /= 1 + currentDrag.y;
            _velocity.z /= 1 + currentDrag.z;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void Dash()
        {
            StartCoroutine(AnimateDash());
            _velocity += Vector3.Scale(_movement, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
        }

        private IEnumerator AnimateDash()
        {
            _animator.SetBool(IsDashing, true);
            yield return new WaitForSeconds(this.dashDuration);
            _animator.SetBool(IsDashing, false);
        }
    }
}
