using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public Animator transition; // Kéo Animator của tấm màn đen vào đây
    public float transitionTime = 1f; // Thời gian hiệu ứng mờ (1 giây)
    public float waitTime = 1f;       // Thời gian giữ màn hình đen (1 giây nữa)

    // Hàm này gắn vào nút New Game
    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        // 1. Chạy hiệu ứng tối dần (Fade Out)
        transition.SetTrigger("Start");

        // 2. Chờ cho hiệu ứng chạy xong (ví dụ 1 giây)
        yield return new WaitForSeconds(transitionTime);

        // 3. (Tùy chọn) Chờ thêm 1 lúc ở màn hình đen cho "nguy hiểm"
        yield return new WaitForSeconds(waitTime);

        // 4. Chuyển Scene
        SceneManager.LoadScene(sceneName);
    }
}