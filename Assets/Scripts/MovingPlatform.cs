using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 5f;
    private Vector3 startPosition;
    private bool movingRight = true; 
    private Transform cachedTransform;

    private void Start()
    {
        cachedTransform = transform;
        startPosition = cachedTransform.position;
    }

    private void FixedUpdate()
    {
        float moveDistance = speed * Time.fixedDeltaTime;
        if (movingRight)
        {
            cachedTransform.position += new Vector3(moveDistance, 0, 0);
            if (cachedTransform.position.x >= startPosition.x + distance)
            {
                movingRight = false;
            }
        }
        else
        {
            cachedTransform.position -= new Vector3(moveDistance, 0, 0);
            if (cachedTransform.position.x <= startPosition.x - distance)
            {
                movingRight = true;
            }
        }
    }
}