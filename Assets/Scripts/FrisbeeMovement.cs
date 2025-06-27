using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems; 

[RequireComponent(typeof(Rigidbody))]
public class FrisbeeMovement : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public bool controlsEnabled = true;
    public bool gameStarted = false;
    public bool levelCompleted = false;

    public float flipDuration = 0.3f;
    public float tiltAmount = 15f;
    public float tiltSpeed = 5f;
    public float moveSpeed = 10f;
    public float rotationSpeed = 200f;
    public float verticalSpeed = 5f;
    public float baseSpeed = 10f;
    public float speedUpAmount = 5f;
    public float speedDownAmount = 5f;

    public float dashMultiplier = 3f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;

    private bool canDash = true;
    private float dashCooldownTimer = 0f;

    private FrisbeeEnergy energySystem;
    private Rigidbody rb;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private bool hasCollided = false;
    private bool isFlipping = false;
    private bool isDashing = false;
    private bool inputEnabled = false;

    private float currentTilt = 0f;
    private float currentSpeed;

    void Start()
    {
        energySystem = GetComponent<FrisbeeEnergy>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        startPosition = transform.position;
        startRotation = transform.rotation;
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (!gameStarted || !controlsEnabled || hasCollided) return;

        if (hasCollided && Input.GetKeyDown(KeyCode.R))
        {
            Retry();
            return;
        }

        // Cooldown güncellemesi (her frame)
        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0f)
            {
                canDash = true;
                dashCooldownTimer = 0f;
            }
        }

        currentSpeed = baseSpeed;
        if (Input.GetKey(KeyCode.W)) currentSpeed += speedUpAmount;
        if (Input.GetKey(KeyCode.S)) currentSpeed -= speedDownAmount;
        if (currentSpeed < 0f) currentSpeed = 0f;

        if (speedText != null)
            speedText.text = "Speed: " + Mathf.RoundToInt(currentSpeed).ToString();

        float targetTilt = 0f;
        if (Input.GetKey(KeyCode.A)) targetTilt = tiltAmount;
        else if (Input.GetKey(KeyCode.D)) targetTilt = -tiltAmount;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

        if (Input.GetMouseButtonDown(1) && !isFlipping)
        {
            if (Input.GetKey(KeyCode.A)) StartCoroutine(Flip(Vector3.left));
            else if (Input.GetKey(KeyCode.D)) StartCoroutine(Flip(Vector3.right));
        }

        if (gameStarted && inputEnabled && Input.GetMouseButtonDown(0) &&
            !isFlipping && !isDashing && canDash &&
            !EventSystem.current.IsPointerOverGameObject()) // UI'ye tıklanmadıysa
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!controlsEnabled || hasCollided || isFlipping) return;

        if (energySystem != null && energySystem.isOutOfEnergy)
        {
            rb.useGravity = true;
            rb.linearDamping = 1f;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.down * 3f, Time.fixedDeltaTime * 0.5f);
            return;
        }

        float h = 0f;
        if (Input.GetKey(KeyCode.A)) h = -1f;
        if (Input.GetKey(KeyCode.D)) h = 1f;

        float turnAmount = h * rotationSpeed * Time.fixedDeltaTime;
        Quaternion yawRotation = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * yawRotation);

        float pitch = 0f;
        if (Input.GetKey(KeyCode.Space)) pitch = -15f;
        else if (Input.GetKey(KeyCode.LeftShift)) pitch = 15f;

        float roll = currentTilt;
        Quaternion targetRotation = Quaternion.Euler(pitch, rb.rotation.eulerAngles.y, roll);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * tiltSpeed));

        Vector3 move = transform.forward * currentSpeed;
        if (isDashing) move *= dashMultiplier;
        if (Input.GetKey(KeyCode.Space)) move += Vector3.up * verticalSpeed;
        else if (Input.GetKey(KeyCode.LeftShift)) move += Vector3.down * verticalSpeed;

        rb.linearVelocity = move;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided || levelCompleted) return;

        hasCollided = true;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        RetryManager retryManager = Object.FindFirstObjectByType<RetryManager>();
        if (retryManager != null)
            retryManager.ShowRetry();

        transform.position += -transform.forward * 0.2f;

        Vector3 bounceDirection = (-transform.forward + Vector3.up * 0.5f).normalized;
        rb.AddForce(bounceDirection * 15f, ForceMode.Impulse);
        rb.AddTorque(Random.onUnitSphere * 20f, ForceMode.Impulse);
        rb.angularDamping = 2f;
    }

    void Retry()
    {
        StopAllCoroutines();
        StartCoroutine(ResetFrisbee());
    }

    IEnumerator ResetFrisbee()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        yield return new WaitForFixedUpdate();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.isKinematic = false;

        hasCollided = false;
        isFlipping = false;
        isDashing = false;
    }

    IEnumerator Flip(Vector3 direction)
    {
        isFlipping = true;

        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 localDirection = (direction == Vector3.left) ? -transform.right : transform.right;
        Vector3 targetPos = startPos + localDirection * 6f;
        float flipAngle = (direction == Vector3.left) ? 360f : -360f;

        while (elapsed < flipDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / flipDuration;
            float zRotation = Mathf.Lerp(0f, flipAngle, progress);
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, zRotation);
            transform.position = Vector3.Lerp(startPos, targetPos, progress);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        isFlipping = false;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        dashCooldownTimer = dashCooldown;

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    public void EnableInput() => inputEnabled = true;
    public bool IsFlipping => isFlipping;
    public bool IsDashAvailable() => canDash;
    public float GetDashCooldownProgress() => Mathf.Clamp01(1f - (dashCooldownTimer / dashCooldown));
}