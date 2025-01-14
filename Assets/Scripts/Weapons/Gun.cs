using System.Collections;
using UnityEngine;

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
    }
    public void Shoot()
    {
        if (!CanShoot())
        {
            if (gunData.ammoInMag <= 0 && !reloading)
            {
                StartCoroutine(Reload());
            }
            return;
        }

        Vector3 aimPoint = GetAimPoint();
        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
        
        if (projectile.TryGetComponent<Projectile>(out var projectileComponent))
        {
            projectileComponent.Initialize(gunData.damage);
        }
        
        if (projectile.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = muzzle.forward * gunData.projectileSpeed;
        }

        gunData.ammoInMag--;
        timeSinceLastShot = 0;

        if (gunData.shootSFX != null)
        {
            //No ammo player notification here maybe?

        }
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
}
