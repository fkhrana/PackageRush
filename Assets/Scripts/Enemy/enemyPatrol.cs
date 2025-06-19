using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float kecepatanGerak;
    public bool berbalik;

    void Start()
    {
        berbalik = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D tidak ditemukan pada GameObject ini! Pastikan ada Rigidbody2D.", this);
        }
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer tidak ditemukan pada GameObject ini! Pastikan ada SpriteRenderer.", this);
        }
    }

    void Update()
    {
        if (berbalik)
        {
            rb.linearVelocity = new Vector2(kecepatanGerak, rb.linearVelocity.y);

            {
                sr.flipX = true;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-kecepatanGerak, rb.linearVelocity.y);

            if (sr != null)
            {
                sr.flipX = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Balik")){
            berbalik = !berbalik;
        }
    }
}