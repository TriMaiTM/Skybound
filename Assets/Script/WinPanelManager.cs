using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinPanelManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI timeDisplayText;

    [Header("Audio Settings")] // [MỚI] Phần cài đặt âm thanh
    public AudioSource audioSource; // Kéo cái Loa vào đây
    public AudioClip winMusicClip;  // Kéo file nhạc chiến thắng vào đây

    void Start()
    {
        // 1. Xử lý hiển thị thời gian
        float gameTime = PlayerPrefs.GetFloat("GameTime", 0f);

        if (timeDisplayText != null)
        {
            timeDisplayText.text = $"Time: {gameTime:F2} s";
        }
        Debug.Log($"Chiến thắng! Thời gian: {gameTime:F2} giây");

        // 2. [MỚI] Xử lý phát nhạc chiến thắng
        PlayWinMusic();
    }

    void PlayWinMusic()
    {
        // Kiểm tra xem người chơi có Tắt Nhạc trong Options không? (Lấy từ bài trước)
        // Mặc định là 1 (Bật) nếu không tìm thấy
        bool isMusicOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        if (audioSource != null && winMusicClip != null)
        {
            audioSource.clip = winMusicClip;

            // Nếu Music đang BẬT -> Phát nhạc
            if (isMusicOn)
            {
                audioSource.Play();
            }
            // Nếu Music đang TẮT -> Không phát (hoặc Mute)
            else
            {
                audioSource.Stop();
            }
        }
    }

    // Hàm quay lại Menu chính
    public void OnClick_BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene"); // Nhớ sửa đúng tên Scene Menu của bạn
    }

    // Hàm chơi lại
    public void OnClick_Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}