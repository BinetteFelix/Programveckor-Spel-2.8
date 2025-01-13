using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Gun equippedGun;

    private void Update()
    {
        if (Input.GetButton("Fire1"))
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
