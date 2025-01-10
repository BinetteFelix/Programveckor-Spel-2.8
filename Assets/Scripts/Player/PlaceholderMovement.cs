using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public static FirstPersonController Instance;

    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpHeight = 1.2f;
    public float gravity = -20f;

    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    public float lookUpLimit = 80f;
    public float lookDownLimit = -80f;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private float rotationX = 0f;
    public bool _LockState_Locked = false;

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
        characterController = GetComponent<CharacterController>();

        // Lock the cursor
        ResetLockstate();
    }

    void Update()
    {
        MovePlayer();
        LookAround();
    }

    void MovePlayer()
    {
        // Check if the player is grounded
        if (characterController.isGrounded)
        {
            // Get input for movement
            float moveZ = Input.GetAxis("Vertical");
            float moveX = Input.GetAxis("Horizontal");

            // Calculate movement direction
            Vector3 move = transform.forward * moveZ + transform.right * moveX;
            move.Normalize();

            // Determine speed
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float speed = isRunning ? runSpeed : walkSpeed;

            moveDirection = move * speed;

            // Jumping
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move((moveDirection + velocity) * Time.deltaTime);
    }

    void LookAround()
    {
        if (_LockState_Locked)
        {
            // Get mouse movement
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Rotate the player left and right
            transform.Rotate(Vector3.up * mouseX);

            // Rotate the camera up and down
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, lookDownLimit, lookUpLimit);
            Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
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