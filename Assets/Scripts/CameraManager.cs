using System;
using Cinemachine;
using DarkHavoc.CustomUtils;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera grannyVirtualCamera;

    private Camera _camera;
    private PlayerController _player;

    protected override void SingletonAwake()
    {
        _camera = GetComponentInChildren<Camera>();
        playerVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        PlayerController.OnPlayerSpawn += PlayerControllerOnPlayerSpawn;
        PlayerController.OnGrannyGrab += PlayerControllerOnGrannyGrab;
        
        PlayerControllerOnGrannyGrab(false);
    }

    private void PlayerControllerOnGrannyGrab(bool isGrabbed)
    {
        grannyVirtualCamera.gameObject.SetActive(isGrabbed);
    }

    private void PlayerControllerOnPlayerSpawn(PlayerController player)
    {
        _player = player;
        Transform t = player.transform;

        playerVirtualCamera.m_Follow = t;
        playerVirtualCamera.m_LookAt = t;

        grannyVirtualCamera.m_Follow = t;
        grannyVirtualCamera.m_LookAt = t;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerSpawn -= PlayerControllerOnPlayerSpawn;
        PlayerController.OnGrannyGrab -= PlayerControllerOnGrannyGrab;
    }
}