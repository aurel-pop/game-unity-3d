using Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Enemy
{
    [RequireComponent(typeof(Animator))]
    public class AI : MonoBehaviour
    {
        [SerializeField] private Text stateTextUI;
        private Health _health;
        private Attacks _attacks;
        private NavMeshAgent _agent;
        private Animator _anim;
        private State _currentState;
        private Transform _player;

        private void Start()
        {
            _health = GetComponent<Health>();
            _attacks = GetComponent<Attacks>();
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _player = GameObject.FindWithTag("Player").transform;
            _currentState = new Idle(gameObject, _agent, _anim, _player);
        }

        private void Update()
        {
            _currentState = _currentState.Process();
            UpdateStateUI();
        }

        private void UpdateStateUI()
        {
            stateTextUI.text = _currentState.name.ToString();
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
            ResetAttacks();
            _currentState.Exit();
        }
        
        private void ResetAttacks()
        {
            if (_health.IsDead) return;
            _health.IsShielded = false;
            _health.IsInvulnerable = false;
            _attacks.IsPenetrating = false;
            _attacks.IsStunning = false;
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