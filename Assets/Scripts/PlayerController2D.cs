using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController2D : MonoBehaviour
{
    private const string SpeedParameter = "Speed";
    private const float GroundContactThreshold = 0.5f;
    private const int DamageAmount = 20;
    private const string GameOverSceneName = "GameOver";

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
    [SerializeField] private Animator animator;
    private AudioManager audioManager;
    private float moveInput;
    private bool jumpRequested;
    [SerializeField] private bool allowExternalInput = true;
    [SerializeField] private float externalInputTimeout = 0.25f;
    private float externalMoveInput;
    private bool hasExternalMoveInput;
    private bool externalJumpRequested;
    private float lastExternalInputTime = -999f;
    private readonly HashSet<Collider2D> groundedColliders = new HashSet<Collider2D>();

    private AudioManager CurrentAudioManager => audioManager != null ? audioManager : AudioManager.instance;

    private void Start()
    {
        if (rb == null)
        {
            TryGetComponent(out rb);
        }

        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.freezeRotation = true;
        }

        if (spriteRenderer == null)
        {
            TryGetComponent(out spriteRenderer);
        }

        if (animator == null)
        {
            TryGetComponent(out animator);
        }

        audioManager = AudioManager.instance;
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        else
        {
            Debug.LogWarning("HealthBar is not assigned on PlayerController2D.");
        }

        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found! Please add AudioManager to the scene.");
        }
    }

    private void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameFinished)
        {
            moveInput = 0f;
            jumpRequested = false;
            externalJumpRequested = false;

            return;
        }

        if (rb == null || spriteRenderer == null)
        {
            return;
        }

        float keyboardMoveInput = Input.GetAxisRaw("Horizontal");
        bool useExternalMoveInput = allowExternalInput && hasExternalMoveInput && (Time.unscaledTime - lastExternalInputTime) <= externalInputTimeout;
        moveInput = useExternalMoveInput ? externalMoveInput : keyboardMoveInput;
        if (animator != null)
        {
            animator.SetFloat(SpeedParameter, Mathf.Abs(moveInput * moveSpeed));
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpRequested = true;
        }

        if (externalJumpRequested)
        {
            jumpRequested = true;
            externalJumpRequested = false;
        }

        if (moveInput > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance != null && GameManager.instance.isGameFinished)
        {
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }

            return;
        }

        if (rb == null)
        {
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (jumpRequested && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        jumpRequested = false;

        // batas posisi X player
        float clampedX = Mathf.Clamp(rb.position.x, minX, maxX);
        rb.position = new Vector2(clampedX, rb.position.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        UpdateGroundedState(other);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        UpdateGroundedState(other);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (IsGroundLike(other.gameObject))
        {
            groundedColliders.Remove(other.collider);
            isGrounded = groundedColliders.Count > 0;
        }
    }

    private void UpdateGroundedState(Collision2D other)
    {
        if (!IsGroundLike(other.gameObject))
        {
            return;
        }

        foreach (ContactPoint2D contact in other.contacts)
        {
            if (contact.normal.y > GroundContactThreshold)
            {
                groundedColliders.Add(other.collider);
                isGrounded = true;
                return;
            }
        }
    }

    private static bool IsGroundLike(GameObject otherObject)
    {
        return otherObject.CompareTag("Ground") || otherObject.CompareTag("Platform") || otherObject.CompareTag("Box");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.instance != null && GameManager.instance.isGameFinished) return;

        if (other.CompareTag("Item"))
        {
            Debug.Log("Player touched score item: " + other.gameObject.name);
            CurrentAudioManager?.PlaySoundEffect("StarCollect");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("WaterObstacle"))
        {
            ApplyDamageIfPossible("WaterSplash");
        }
        else if (other.CompareTag("Police"))
        {
            ApplyDamageIfPossible("PoliceHit");
        }
        else if (other.CompareTag("Dog"))
        {
            ApplyDamageIfPossible("DogHit");
        }
    }

    private void ApplyDamageIfPossible(string soundEffect)
    {
        if (isBlinking)
        {
            return;
        }

        StartCoroutine(BlinkEffect());
        TakeDamage(DamageAmount);
        CurrentAudioManager?.PlaySoundEffect(soundEffect);
    }

    private void TakeDamage(int damage)
    {
        if (GameManager.instance != null && GameManager.instance.isGameFinished) return;

        currentHealth -= damage;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.isGameFinished = true;
            }
            Debug.Log("Game Over: Health depleted!");
            CurrentAudioManager?.PlaySoundEffect("GameOver");
            CurrentAudioManager?.StopBackgroundMusic();
            SceneManager.LoadScene(GameOverSceneName);
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

    public void MoveLeft()
    {
        RegisterExternalMove(-1f);
    }

    public void MoveRight()
    {
        RegisterExternalMove(1f);
    }

    public void StopMove()
    {
        RegisterExternalMove(0f);
    }

    public void Jump()
    {
        lastExternalInputTime = Time.unscaledTime;
        externalJumpRequested = true;
    }

    private void RegisterExternalMove(float moveValue)
    {
        lastExternalInputTime = Time.unscaledTime;
        externalMoveInput = moveValue;
        hasExternalMoveInput = true;
    }
}