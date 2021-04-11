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
        private Actions _actions;
        private NavMeshAgent _agent;
        private Animator _anim;
        private Hurtbox _hurtbox;
        private State _currentState;
        private Transform _player;

        private void Start()
        {
            _health = GetComponent<Health>();
            _actions = GetComponent<Actions>();
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _hurtbox = GetComponentInChildren<Hurtbox>();
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
        
        public void GotHit()
        {
            _currentState = new Hit(gameObject, _agent, _anim, _player);
        }
        
        public void ExitState()
        {
            _currentState.Exit();
        }

        public void Die()
        {
            _currentState = new Dead(gameObject, _agent, _anim, _player);
            _hurtbox.DisableHitbox();
            _hurtbox.DisableHurtbox();
        }
    }
}