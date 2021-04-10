using System;
using UnityEngine;

namespace Combat
{
    public class Attacks : MonoBehaviour
    {
        public enum Type
        {
            None,
            Light,
            Combo,
            Heavy,
            Super,
            Enrage,
            Shield,
            ShieldRun
        }
        
        private static readonly int Light = Animator.StringToHash("light");
        private static readonly int Combo = Animator.StringToHash("combo");
        private static readonly int Heavy = Animator.StringToHash("heavy");
        private static readonly int Super = Animator.StringToHash("super");
        private static readonly int Enrage = Animator.StringToHash("enrage");
        private static readonly int Shield = Animator.StringToHash("shield");
        private Animator _animator;
        
        private Health _health;
        [SerializeField] private int maxRage;
        [SerializeField] private ProgressBarPro rageBar;
        private int _rage;
        public int RageGain { get; set; }
        public int Damage { get; set; }
        public bool IsPenetrating { get; set; }
        public bool IsStunning { get; set; }
        public bool IsEnraged { get; set; }

        public int Rage
        {
            get => _rage;
            set
            {
                _rage = Mathf.Clamp(value, 0, maxRage);
                rageBar.SetValue(_rage, maxRage);

                if (_rage < maxRage) return;
                IsEnraged = true;
                PerformAttack(Type.Enrage);
            }
        }

        private void Start()
        {
            _health = GetComponentInChildren<Health>();
            _animator = GetComponentInChildren<Animator>();
            Rage = 0;
        }

        public void PerformAttack(Type type)
        {
            switch (type)
            {
                case Type.None:
                    break;
                case Type.Light:
                    _animator.SetTrigger(Light);
                    RageGain = 10;
                    Damage = 10;
                    break;
                case Type.Combo:
                    _animator.SetTrigger(Combo);
                    RageGain = 20;
                    Damage = 10;
                    break;
                case Type.Heavy:
                    _animator.SetTrigger(Heavy);
                    IsPenetrating = true;
                    RageGain = 50;
                    Damage = 50;
                    break;
                case Type.Super:
                    if (HasAmountOfRage(100)) _animator.SetTrigger(Super);
                    _health.IsInvulnerable = true;
                    IsPenetrating = true;
                    RageGain = 0;
                    Damage = 100;
                    break;
                case Type.Enrage:
                    _animator.SetTrigger(Enrage);
                    _health.IsInvulnerable = true;
                    IsStunning = true;
                    IsEnraged = true;
                    RageGain = 0;
                    Damage = 0;
                    break;
                case Type.Shield:
                    _animator.SetTrigger(Shield);
                    _health.IsShielded = true;
                    RageGain = 50;
                    Damage = 0;
                    break;
                case Type.ShieldRun:
                    if (HasAmountOfRage(50)) _animator.SetTrigger(Shield);
                    _health.IsShielded = true;
                    RageGain = 0;
                    Damage = 50;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        private bool HasAmountOfRage(int amount)
        {
            if (Rage < amount) return false;
            Rage -= amount;
            return true;
        }
    }
}