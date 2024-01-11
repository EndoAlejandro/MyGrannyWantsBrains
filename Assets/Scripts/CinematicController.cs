using UnityEngine;
using UnityEngine.Video;

public class CinematicController : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    private InputReader _input;

    private void Awake() => _videoPlayer = GetComponent<VideoPlayer>();

    private void Start()
    {
        _input = new InputReader();
        _videoPlayer.loopPointReached += VideoPlayerOnLoopPointReached;
    }

    private void Update()
    {
        if (_input.Grab) GameManager.Instance.GoToGame();
    }

    private void VideoPlayerOnLoopPointReached(VideoPlayer source)
    {
        GameManager.Instance.GoToGame();
    }

    private void OnDestroy()
    {
        _videoPlayer.loopPointReached -= VideoPlayerOnLoopPointReached;
    }
}