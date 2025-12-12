using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;   // Kéo cái Panel to đùng vào đây
    public GameObject optionsPanel; // Kéo cái bảng Options (vừa paste sang) vào đây
    public GameObject buttonsHolder; // (Tùy chọn) Cái nhóm chứa 3 nút Resume/Options/Menu để ẩn đi khi bật Options

    [Header("Options Toggles")]
    public Toggle musicToggle;
    public Toggle sfxToggle;

    public static bool isPaused = false; // Biến tĩnh để các script khác có thể check nếu cần

    void Start()
    {
        // Đảm bảo lúc đầu game chạy thì menu tắt
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f; // Chắc chắn thời gian chạy bình thường
        isPaused = false;
    }

    void Update()
    {
        // Bấm ESC để bật/tắt
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // --- CÁC HÀM CHỨC NĂNG ---

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);

        // Reset giao diện: Hiện nút, ẩn options
        if (optionsPanel) optionsPanel.SetActive(false);
        if (buttonsHolder) buttonsHolder.SetActive(true);

        Time.timeScale = 0f; // [QUAN TRỌNG] Đóng băng thời gian
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // [QUAN TRỌNG] Trả lại thời gian
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Nhớ trả lại thời gian trước khi chuyển cảnh
        SceneManager.LoadScene("Main Menu"); // Thay tên Scene Menu của bạn
    }

    public void OpenOptions()
    {
        // Ẩn các nút chính, hiện bảng Options
        if (buttonsHolder) buttonsHolder.SetActive(false);
        optionsPanel.SetActive(true);

        // Đồng bộ trạng thái Toggle với PlayerPrefs (để nó hiện đúng tích V hay không)
        bool isMusicOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        bool isSFXOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        if (musicToggle) musicToggle.isOn = isMusicOn;
        if (sfxToggle) sfxToggle.isOn = isSFXOn;
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        if (buttonsHolder) buttonsHolder.SetActive(true);
    }

    // --- LOGIC OPTIONS (Copy y hệt bên MainMenu) ---
    // Nhưng có thêm đoạn cập nhật trực tiếp nhạc nền

    public void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();

        // [MỚI] Tìm cái loa nhạc nền và Mute nó ngay lập tức
        GameObject bgm = GameObject.Find("AudioManager"); // Hoặc tên object chứa nhạc của bạn
        if (bgm != null)
        {
            AudioSource source = bgm.GetComponent<AudioSource>();
            if (source) source.mute = !isOn;
        }
    }

    public void OnSFXToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("SFXEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
        // SFX thì không cần cập nhật ngay, vì code PlayerController tự check mỗi khi phát tiếng
    }
}