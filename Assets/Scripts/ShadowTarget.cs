using UnityEngine;

public class ShadowTarget : MonoBehaviour, IInteractable
{
    [Header("Shadow Logic")]
    public bool isCorrect = false;          // يعيّنها المدير
    public Transform shadowCue;             // الطفل الصغير الذي يظهر فرق الظل

    [Header("Optional Visuals")]
    public Renderer rend;                   // لتلوين بسيط عند الهوفر (اختياري)
    public Material normalMat;
    public Material hoverMat;

    void Reset()
    {
        if (!rend) rend = GetComponentInChildren<Renderer>();
        if (!shadowCue)
        {
            var t = transform.Find("ShadowCue");
            if (t) shadowCue = t;
        }
    }

    void Awake()
    {
        if (!rend) rend = GetComponentInChildren<Renderer>();
        EnableCue(false); 
    }

    public void Interact()
    {
        GameManager_Shadow.Instance.OnPick(this);
    }

   
    public void EnableCue(bool on)
    {
        if (shadowCue)
        {
            shadowCue.gameObject.SetActive(on);
            //shadowCue.localScale = Vector3.one * scale;
        }
    }

    
    void OnMouseEnter() { if (rend && hoverMat) rend.material = hoverMat; }
    void OnMouseExit() { if (rend && normalMat) rend.material = normalMat; }
}
