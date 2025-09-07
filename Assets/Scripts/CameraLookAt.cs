using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform target;   
    public Vector3 offset = new Vector3(0, 4, -8); 

    void LateUpdate()
    {
        if (target == null) return;

       
        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }
}
