using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        ObjectHealth playerHealth;
        public bool isGameOver;

        void Awake()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<ObjectHealth>();
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
            Debug.Log("GAME OVER!");
        }
    }
}
