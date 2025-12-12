using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc phải có dòng này để chuyển cảnh

public class ChestController : MonoBehaviour
{
    // Thay vì kéo thả GameObject, ta chỉ cần nhập tên Scene vào đây
    [Header("Tên màn hình muốn chuyển đến")]
    public string sceneToLoad = "WinPanel"; 
    
    private bool hasWon = false; // Cờ để đảm bảo chỉ chiến thắng 1 lần

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu đã chiến thắng rồi thì không làm gì
        if (hasWon) return;
        
        if (other.CompareTag("Player"))
        {
            hasWon = true; // Đánh dấu đã chiến thắng
            
            // Dừng timer khi thắng
            if (GameTimer.instance != null)
            {
                GameTimer.instance.StopTimer();
            }

            Debug.Log("Chiến thắng! Đang chuyển sang màn hình WinPanel...");
            // Lệnh chuyển sang Scene mới
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}