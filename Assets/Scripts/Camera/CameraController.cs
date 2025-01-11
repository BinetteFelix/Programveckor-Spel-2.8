using UnityEngine;

public class CameraController : MonoBehaviour
{
    public BikeControls bikeControl;

    [HideInInspector] public static CameraController Instance;
    [HideInInspector] public float mouseSensitivity = 175f;
    public Transform playerBody;

    private float xRotation = 0f;
    private float yRotation = 0f;
    [HideInInspector] public bool _LockState_Locked = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        ResetLockstate();
    }

    void Update()
    {
        if (_LockState_Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;

            if (bikeControl.isRidden)
            {
                yRotation += mouseX;
                yRotation = Mathf.Clamp(yRotation, -120f, 120f);  // Clamp horizontal rotation for bike
                transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);  // Rotate only the player body on the Y-axis
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Clamp vertical rotation
            }
        }
    }

    public void ResetLockstate()
    {
        if (_LockState_Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            _LockState_Locked = false;
        }
        else if (!_LockState_Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _LockState_Locked = true;
        }
    }
}
