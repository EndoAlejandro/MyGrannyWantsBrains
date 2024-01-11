using PlayerComponents;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!other.transform.root.TryGetComponent(out PlayerController playerController)) return;
        if (!playerController.IsGrabbingGranny) return;
        GameManager.Instance.GoToEndScreen(goodEnd: true);
        Destroy(gameObject);
    }
}