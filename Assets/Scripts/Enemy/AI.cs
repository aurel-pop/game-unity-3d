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
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player").transform;

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
        }

        void AnimationStart()
        {

        }

        void AnimationDelayedStart()
        {

        }

        void AnimationAttackHit()
        {
            GetComponentInParent<Transform>().GetComponentInChildren<Hurtbox>().EnableHitbox();
        }

        void AnimationAttackHitEnd()
        {
            GetComponentInParent<Transform>().GetComponentInChildren<Hurtbox>().DisableHitbox();
        }

        void AnimationEnd()
        {
            currentState.Exit();
        }
    }
}
