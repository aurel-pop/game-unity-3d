using Combat;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class State
    {
        protected enum States { Idle, Chase, Attack, Hit, Dead, Won }
        protected enum Event { Enter, Update, Exit }
        protected States name;
        protected Event phase;

        protected readonly GameObject npc;
        protected readonly Animator animator;
        protected readonly Transform player;
        protected readonly NavMeshAgent navMeshAgent;
        protected State nextState;

        private const float VisDist = 15.0f;
        private const float VisAngle = 90.0f;
        private const float AttackDist = 3f;
        protected const float RotationSpeed = 5.0f;

        protected State(GameObject npc, NavMeshAgent navMeshAgent, Animator animator, Transform player)
        {
            this.npc = npc;
            this.navMeshAgent = navMeshAgent;
            this.animator = animator;
            this.player = player;
            phase = Event.Enter;
        }

        protected virtual void Enter()
        {
            Debug.Log(name);
            phase = Event.Update;
        }

        protected virtual void Update()
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

        private void UpdateAnimator()
        {
            Vector3 localVelocity = npc.transform.InverseTransformDirection(navMeshAgent.velocity);
            animator.SetFloat("forwardSpeed", localVelocity.z * 10 / navMeshAgent.speed);
        }

        protected bool CanSeePlayer()
        {
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);

            return direction.magnitude < VisDist && angle < VisAngle;
        }

        protected bool CanAttackPlayer()
        {
            Vector3 direction = player.position - npc.transform.position;

            return direction.magnitude < AttackDist;
        }
    }

    public class Idle : State
    {
        public Idle(GameObject npc, NavMeshAgent navMeshAgent, Animator animator, Transform player) : base(npc, navMeshAgent, animator, player)
        {
            name = States.Idle;
        }

        protected override void Update()
        {
            if (CanSeePlayer())
            {
                nextState = new Chase(npc, navMeshAgent, animator, player);
                phase = Event.Exit;
            }

            base.Update();
        }
    }

    public class Chase : State
    {
        public Chase(GameObject npc, NavMeshAgent navMeshAgent, Animator animator, Transform player) : base(npc, navMeshAgent, animator, player)
        {
            name = States.Chase;
            navMeshAgent.isStopped = false;
        }

        protected override void Update()
        {
            navMeshAgent.SetDestination(player.position);

            if (navMeshAgent.hasPath)
            {
                if (CanAttackPlayer())
                {
                    nextState = new Attack(npc, navMeshAgent, animator, player);
                    phase = Event.Exit;
                }
                else if (!CanSeePlayer())
                {
                    nextState = new Idle(npc, navMeshAgent, animator, player);
                    phase = Event.Exit;
                }
            }

            base.Update();
        }
    }

    public class Attack : State
    {
        public Attack(GameObject npc, NavMeshAgent navMeshAgent, Animator animator, Transform player) : base(npc, navMeshAgent, animator, player)
        {
            name = States.Attack;
        }

        protected override void Enter()
        {
            navMeshAgent.isStopped = true;
            Combat.Attack.Directions attack;
            int rng = Random.Range(0, 100);
            if (rng < 25)
            {
                attack = Combat.Attack.Directions.Combo;
            }
            else if (rng < 50)
            {
                attack = Combat.Attack.Directions.Light;
            }
            else if (rng < 75)
            {
                attack = Combat.Attack.Directions.Super;
            }
            else
            {
                attack = Combat.Attack.Directions.Heavy;
            }

            npc.GetComponent<TriggerAttacks>().TriggerAttack(attack);

            base.Enter();
        }

        protected override void Update()
        {
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            direction.y = 0;
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * RotationSpeed);

            base.Update();
        }

        public override void Exit()
        {
            if (npc.GetComponent<Health>().IsDead)
                nextState = new Dead(npc, navMeshAgent, animator, player);
            else if (player.GetComponentInChildren<Health>().IsDead)
                nextState = new Won(npc, navMeshAgent, animator, player);
            else if (CanAttackPlayer())
                nextState = new Attack(npc, navMeshAgent, animator, player);
            else if (CanSeePlayer())
                nextState = new Chase(npc, navMeshAgent, animator, player);
            else
                nextState = new Idle(npc, navMeshAgent, animator, player);

            base.Exit();
        }
    }

    public class Hit : State
    {
        public Hit(GameObject npc, NavMeshAgent navMeshAgent, Animator animator, Transform player) : base(npc, navMeshAgent, animator, player)
        {
            name = States.Hit;
        }

        public override void Exit()
        {
            if (npc.GetComponent<Health>().IsDead)
                nextState = new Dead(npc, navMeshAgent, animator, player);
            else if (CanAttackPlayer())
                nextState = new Attack(npc, navMeshAgent, animator, player);
            else if (CanSeePlayer())
                nextState = new Chase(npc, navMeshAgent, animator, player);
            else
                nextState = new Idle(npc, navMeshAgent, animator, player);

            base.Exit();
        }
    }

    public class Dead : State
    {
        public Dead(GameObject npc, NavMeshAgent navMeshAgent, Animator animator, Transform player) : base(npc, navMeshAgent, animator, player)
        {
            name = States.Dead;
        }
    }

    public class Won : State
    {
        public Won(GameObject npc, NavMeshAgent navMeshAgent, Animator animator, Transform player) : base(npc, navMeshAgent, animator, player)
        {
            name = States.Won;
        }
    }
}
