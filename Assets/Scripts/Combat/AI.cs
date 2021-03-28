using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Combat
{
    public class AI : MonoBehaviour
    {
        NavMeshAgent agent;
        Transform player;
        Animator anim;
        State currentState;
        public AudioClip[] clips;

        void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            anim = this.GetComponent<Animator>();
            player = GameObject.FindWithTag("Player").transform;
            currentState = new Idle(this.gameObject, agent, anim, player);
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
            currentState.stage = State.Event.Exit;
        }
    }
}
