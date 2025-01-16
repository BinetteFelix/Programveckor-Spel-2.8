using UnityEngine;

public class Player_ADS : MonoBehaviour
{
    public static Player_ADS Instance { get; private set; }
    public bool IsAiming { get; private set; }
    public event System.Action<bool> OnAimStateChanged;

    [SerializeField] private CameraController cameraController; // Reference to camera controller in inspector
    [SerializeField] private GameObject ReticlePanel;
    private WeaponSwitcher weaponSwitcher;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        weaponSwitcher = GetComponent<WeaponSwitcher>();
        
        // If not assigned in inspector, try to find in scene
        if (cameraController == null)
        {
            cameraController = FindObjectOfType<CameraController>();
        }

        if (weaponSwitcher == null)
            Debug.LogError("WeaponSwitcher not found on " + gameObject.name);
        if (cameraController == null)
            Debug.LogError("CameraController not found! Please assign in inspector or check scene.");
    }

    private void Update()
    {
        if (!cameraController || !weaponSwitcher) return;

        if (cameraController._LockStateLocked)
        {
            bool wasAiming = IsAiming;
            GunData currentGun = GunLibrary.Instance?.GetEquippedGun();
            
            bool canAim = weaponSwitcher.IsGunEquipped() && 
                         !weaponSwitcher.IsSwitching() && 
                         currentGun != null && 
                         currentGun.canAimDownSights;

            IsAiming = Input.GetMouseButton(1) && canAim;

            if (wasAiming != IsAiming)
            {
                if (currentGun.gunName == "Sniper" && weaponSwitcher.weaponSlots[2].weaponObject.activeSelf == true)
                {
                    Invoke("HideSniperOnAim", 0.04f);
                }
                else if (currentGun.gunName == "Sniper" && weaponSwitcher.weaponSlots[2].weaponObject.activeSelf == false)
                {
                    weaponSwitcher.weaponSlots[2].weaponObject.SetActive(true);
                    ReticlePanel.SetActive(false);
                }
                OnAimStateChanged?.Invoke(IsAiming);
            }
        }
    }
    public void HideSniperOnAim()
    {
        weaponSwitcher.weaponSlots[2].weaponObject.SetActive(false);
        ReticlePanel.SetActive(true);
    }
}
