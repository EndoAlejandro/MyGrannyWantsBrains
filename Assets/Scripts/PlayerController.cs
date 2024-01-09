using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private float walkMaxSpeed = 5f;

    [SerializeField] private float walkAcceleration = 80f;
    [SerializeField] private float walkRotationSpeed = 10f;

    [Header("Granny")]
    [SerializeField] private float grannyMaxSpeed = 3f;

    [SerializeField] private float grannyAcceleration = 40f;
    [SerializeField] private float grannyRotationSpeed = 5f;

    [Header("Visuals")]
    [SerializeField] private Transform model;

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

    private void Awake()
    {
        _input = new InputReader();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        _movement = new Vector3(_input.Movement.x, 0f, _input.Movement.y).normalized;
        RotateModel();

        if (!_grannyController) return;
        if (_input.Grab)
        {
            ToggleGrab();
        }
    }

    private void ToggleGrab()
    {
        if (!_isGrabbingGranny)
        {
            _isGrabbingGranny = true;
            _grannyController.Grab(wheelchairPivot, _collider);
        }
        else
        {
            _isGrabbingGranny = false;
            _grannyController.Release();
        }
    }

    private void RotateModel()
    {
        Vector2 playerViewPort = Camera.main.WorldToViewportPoint(transform.position);
        var viewportDirection = (_input.Aim - playerViewPort).normalized;
        var aimDirection = new Vector3(viewportDirection.x, 0f, viewportDirection.y);

        model.forward =
            Vector3.Lerp(model.forward, aimDirection, Time.deltaTime * RotationSpeed);
    }

    private void FixedUpdate()
    {
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
}