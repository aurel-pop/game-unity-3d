using Game.Control;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerRotate : MonoBehaviour
    {

        [Range(0.0f, 10.0f)] public float rotateSpeed;

        PlayerInputHandler PlayerInputHandler;

        Inputs inputs;
        Vector2 inputMoveVector;

        void Awake()
        {
            inputs = new Inputs();
        }

        void Start()
        {
            PlayerInputHandler = transform.parent.GetComponent<PlayerInputHandler>();
        }

        void Update()
        {
            inputMoveVector = inputs.Player.Move.ReadValue<Vector2>();

            if (PlayerInputHandler.takeRotation)
            {
                // right
                if (inputMoveVector.x == 1f)
                    Rotate(90f);
                // left
                else if (inputMoveVector.x == -1f)
                    Rotate(270f);

                // top
                if (inputMoveVector.y == 1f)
                    Rotate(0f);
                // down
                else if (inputMoveVector.y == -1f)
                    Rotate(180f);

                // top right
                if ((inputMoveVector.x > 0) && (inputMoveVector.x < 1f) && (inputMoveVector.y > 0) && (inputMoveVector.y < 1f))
                    Rotate(45f);
                // bottom right
                else if ((inputMoveVector.x > 0) && (inputMoveVector.x < 1f) && (inputMoveVector.y < 0) && (inputMoveVector.y > -1f))
                    Rotate(135f);
                // bottom left
                else if ((inputMoveVector.x < 0) && (inputMoveVector.x > -1f) && (inputMoveVector.y < 0) && (inputMoveVector.y > -1f))
                    Rotate(225f);
                // bottom right
                else if ((inputMoveVector.x < 0) && (inputMoveVector.x > -1f) && (inputMoveVector.y > 0) && (inputMoveVector.y < 1f))
                    Rotate(315f);
            }
        }

        void Rotate(float rotationDegrees)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, rotationDegrees, 0f);
            transform.localRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * 100 * Time.deltaTime);
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
