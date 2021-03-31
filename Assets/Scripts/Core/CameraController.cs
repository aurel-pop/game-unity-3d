using UnityEngine;

namespace Game.Core
{
    public class CameraController : MonoBehaviour
    {
        Transform player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        void Update()
        {
            transform.position = player.position;
        }
    }
}
