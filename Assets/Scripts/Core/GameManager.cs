using Combat;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        private Health _playerHealth;
        private static GameManager Instance { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _playerHealth = GameObject.FindWithTag("Player").GetComponentInChildren<Health>();
        }

        public static void GameOver()
        {
        }
    }
}