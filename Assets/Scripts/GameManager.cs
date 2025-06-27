using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public EnergyRing firstRing; // İlk halka (sahneden atanacak)
    public GameObject finishTextPanel; // UI'deki “Finished!” paneli

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (firstRing != null)
        {
            firstRing.SetActive(true); // Sadece ilk halka aktif başlar
        }
        else
        {
            Debug.LogWarning("GameManager: FirstRing atanmamış!");
        }

        if (finishTextPanel != null)
        {
            finishTextPanel.SetActive(false); // Yazı ve butonlar başta gizli
        }
        else
        {
            Debug.LogWarning("GameManager: Finish Text Panel sahnede atanmamış!");
        }
    }

    public void AllRingsCompleted()
    {
        Debug.Log("Tüm halkalardan geçildi!");

        if (finishTextPanel != null)
        {
            finishTextPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Finish panel aktif edilemedi, null!");
        }

        GameObject frisbee = GameObject.FindGameObjectWithTag("Player");
        if (frisbee != null)
        {
            Debug.Log("Frisbee bulundu!");

            FrisbeeMovement movement = frisbee.GetComponent<FrisbeeMovement>();
            if (movement != null)
            {
                Debug.Log("FrisbeeMovement bulundu! Kontroller devre dışı bırakılıyor.");
                movement.controlsEnabled = false;
                movement.levelCompleted = true; // ✅ EKLENDİ
            }
            else
            {
                Debug.LogWarning("FrisbeeMovement componenti bulunamadı!");
            }
        }
        else
        {
            Debug.LogWarning("Frisbee (Player tag'lı obje) bulunamadı!");
        }
    }

    public void NextLevel()
    {
        Debug.Log("Next Level butonuna basıldı!");
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Bir sonraki sahne yükleniyor. Index: " + nextSceneIndex);
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Son seviyedeyiz, ileri gidilemiyor.");
        }
    }

    public void RetryLevel()
    {
        Debug.Log("Retry butonuna basıldı. Sahne yeniden yükleniyor.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMenu()
    {
        Debug.Log("Ana menüye dönülüyor.");
        SceneManager.LoadScene(0); // Menü sahnesi build index 0 ise
    }
}