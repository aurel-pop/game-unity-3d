using Combat;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager Instance { get; set; }
        private Health _playerHealth;

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

        private void Start()
        {
            _playerHealth = GameObject.FindWithTag("Player").GetComponentInChildren<Health>();
        }

        public static void GameOver()
        {
        }
    }
}
