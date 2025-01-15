using UnityEngine;

public class Player_ADS : MonoBehaviour
{
    public static Player_ADS Instance { get; private set; }
    public bool IsAiming { get; private set; }
    public event System.Action<bool> OnAimStateChanged;

    public CameraController cameraController;
    [SerializeField] GameObject Weapon;
    [SerializeField] Transform ADSPosition;
    [SerializeField] Transform DefaultPos;
    private float AnimationSpeed = 9.5f;

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
        // Get required components
        weaponSwitcher = GetComponent<WeaponSwitcher>();
        
        // Validate required references
        if (weaponSwitcher == null)
        {
            Debug.LogError("WeaponSwitcher not found on " + gameObject.name);
            enabled = false;
            return;
        }

        if (cameraController == null)
        {
            Debug.LogError("CameraController reference not set on " + gameObject.name);
            enabled = false;
            return;
        }

        if (Weapon == null)
        {
            Debug.LogError("Weapon reference not set on " + gameObject.name);
            enabled = false;
            return;
        }

        if (ADSPosition == null || DefaultPos == null)
        {
            Debug.LogError("ADS or Default position references not set on " + gameObject.name);
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Add null checks
        if (!cameraController || !Weapon || !weaponSwitcher) return;

        if (cameraController._LockStateLocked)
        {
            bool wasAiming = IsAiming;
            GunData currentGun = GunLibrary.Instance?.GetEquippedGun();
            
            bool canAim = weaponSwitcher.IsGunEquipped() && !weaponSwitcher.IsSwitching();
            IsAiming = Input.GetMouseButton(1) && currentGun != null && currentGun.canAimDownSights && canAim;

            if (IsAiming)
            {
                Weapon.transform.position = Vector3.Slerp(Weapon.transform.position, ADSPosition.position, AnimationSpeed * Time.deltaTime);
            }
            else
            {
                Weapon.transform.position = Vector3.Slerp(Weapon.transform.position, DefaultPos.position, AnimationSpeed * Time.deltaTime);
            }

            if (wasAiming != IsAiming)
            {
                OnAimStateChanged?.Invoke(IsAiming);
            }
        }
    }
}
