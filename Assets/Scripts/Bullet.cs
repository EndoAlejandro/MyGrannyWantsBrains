using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;

    private Rigidbody _rigidbody;
    private Vector3 _direction;
    private float _speed;

    private IEnumerator _destroyAfterSeconds;
    private float _damage;

    private void Awake()
    {
        _rigidbody ??= GetComponent<Rigidbody>();
    }

    public void Setup(float damage,float speed)
    {
        _rigidbody ??= GetComponent<Rigidbody>();
        _speed = speed;
        _damage = damage;
        _direction = transform.forward;

        _destroyAfterSeconds = DestroyAfterSeconds();
        StartCoroutine(_destroyAfterSeconds);
    }

    private void Update()
    {
        if (_direction == Vector3.zero) return;
        _rigidbody.velocity = _direction.normalized * _speed;
    }

    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyBullet();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Zombie zombie)) zombie.TakeDamage(_damage);
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}