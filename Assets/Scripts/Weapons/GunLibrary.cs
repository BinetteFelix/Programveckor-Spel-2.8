using System.Collections.Generic;
using UnityEngine;

public class GunLibrary : MonoBehaviour
{
    public List<GunData> availableGuns; // A list of all available GunData assets
    private GunData equippedGun;

    public static GunLibrary Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Keeps this object across scenes
    }

    public void EquipGun(string gunName)
    {
        equippedGun = availableGuns.Find(gun => gun.gunName == gunName);
        if (equippedGun == null)
        {
            Debug.LogError($"Gun with name {gunName} not found in GunLibrary!");
        }
    }

    public GunData GetEquippedGun()
    {
        if (equippedGun == null)
        {
            Debug.LogError("No gun is equipped. Ensure a gun is equipped before accessing.");
        }
        return equippedGun;
    }
}