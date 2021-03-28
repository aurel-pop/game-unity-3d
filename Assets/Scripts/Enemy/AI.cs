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

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            player = GameObject.FindWithTag("Player").transform;

            currentState = new Idle(gameObject, agent, anim, player);
        }

        void Update()
        {
            currentState = currentState.Process();
        }

        public void AnimationStart()
        {

        }

        public void AnimationDelayedStart()
        {

        }

        public void AnimationEnd()
        {
            currentState.Exit();
        }
    }
}
