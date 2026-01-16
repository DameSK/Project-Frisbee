using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetryManager : MonoBehaviour
{
    public GameObject retryButton;

    void Start()
    {
        retryButton.SetActive(false);
    }

    public void ShowRetry()
    {
        retryButton.SetActive(true);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f; // Sahne yenilenmeden önce donmuş olabilir, sıfırla
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}