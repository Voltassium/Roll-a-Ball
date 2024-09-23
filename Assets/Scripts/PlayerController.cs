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

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool isGrounded = true;

    public static bool IsPaused { get; private set; } = false;

    void Start()
    {
        // Error Handling: Make sure required components are assigned.
        if (winTextObject == null || countText == null || pauseMenuCanvas == null)
        {
            Debug.LogError("One or more GameObjects are not assigned in the Inspector.");
            return;
        }

        winTextObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();

        pauseMenuCanvas.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void OnMove(InputValue movementValue)
    {
        if (!IsPaused)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
        }
    }

    void OnJump(InputValue jumpValue)
    {
        if (!IsPaused && isGrounded)
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
        }
    }

    void FixedUpdate()
    {
        if (!IsPaused)
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsPaused && other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
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
        IsPaused = !IsPaused;
        pauseMenuCanvas.SetActive(IsPaused);
        Time.timeScale = IsPaused ? 0f : 1f;
    }
}