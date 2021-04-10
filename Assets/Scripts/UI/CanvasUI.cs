using UnityEngine;

namespace UI
{
    public class CanvasUI : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;

        private void Update()
        {
            transform.position = target.position + offset;
        }
    }
}