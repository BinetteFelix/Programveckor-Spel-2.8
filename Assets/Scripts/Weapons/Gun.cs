using System.Collections;
using UnityEngine;

//andvänd GunLibrary.Instance.EquipGun("GunName") för att lyckas equippa en ny pistol...

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
        gunData = GunLibrary.Instance.GetEquippedGun();
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
        if (gunData.ammoInMag > 0 && CanShoot())
        {
            Debug.Log("Shooting!");

            //Spawn and fix the projectile according to the GUN DATA
            Vector3 aimPoint = GetAimPoint();
            weapon.LookAt(aimPoint);

            GameObject projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.linearVelocity = muzzle.forward * gunData.projectileSpeed;

            gunData.ammoInMag -= 1;
            timeSinceLastShot = 0;

            //SHOOT SOUND FX HERE!

        } 
        else if(gunData.ammoInMag <= 0)
        {
            Reload();
        }
        else
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
