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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerTransform = FindFirstObjectByType<PlayerController2D>().gameObject.transform;

        Collider2D playerCollider = playerTransform.GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            Physics2D.IgnoreCollision(boxCollider, playerCollider);
        }
    }

    void Update()
    {
        if (!isGrounded)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        // Hitung perbedaan koordinat X dan Y
        float deltaX = Mathf.Abs(transform.position.x - playerTransform.position.x);
        float deltaY = Mathf.Abs(transform.position.y - playerTransform.position.y);

        if (isChasing)
        {
            // ngecek player di luar area persegi panjang atau ada penghalang (box)
            if (deltaX > chaseDistanceX || deltaY > chaseDistanceY)
            {
                isChasing = false;
            }
            else
            {
                // ngecek penghalang (box) dengan raycast
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), 
                direction, Vector2.Distance(transform.position, playerTransform.position), LayerMask.GetMask("Obstacles"));
                if (hit.collider != null && hit.collider.CompareTag("Box")) // kalo kena box dia berhenti ngejar
                {
                    isChasing = false;
                }
                else
                {
                    // gerakkan anjing menuju player
                    if (transform.position.x > playerTransform.position.x) // kalo posisi x nya lebih besar 
                    // daripada posisi x si player, anjing ngejar ke kiri
                    {
                        spriteRenderer.flipX = true;
                        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
                    }
                    else if (transform.position.x < playerTransform.position.x) // kebalikannya
                    {
                        spriteRenderer.flipX = false;
                        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
                    }
                }
            }
        }
        else // mode patrol
        {
            // ngecek player apakah dalam area anjing
            if (deltaX < chaseDistanceX && deltaY < chaseDistanceY)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), 
                direction, Vector2.Distance(transform.position, playerTransform.position), LayerMask.GetMask("Obstacles"));
                if (hit.collider == null || !hit.collider.CompareTag("Box"))
                {
                    isChasing = true;
                }
            }

            // sistem patrol menggunakan MoveTowards
            Vector2 targetPosition = patrolDestination == 0 ? patrolPoints[0].position : patrolPoints[1].position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // mengatur arah sprite berdasarkan pergerakan menuju titik patrol
            if (transform.position.x > targetPosition.x)
            {
                spriteRenderer.flipX = true; // menghadap kiri
            }
            else if (transform.position.x < targetPosition.x)
            {
                spriteRenderer.flipX = false; // menghadap kanan
            }

            // cek jika sampai di titik patrol
            if (patrolDestination == 0)
            {
                if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.2f)
                {
                    patrolDestination = 1;
                }
            }
            else if (patrolDestination == 1)
            {
                if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.2f)
                {
                    patrolDestination = 0;
                }
            }
        }
    }

    #region ground check
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }
    #endregion

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            float deltaX = Mathf.Abs(transform.position.x - playerTransform.position.x);
            float deltaY = Mathf.Abs(transform.position.y - playerTransform.position.y);
            if (deltaX < chaseDistanceX && deltaY < chaseDistanceY)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), 
                direction, Vector2.Distance(transform.position, playerTransform.position), LayerMask.GetMask("Obstacles"));
                if (hit.collider == null || !hit.collider.CompareTag("Box"))
                {
                    isChasing = true;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isChasing = false;
        }
    }

    void OnDrawGizmos()
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
}