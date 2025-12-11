using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float maxJumpForce = 20f;
    public float chargeSpeed = 15f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public bool isGrounded = false;

    private Rigidbody2D rb;
    private float moveInput;
    private float currentJumpForce = 0f;
    private bool isCharging = false;
    private bool wasGrounded = false;
    private bool didJump = false; // Cờ đánh dấu vừa nhảy

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset cờ nhảy khi chạm đất
        if (isGrounded && !wasGrounded)
        {
            didJump = false;
        }

        // Chỉ reset vận tốc X khi ĐI BỘ rơi xuống (không phải nhảy)
        if (wasGrounded && !isGrounded && !didJump)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        wasGrounded = isGrounded;

        moveInput = Input.GetAxisRaw("Horizontal");

        // --- DI CHUYỂN ---
        if (isGrounded && !isCharging)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            float currentScaleX = Mathf.Abs(transform.localScale.x);
            if (moveInput > 0)
                transform.localScale = new Vector3(currentScaleX, transform.localScale.y, transform.localScale.z);
            else if (moveInput < 0)
                transform.localScale = new Vector3(-currentScaleX, transform.localScale.y, transform.localScale.z);
        }

        // --- LOGIC NẠP LỰC ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isCharging = true;
            currentJumpForce = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        if (Input.GetKey(KeyCode.Space) && isCharging)
        {
            rb.linearVelocity = Vector2.zero;
            currentJumpForce += chargeSpeed * Time.deltaTime;
            currentJumpForce = Mathf.Clamp(currentJumpForce, 0, maxJumpForce);
        }

        // --- NHẢY ---
        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            didJump = true; // Đặt cờ TRƯỚC khi nhảy
            Jump();
        }
    }

    void Jump()
    {
        float jumpX = 0f;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            jumpX = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            jumpX = -1f;

        Debug.Log("Hướng nhảy (X): " + jumpX + " | Lực: " + currentJumpForce);

        Vector2 jumpVector = new Vector2(jumpX * 1f, 1f).normalized;

        rb.AddForce(jumpVector * currentJumpForce, ForceMode2D.Impulse);

        currentJumpForce = 0f;
        isCharging = false;
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}