using System;
using UnityEngine;
using UnityEngine.UI;

namespace UiComponents
{
    public class CreditsController : MonoBehaviour
    {
        [SerializeField] private Button button;

        private void Start()
        {
            button.onClick.AddListener(GoToMainMenu);
        }

        private void GoToMainMenu() => GameManager.Instance.GoToMainMenu();
    }
}