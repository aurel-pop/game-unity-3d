using System.Collections;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerDash : MonoBehaviour
    {
        public float dashDistance;
        public Vector3 drag;
        public float dashDuration;

        CharacterController characterController;
        Animator animator;

        const float _GRAVITY = -9.81f;
        Vector3 _velocity;
        Inputs _inputs;
        Vector2 _inputMoveVector;
        Vector3 _movement;

        void Awake()
        {
            _inputs = new Inputs();
            _inputs.Player.Dash.performed += ctx => Dash();
            _inputs.Player.MouseMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
            _inputs.Player.GamepadMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
        }

        void Start()
        {
            characterController = GetComponentInParent<CharacterController>();
            animator = GetComponentInParent<Animator>();
        }

        void Update()
        {
            CalculateDrag();
        }

        void CalculateDrag()
        {
            _movement = (_inputMoveVector.y * transform.forward) + (_inputMoveVector.x * transform.right);
            _velocity.y += _GRAVITY * Time.deltaTime;
            _velocity.x /= 1 + drag.x * Time.deltaTime;
            _velocity.y /= 1 + drag.y * Time.deltaTime;
            _velocity.z /= 1 + drag.z * Time.deltaTime;
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
