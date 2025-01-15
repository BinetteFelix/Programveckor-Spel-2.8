using System;
using System.Collections.Generic;
using UnityEngine;

public class GunLibrary : MonoBehaviour
{
    public static GunLibrary Instance { get; private set; }
    public event Action<GunData> OnGunEquipped;

    public List<GunData> availableGuns;
    private GunData equippedGun;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EquipGun(string gunName)
    {
        GunData newGun = availableGuns.Find(gun => gun.gunName == gunName);
        if (newGun == null)
        {
            Debug.LogError($"Gun with name {gunName} not found in GunLibrary!");
            return;
        }

        equippedGun = newGun;
        OnGunEquipped?.Invoke(equippedGun);
    }

    public GunData GetEquippedGun()
    {
        if (equippedGun == null)
        {
            Debug.LogWarning("No gun is equipped. Equipping first available gun.");
            if (availableGuns.Count > 0)
            {
                EquipGun(availableGuns[0].gunName);
            }
            else
            {
                Debug.LogError("No guns available in the library!");
            }
        }
        return equippedGun;
    }
}