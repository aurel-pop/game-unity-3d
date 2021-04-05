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

        void Start()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponentInChildren<Health>();
        }

        void Update()

        {
            if (playerHealth.Current < 1 && !isGameOver)
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
