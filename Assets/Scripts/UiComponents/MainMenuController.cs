using System;
using UnityEngine;
using UnityEngine.UI;

namespace UiComponents
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            startButton.onClick.AddListener(StartButtonPressed);
            creditsButton.onClick.AddListener(CreditsButtonPressed);
            exitButton.onClick.AddListener(ExitButtonPressed);
        }

        private void CreditsButtonPressed() => GameManager.Instance.GoToCredits();
        private void StartButtonPressed() => GameManager.Instance.GoToCinematic();
        private void ExitButtonPressed() => Application.Quit();
    }
}