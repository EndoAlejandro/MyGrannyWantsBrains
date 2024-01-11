using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public event Action OnAttack;
    public event Action OnDead;

    [SerializeField] private float maxHealth;

    [SerializeField] private float damage = 1f;
    [SerializeField] private float attackTime = 2f;
    [SerializeField] private float attackRadius = 2f;

    private NavMeshAgent _navMeshAgent;
    private PlayerController _player;
    private Collider _collider;

    private float health;

    public void Setup(PlayerController player)
    {
        VfxManager.Instance.PlayFx(Vfx.ZombieSpawn, transform.position);

        _collider = GetComponent<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = player;

        health = maxHealth;
    }

    private void Update()
    {
        if (_player == null) return;
        if (health <= 0f) return;

        _navMeshAgent.SetDestination(_player.transform.position);

        if (Vector3.Distance(transform.position, _player.transform.position) <= 1.1f)
        {
            StartCoroutine(AttackAsync());
        }
    }

    private IEnumerator AttackAsync()
    {
        _navMeshAgent.isStopped = true;
        OnAttack?.Invoke();
        yield return new WaitForSeconds(attackTime);
    }

    public void PerformAttack()
    {
        var result = Physics.OverlapSphere(transform.position, attackRadius, ~_player.gameObject.layer);
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] == null) continue;
            if (!result[i].TryGetComponent(out PlayerController playerController)) continue;

            var forward = transform.forward;
            var direction = (_player.transform.position - transform.position).normalized;
            var angle = Vector3.Dot(forward, direction);
            if (angle > .5f)
                playerController.TakeDamage(damage);
        }
    }

    public void AttackEnded()
    {
        _navMeshAgent.isStopped = false;
    }

    public void TakeDamage(float incomingDamage)
    {
        health = Mathf.Max(health - incomingDamage, 0f);
        VfxManager.Instance.PlayFx(Vfx.ZombieTakeDamage, transform.position + Vector3.up);

        if (health <= 0f)
        {
            _collider.enabled = false;
            _navMeshAgent.enabled = false;
            StartCoroutine(DespawnAsync());
            OnDead?.Invoke();
        }
    }

    private IEnumerator DespawnAsync()
    {
        yield return new WaitForSeconds(2f);
        VfxManager.Instance.PlayFx(Vfx.ZombieDespawn, transform.position);
        yield return null;
        Destroy(gameObject);
    }
}