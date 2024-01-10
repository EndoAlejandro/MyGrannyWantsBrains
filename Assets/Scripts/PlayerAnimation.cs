using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimation : MonoBehaviour
{
    private PlayerController _player;
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start() => PlayerController.OnGrannyGrab += PlayerOnGrannyGrab;

    private void Update()
    {
        _animator.SetFloat(Speed, _player.NormalizedSpeed);
    }

    private void PlayerOnGrannyGrab(bool isGrabbed)
    {
        _animator.SetLayerWeight(1, isGrabbed ? 1 : 0);
    }

    private void OnDestroy() => PlayerController.OnGrannyGrab -= PlayerOnGrannyGrab;
}