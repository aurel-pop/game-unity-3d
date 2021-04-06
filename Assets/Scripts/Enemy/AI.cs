using Combat;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class AI : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Transform _player;
        private Animator _anim;
        private State _currentState;
        private Health _health;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _player = GameObject.FindWithTag("Player").transform;
            _health = GetComponent<Health>();
            _currentState = new Idle(gameObject, _agent, _anim, _player);
        }

        private void Update()
        {
            _currentState = _currentState.Process();
        }

        public void Die()
        {
            _currentState = new Dead(gameObject, _agent, _anim, _player);
            GetComponentInChildren<Hurtbox>().DisableHitbox();
            GetComponentInChildren<Hurtbox>().DisableHurtbox();
        }

        private void AnimationStart()
        {

        }

        private void AnimationDelayedStart()
        {

        }

        private void AnimationEnd()
        {
            GetComponentInParent<TriggerAttacks>().StopShielded();
            _currentState.Exit();
        }

        private void AnimationAttackHitStart()
        {
            GetComponentInChildren<Hurtbox>().EnableHitbox();
        }

        private void AnimationAttackHitEnd()
        {
            GetComponentInChildren<Hurtbox>().DisableHitbox();
        }

        private void AnimationIsHitStart()
        {
            _currentState = new Hit(gameObject, _agent, _anim, _player);
        }

        private void AnimationIsHitEnd()
        {
            
        }
    }
}
