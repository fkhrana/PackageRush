using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; // Kecepatan gerak platform
    public float distance = 5f; // Jarak patrol (kanan/kiri dari posisi awal)
    private Vector3 startPosition;
    private bool movingRight = true; 

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        if (movingRight)
        {
            transform.position += new Vector3(moveDistance, 0, 0);
            if (transform.position.x >= startPosition.x + distance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position -= new Vector3(moveDistance, 0, 0);
            if (transform.position.x <= startPosition.x - distance)
            {
                movingRight = true;
            }
        }
    }
}