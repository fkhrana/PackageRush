using System.Collections;
using System.Collections.Generic;
// using System.Runtime.InteropServices.WindowsRuntime; // Baris ini tidak diperlukan dan bisa dihapus
using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr; // Tambahkan referensi ke SpriteRenderer
    public float kecepatanGerak;
    public bool berbalik;

    void Start()
    {
        berbalik = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); // Dapatkan komponen SpriteRenderer

        // Pengecekan keamanan: Pastikan komponen ditemukan
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D tidak ditemukan pada GameObject ini! Pastikan ada Rigidbody2D.", this);
        }
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer tidak ditemukan pada GameObject ini! Pastikan ada SpriteRenderer.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (berbalik)
        {
            rb.linearVelocity = new Vector2(kecepatanGerak, rb.linearVelocity.y);

            // Jika sedang bergerak ke kanan, pastikan sprite tidak ter-flip (menghadap kanan)
            if (sr != null)
            {
                sr.flipX = true;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-kecepatanGerak, rb.linearVelocity.y);

            // Jika sedang bergerak ke kiri, pastikan sprite ter-flip (menghadap kiri)
            if (sr != null)
            {
                sr.flipX = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Balik")){
            berbalik = !berbalik; // Membalik nilai boolean 'berbalik'
            // Perubahan flipX akan diurus di Update() pada frame berikutnya
            // atau bisa langsung di sini jika ingin respons instan
        }
    }
}