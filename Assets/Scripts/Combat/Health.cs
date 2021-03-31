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

            if(gameObject.tag == "Player")
            {
                GetComponent<PlayerInputHandler>().takeAttacks = false;
                GetComponent<PlayerInputHandler>().takeMovement = false;
                GetComponent<PlayerInputHandler>().takeRotation = false;
                GetComponent<PlayerInputHandler>().takeDamage = false;
            }
        }
    }
}
