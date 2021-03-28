using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class State
    {
        public enum STATE { Idle, Chase, Attack, Hit, MoveBack, Dead };
        protected enum Event { Enter, Update, Exit };

        protected STATE name;
        protected Event stage;
        protected GameObject npc;
        protected Animator anim;
        protected Transform player;
        protected State nextState;
        protected NavMeshAgent agent;

        float visDist = 15.0f;
        float visAngle = 90.0f;
        float attackDist = 3f;
        public float rotationSpeed = 5.0f;

        public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        {
            npc = _npc;
            agent = _agent;
            anim = _anim;
            stage = Event.Enter;
            player = _player;
        }

        public virtual void Enter() { stage = Event.Update; }
        public virtual void Update() { stage = Event.Update; }
        public virtual void Exit() { stage = Event.Exit; }

        public State Process()
        {
            if (stage == Event.Enter) Enter();
            if (stage == Event.Update) Update();
            if (stage == Event.Exit)
            {
                Exit();
                return nextState;
            }
            return this;
        }

        public bool CanSeePlayer()
        {
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);

            if (direction.magnitude < visDist && angle < visAngle)
            {
                return true;
            }
            return false;
        }

        public bool CanAttackPlayer()
        {
            Vector3 direction = player.position - npc.transform.position;

            if (direction.magnitude < attackDist)
            {
                return true;
            }
            return false;
        }
    }

    public class Idle : State
    {
        public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
        {
            name = STATE.Idle;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            if (CanSeePlayer())
            {
                nextState = new Chase(npc, agent, anim, player);
                stage = Event.Exit;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Chase : State
    {
        public Chase(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
        {
            name = STATE.Chase;
            agent.isStopped = false;
        }

        public override void Enter()
        {
            anim.SetBool("isRunning", true);
            base.Enter();
        }

        public override void Update()
        {
            agent.SetDestination(player.position);

            if (agent.hasPath)
            {
                if (CanAttackPlayer())
                {
                    nextState = new Attack(npc, agent, anim, player);
                    stage = Event.Exit;
                }
                else if (!CanSeePlayer())
                {
                    nextState = new Idle(npc, agent, anim, player);
                    stage = Event.Exit;
                }
            }
        }

        public override void Exit()
        {
            anim.SetBool("isRunning", false);
            base.Exit();
        }
    }

    public class Attack : State
    {
        public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
        {
            name = STATE.Attack;
        }

        public override void Enter()
        {
            agent.isStopped = true;
            Attacks.Direction attack = Attacks.Direction.Null;

            int rng = Random.Range(0, 100);

            if (rng < 25)
            {
                attack = Attacks.Direction.Right;
            }
            else if (rng >= 25 && rng < 50)
            {
                attack = Attacks.Direction.Left;
            }
            else if (rng >= 50 && rng < 75)
            {
                attack = Attacks.Direction.Up;
            }
            else
            {
                attack = Attacks.Direction.Down;
            }

            npc.GetComponent<TriggerAttacks>().Trigger(attack);

            base.Enter();
        }

        public override void Update()
        {
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            direction.y = 0;
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        }

        public override void Exit()
        {
            if (CanAttackPlayer())
                nextState = new Attack(npc, agent, anim, player);
            else if (CanSeePlayer())
                nextState = new Chase(npc, agent, anim, player);
            else
                nextState = new Idle(npc, agent, anim, player);

            base.Exit();
        }
    }

    public class Hit : State
    {
        public Hit(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
        {
            name = STATE.Hit;
        }

        public override void Enter()
        {                
            base.Enter();
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            if (CanAttackPlayer())
                nextState = new Attack(npc, agent, anim, player);
            else if (CanSeePlayer())
                nextState = new Chase(npc, agent, anim, player);
            else
                nextState = new Idle(npc, agent, anim, player);

            base.Exit();
        }
    }

    public class MoveBack : State
    {
        public MoveBack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
        {
            name = STATE.MoveBack;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Dead : State
    {
        public Dead(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
        {
            name = STATE.Dead;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
