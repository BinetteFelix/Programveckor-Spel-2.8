using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Guns equippedGun;

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            //SHOOT SFX HERE

            equippedGun.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //RELOAD SFX HERE

            StartCoroutine(equippedGun.Reload());
        }
    }
}
