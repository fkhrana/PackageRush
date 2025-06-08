using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController2D : MonoBehaviour
{
    // Dependencies
    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private HealthBar healthBar;

    // Settings
    [Header("Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    // Internal State
    [Header("State")]
    private int currentHealth;
    private bool isGrounded;
    private bool isBlinking = false;
    public Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameFinished)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(moveX * moveSpeed));

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (moveX > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveX < 0)
        {
            spriteRenderer.flipX = false;
        }

        // batas posisi X player
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Box"))
        {
            foreach (ContactPoint2D contact in other.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.instance != null && GameManager.instance.isGameFinished) return;

        if (other.CompareTag("Item"))
        {
            Debug.Log("Player touched score item: " + other.gameObject.name);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("WaterObstacle") || other.CompareTag("Police") || other.CompareTag("Dog"))
        {
            if (!isBlinking)
            {
                StartCoroutine(BlinkEffect());
                TakeDamage(20);
            }
        }
    }

    private void TakeDamage(int damage)
    {
        if (GameManager.instance != null && GameManager.instance.isGameFinished) return;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.isGameFinished = true;
            }
            Debug.Log("Game Over: Health depleted!");
            SceneManager.LoadScene("GameOver");
        }
    }

    private IEnumerator BlinkEffect()
    {
        isBlinking = true;
        Color originalColor = spriteRenderer.color;
        float blinkDuration = 0.3f;
        float blinkInterval = 0.1f;
        int blinkCount = (int)(blinkDuration / blinkInterval);

        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }

        isBlinking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(minX, -10, 0), new Vector3(minX, 10, 0));
        Gizmos.DrawLine(new Vector3(maxX, -10, 0), new Vector3(maxX, 10, 0));
    }
}