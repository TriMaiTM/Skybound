using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Cài đặt Di chuyển")]
    public float moveSpeed = 5f;
    public float maxJumpForce = 20f;
    public float chargeSpeed = 15f;

    [Header("Kiểm tra mặt đất")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // Các biến trạng thái
    public bool isGrounded = false;
    private bool isCharging = false;
    private bool wasGrounded = false;
    private bool didJump = false;
    private float moveInput;
    private float currentJumpForce = 0f;

    // Các Component
    private Rigidbody2D rb;
    private Animator anim; // [QUAN TRỌNG] Biến để gọi Animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // [QUAN TRỌNG] Lấy Animator về dùng
    }

    void Update()
    {
        // 1. Kiểm tra chạm đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset cờ nhảy khi vừa chạm đất
        if (isGrounded && !wasGrounded)
        {
            didJump = false;
        }

        // Dừng trượt khi rơi xuống đất (chỉ khi đi bộ)
        if (wasGrounded && !isGrounded && !didJump)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        wasGrounded = isGrounded;

        // 2. Xử lý Input di chuyển
        moveInput = Input.GetAxisRaw("Horizontal");

        // Logic đi bộ (Chỉ đi khi chạm đất và không tụ lực)
        if (isGrounded && !isCharging)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            FlipCharacter(); // Hàm quay mặt nhân vật
        }

        // 3. Logic Nạp lực (Giữ Space)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isCharging = true;
            currentJumpForce = 0f;
            rb.linearVelocity = Vector2.zero; // Đứng im để tụ lực
        }

        if (Input.GetKey(KeyCode.Space) && isCharging)
        {
            rb.linearVelocity = Vector2.zero; // Khóa vị trí
            currentJumpForce += chargeSpeed * Time.deltaTime;
            currentJumpForce = Mathf.Clamp(currentJumpForce, 0, maxJumpForce);
        }

        // 4. Logic Nhảy (Thả Space)
        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            didJump = true;
            Jump();
        }

        // [QUAN TRỌNG NHẤT] Cập nhật Animation
        UpdateAnimations();
    }

    // Hàm riêng để quản lý Animation cho gọn
    void UpdateAnimations()
    {
        if (anim == null) return;

        // Gửi thông tin: Có đang đứng trên đất không?
        anim.SetBool("isGrounded", isGrounded);

        // Gửi thông tin: Có đang tụ lực không?
        anim.SetBool("isCharging", isCharging);

        // Gửi thông tin: Tốc độ chạy (Lấy trị tuyệt đối để luôn dương)
        // Nếu đang tụ lực thì coi như Speed = 0 (để nó không chạy animation Run)
        if (isCharging) anim.SetFloat("Speed", 0);
        else anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // Gửi thông tin: Vận tốc dọc (Để biết đang bay lên hay rơi xuống)
        anim.SetFloat("verticalVelocity", rb.linearVelocity.y);
    }

    void Jump()
    {
        float jumpX = 0f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) jumpX = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) jumpX = -1f;

        Vector2 jumpVector = new Vector2(jumpX * 1f, 1f).normalized;
        rb.AddForce(jumpVector * currentJumpForce, ForceMode2D.Impulse);

        currentJumpForce = 0f;
        isCharging = false;
    }

    void FlipCharacter()
    {
        float currentScaleX = Mathf.Abs(transform.localScale.x);
        if (moveInput > 0)
            transform.localScale = new Vector3(currentScaleX, transform.localScale.y, transform.localScale.z);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-currentScaleX, transform.localScale.y, transform.localScale.z);
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