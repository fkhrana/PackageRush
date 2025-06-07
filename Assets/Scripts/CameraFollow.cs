using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    void Start()
    {
        if (transform.parent == null)
        {
            Debug.LogError("MainCamera harus menjadi child dari player!");
        }
    }

    void LateUpdate()
    {
        if (transform.parent == null) return;
    }
}