using UnityEngine;

public class CameraController : MonoBehaviour
{
    public BikeControls bikeControl;

    [HideInInspector] public static CameraController Instance;
    [HideInInspector] public float mouseSensitivity = 175f;
    public Transform playerBody;

    public Vector3 offset;

    private float xRotation = 0f;
    private float yRotation = 0f;
    [HideInInspector] public bool _LockStateLocked = false;

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

    void LateUpdate()
    {
        if (_LockStateLocked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            if (bikeControl.isRidden)
            {
                yRotation += mouseX;
                yRotation = Mathf.Clamp(yRotation, -120f, 120f);
                transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            }
            else
            {
                
                transform.parent.Rotate(Vector3.up * mouseX);
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }

            if (!bikeControl.isRidden)
            {
                Vector3 rotation = playerBody.eulerAngles;
                transform.rotation = Quaternion.Euler(0, rotation.y, 0); // Ensures upright rotation
                transform.parent.Rotate(Vector3.up * mouseX);
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }

        }
    }

    public void ResetCameraRotation(Quaternion playerRotation)
    {
        // Align camera rotation with the player's upright rotation
        xRotation = 0f; // Reset vertical rotation
        yRotation = playerRotation.eulerAngles.y; // Align with the player's horizontal rotation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }


    public void ResetLockstate()
    {
        if (_LockStateLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            _LockStateLocked = false;
        }
        else if (!_LockStateLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _LockStateLocked = true;
        }
    }
}
