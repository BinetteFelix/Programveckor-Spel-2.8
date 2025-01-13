using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Gun equippedGun;
    [SerializeField] private GunData gunData;

    private void Update()
    {
        if ((gunData.isAutomatic && Input.GetButton("Fire1")) || (!gunData.isAutomatic && Input.GetButtonDown("Fire1")))
        {
            equippedGun.Shoot();
            Debug.Log("Fire1 pressed!");
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(equippedGun.Reload());
        }
    }
}
