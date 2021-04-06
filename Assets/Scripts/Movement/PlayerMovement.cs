using Control;
using UnityEngine;

namespace Movement
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
        private CharacterController _characterController;
        private Animator _animator;
        private Inputs _inputs;
        private Vector2 _inputMoveVector;
        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");

        private void Start()
        {
            _inputs = InputHandler.Instance.Inputs;
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
            _inputs.Player.MouseMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
            _inputs.Player.GamepadMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
        }

        private void Update()
        {
            if (InputHandler.Instance.TakeMovement)
            {
                MoveCharacter();
            }

            UpdateAnimator();
        }

        private void MoveCharacter()
        {
            Vector3 velocity = (_inputMoveVector.y * transform.forward) + (_inputMoveVector.x * transform.right);
            _characterController.Move(velocity * (moveSpeed * Time.deltaTime));
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _characterController.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            _animator.SetFloat(ForwardSpeed, Mathf.Lerp(_animator.GetFloat(ForwardSpeed), localVelocity.magnitude, animationSpeed * Time.deltaTime));
        }
    }
}
