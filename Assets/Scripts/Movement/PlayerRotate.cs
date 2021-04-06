using Control;
using UnityEngine;

namespace Movement
{
    public class PlayerRotate : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 20.0f)] private float rotateSpeed = 4f;
        private Inputs _inputs;
        private Vector2 _inputMoveVector;

        private void Start()
        {
            _inputs = InputHandler.Instance.Inputs;
            _inputs.Player.MouseMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
            _inputs.Player.GamepadMove.performed += ctx => _inputMoveVector = ctx.ReadValue<Vector2>();
        }

        private void Update()
        {
            if (InputHandler.Instance.TakeRotation)
            {
                RotateCharacter();
            }
        }

        private void RotateCharacter()
        {
            Vector3 direction = new Vector3(_inputMoveVector.x, 0f, _inputMoveVector.y);
            if (_inputMoveVector.magnitude > 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSpeed);
        }
    }
}
