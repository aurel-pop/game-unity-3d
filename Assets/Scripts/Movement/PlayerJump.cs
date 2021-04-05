using Game.Control;
using System;
using UnityEngine;

namespace Game.Movement
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] Transform groundChecker;
        public float jumpHeight;
        bool _isGrounded;
        Inputs Inputs;

        void Start()
        {
            Inputs = InputHandler.Instance.Inputs;
        }
    }
}
