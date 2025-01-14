using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    [Header("Crosshair Elements")]
    [SerializeField] private RectTransform crosshairContainer;
    [SerializeField] private RectTransform[] crosshairLines;
    [SerializeField] private CanvasGroup crosshairCanvasGroup;

    [Header("Crosshair Settings")]
    [SerializeField] private float baseSpread = 10f;
    [SerializeField] private float spreadMultiplier = 100f;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private float fadeSpeed = 10f;

    private GunData currentGunData;
    private float currentSpread;
    private float targetSpread;
    private PlayerMovement playerMovement;
    private ButtonController buttonController;

    private void Start()
    {
        if (GunLibrary.Instance == null)
        {
            Debug.LogError("No GunLibrary instance found!");
            enabled = false;
            return;
        }

        playerMovement = FindObjectOfType<PlayerMovement>();
        buttonController = FindObjectOfType<ButtonController>();
        
        if (crosshairCanvasGroup == null)
        {
            crosshairCanvasGroup = GetComponent<CanvasGroup>();
        }

        GunLibrary.Instance.OnGunEquipped += UpdateGunReference;
        currentGunData = GunLibrary.Instance.GetEquippedGun();
    }

    private void OnDestroy()
    {
        if (GunLibrary.Instance != null)
        {
            GunLibrary.Instance.OnGunEquipped -= UpdateGunReference;
        }
    }

    private void UpdateGunReference(GunData newGunData)
    {
        currentGunData = newGunData;
    }

    private void Update()
    {
        if (currentGunData == null) return;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && playerMovement.currentSpeed == playerMovement.sprintSpeed;
        bool isJumping = !playerMovement.isGrounded;
        bool isPaused = buttonController._IsPaused;

        // Handle crosshair visibility
        float targetAlpha = (isSprinting || isPaused) ? 0f : 1f;
        crosshairCanvasGroup.alpha = Mathf.Lerp(crosshairCanvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

        // Only update spread if visible
        if (crosshairCanvasGroup.alpha > 0.01f)
        {
            // Don't allow aiming while jumping
            bool canAim = !isJumping && Player_ADS.Instance.IsAiming;
            float spreadValue = canAim ? currentGunData.aimDownSightsSpread : currentGunData.hipFireSpread;
            
            // Calculate target spread
            targetSpread = baseSpread + (spreadValue * spreadMultiplier);

            // Smoothly interpolate current spread
            currentSpread = Mathf.Lerp(currentSpread, targetSpread, Time.deltaTime * smoothSpeed);

            // Update crosshair position
            UpdateCrosshairSpread();
        }
    }

    private void UpdateCrosshairSpread()
    {
        if (crosshairLines == null || crosshairLines.Length != 4) return;

        // Reset positions
        crosshairLines[0].anchoredPosition = Vector2.up * currentSpread;      // Top
        crosshairLines[1].anchoredPosition = Vector2.right * currentSpread;   // Right
        crosshairLines[2].anchoredPosition = Vector2.down * currentSpread;    // Bottom
        crosshairLines[3].anchoredPosition = Vector2.left * currentSpread;    // Left
    }
} 