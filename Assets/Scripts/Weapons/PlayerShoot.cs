using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Gun equippedGun;
    private GunData gunData;

    private void Start()
    {
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

        if ((gunData.isAutomatic && Input.GetButton("Fire1")) ||
            (!gunData.isAutomatic && Input.GetButtonDown("Fire1")))
        {
            equippedGun.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(equippedGun.Reload());
        }
    }
}