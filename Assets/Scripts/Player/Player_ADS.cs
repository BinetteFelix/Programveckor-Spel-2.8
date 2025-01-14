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

    private PlayerMovement playerMovement;

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
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement not found!");
        }
    }

    void Update()
    {
        if (cameraController._LockStateLocked)
        {
            bool wasAiming = IsAiming;
            GunData currentGun = GunLibrary.Instance.GetEquippedGun();
            
            // Check if player is not sprinting before allowing ADS
            bool canAim = !Input.GetKey(KeyCode.LeftShift) || playerMovement.currentSpeed != playerMovement.sprintSpeed;
            IsAiming = Input.GetMouseButton(1) && currentGun != null && currentGun.canAimDownSights && canAim;

            // Handle weapon position
            if (IsAiming)
            {
                Weapon.transform.position = Vector3.Slerp(Weapon.transform.position, ADSPosition.transform.position, AnimationSpeed * Time.deltaTime);
            }
            else
            {
                Weapon.transform.position = Vector3.Slerp(Weapon.transform.position, DefaultPos.transform.position, AnimationSpeed * Time.deltaTime);
            }

            // Notify listeners if aim state changed
            if (wasAiming != IsAiming)
            {
                OnAimStateChanged?.Invoke(IsAiming);
            }
        }
    }
}
