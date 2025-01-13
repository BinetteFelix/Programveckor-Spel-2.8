using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject projectilePrefab;

    private float timeSinceLastShot;

    public void Shoot()
    {
        if (gunData.currentAmmo > 0 && CanShoot())
        {
            //Spawn and fix the projectile according to the GUN DATA
            GameObject projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            Vector3 initialVelocity = muzzle.forward * gunData.projectileSpeed;
            initialVelocity.y += gunData.bulletArc;

            //bullet spread for ADS or not ADS (aim down sights)
            float spread = gunData.canAimDownSights && Input.GetButton("Fire2")
                ? gunData.aimDownSightsSpread
                : gunData.hipFireSpread;

            initialVelocity += new Vector3(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                Random.Range(-spread, spread)
            );

            rb.linearVelocity = initialVelocity;

            gunData.currentAmmo--;
            timeSinceLastShot = 0;

            //SHOOT SOUND FX HERE!

        }
        else
        {
            //No ammo player notification here maybe?
        }
    }

    public bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public IEnumerator Reload()
    {
        gunData.reloading = true;

        //RELOAD SOUND FX HEREE!

        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }
}
