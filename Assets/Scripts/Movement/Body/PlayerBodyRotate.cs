using Game.Control;
using UnityEngine;

namespace Game.Movement.Body
{
    public class PlayerBodyRotate : MonoBehaviour
    {
        [SerializeField] Transform target;
        [Range(0.0f, 20.0f)] public float rotateSpeed;

        PlayerInputHandler PlayerInputHandler;

        void Awake()
        {
            PlayerInputHandler = GetComponentInParent<PlayerInputHandler>();
        }

        void Update()
        {
            if (PlayerInputHandler.takeRotation)
            {
                FaceTarget();
            }
        }

        void FaceTarget()
        {
            Vector3 direction = target.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSpeed);
        }
    }
}
