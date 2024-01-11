using LevelGeneration;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(out PlayerController playerController))
        {
            LevelManager.Instance.ResetTime();
            FxManager.Instance.PlaySfx(Sfx.Pickup);
            Destroy(gameObject);
        }
    }
}