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
    public float chaseDistance;

    private bool isGrounded;

    void Update()
    {
        if (!isGrounded)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (isChasing)
        {
            if (distanceToPlayer > chaseDistance)
            {
                isChasing = false;
            }
            else
            {
                if (transform.position.x > playerTransform.position.x)
                {
                    transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                }
                else if (transform.position.x < playerTransform.position.x)
                {
                    transform.localScale = new Vector3(-0.6f, 0.6f, 0.6f);
                    transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                }
            }
        }
        else
        {
            if (distanceToPlayer < chaseDistance)
            {
                isChasing = true;
            }

            if (patrolDestination == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.2f)
                {
                    transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    patrolDestination = 1;
                }
            }
            else if (patrolDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.2f)
                {
                    transform.localScale = new Vector3(-0.6f, 0.6f, 0.6f);
                    patrolDestination = 0;
                }
            }
        }
    }

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isChasing = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isChasing = false;
        }
    }
}
