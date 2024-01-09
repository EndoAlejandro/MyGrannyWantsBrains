using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkMaxSpeed = 5f;

    [SerializeField] private float walkAcceleration = 8f;

    [Header("Visuals")]
    [SerializeField] private Transform model;

    [SerializeField] private Transform wheelchairPivot;
    [SerializeField] private float rotationSpeed = 2f;

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

    private void Start()
    {
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
        /*Vector3 targetRotation = _rigidbody.velocity.normalized;
        model.forward = Vector3.Lerp(model.forward, _in, Time.deltaTime * rotationSpeed);*/

        Vector2 playerViewPort = Camera.main.WorldToViewportPoint(transform.position);
        var viewportDirection = (_input.Aim - playerViewPort).normalized;
        var aimDirection = new Vector3(viewportDirection.x, 0f, viewportDirection.y);

        model.forward =
            Vector3.Lerp(model.forward, aimDirection, Time.deltaTime * rotationSpeed);
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_movement * walkAcceleration, ForceMode.Acceleration);

        if (_rigidbody.velocity.magnitude > walkMaxSpeed)
            _rigidbody.velocity = _rigidbody.velocity.normalized * walkMaxSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out GrannyController grannyController)) return;
        _grannyController = grannyController;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_grannyController == null || !other.TryGetComponent(out GrannyController grannyController)) return;
        // _grannyController.Release();
        // _grannyController = null;
    }
}