using UnityEngine;

public class PlayerRaycaster : MonoBehaviour
{
    public float rayDistance = 100f;
    public LayerMask interactMask = ~0; 

    void Update()
    {
        var gm = GameManager_Shadow.Instance;
        if (!gm || gm.IsOver) return;

        if (Input.GetMouseButtonDown(0))
            TryRay(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            TryRay(Camera.main.ScreenPointToRay(Input.GetTouch(0).position));
    }

    void TryRay(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactMask))
        {
            var it = hit.collider.GetComponentInParent<IInteractable>() ?? hit.collider.GetComponent<IInteractable>();
            if (it != null) it.Interact();
        }
    }
}
