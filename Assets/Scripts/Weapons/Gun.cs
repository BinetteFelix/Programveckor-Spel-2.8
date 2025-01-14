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
    }

    private void Update()
    {
        GetAimPoint();
        aimPoint = GetAimPoint();
        weapon.LookAt(aimPoint);
        timeSinceLastShot += Time.deltaTime;

        //check aiming state
        isAiming = Input.GetButton("Fire2") && gunData.canAimDownSights;
    }
    public void Shoot()
    {
        //Shoot SFX HERE!

        if (!CanShoot())
        {
            if (gunData.ammoInMag <= 0 && !reloading)
            {
                StartCoroutine(Reload());
            }
            return;
        }

        // Trigger muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Flash();
        }

        Vector3 aimPoint = GetAimPoint();
        
        // Apply spread
        float currentSpread = isAiming ? gunData.aimDownSightsSpread : gunData.hipFireSpread;
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

        //RELOAD SOUND FX HEREE!

        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.ammoInMag = gunData.magSize;

        //when ammo is added as a resorsce this is where the subtraction will take place 
        //inventory.currentAmmo - gunData.magSize;
        reloading = false;
    }
    private Vector3 GetAimPoint()
    {
        // Create a ray from the center of the screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of the screen

        // Perform a raycast to detect objects in the aim path
        if (Physics.Raycast(ray, out RaycastHit hitInfo, gunData.maxDistance))
        {
            return hitInfo.point; // Return the point where the ray hits an object
        }
        else
        {
            return ray.GetPoint(gunData.maxDistance); // Return a point far away if no object is hit
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
    }
}
