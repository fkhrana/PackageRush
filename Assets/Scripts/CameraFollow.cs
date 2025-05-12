using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = player.position + offset;

        targetPos.y = transform.position.y;
        targetPos.z = transform.position.z;

        Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed);

        transform.position = smoothedPos;
    }
}