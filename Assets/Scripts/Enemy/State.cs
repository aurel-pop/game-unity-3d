using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class State
    {
        public enum States { Idle, Chase, Attack, Hit, MoveBack, Dead, Won }
        public enum Event { Enter, Update, Exit }
        public States name;
        protected Event phase;

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
            phase = Event.Enter;
            player = _player;
        }

        public virtual void Enter()
        {
            Debug.Log(name);
            phase = Event.Update;
        }
        public virtual void Update()
        {
            UpdateAnimator();
        }
        public virtual void Exit() { phase = Event.Exit; }

        public State Process()
        {
            if (phase == Event.Enter) Enter();
            if (phase == Event.Update) Update();
            if (phase == Event.Exit)
            {
                Exit();
                return nextState;
            }
            return this;
        }

        void UpdateAnimator()
        {
            Vector3 localVelocity = npc.transform.InverseTransformDirection(agent.velocity);
            anim.SetFloat("forwardSpeed", localVelocity.z * 10 / agent.speed);
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
            name = States.Idle;
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
                phase = Event.Exit;
            }

            base.Update();
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
            name = States.Chase;
            agent.isStopped = false;
        }

        public override void Enter()
        {
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
                    phase = Event.Exit;
                }
                else if (!CanSeePlayer())
                {
                    nextState = new Idle(npc, agent, anim, player);
                    phase = Event.Exit;
                }
            }

            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Attack : State
    {
        public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
        {
            name = States.Attack;
        }

        public override void Enter()
        {
            agent.isStopped = true;
            Combat.Attack.Directions attack = Combat.Attack.Directions.None;

            int rng = Random.Range(0, 200);

            if (rng < 25)
            {
                attack = Combat.Attack.Directions.Right;
            }
            else if (rng >= 25 && rng < 50)
            {
                attack = Combat.Attack.Directions.Left;
            }
            else if (rng >= 50 && rng < 75)
            {
                attack = Combat.Attack.Directions.Up;
            }
            else if (rng >= 75 && rng < 100)
            {
                attack = Combat.Attack.Directions.Down;
            }
            else
            {
                attack = Combat.Attack.Directions.Shield;
            }

            npc.GetComponent<TriggerAttacks>().TriggerAttack(attack);

            base.Enter();
        }

        public override void Update()
        {
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            direction.y = 0;
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

            base.Update();
        }

        public override void Exit()
        {
            if (npc.GetComponent<Health>().isDead)
                nextState = new Dead(npc, agent, anim, player);
            else if (player.GetComponentInChildren<Health>().isDead)
                nextState = new Won(npc, agent, anim, player);
            else if (CanAttackPlayer())
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
            name = States.Hit;
        }

        public override void Enter()
        {                
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            if (npc.GetComponent<Health>().isDead)
                nextState = new Dead(npc, agent, anim, player);
            else if (CanAttackPlayer())
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
            name = States.MoveBack;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
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
            name = States.Dead;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Won : State
    {
        public Won(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
        {
            name = States.Won;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
