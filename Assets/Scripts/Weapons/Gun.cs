using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

//andv�nd GunLibrary.Instance.EquipGun("GunName") f�r att lyckas equippa en ny pistol...

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform weapon;
    private bool reloading;
    private Vector3 aimPoint;
    private float timeSinceLastShot;
    private GunData gunData;
    private bool isAiming = false;
    [SerializeField] private MuzzleFlash muzzleFlash;
    private float spreadMultiplier = 1f;

    [SerializeField] private float swayAmount = 0.02f;
    [SerializeField] private float smoothing = 8f;
    [SerializeField] private float maxSway = 0.06f;
    
    private Vector3 initialWeaponPosition;
    private Vector3 targetWeaponPosition;
    private Transform cameraHolder;

    private void Start()
    {
        if (GunLibrary.Instance == null)
        {
            Debug.LogError("GunLibrary instance not found!");
            enabled = false;
            return;
        }

        gunData = GunLibrary.Instance.GetEquippedGun();
        if (gunData == null)
        {
            Debug.LogError("No gun data available!");
            enabled = false;
            return;
        }

        initialWeaponPosition = weapon.localPosition;
        targetWeaponPosition = initialWeaponPosition;
        cameraHolder = transform.root.Find("CameraHolder");
        if (cameraHolder == null)
            Debug.LogError("CameraHolder not found!");
    }

    private void Update()
    {
        GetAimPoint();
        aimPoint = GetAimPoint();
        weapon.LookAt(aimPoint);
        timeSinceLastShot += Time.deltaTime;

        //check aiming state
        isAiming = Input.GetButton("Fire2") && gunData.canAimDownSights;

        HandleWeaponSway();
    }

    public void Shoot()
    {
        if (!CanShoot())
        {
            if (gunData.ammoInMag <= 0 && !reloading)
            {
                // Play empty mag sound
                if (gunData.emptyMagSound != null)
                {
                    SoundFXManager.instance.PlaySoundFXclip(gunData.emptyMagSound, transform, 1f);
                }
                StartCoroutine(Reload());
            }
            return;
        }

        // Play shoot sound
        if (gunData.shootSounds != null && gunData.shootSounds.Length > 0)
        {
            SoundFXManager.instance.PlayRandomSoundFXclip(gunData.shootSounds, transform, 1f);
        }

        // Trigger muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Flash();
        }

        Vector3 aimPoint = GetAimPoint();
        
        // Apply spread
        float currentSpread = (Player_ADS.Instance.IsAiming ? gunData.aimDownSightsSpread : gunData.hipFireSpread) * spreadMultiplier;
        Quaternion randomRotation = Quaternion.Euler(
            Random.Range(-currentSpread, currentSpread),
            Random.Range(-currentSpread, currentSpread),
            0f
        );

        // Apply the spread to the muzzle rotation
        Quaternion finalRotation = muzzle.rotation * randomRotation;
        
        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, finalRotation);

        if (projectile.TryGetComponent<Projectile>(out var projectileComponent))
        {
            projectileComponent.Initialize(gunData.damage, gunData.headshotMultiplier);
        }

        if (projectile.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = finalRotation * Vector3.forward * gunData.projectileSpeed;
        }

        gunData.ammoInMag -= 1;
        timeSinceLastShot = 0;
    }

    public bool CanShoot()
    {
        return !reloading && timeSinceLastShot > gunData.fireRate;
    }

    public IEnumerator Reload()
    {
        if (reloading) yield break;

        reloading = true;

        // Play reload sound
        if (gunData.reloadSound != null)
        {
            SoundFXManager.instance.PlaySoundFXclip(gunData.reloadSound, transform, 1f);
        }

        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.ammoInMag = gunData.magSize;
        reloading = false;
    }

    private Vector3 GetAimPoint()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hitInfo, gunData.maxDistance))
        {
            return hitInfo.point;
        }
        else
        {
            return ray.GetPoint(gunData.maxDistance);
        }
    }

    private void HandleWeaponSway()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * swayAmount;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayAmount;

        // Calculate target position
        targetWeaponPosition = new Vector3(
            Mathf.Clamp(mouseX, -maxSway, maxSway),
            Mathf.Clamp(mouseY, -maxSway, maxSway),
            initialWeaponPosition.z
        );

        // Apply sway with smoothing
        weapon.localPosition = Vector3.Lerp(
            weapon.localPosition, 
            initialWeaponPosition + targetWeaponPosition, 
            Time.deltaTime * smoothing
        );

        // Reduce sway while aiming
        if (isAiming)
        {
            weapon.localPosition = Vector3.Lerp(
                weapon.localPosition,
                initialWeaponPosition,
                Time.deltaTime * smoothing * 2
            );
        }
    }

    private void OnEnable()
    {
        if (GunLibrary.Instance != null)
        {
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
        // Reset shooting timer when switching weapons
        timeSinceLastShot = gunData.fireRate;  // This ensures we can shoot immediately after switching
        // Reset reloading state
        reloading = false;
    }

    public void SetSpreadMultiplier(float multiplier)
    {
        spreadMultiplier = multiplier;
    }
}
