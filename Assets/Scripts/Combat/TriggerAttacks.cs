using System;
using UnityEngine;

namespace Combat
{
    public class TriggerAttacks : MonoBehaviour
    {
        private static readonly int Light = Animator.StringToHash("light");
        private static readonly int Combo = Animator.StringToHash("combo");
        private static readonly int Heavy = Animator.StringToHash("heavy");
        private static readonly int Super = Animator.StringToHash("super");
        private static readonly int Enrage = Animator.StringToHash("enrage");
        private static readonly int Shield = Animator.StringToHash("shield");
        [SerializeField] private AudioClip[] attackAudioClips;
        private Animator _animator;
        private AudioSource _audioSource;
        private Health _health;

        private void Start()
        {
            _health = GetComponentInChildren<Health>();
            _audioSource = GetComponentInParent<AudioSource>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void TriggerAttack(Attack.Directions dir)
        {
            switch (dir)
            {
                case Attack.Directions.None:
                    break;
                case Attack.Directions.Light:
                    _animator.SetTrigger(Light);
                    break;
                case Attack.Directions.Combo:
                    _animator.SetTrigger(Combo);
                    break;
                case Attack.Directions.Heavy:
                    _animator.SetTrigger(Heavy);
                    break;
                case Attack.Directions.Super:
                    _animator.SetTrigger(Super);
                    break;
                case Attack.Directions.Enrage:
                    _animator.SetTrigger(Enrage);
                    break;
                case Attack.Directions.Shield:
                    _health.IsShielded = true;
                    _animator.SetTrigger(Shield);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        }
    }
}