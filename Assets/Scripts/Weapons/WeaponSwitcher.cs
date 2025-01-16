using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [System.Serializable]
    public enum WeaponType
    {
        Gun,
        Knife,
        Throwable
    }

    [System.Serializable]
    public class WeaponSlot
    {
        public WeaponType type;
        public GameObject weaponObject;
        public KeyCode switchKey;
        public string weaponName; // Used for guns to match GunData
    }

    [SerializeField] private Transform weaponHolder;  // Reference to the weapon holder object
    [SerializeField] public List<WeaponSlot> weaponSlots = new List<WeaponSlot>();
    [SerializeField] private float switchDelay = 0.5f;
    [SerializeField] private Vector3 hipPosition = new Vector3(0.2f, -0.1f, 0.4f);
    [SerializeField] private Vector3 adsPosition = new Vector3(0f, -0.1f, 0.3f);
    [SerializeField] private float positionSmoothing = 12f;
    private Camera mainCamera;

    private int currentWeaponIndex = 0;
    private bool isSwitching = false;
    private float lastSwitchTime;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }

        InitializeWeapons();
    }

    private void InitializeWeapons()
    {
        // Set up guns from GunLibrary
        if (GunLibrary.Instance != null)
        {
            foreach (var slot in weaponSlots)
            {
                if (slot.type == WeaponType.Gun)
                {
                    // Find matching GunData
                    GunData gunData = GunLibrary.Instance.availableGuns.Find(g => g.gunName == slot.weaponName);
                    if (gunData == null)
                    {
                        Debug.LogError($"No GunData found for weapon: {slot.weaponName}");
                        continue;
                    }
                }
            }
        }

        // Initialize all weapons to inactive except the first one
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if (weaponSlots[i].weaponObject != null)
            {
                weaponSlots[i].weaponObject.SetActive(i == currentWeaponIndex);
            }
        }

        // Set initial weapon
        if (weaponSlots.Count > 0)
        {
            SwitchToWeapon(0);
        }
    }

    private void Update()
    {
        HandleWeaponSwitching();
        UpdateWeaponPosition();
    }

    private void HandleWeaponSwitching()
    {
        if (isSwitching) return;

        // Check number keys
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if (Input.GetKeyDown(weaponSlots[i].switchKey))
            {
                SwitchToWeapon(i);
                break;
            }
        }

        // Check scroll wheel
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0f && Time.time >= lastSwitchTime + switchDelay)
        {
            if (scrollWheel > 0f)
            {
                SwitchToNextWeapon();
            }
            else
            {
                SwitchToPreviousWeapon();
            }
        }
    }

    private void UpdateWeaponPosition()
    {
        if (mainCamera == null || weaponHolder == null) return;

        // Determine target position based on ADS state
        Vector3 targetPos = hipPosition;
        if (Player_ADS.Instance.IsAiming && IsGunEquipped() && !isSwitching)
        {
            targetPos = adsPosition;
        }

        // Calculate final position relative to camera
        Vector3 targetPosition = mainCamera.transform.position +
                               mainCamera.transform.right * targetPos.x +
                               mainCamera.transform.up * targetPos.y +
                               mainCamera.transform.forward * targetPos.z;

        // Update position and rotation smoothly
        weaponHolder.position = Vector3.Lerp(weaponHolder.position, targetPosition, Time.deltaTime * positionSmoothing);
        weaponHolder.rotation = mainCamera.transform.rotation;
    }

    private void SwitchToWeapon(int newIndex)
    {
        if (newIndex == currentWeaponIndex || isSwitching) return;
        if (Time.time < lastSwitchTime + switchDelay) return;

        // Deactivate current weapon
        if (currentWeaponIndex < weaponSlots.Count && weaponSlots[currentWeaponIndex].weaponObject != null)
        {
            weaponSlots[currentWeaponIndex].weaponObject.SetActive(false);
        }

        currentWeaponIndex = newIndex;
        isSwitching = true;
        lastSwitchTime = Time.time;

        WeaponSlot newWeapon = weaponSlots[currentWeaponIndex];

        // Handle weapon type specific logic
        switch (newWeapon.type)
        {
            case WeaponType.Gun:
                GunLibrary.Instance?.EquipGun(newWeapon.weaponName);
                break;
            case WeaponType.Throwable:
                // Future throwable logic here
                break;
        }

        // Activate new weapon
        if (newWeapon.weaponObject != null)
        {
            newWeapon.weaponObject.SetActive(true);
        }

        Invoke(nameof(FinishSwitch), switchDelay);
    }

    private void SwitchToNextWeapon()
    {
        int newIndex = (currentWeaponIndex + 1) % weaponSlots.Count;
        SwitchToWeapon(newIndex);
    }

    private void SwitchToPreviousWeapon()
    {
        int newIndex = (currentWeaponIndex - 1 + weaponSlots.Count) % weaponSlots.Count;
        SwitchToWeapon(newIndex);
    }

    private void FinishSwitch()
    {
        isSwitching = false;
    }

    public bool IsSwitching() => isSwitching;

    public WeaponType GetCurrentWeaponType() => weaponSlots[currentWeaponIndex].type;

    public string GetCurrentWeaponName() => weaponSlots[currentWeaponIndex].weaponName;

    public bool IsGunEquipped() => GetCurrentWeaponType() == WeaponType.Gun;

    public GameObject GetCurrentWeaponObject()
    {
        if (currentWeaponIndex < weaponSlots.Count)
        {
            return weaponSlots[currentWeaponIndex].weaponObject;
        }
        return null;
    }
} 