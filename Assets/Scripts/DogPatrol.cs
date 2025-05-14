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
        if (!isGrounded) // good guard clause, the ground check could be better but oh well if it works it works
        {
            return;
        }

        // this we could get rid of and use triggers
        // we can put all the chasing logic inside
        // OnTrigerStay2d()
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (isChasing)
        {
            if (distanceToPlayer > chaseDistance)
            {
                // this is called a state.
                // generally we need to get rid of as much state manipulation as possible
                // other than this chase state there is also groud check.

                // we can remove this chase state and let it be handled by OnTriggerStay2d()
                isChasing = false; 
            }
            else
            {
                // why

                // basically if player is on the right go right 
                // else go left.
                // pls dont do this.

                // we can calculate the directional vector usually
                // dir = player.transform.position - transform.position
                // then this dir is a vector of (x,y,z)
                // we can ignore y and z and just use x, we dont want the dog to fly dont we?
                // but first we need to normalize it so dir.normalize
                // then tranform.position += dir.x * moveSpeed * Time.deltaTime;
                if (transform.position.x > playerTransform.position.x)
                {
                    transform.localScale = new Vector3(0.6f, 0.6f, 0.6f); // we can flip it from the sprite renderer i think renderer.Flip = true or smth me forgor
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
                isChasing = true; // state manipulation
            }

            if (patrolDestination == 0) // what
            {
                // wtf is movetowards?
                // you used transform.position above why use this too :sob:
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.2f)
                {
                    transform.localScale = new Vector3(0.6f, 0.6f, 0.6f); // we can flip it from the sprite renderer
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
    #region ground check

    // good enough lmao
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


    // BRO WHY IS THERE ALSO A STATE MANIPULATION HERE
    // this will cause a mutation most likely why the dog isnt chasing back when i play

    // state mutation mean there is more than one thing that changes the state
    // this will make it hard to tell "who change the state?"
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
