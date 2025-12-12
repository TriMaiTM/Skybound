using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Kiểm tra cài đặt từ Menu
        bool isMusicOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        // Nếu tắt nhạc -> Mute cái loa đi
        if (audioSource != null)
        {
            audioSource.mute = !isMusicOn;
        }
    }
}