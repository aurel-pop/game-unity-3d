using System;
using UnityEngine;

namespace Combat
{
    public class Actions : MonoBehaviour
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
            ShieldRun,
            Stunned
        }
        
        private static readonly int Light = Animator.StringToHash("light");
        private static readonly int Combo = Animator.StringToHash("combo");
        private static readonly int Heavy = Animator.StringToHash("heavy");
        private static readonly int Super = Animator.StringToHash("super");
        private static readonly int Enrage = Animator.StringToHash("enrage");
        private static readonly int Shield = Animator.StringToHash("shield");
        private static readonly int Shieldrun = Animator.StringToHash("shieldrun");
        private static readonly int Stun = Animator.StringToHash("stun");
        private Animator _animator;
        private AudioSource _audio;
        private Health _health;
        [SerializeField] private int maxRage;
        [SerializeField] private ProgressBarPro rageBar;
        private int _rage;
        public int RageGain { get; set; }
        public int Damage { get; set; }
        public bool IsPenetrating { get; set; }
        public bool IsStunning { get; set; }
        public bool IsEnraged { get; set; }
        [SerializeField] private AudioClip[] sfx;

        public int Rage
        {
            get => _rage;
            set
            {
                _rage = Mathf.Clamp(value, 0, maxRage);
                rageBar.SetValue(_rage, maxRage);

                if (_rage < maxRage) return;
                IsEnraged = true;
                PerformAction(Type.Enrage);
            }
        }

        private void Start()
        {
            _health = GetComponentInChildren<Health>();
            _animator = GetComponentInChildren<Animator>();
            _audio = GetComponentInParent<AudioSource>();
            Rage = 0;
        }

        public void PerformAction(Type type)
        {
            switch (type)
            {
                case Type.None:
                    break;
                case Type.Light:
                    _animator.SetTrigger(Light);
                    _audio.PlayOneShot(sfx[0]);
                    RageGain = 10;
                    Damage = 10;
                    break;
                case Type.Combo:
                    _animator.SetTrigger(Combo);
                    _audio.PlayOneShot(sfx[1]);
                    RageGain = 20;
                    Damage = 20;
                    break;
                case Type.Heavy:
                    _animator.SetTrigger(Heavy);
                    _audio.PlayOneShot(sfx[2]);
                    IsPenetrating = true;
                    RageGain = 50;
                    Damage = 50;
                    break;
                case Type.Super:
                    if (!HasAmountOfRage(100)) return;
                    _animator.SetTrigger(Super);
                    _audio.PlayOneShot(sfx[3]);
                    IsPenetrating = true;
                    Damage = 100;
                    break;
                case Type.Enrage:
                    _animator.SetTrigger(Enrage);
                    IsStunning = true;
                    Rage -= 250;
                    break;
                case Type.Shield:
                    _animator.SetTrigger(Shield);
                    RageGain = 50;
                    break;
                case Type.ShieldRun:
                    if (!HasAmountOfRage(50)) return;
                    _animator.SetTrigger(Shieldrun);
                    break;
                case Type.Stunned:
                    _animator.SetTrigger(Stun);
                    _health.IsStunned = true;
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