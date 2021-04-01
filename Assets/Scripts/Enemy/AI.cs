using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class AI : MonoBehaviour
    {
        NavMeshAgent agent;
        Transform player;
        Animator anim;
        State currentState;
        Health health;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            player = GameObject.FindWithTag("Player").transform;
            health = GetComponent<Health>();

            currentState = new Idle(gameObject, agent, anim, player);
        }

        void Update()
        {
            if (health.isDead)
            {
                currentState = new Dead(gameObject, agent, anim, player);
                GetComponentInChildren<Hurtbox>().DisableHitbox();
                GetComponentInChildren<Hurtbox>().DisableHurtbox();
            }

            currentState = currentState.Process();
            Debug.Log(currentState);
        }

        void AnimationStart()
        {

        }

        void AnimationDelayedStart()
        {

        }

        void AnimationAttackHit()
        {
            GetComponentInChildren<Hurtbox>().EnableHitbox();
        }

        void AnimationAttackHitEnd()
        {
            GetComponentInChildren<Hurtbox>().DisableHitbox();
        }

        void AnimationEnd()
        {
            currentState.Exit();
        }
    }
}
