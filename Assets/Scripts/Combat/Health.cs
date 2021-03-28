using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] ProgressBarPro healthBar;
        int health;
        public bool isDead;

        public int Value
        {
            get => health;
            set
            {
                health = Mathf.Clamp(value, 0, maxHealth);
                healthBar.SetValue(health, maxHealth);

                if (health < 1)
                {
                    Die();
                }
            }
        }

        void Start()
        {
            health = maxHealth;
            isDead = false;
        }

        void Die()
        {
            GetComponent<Animator>().SetTrigger("Die");
            isDead = true;
        }
    }
}
