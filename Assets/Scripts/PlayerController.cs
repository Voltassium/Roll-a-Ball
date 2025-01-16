using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float jumpForce = 5.0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject pauseMenuCanvas;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText; // Reference to health text
    public AudioClip pickupSound; // Sound to play when a pickup is collected
    public AudioClip damageSound; // Sound to play when taking damage

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool isGrounded = true;
    private float timeRemaining;
    private float timeAddedPerPickup = 5f;
    private int maxHealth = 3; // Maximum health of the player
    private int currentHealth; // Current health of the player
    private AudioSource audioSource; // AudioSource component to play sounds

    public static bool IsPaused { get; private set; } = false;
    private bool isGameOver = false;
    private GameState currentState = GameState.Playing;
    private const float MIN_VELOCITY_THRESHOLD = 0.1f;

    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    void Start()
    {
        if (winTextObject == null || countText == null || pauseMenuCanvas == null || timerText == null || scoreText == null || healthText == null)
        {
            Debug.LogError("One or more GameObjects are not assigned in the Inspector.");
            return;
        }

        winTextObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        count = 0;
        timeRemaining = 10f;
        currentHealth = maxHealth;
        SetCountText();
        UpdateHealthText();

        pauseMenuCanvas.SetActive(false);
        scoreText.gameObject.SetActive(false);

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (currentState == GameState.Playing)
        {
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        timerText.text = "Time: " + timeRemaining.ToString("F2");

        if (timeRemaining <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        currentState = GameState.GameOver;
        ShowScore();
        pauseMenuCanvas.SetActive(true);
    }

    void OnMove(InputValue movementValue)
    {
        if (!IsPaused && !isGameOver)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
        }
    }

    void OnJump(InputValue jumpValue)
    {
        if (!IsPaused && !isGameOver && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 9)
        {
            winTextObject.SetActive(true);
            GameOver(); // Pause the game immediately on win
        }
    }

    void ShowScore()
    {
        float score = CalculateScore(timeRemaining);
        scoreText.text = "Score: " + score.ToString("F2");
        scoreText.gameObject.SetActive(true);
    }

    float CalculateScore(float timeRemaining)
    {
        return timeRemaining * 10;
    }

    void FixedUpdate()
    {
        if (currentState == GameState.Playing)
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY).normalized;
            rb.AddForce(movement * speed);
            
            // Add drag when not moving to prevent infinite sliding
            if (movement.magnitude < MIN_VELOCITY_THRESHOLD)
            {
                rb.velocity *= 0.95f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsPaused && !isGameOver && other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            AddTime(timeAddedPerPickup);

            // Play the pickup sound
            if (audioSource != null && pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
        }
        else if (!IsPaused && !isGameOver && other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1); // Example: Take 1 damage from enemy
        }
    }

    void AddTime(float timeToAdd)
    {
        timeRemaining += timeToAdd;
        timerText.text = "Time: " + timeRemaining.ToString("F2");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthText();

        // Play the damage sound
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHealth.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void TogglePause()
    {
        currentState = (currentState == GameState.Paused) ? GameState.Playing : GameState.Paused;
        pauseMenuCanvas.SetActive(currentState == GameState.Paused);
        Time.timeScale = (currentState == GameState.Paused) ? 0f : 1f;
    }
}