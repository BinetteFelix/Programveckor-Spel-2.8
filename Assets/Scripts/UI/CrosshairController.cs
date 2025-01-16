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

    [Header("Weapon Type Settings")]
    [SerializeField] private float meleeSpread = 5f;     // Small, fixed spread for knife
    [SerializeField] private float throwableSpread = 5f; // Small, fixed spread for throwables

    [Header("Crosshair Colors")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color enemyColor = Color.red;
    private Image crosshairTop;
    private Image crosshairRight;
    private Image crosshairBottom;
    private Image crosshairLeft;
    [SerializeField] private float raycastRange = 100f; // How far to check for enemies
    [SerializeField] private LayerMask enemyLayer; // Set this to your enemy layer in inspector

    private GunData currentGunData;
    private float currentSpread;
    private float targetSpread;
    private PlayerMovement playerMovement;
    private ButtonController buttonController;
    private WeaponSwitcher weaponSwitcher;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        buttonController = FindObjectOfType<ButtonController>();
        weaponSwitcher = FindObjectOfType<WeaponSwitcher>();
        
        if (crosshairCanvasGroup == null)
        {
            crosshairCanvasGroup = GetComponent<CanvasGroup>();
        }

        if (GunLibrary.Instance != null)
        {
            GunLibrary.Instance.OnGunEquipped += UpdateGunReference;
            currentGunData = GunLibrary.Instance.GetEquippedGun();
        }

        // Find all crosshair images by name
        crosshairTop = transform.Find("CrosshairTop").GetComponent<Image>();
        crosshairRight = transform.Find("CrosshairRight").GetComponent<Image>();
        crosshairBottom = transform.Find("CrosshairBottom").GetComponent<Image>();
        crosshairLeft = transform.Find("CrosshairLeft").GetComponent<Image>();

        // Initialize crosshair color
        SetCrosshairColor(defaultColor);
    }

    private void Update()
    {
        if (weaponSwitcher == null) return;

        bool isKnifeEquipped = weaponSwitcher.GetCurrentWeaponType() == WeaponSwitcher.WeaponType.Knife;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && playerMovement.currentSpeed == playerMovement.sprintSpeed;
        bool isPaused = buttonController._IsPaused;

        // Only fade crosshair when sprinting with gun, not with knife
        float targetAlpha = ((!isKnifeEquipped && isSprinting) || isPaused) ? 0f : 1f;
        crosshairCanvasGroup.alpha = Mathf.Lerp(crosshairCanvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

        // Only update spread if visible
        if (crosshairCanvasGroup.alpha > 0.01f)
        {
            UpdateCrosshairBasedOnWeaponType();
        }

        // Add enemy detection
        CheckForEnemy();
    }

    private void UpdateCrosshairBasedOnWeaponType()
    {
        switch (weaponSwitcher.GetCurrentWeaponType())
        {
            case WeaponSwitcher.WeaponType.Gun:
                UpdateGunCrosshair();
                break;

            case WeaponSwitcher.WeaponType.Knife:
                targetSpread = meleeSpread;
                break;

            case WeaponSwitcher.WeaponType.Throwable:
                targetSpread = throwableSpread;
                break;
        }

        // Smoothly interpolate current spread
        currentSpread = Mathf.Lerp(currentSpread, targetSpread, Time.deltaTime * smoothSpeed);

        // Update crosshair position
        UpdateCrosshairSpread();
    }

    private void UpdateGunCrosshair()
    {
        if (currentGunData == null) return;

        bool canAim = !weaponSwitcher.IsSwitching();
        float spreadValue = Player_ADS.Instance.IsAiming && canAim
            ? currentGunData.aimDownSightsSpread
            : currentGunData.hipFireSpread;

        targetSpread = baseSpread + (spreadValue * spreadMultiplier);
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

    private void UpdateGunReference(GunData newGunData)
    {
        currentGunData = newGunData;
    }

    private void CheckForEnemy()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.1f);

            if (hit.collider.GetComponentInParent<EnemyHP>() != null)
            {
                SetCrosshairColor(enemyColor);
                return;
            }
        }
        
        SetCrosshairColor(defaultColor);
    }

    private void SetCrosshairColor(Color color)
    {
        // Set color for each crosshair part
        crosshairTop.color = color;
        crosshairRight.color = color;
        crosshairBottom.color = color;
        crosshairLeft.color = color;
    }
} 