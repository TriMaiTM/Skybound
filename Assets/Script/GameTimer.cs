using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isRunning = true;

    public static GameTimer instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    void Update()
    {
        // Nếu bạn chưa làm tính năng Pause game, hãy xóa phần "&& !PauseMenuManager.isPaused" đi
        // để tránh bị báo lỗi đỏ nhé.
        if (isRunning) 
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log($"⏱️ Hoàn thành: {elapsedTime:F2} giây");

        // Lưu thời gian vào bộ nhớ máy
        PlayerPrefs.SetFloat("GameTime", elapsedTime);
        PlayerPrefs.Save();
    }
}