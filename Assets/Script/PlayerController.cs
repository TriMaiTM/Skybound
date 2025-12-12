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

    [Header("Audio Settings")] // [AUDIO] Khai báo biến âm thanh
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip landClip;
    public AudioClip bumpClip;

    // Các biến trạng thái
    public bool isGrounded = false;
    private bool isCharging = false;
    private bool wasGrounded = false;
    private bool didJump = false;
    private float moveInput;
    private float currentJumpForce = 0f;

    // Các Component
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // [AUDIO] Tự tìm AudioSource nếu chưa gán
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 1. Kiểm tra chạm đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // [AUDIO] Xử lý âm thanh Đáp Đất (Land)
        // Nếu hiện tại chạm đất, mà frame trước đó chưa chạm đất -> Vừa tiếp đất
        if (isGrounded && !wasGrounded)
        {
            didJump = false;
            // Chỉ phát tiếng land nếu không phải vừa bắt đầu game (rb velocity có tốc độ rơi)
            PlaySound(landClip);
        }

        // Dừng trượt khi rơi xuống đất (chỉ khi đi bộ)
        if (wasGrounded && !isGrounded && !didJump)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        wasGrounded = isGrounded;

        // 2. Xử lý Input di chuyển
        moveInput = Input.GetAxisRaw("Horizontal");

        // Logic đi bộ
        if (isGrounded && !isCharging)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            FlipCharacter();
        }

        // 3. Logic Nạp lực
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

        // 4. Logic Nhảy
        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            didJump = true;
            Jump();
        }

        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        if (anim == null) return;
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isCharging", isCharging);
        if (isCharging) anim.SetFloat("Speed", 0);
        else anim.SetFloat("Speed", Mathf.Abs(moveInput));
        anim.SetFloat("verticalVelocity", rb.linearVelocity.y);
    }

    void Jump()
    {
        float jumpX = 0f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) jumpX = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) jumpX = -1f;

        // [AUDIO] Phát tiếng nhảy
        PlaySound(jumpClip);

        Vector2 jumpVector = new Vector2(jumpX * 1f, 1f).normalized;
        rb.AddForce(jumpVector * currentJumpForce, ForceMode2D.Impulse);

        currentJumpForce = 0f;
        isCharging = false;
    }

    // [AUDIO] Hàm xử lý va chạm để phát tiếng Bump
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Chỉ phát tiếng Bump khi va chạm mạnh vào TƯỜNG hoặc TRẦN (khi đang bay)
        // Ta kiểm tra nếu không phải đang đứng dưới đất (isGrounded = false) thì khả năng cao là đập tường
        if (!isGrounded)
        {
            // Kiểm tra lực va chạm để tránh tiếng kêu lắt nhắt
            if (collision.relativeVelocity.magnitude > 2f)
            {
                PlaySound(bumpClip);
            }
        }
    }

    // [AUDIO] Hàm tiện ích để phát âm thanh
    void PlaySound(AudioClip clip)
    {
        // [MỚI] Kiểm tra xem SFX có đang bật không?
        bool isSFXOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        // Nếu chưa có clip, chưa có loa, hoặc SFX đang tắt -> Thì không phát
        if (clip == null || audioSource == null || !isSFXOn)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
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