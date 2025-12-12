using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinPanelManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI timeDisplayText; // Kéo Text hiển thị thời gian vào đây

    void Start()
    {
        // Lấy thời gian từ PlayerPrefs (được lưu bởi GameTimer)
        float gameTime = PlayerPrefs.GetFloat("GameTime", 0f);

        // Hiển thị thời gian với 2 chữ số thập phân
        if (timeDisplayText != null)
        {
            timeDisplayText.text = $"Time: {gameTime:F2} s";
        }

        Debug.Log($" Chiến thắng! Thời gian: {gameTime:F2} giây");
    }

    // Hàm quay lại Menu chính
    public void OnClick_BackToMenu()
    {
        Time.timeScale = 1f; // Đảm bảo time chạy bình thường
        SceneManager.LoadScene("Main Menu");
    }

    // Hàm chơi lại
    public void OnClick_Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}
