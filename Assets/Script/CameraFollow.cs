using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float screenHeight = 10f;
    public float screenWidth = 17.77f;

    public float smoothSpeed = 10f; // Tốc độ chuyển cảnh nhanh dứt khoát

    void Update()
    {
        if (player == null) return;

        // Tính toán vị trí Y (Chỉ quan tâm chiều dọc như Jump King gốc)
        float targetY = Mathf.Round(player.position.y / screenHeight) * screenHeight;

        // Nếu muốn game chỉ cuộn dọc (không cuộn ngang), giữ nguyên X của Camera
        // Hoặc nếu map rộng thì tính cả X:
        // float targetX = Mathf.Round(player.position.x / screenWidth) * screenWidth;

        // Ở đây tôi set cứng X = 0 để camera luôn ở giữa map theo chiều ngang
        Vector3 targetPosition = new Vector3(0, targetY, -10f);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}