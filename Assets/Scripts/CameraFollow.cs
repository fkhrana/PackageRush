using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 1, -10);
    [Range(0f, 1f)]
    public float smoothSpeed = 0.125f;

    [Header("Y Position Limits")]
    public float minY = -2f;
    public float maxY = 5f;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player tidak ditemukan! Pastikan GameObject player punya tag 'Player'.");
            }
        }
    }

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + offset;
        desiredPosition.z = transform.position.z;
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float leftX = Camera.main != null ? Camera.main.transform.position.x - 20f : -20f;
        float rightX = Camera.main != null ? Camera.main.transform.position.x + 20f : 20f;

        // Garis batas bawah (minY)
        Gizmos.DrawLine(
            new Vector3(leftX, minY, 0),
            new Vector3(rightX, minY, 0)
        );

        // Garis batas atas (maxY)
        Gizmos.DrawLine(
            new Vector3(leftX, maxY, 0),
            new Vector3(rightX, maxY, 0)
        );
    }
}
