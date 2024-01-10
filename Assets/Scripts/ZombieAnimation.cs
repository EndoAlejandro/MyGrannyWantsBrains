using System;
using UnityEngine;

public class ZombieAnimation : MonoBehaviour
{
    private Animator _animator;
    private Zombie _zombie;
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _zombie = GetComponentInParent<Zombie>();
    }

    private void Start()
    {
        _zombie.OnAttack += ZombieOnAttack;
    }

    private void ZombieOnAttack()
    {
        _animator.SetTrigger(Attack);
    }

    private void PerformAttack() => _zombie.PerformAttack();
}