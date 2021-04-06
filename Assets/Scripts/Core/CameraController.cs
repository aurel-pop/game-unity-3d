using UnityEngine;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        private Transform _player;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            transform.position = _player.position;
        }
    }
}