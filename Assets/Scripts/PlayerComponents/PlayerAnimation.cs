using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerController _player;
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Dead = Animator.StringToHash("Dead");

    private void Awake()
    {
        _player = GetComponentInParent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerController.OnGrannyGrab += PlayerOnGrannyGrab;
        PlayerController.OnPlayerDead += PlayerControllerOnPlayerDead;
    }

    private void PlayerControllerOnPlayerDead() => _animator.SetTrigger(Dead);

    private void Update()
    {
        _animator.SetFloat(Speed, _player.NormalizedSpeed);
    }

    private void PlayerOnGrannyGrab(bool isGrabbed)
    {
        _animator.SetLayerWeight(1, isGrabbed ? 1 : 0);
    }

    private void WalkDust() => FxManager.Instance.PlayVfx(Vfx.WalkDust, transform.position);

    private void OnDestroy()
    {
        PlayerController.OnGrannyGrab -= PlayerOnGrannyGrab;
        PlayerController.OnPlayerDead -= PlayerControllerOnPlayerDead;
    }
}