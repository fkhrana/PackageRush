using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 0, 0);
    [Range(0f, 1f)]
    public float smoothSpeed = 0.125f;
    public bool useSmoothDamp = true;

    // Internal velocity used by SmoothDamp
    private Vector3 smoothVelocity = Vector3.zero;

    [Header("Position Limits")]
    public float minX = -10f;
    public float maxX = 10f;
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

        Transform cameraTransform = transform;
        Vector3 desiredPosition = player.position + offset;
        desiredPosition.z = cameraTransform.position.z;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        if (useSmoothDamp)
        {
            // smoothSpeed here is used as smoothTime (seconds). Small values = snappier follow.
            cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosition, ref smoothVelocity, Mathf.Max(0.001f, smoothSpeed));
        }
        else
        {
            float lerpFactor = Mathf.Clamp01(smoothSpeed * Time.deltaTime * 60f);
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, lerpFactor);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float left = minX;
        float right = maxX;
        float bottom = minY;
        float top = maxY;

        Vector3 topLeft = new Vector3(left, top, 0);
        Vector3 topRight = new Vector3(right, top, 0);
        Vector3 bottomLeft = new Vector3(left, bottom, 0);
        Vector3 bottomRight = new Vector3(right, bottom, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft); 
    }
}
