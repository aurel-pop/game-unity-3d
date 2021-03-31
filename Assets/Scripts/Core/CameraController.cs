using UnityEngine;

namespace Game.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target;

        void Start()
        {

        }


        void Update()
        {
            transform.position = target.position;
        }
    }
}
