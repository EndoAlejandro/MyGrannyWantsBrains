using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UiComponents
{
    public class ResumeScreen : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text resumeText;

        [SerializeField] private string goodText;
        [SerializeField] private string badText;

        private void Start()
        {
            resumeText.text = GameManager.Instance.GoodEnd ? goodText : badText;
            button.onClick.AddListener(OnButtonPressed);
        }

        private void OnButtonPressed() => GameManager.Instance.GoToMainMenu();
    }
}