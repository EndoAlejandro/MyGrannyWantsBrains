using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GrannyController : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform[] nozzles;

    private Collider[] _colliders;
    private Rigidbody _rigidbody;
    private Collider _playerCollider;

    private bool _grabbed;

    private InputReader _input;

    private void Awake()
    {
        _input = new InputReader();

        _rigidbody = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        if (!_grabbed) return;
        if (_input.Shoot) Shoot();
        ResetTransform();
    }

    private void Shoot()
    {
        Transform randomNozzle = nozzles[Random.Range(0, nozzles.Length)];
        Bullet bullet = Instantiate(bulletPrefab, randomNozzle.position, randomNozzle.rotation);
        bullet.Setup(damage, bulletSpeed);
    }

    public void Grab(Transform newParent, Collider playerCollider)
    {
        _playerCollider = playerCollider;
        foreach (Collider collider1 in _colliders)
        {
            Physics.IgnoreCollision(collider1, _playerCollider, true);
            collider1.enabled = false;
        }

        transform.parent = newParent;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        _grabbed = true;
    }

    public void Release()
    {
        foreach (Collider collider1 in _colliders)
        {
            Physics.IgnoreCollision(collider1, _playerCollider, false);
            collider1.enabled = true;
        }

        _playerCollider = null;

        _grabbed = false;
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
    }

    private void ResetTransform()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}