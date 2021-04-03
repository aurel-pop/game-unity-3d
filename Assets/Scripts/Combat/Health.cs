using Game.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] ProgressBarPro healthBar;

        PlayerInputHandler playerInputHandler;
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

        void Awake()
        {
            playerInputHandler = GetComponentInParent<PlayerInputHandler>();
        }

        void Start()
        {
            health = maxHealth;
            isDead = false;
        }

        void Die()
        {
            isDead = true;
            GetComponentInChildren<Animator>().SetTrigger("Die");
        }
    }
}
