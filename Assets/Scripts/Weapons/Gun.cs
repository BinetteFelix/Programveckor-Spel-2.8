using System.Collections;
using UnityEngine;

public class Guns : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;

    private float timeSinceLastShot;

    public void Shoot()
    {
        if (gunData.currentAmmo > 0 && CanShoot())
        {
            if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunData.maxDistance))
            {
                //DELEAT LATER!
                Debug.Log($"Hit: {hitInfo.transform.name}");

                Damageable target = hitInfo.collider.GetComponent<Damageable>();
                if (target != null)
                {
                    bool isHeadshot = hitInfo.collider.CompareTag("Head");
                    float damage = isHeadshot ? gunData.damage * gunData.headshotMultiplier : gunData.damage;

                    target.TakeDamage(damage);

                    //Check if bs/hs works REMOVE LATER
                    Debug.Log(isHeadshot ? "Headshot!" : "Body hit");
                }
            }

            gunData.currentAmmo--;
            timeSinceLastShot = 0;
        }
        else
        {
            //NO AMMO LEFT SFX HERE!

            //Remove this later, maybe replace with text or some kind of notification
            Debug.Log("Out of ammo! Reload required.");
        }
    }

    public bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public IEnumerator Reload()
    {
        //RELOAD SFX HERE!

        gunData.reloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
        Debug.Log("Reload complete!");
    }
}