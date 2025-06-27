using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -4f);
    public float normalSmoothSpeed = 10f;
    public float flipSmoothSpeed = 2f;
    public float transitionSpeed = 2f;

    public bool followEnabled = false; // ⭐ Ekledik

    private Vector3 currentOffset;

    private FrisbeeMovement frisbeeMovement;
    private Vector3 velocity = Vector3.zero;
    private float currentSmoothSpeed;

    void Start()
    {
        if (target != null)
            frisbeeMovement = target.GetComponent<FrisbeeMovement>();

        currentSmoothSpeed = normalSmoothSpeed;
        currentOffset = offset;
    }

    void LateUpdate()
    {
        if (!followEnabled || target == null || frisbeeMovement == null) return;

        float targetSpeed = frisbeeMovement.IsFlipping ? flipSmoothSpeed : normalSmoothSpeed;
        currentSmoothSpeed = Mathf.Lerp(currentSmoothSpeed, targetSpeed, Time.deltaTime * transitionSpeed);

        // Smooth offset update (asıl fark burada)
        Vector3 desiredOffset = target.rotation * offset;
        currentOffset = Vector3.Lerp(currentOffset, desiredOffset, Time.deltaTime * 5f); // 5f değeri smoothluğu belirler

        Vector3 desiredPosition = target.position + currentOffset;

        float smoothTime = 1f / currentSmoothSpeed;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        transform.LookAt(target);
    }

    public void SnapToTarget()
    {
        if (target == null) return;

        // Kamerayı doğrudan hedef pozisyona ayarla
        Vector3 rotatedOffset = target.rotation * offset;
        Vector3 desiredPosition = target.position + rotatedOffset;
        transform.position = desiredPosition;
        transform.LookAt(target);

        currentOffset = rotatedOffset; // Smooth geçişi de eşitle
    }
}