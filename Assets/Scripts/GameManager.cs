using System.Collections;
using DarkHavoc.CustomUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool GoodEnd { get; private set; }
    protected override void SingletonAwake() => DontDestroyOnLoad(gameObject);

    public void GoToEndScreen(bool goodEnd = false)
    {
        GoodEnd = goodEnd;
        StartCoroutine(GoToSceneAsync("EndScreen"));
    }
    
    public void GoToMainMenu() => StartCoroutine(GoToSceneAsync("MainMenu"));
    public void GoToCredits() => StartCoroutine(GoToSceneAsync("Credits"));
    public void GoToCinematic() => StartCoroutine(GoToSceneAsync("Cinematic"));
    public void GoToGame() => StartCoroutine(GoToSceneAsync("MainGame"));

    private IEnumerator GoToSceneAsync(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync("Loading");
        yield return new WaitForSeconds(1.2f);
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return null;
        yield return SceneManager.UnloadSceneAsync("Loading");
    }
}