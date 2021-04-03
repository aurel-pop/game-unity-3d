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
            currentState = currentState.Process();
        }

        public void Die()
        {
            currentState = new Dead(gameObject, agent, anim, player);
            GetComponentInChildren<Hurtbox>().DisableHitbox();
            GetComponentInChildren<Hurtbox>().DisableHurtbox();
        }

        void AnimationStart()
        {

        }

        void AnimationDelayedStart()
        {

        }

        void AnimationEnd()
        {
            GetComponentInParent<TriggerAttacks>().StopShielded();
            currentState.Exit();
        }

        void AnimationAttackHitStart()
        {
            GetComponentInChildren<Hurtbox>().EnableHitbox();
        }

        void AnimationAttackHitEnd()
        {
            GetComponentInChildren<Hurtbox>().DisableHitbox();
        }

        void AnimationIsHitStart()
        {
            currentState = new Hit(gameObject, agent, anim, player);
        }

        void AnimationIsHitEnd()
        {
            
        }
    }
}
