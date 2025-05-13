using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;
    public float kecepatanGerak;
    public bool berbalik;
    void Start()
    {
        berbalik = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (berbalik)
        {
            rb.linearVelocity = new Vector2(kecepatanGerak, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-kecepatanGerak, rb.linearVelocity.y);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Balik")){
            berbalik = !berbalik;
        }
    }
}
