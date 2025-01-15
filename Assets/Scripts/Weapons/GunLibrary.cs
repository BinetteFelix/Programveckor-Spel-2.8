using System;
using System.Collections.Generic;
using UnityEngine;

public class GunLibrary : MonoBehaviour, ISaveable
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

    private void Start()
    {
        SaveManager.Instance.RegisterSaveable(this);
    }

    private void OnDestroy()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.UnregisterSaveable(this);
        }
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

    public void Save(SaveData saveData)
    {
        saveData.weaponData.equippedGunName = equippedGun.gunName;
        
        // Save ammo counts
        saveData.weaponData.gunAmmoData.Clear();
        foreach (var gun in availableGuns)
        {
            saveData.weaponData.gunAmmoData[gun.gunName] = gun.ammoInMag;
        }
    }

    public void Load(SaveData saveData)
    {
        // Load equipped gun
        GunData savedGun = GetGunByName(saveData.weaponData.equippedGunName);
        if (savedGun != null)
        {
            EquipGun(savedGun.gunName);
        }

        // Load ammo counts
        foreach (var gunAmmo in saveData.weaponData.gunAmmoData)
        {
            GunData gun = GetGunByName(gunAmmo.Key);
            if (gun != null)
            {
                gun.ammoInMag = gunAmmo.Value;
            }
        }
    }

    public GunData GetGunByName(string gunName)
    {
        return availableGuns.Find(gun => gun.gunName == gunName);
    }
}