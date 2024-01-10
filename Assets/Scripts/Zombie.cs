using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public event Action OnAttack;

    [SerializeField] private float damage = 1f;
    [SerializeField] private float attackTime = 2f;
    [SerializeField] private float attackRadius = 1.25f;

    private NavMeshAgent _navMeshAgent;
    private PlayerController _player;

    public void Setup(PlayerController player)
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = player;
    }

    private void Update()
    {
        if (_player == null) return;
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
        _navMeshAgent.isStopped = false;
    }

    public void PerformAttack()
    {
        var result = Physics.OverlapSphere(transform.position, attackRadius, _player.gameObject.layer);
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] == null) continue;
            if (!result[i].TryGetComponent(out PlayerController playerController)) continue;

            var forward = transform.forward;
            var direction = (_player.transform.position - transform.position).normalized;
            if (Vector3.Angle(forward, direction) < 45f) playerController.TakeDamage(damage);
        }
    }
}