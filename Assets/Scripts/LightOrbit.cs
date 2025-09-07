using UnityEngine;
public class LightOrbit : MonoBehaviour
{
    public Transform pivot;
    public float angularSpeed = 45f;
    void Update()
    {
        if (!pivot) return;
        float h = Input.GetAxis("Horizontal");
        transform.RotateAround(pivot.position, Vector3.up, h * angularSpeed * Time.deltaTime);
        transform.LookAt(pivot.position + Vector3.down * 0.5f);
    }
}
