using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class Hitbox : MonoBehaviour
    {
        [SerializeField] float radius;
        public LayerMask mask;
        public Color inactiveColor = new Color(1f, 0f, 0f, 0.3f);
        public Color collisionOpenColor = new Color(0f, 1f, 0f, 0.3f);
        public Color collidingColor = new Color(1f, 0.92f, 0.016f, 0.3f);

        public enum ColliderState { Closed, Open, Colliding };
        ColliderState _state;

        IHitboxResponder _responder = null;

        void Update()
        {
            CheckCollisions();
        }

        void CheckCollisions()
        {
            if (_state == ColliderState.Closed) { return; }

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, mask);

            for (int i = 0; i < colliders.Length; i++)
            {

                Collider aCollider = colliders[i];

                _responder?.collisionedWith(aCollider);

            }

            _state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
        }

        public void useResponder(IHitboxResponder responder)
        {
            _responder = responder;
        }

        public void OnDrawGizmosSelected()
        {
            checkGizmoColor();
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.DrawSphere(Vector3.zero, radius);
        }

        void checkGizmoColor()
        {
            switch (_state)
            {
                case ColliderState.Closed:
                    Gizmos.color = inactiveColor;
                    break;
                case ColliderState.Open:
                    Gizmos.color = collisionOpenColor;
                    break;
                case ColliderState.Colliding:
                    Gizmos.color = collidingColor;
                    break;
            }
        }

        public void startCheckingCollision()
        {
            _state = ColliderState.Open;
        }

        public void stopCheckingCollision()
        {
            _state = ColliderState.Closed;
        }

        public interface IHitboxResponder
        {

            void collisionedWith(Collider collider);
        }
    }
}