using UnityEngine;
using System.Collections;

public class MenuCameraController : MonoBehaviour
{
    public Transform levelSelectPosition; // Kamera hedef konumu
    public GameObject playButton;
    public GameObject levelButtons;       // Level 1-2-3 ve Exit'in bağlı olduğu canvas
    public GameObject backButton;
    public float moveDuration = 1.5f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Başlangıç konumunu kaydet
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        playButton.SetActive(true);
        levelButtons.SetActive(false);
        backButton.SetActive(false);
    }

    public void OnPlayPressed()
    {
        // Butonları hemen aktif et
        levelButtons.SetActive(true);
        backButton.SetActive(true);
        playButton.SetActive(false); // Play butonunu gizle

        // Kamerayı hareket ettir
        StartCoroutine(MoveCamera(levelSelectPosition.position, levelSelectPosition.rotation));
    }

    public void OnBackPressed()
    {
        // Butonları ayarla
        levelButtons.SetActive(false);
        backButton.SetActive(false);
        playButton.SetActive(true);

        // Kamerayı başlangıç konumuna döndür
        StartCoroutine(MoveCamera(initialPosition, initialRotation));
    }

    private IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;
    }
}