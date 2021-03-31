using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        Health playerHealth;
        public bool isGameOver;

        void Awake()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponentInChildren<Health>();
        }

        void Start()
        {

        }

        void Update()

        {
            if (playerHealth.Value < 1 && !isGameOver)
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            isGameOver = true;
        }
    }
}
