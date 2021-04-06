using Control;
using Core;
using Enemy;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        private static readonly int Die1 = Animator.StringToHash("die");
        [SerializeField] private int maxHealth;
        [SerializeField] private ProgressBarPro healthBar;
        private int _health;
        public bool IsDead { get; private set; }
        public bool IsShielded { get; set; }

        public int Current
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, maxHealth);
                healthBar.SetValue(_health, maxHealth);

                if (_health <= 0) Die();
            }
        }

        private void Start()
        {
            _health = maxHealth;
        }

        private void Die()
        {
            IsDead = true;
            GetComponentInChildren<Animator>().SetTrigger(Die1);

            if (CompareTag("Player"))
            {
                InputHandler.Instance.TakeAttacks = false;
                InputHandler.Instance.TakeMovement = false;
                InputHandler.Instance.TakeRotation = false;
                GameManager.GameOver();
            }
            else if (CompareTag("AI"))
            {
                GetComponent<AI>().Die();
            }
        }
    }
}