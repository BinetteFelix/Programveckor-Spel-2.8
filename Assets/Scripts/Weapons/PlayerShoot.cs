using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Gun equippedGun;
    [SerializeField] private float sprintingSpreadMultiplier = 2.5f;
    [SerializeField] private float jumpingSpreadMultiplier = 3f;
    
    private GunData gunData;
    private PlayerMovement playerMovement;
    private WeaponSwitcher weaponSwitcher;

    private void Start()
    {
        weaponSwitcher = GetComponent<WeaponSwitcher>();
        playerMovement = GetComponent<PlayerMovement>();
        
        if (weaponSwitcher == null)
        {
            Debug.LogError("WeaponSwitcher not found on " + gameObject.name);
            enabled = false;
            return;
        }

        if (GunLibrary.Instance != null)
        {
            gunData = GunLibrary.Instance.GetEquippedGun();
            GunLibrary.Instance.OnGunEquipped += UpdateGunData;
            
            if (weaponSwitcher.IsGunEquipped())
            {
                equippedGun = weaponSwitcher.GetCurrentWeaponObject().GetComponent<Gun>();
            }
        }
        else
        {
            Debug.LogError("GunLibrary instance not found!");
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
        if (weaponSwitcher.IsGunEquipped())
        {
            equippedGun = weaponSwitcher.GetCurrentWeaponObject().GetComponent<Gun>();
        }
    }

    private void Update()
    {
        if (gunData == null || equippedGun == null || !weaponSwitcher.IsGunEquipped() || weaponSwitcher.IsSwitching()) 
            return;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && playerMovement.currentSpeed == playerMovement.sprintSpeed;
        bool isJumping = !playerMovement.isGrounded;

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

        if (!ButtonController.Instance._IsPaused && !ButtonController.Instance._Inv_IsActive)
        {
            if (Input.GetMouseButton(0))  // For automatic weapons
            {
                if (gunData.isAutomatic && gunData.ammoInMag > 0)
                {
                    equippedGun.Shoot();
                }
            }
            
            if (Input.GetMouseButtonDown(0))  // For semi-automatic weapons
            {
                if (!gunData.isAutomatic && gunData.ammoInMag > 0)
                {
                    equippedGun.Shoot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) || gunData.ammoInMag <= 0)
        {
            StartCoroutine(equippedGun.Reload());

        }
    }
}