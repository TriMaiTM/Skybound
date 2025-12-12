using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // [QUAN TRỌNG] Phải có thư viện này để dùng Toggle

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject instructionsPanel;
    public GameObject optionsPanel;

    [Header("Options UI")] // [MỚI] Kéo 2 cái Toggle vào đây
    public Toggle musicToggle;
    public Toggle sfxToggle;

    void Start()
    {
        // Khi game bật lên, đọc dữ liệu cũ đã lưu
        // 1 = Bật, 0 = Tắt. Mặc định là 1 (Bật)
        bool isMusicOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        bool isSFXOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        // Cập nhật giao diện Toggle cho đúng với dữ liệu
        if (musicToggle != null) musicToggle.isOn = isMusicOn;
        if (sfxToggle != null) sfxToggle.isOn = isSFXOn;
    }

    // --- CÁC HÀM XỬ LÝ OPTIONS (Gắn vào sự kiện OnValueChanged của Toggle) ---

    public void OnMusicToggleChanged(bool isOn)
    {
        // Lưu lại cài đặt: Nếu bật thì lưu 1, tắt thì lưu 0
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save(); // Lưu ngay lập tức
    }

    public void OnSFXToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("SFXEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // --- CÁC HÀM CŨ ---

    void CloseAllPanels()
    {
        instructionsPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    public void OnClick_NewGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnClick_Instructions()
    {
        bool isActive = instructionsPanel.activeSelf;
        CloseAllPanels();
        if (!isActive) instructionsPanel.SetActive(true);
    }

    public void OnClick_Options()
    {
        bool isActive = optionsPanel.activeSelf;
        CloseAllPanels();
        if (!isActive) optionsPanel.SetActive(true);
    }

    public void OnClick_Exit()
    {
        Application.Quit();
    }
}