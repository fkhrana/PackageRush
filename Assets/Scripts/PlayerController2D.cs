using System.Collections;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ambil komponen SpriteRenderer
    }

    void Update()
    {
        // Gerakan
        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // Lompat
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("WaterObstacle")) // Jika terkena obstacle air
        {
            StartCoroutine(BlinkEffect()); // Panggil coroutine untuk efek kedap-kedip
        }
    }

    private IEnumerator BlinkEffect()
    {
        Color originalColor = spriteRenderer.color; // Simpan warna asli
        float blinkDuration = 1f; // Durasi kedap-kedip
        float blinkInterval = 0.1f; // Interval kedap-kedip
        int blinkCount = (int)(blinkDuration / blinkInterval); // Hitung jumlah kedap-kedip

        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // Transparan
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.color = originalColor; // Kembali ke warna asli
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}