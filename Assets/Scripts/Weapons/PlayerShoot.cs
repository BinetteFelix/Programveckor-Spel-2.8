using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Gun equippedGun;
    [SerializeField] private float sprintingSpreadMultiplier = 2.5f;
    [SerializeField] private float jumpingSpreadMultiplier = 3f;
    
    private GunData gunData;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (GunLibrary.Instance != null)
        {
            gunData = GunLibrary.Instance.GetEquippedGun();
            GunLibrary.Instance.OnGunEquipped += UpdateGunData;
        }
    }

    private void OnDisable()
    {
        if (GunLibrary.Instance != null)
        {
            GunLibrary.Instance.OnGunEquipped -= UpdateGunData;
        }
    }

    private void UpdateGunData(GunData newGunData)
    {
        gunData = newGunData;
    }

    private void Update()
    {
        if (gunData == null) return;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && playerMovement.currentSpeed == playerMovement.sprintSpeed;
        bool isJumping = !playerMovement.isGrounded;

        // Apply spread multipliers based on state
        if (isSprinting)
        {
            equippedGun.SetSpreadMultiplier(sprintingSpreadMultiplier);
        }
        else if (isJumping)
        {
            equippedGun.SetSpreadMultiplier(jumpingSpreadMultiplier);
        }
        else
        {
            equippedGun.SetSpreadMultiplier(1f);
        }

        // Allow shooting in all states
        if ((gunData.isAutomatic && Input.GetButton("Fire1") && gunData.ammoInMag > 0) ||
            (!gunData.isAutomatic && Input.GetButtonDown("Fire1") && gunData.ammoInMag > 0))
        {
            equippedGun.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) || gunData.ammoInMag <= 0)
        {
            StartCoroutine(equippedGun.Reload());
        }
    }
}