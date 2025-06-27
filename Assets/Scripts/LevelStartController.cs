using UnityEngine;
using System.Collections;

public class LevelStartController : MonoBehaviour
{
    public GameObject clickToPlayText;
    public GameObject controlsPanel;

    private bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f;
        clickToPlayText.SetActive(true);
        if (controlsPanel != null)
            controlsPanel.SetActive(true);
    }

        void Update()
    {
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            clickToPlayText.SetActive(false);
            if (controlsPanel != null)
                controlsPanel.SetActive(false);

            Time.timeScale = 1f;
            gameStarted = true;

            var camFollow = Camera.main.GetComponent<CameraFollow>();
            if (camFollow != null)
            {
                camFollow.SnapToTarget();       // ✅ Pozisyona zıplatmadan yerleştir
                camFollow.followEnabled = true; // ✅ Sonra takibi başlat
            }

            GameObject frisbee = GameObject.FindWithTag("Player");
            if (frisbee != null)
            {
                var movement = frisbee.GetComponent<FrisbeeMovement>();
                if (movement != null)
                {
                    movement.gameStarted = true;
                    StartCoroutine(EnableInputNextFrame(movement));
                }
            }
        }
}

    IEnumerator EnableInputNextFrame(FrisbeeMovement movement)
    {
        yield return null; // bir frame bekle
        movement.EnableInput(); // ✅ input bir sonraki frame'de açılır
    }
}