using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination;

    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistanceX;
    public float chaseDistanceY;

    private bool isGrounded;

    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D boxCollider;

    private const float RaycastYOffset = 0.5f;
    private const float PatrolReachedDistance = 0.2f;

    private void Start()
    {
        TryGetComponent(out rb);
        TryGetComponent(out boxCollider);

        if (spriteRenderer == null)
        {
            TryGetComponent(out spriteRenderer);
        }

        PlayerController2D playerController = FindFirstObjectByType<PlayerController2D>();
        if (playerController != null)
        {
            playerTransform = playerController.transform;
        }

        if (rb == null || boxCollider == null || spriteRenderer == null || playerTransform == null)
        {
            Debug.LogError("DogPatrol is missing required references.", this);
            enabled = false;
            return;
        }

        if (patrolPoints == null || patrolPoints.Length < 2 || patrolPoints[0] == null || patrolPoints[1] == null)
        {
            Debug.LogError("DogPatrol needs at least two patrol points.", this);
            enabled = false;
            return;
        }

        Collider2D playerCollider = playerTransform.GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            Physics2D.IgnoreCollision(boxCollider, playerCollider);
        }
    }

    private void Update()
    {
        if (!isGrounded)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        float deltaX = Mathf.Abs(transform.position.x - playerTransform.position.x);
        float deltaY = Mathf.Abs(transform.position.y - playerTransform.position.y);

        if (isChasing)
        {
            if (deltaX > chaseDistanceX || deltaY > chaseDistanceY)
            {
                isChasing = false;
            }
            else
            {
                if (HasObstacleBetweenDogAndPlayer())
                {
                    isChasing = false;
                }
                else
                {
                    if (transform.position.x > playerTransform.position.x)
                    {
                        spriteRenderer.flipX = true;
                        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
                    }
                    else if (transform.position.x < playerTransform.position.x)
                    {
                        spriteRenderer.flipX = false;
                        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
                    }
                }
            }
        }
        else
        {
            if (deltaX < chaseDistanceX && deltaY < chaseDistanceY)
            {
                if (!HasObstacleBetweenDogAndPlayer())
                {
                    isChasing = true;
                }
            }

            Vector2 targetPosition = patrolDestination == 0 ? patrolPoints[0].position : patrolPoints[1].position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position.x > targetPosition.x)
            {
                spriteRenderer.flipX = true;
            }
            else if (transform.position.x < targetPosition.x)
            {
                spriteRenderer.flipX = false;
            }

            if (patrolDestination == 0)
            {
                if (Vector2.Distance(transform.position, patrolPoints[0].position) < PatrolReachedDistance)
                {
                    patrolDestination = 1;
                }
            }
            else if (patrolDestination == 1)
            {
                if (Vector2.Distance(transform.position, patrolPoints[1].position) < PatrolReachedDistance)
                {
                    patrolDestination = 0;
                }
            }
        }
    }

    private bool HasObstacleBetweenDogAndPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, RaycastYOffset, 0),
            direction, Vector2.Distance(transform.position, playerTransform.position), LayerMask.GetMask("Obstacles"));
        return hit.collider != null && hit.collider.CompareTag("Box");
    }

    #region ground check
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            float deltaX = Mathf.Abs(transform.position.x - playerTransform.position.x);
            float deltaY = Mathf.Abs(transform.position.y - playerTransform.position.y);
            if (deltaX < chaseDistanceX && deltaY < chaseDistanceY)
            {
                if (!HasObstacleBetweenDogAndPlayer())
                {
                    isChasing = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isChasing = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 topLeft = new Vector3(transform.position.x - chaseDistanceX, transform.position.y + chaseDistanceY, transform.position.z);
        Vector3 topRight = new Vector3(transform.position.x + chaseDistanceX, transform.position.y + chaseDistanceY, transform.position.z);
        Vector3 bottomLeft = new Vector3(transform.position.x - chaseDistanceX, transform.position.y - chaseDistanceY, transform.position.z);
        Vector3 bottomRight = new Vector3(transform.position.x + chaseDistanceX, transform.position.y - chaseDistanceY, transform.position.z);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);

        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position + new Vector3(0, 0.5f, 0), direction * Mathf.Max(chaseDistanceX, chaseDistanceY));
        }
    }

    #endregion
}