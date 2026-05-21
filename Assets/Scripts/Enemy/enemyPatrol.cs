using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float kecepatanGerak;
    public bool berbalik;

    private void Start()
    {
        berbalik = true;
        TryGetComponent(out rb);
        TryGetComponent(out sr);

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D tidak ditemukan pada GameObject ini! Pastikan ada Rigidbody2D.", this);
        }
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer tidak ditemukan pada GameObject ini! Pastikan ada SpriteRenderer.", this);
        }

        if (rb == null || sr == null)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        if (berbalik)
        {
            rb.linearVelocity = new Vector2(kecepatanGerak, rb.linearVelocity.y);
            sr.flipX = true;
        }
        else
        {
            rb.linearVelocity = new Vector2(-kecepatanGerak, rb.linearVelocity.y);

            sr.flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Balik"))
        {
            berbalik = !berbalik;
        }
    }
}