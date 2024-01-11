using System;
using System.Collections;
using CustomUtils;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static event Action<PlayerController> OnPlayerSpawn;
    public static event Action OnPlayerDead;
    public static event Action<bool> OnGrannyGrab;

    public float NormalizedSpeed => _rigidbody != null ? _rigidbody.velocity.magnitude / MaxSpeed : 0f;
    private static float Health = 1;
    private static float MaxHealth = 1;
    public static float NormalizedHealth => Health / MaxHealth;

    [SerializeField] private float maxHealth = 20f;

    [Header("Walk")]
    [SerializeField] private float walkMaxSpeed = 5f;

    [SerializeField] private float walkAcceleration = 80f;
    [SerializeField] private float walkRotationSpeed = 10f;

    [Header("Granny")]
    [SerializeField] private float grannyMaxSpeed = 3f;

    [SerializeField] private float grannyAcceleration = 40f;
    [SerializeField] private float grannyRotationSpeed = 5f;

    [Header("Visuals")]
    [SerializeField] private Transform wheelchairPivot;

    private float MaxSpeed => _isGrabbingGranny ? grannyMaxSpeed : walkMaxSpeed;
    private float Acceleration => _isGrabbingGranny ? grannyAcceleration : walkAcceleration;
    private float RotationSpeed => _isGrabbingGranny ? grannyRotationSpeed : walkRotationSpeed;

    private Rigidbody _rigidbody;
    private InputReader _input;

    private GrannyController _grannyController;

    private Vector3 _movement;
    private bool _isGrabbingGranny;
    private Collider _collider;

    private bool IsDead => Health <= 0f;

    private void Awake()
    {
        _input = new InputReader();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        Health = maxHealth;
        MaxHealth = maxHealth;
        OnPlayerSpawn?.Invoke(this);
    }

    private void Update()
    {
        if (!_grannyController) return;
        if (_input.Grab) ToggleGrab();
    }

    private void ToggleGrab()
    {
        if (!_isGrabbingGranny)
        {
            _isGrabbingGranny = true;
            _grannyController.Grab(wheelchairPivot, _collider, _input);
        }
        else
        {
            _isGrabbingGranny = false;
            _grannyController.Release();
        }

        OnGrannyGrab?.Invoke(_isGrabbingGranny);
    }

    private void RotateModel()
    {
        Vector3 aimDirection = transform.forward;

        if (_isGrabbingGranny)
        {
            Vector2 playerViewPort = Camera.main.WorldToViewportPoint(transform.position);
            var viewportDirection = (_input.Aim - playerViewPort).normalized;
            aimDirection = new Vector3(viewportDirection.x, 0f, viewportDirection.y);
        }
        else if (_rigidbody.velocity.magnitude > .5f)
            aimDirection = _rigidbody.velocity.With(y: 0f).normalized;

        transform.forward =
            Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * RotationSpeed);
    }

    private void FixedUpdate()
    {
        _movement = new Vector3(_input.Movement.x, 0f, _input.Movement.y).normalized;
        RotateModel();

        _rigidbody.AddForce(_movement * Acceleration, ForceMode.Acceleration);

        if (_rigidbody.velocity.magnitude > MaxSpeed)
            _rigidbody.velocity = _rigidbody.velocity.normalized * MaxSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out GrannyController grannyController)) return;
        _grannyController = grannyController;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isGrabbingGranny && other.TryGetComponent(out GrannyController grannyController))
            _grannyController = null;
    }

    public void TakeDamage(float damage)
    {
        Health = Mathf.Max(Health - damage, 0f);

        if (Health <= 0f)
        {
            _input.DisableInput();
            OnPlayerDead?.Invoke();
            StartCoroutine(EndGameAsync());
        }
    }

    private IEnumerator EndGameAsync()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.GoToEndScreen(goodEnd: false);
    }
}