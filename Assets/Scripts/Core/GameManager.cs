using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public bool IsGameOver { get; private set; }
        Health playerHealth;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponentInChildren<Health>();
        }

        public void GameOver()
        {
            IsGameOver = true;
        }
    }
}
