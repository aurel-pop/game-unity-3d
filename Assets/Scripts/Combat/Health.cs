using Game.Core;
using Game.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] ProgressBarPro healthBar;
        int _health;
        public bool isDead;

        public int Current
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, maxHealth);
                healthBar.SetValue(_health, maxHealth);

                if (_health <= 0)
                {
                    Die();
                }
            }
        }

        void Start()
        {
            _health = maxHealth;
        }

        void Die()
        {
            isDead = true;
            GetComponentInChildren<Animator>().SetTrigger("die");

            if (tag == "Player")
            {
                GameManager.Instance.GameOver();
            }
            else if (tag == "AI")
            {
                GetComponent<AI>().Die();
            }
        }
    }
}
