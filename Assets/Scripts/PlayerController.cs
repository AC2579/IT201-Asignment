using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject door;
    public GameObject door2;
    public GameObject Door3;
    public GameObject Door4;
    public GameObject Enemy;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public float jumpForce = 7f;
    private bool isGrounded = false;
    public float dashForce = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private bool canDash = true;
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    public Transform respawnPoint;
    public AudioClip pickupClip;
    public int lives = 3;
    public float timeRemaining = 60f;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverUI;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        UpdateUI();
        Enemy2.SetActive(false);
        Enemy3.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 2)
        {
            door.SetActive(false);
            Enemy.SetActive(false);
            Enemy2.SetActive(true);
            Enemy3.SetActive(true);
        }
        if (count >= 8)
        {
            door2.SetActive(false);
            Enemy2.SetActive(false);
            Enemy3.SetActive(false);
        }
        if (count >= 10)
        {
            Door3.SetActive(false);
        }
        if (count >= 11)
        {
            Door4.SetActive(false);
        }
        if (count >= 12)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        Vector3 movement = new Vector3(movementX, 0.0f, movementY).normalized;

        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining);
        }

        else
        {
            GameOver();
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && canDash)
        {
            StartCoroutine(PerformDash());
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Killbox"))
        {
            LoseLife();
        }

        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            AudioSource.PlayClipAtPoint(pickupClip, transform.position);
            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LoseLife();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        Vector3 dashDirection = new Vector3(movementX, 0, movementY).normalized;
        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }

        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    void TogglePause()
    {
        if (isPaused)
        {
            // Unpause
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            isPaused = false;
        }
        else
        {
            // Pause
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
            isPaused = true;
        }
    }

    public void Respawn()
    {
        // Reset velocity so player doesn't keep falling
        rb.linearVelocity = Vector3.zero;

        // Move player to respawn position
        transform.position = respawnPoint.position;
    }

    public void LoseLife()
    {
        lives--;

        if (lives > 0)
        {
            Respawn();
            UpdateUI();
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
    }
    void UpdateUI()
    {
        livesText.text = "Lives: " + lives.ToString();
        timerText.text = "Time: " + Mathf.Ceil(timeRemaining);
    }
}
