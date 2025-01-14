using UnityEngine;

public class FPSFOVController : MonoBehaviour
{
    public Camera playerCamera;
    [Header("FOV Settings")]
    [SerializeField] private float defaultFOV = 70f;
    [SerializeField] private float sprintFOV = 80f;
    [SerializeField] private float crouchingFOV = 65f;
    [SerializeField] private float fovTransitionSpeed = 10f;

    private PlayerMovement playerMovement;
    private GunData currentGunData;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = GetComponent<Camera>();
        }

        playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
        playerCamera.fieldOfView = defaultFOV;

        // Subscribe to gun changes
        if (GunLibrary.Instance != null)
        {
            GunLibrary.Instance.OnGunEquipped += UpdateGunData;
            currentGunData = GunLibrary.Instance.GetEquippedGun();
        }
    }

    void OnDestroy()
    {
        if (GunLibrary.Instance != null)
        {
            GunLibrary.Instance.OnGunEquipped -= UpdateGunData;
        }
    }

    private void UpdateGunData(GunData newGunData)
    {
        currentGunData = newGunData;
    }

    void Update()
    {
        if (currentGunData == null) return;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && playerMovement.currentSpeed == playerMovement.sprintSpeed;
        bool isCrouching = playerMovement.isCrouching;
        bool isAiming = Player_ADS.Instance.IsAiming;

        float targetFOV;

        // Priority order: ADS > Crouching > Sprinting > Default
        if (isAiming)
        {
            targetFOV = currentGunData.adsZoomFOV;  // Use gun-specific zoom
        }
        else if (isCrouching)
        {
            targetFOV = crouchingFOV;
        }
        else if (isSprinting)
        {
            targetFOV = sprintFOV;
        }
        else
        {
            targetFOV = defaultFOV;
        }

        // Smoothly transition FOV
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
    }
}