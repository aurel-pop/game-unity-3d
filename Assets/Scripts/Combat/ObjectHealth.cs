using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class ObjectHealth : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] ProgressBarPro healthBar;
        Animator anim;
        int health;

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
            anim = GetComponent<Animator>();
        }

        void Start()
        {
            health = maxHealth;
        }

        void Die()
        {
            anim.SetTrigger("Die");
        }
    }
}
