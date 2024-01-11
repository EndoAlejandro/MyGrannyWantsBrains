using LevelGeneration;
using UnityEngine;
using UnityEngine.UI;

namespace UiComponents
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Image healthBarImage;
        [SerializeField] private Image timerImage;

        private PlayerController _player;

        private void Awake()
        {
            PlayerController.OnPlayerSpawn += PlayerControllerOnPlayerSpawn;
        }

        private void PlayerControllerOnPlayerSpawn(PlayerController player) => _player = player;

        private void Update()
        {
            timerImage.fillAmount = LevelManager.Instance.NormalizedTimer;
            healthBarImage.fillAmount = PlayerController.NormalizedHealth;
        }

        private void OnDestroy()
        {
            PlayerController.OnPlayerSpawn -= PlayerControllerOnPlayerSpawn;
        }
    }
}