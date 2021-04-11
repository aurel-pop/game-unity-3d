using Control;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputHandler))]
    public class PlayerMovement : MonoBehaviour
    {
        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");
        [Range(0.0f, 20.0f)] public float moveSpeed;
        [Range(0.0f, 20.0f)] public float animationSpeed;
        private Animator _animator;
        private CharacterController _characterController;
        private Vector2 _inputMoveVector;
        private Inputs _inputs;

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
            if (InputHandler.Instance.TakeMovement) MoveCharacter();
            UpdateAnimator();
        }

        private void MoveCharacter()
        {
            var velocity = _inputMoveVector.y * transform.forward + _inputMoveVector.x * transform.right;
            _characterController.Move(velocity * (moveSpeed * Time.deltaTime));
        }

        private void UpdateAnimator()
        {
            var velocity = _characterController.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            _animator.SetFloat(ForwardSpeed,
                Mathf.Lerp(_animator.GetFloat(ForwardSpeed), localVelocity.magnitude, animationSpeed * Time.deltaTime));
        }
    }
}