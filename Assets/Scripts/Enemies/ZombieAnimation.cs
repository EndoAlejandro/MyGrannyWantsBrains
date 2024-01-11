using System;
using UnityEngine;

public class ZombieAnimation : MonoBehaviour
{
    private Animator _animator;
    private Zombie _zombie;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Dead = Animator.StringToHash("Dead");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _zombie = GetComponentInParent<Zombie>();
    }

    private void Start()
    {
        _zombie.OnAttack += ZombieOnAttack;
        _zombie.OnDead += ZombieOnDead;
    }

    private void ZombieOnDead() => _animator.SetTrigger(Dead);

    private void ZombieOnAttack()
    {
        _animator.SetTrigger(Attack);
    }

    private void PerformAttack() => _zombie.PerformAttack();
    private void AttackEnded() => _zombie.AttackEnded();

    private void OnDestroy()
    {
        _zombie.OnAttack -= ZombieOnAttack;
        _zombie.OnDead -= ZombieOnDead;
    }
}