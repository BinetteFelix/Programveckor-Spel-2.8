using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 5f;
    public float sprintSpeed { get; private set; } = 7f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float adsWalkSpeed = 3f;    // New ADS walking speed
    [SerializeField] private float adsCrouchSpeed = 1.5f; // New ADS crouching speed
    
    [SerializeField] private LayerMask groundMask;
    protected float jumpHeight = 7f;
    protected float gravity = -20f;

    protected float crouchHeight = 1f;
    protected float standingHeight = 2f;
    protected float crouchTransitionSpeed = 5f;

    //references for the camera holder, stamina controller, and character controller
    public Transform cameraHolder;
    private StaminaController staminaController;
    public CharacterController controller;
    public BikeControls bikeControls;

    protected Vector3 velocity;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public float currentSpeed;
    public bool isCrouching { get; private set; }

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip[] sprintFootstepSounds;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private AudioClip[] landingSounds;
    [SerializeField] private AudioClip[] breathingSounds;
    [SerializeField] private AudioClip[] exhaustedSounds;
    
    private float footstepTimer = 0f;
    private float walkStepInterval = 0.5f;
    private float sprintStepInterval = 0.3f;
    private float breathingTimer = 2f;
    private float breathingInterval = 2f;
    private bool wasSprintingLastFrame;
    private bool isCurrentlyBreathing;

    void Start()
    {
        controller.center = new Vector3(0f, 0f, 0f);
        controller.height = 2f;

        // Set the camera's initial position relative to the player.
        cameraHolder.localPosition = new Vector3(0f, standingHeight/2, 0f); // Adjust based on camera height
        cameraHolder.localRotation = Quaternion.identity;

        controller = GetComponent<CharacterController>();
        staminaController = GetComponent<StaminaController>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        if (!bikeControls.isRidden)
        {
            HandleMovement();
            HandleCrouch();
        }

        HandleMovementSounds();
    }

    private void HandleMovement()
    {
        isGrounded = IsGrounded();
        
        // Reset velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Horizontal movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = cameraHolder.right * moveX + cameraHolder.forward * moveZ;
        move.y = 0;
        
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        // Updated speed selection
        currentSpeed = DetermineMovementSpeed();

        // Apply horizontal movement
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded && staminaController.canJump)
        {
            velocity.y = jumpHeight;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        
        // Apply vertical movement
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCrouch()
    {
        bool shouldCrouch = Input.GetKey(KeyCode.LeftControl);
        isCrouching = shouldCrouch;

        //Adjust height for crouch
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        //adjust camera position smoothly
        Vector3 targetPosition = new Vector3(cameraHolder.localPosition.x, targetHeight/2, cameraHolder.localPosition.z);
        cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, targetPosition, Time.deltaTime * crouchTransitionSpeed);
    }

    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private float DetermineMovementSpeed()
    {
        bool isAiming = Player_ADS.Instance.IsAiming;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && staminaController.canSprint;
        
        if (isSprinting && !isCrouching)
        {
            return sprintSpeed;
        }
        else if (isCrouching)
        {
            return isAiming ? adsCrouchSpeed : crouchSpeed;
        }
        else
        {
            return isAiming ? adsWalkSpeed : walkSpeed;
        }
    }

    private void HandleMovementSounds()
    {
        bool isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && staminaController.canSprint;

        // Footsteps
        if (isGrounded && isMoving)
        {
            footstepTimer += Time.deltaTime;
            float currentStepInterval = isSprinting ? sprintStepInterval : walkStepInterval;

            if (footstepTimer >= currentStepInterval)
            {
                if (footstepSounds != null && footstepSounds.Length > 0)
                {
                    SoundFXManager.instance.PlayRandomSoundFXclip(footstepSounds, transform, 0.7f);
                }
                footstepTimer = 0f;
            }
        }

        // Handle Sprint Breathing
        if (isSprinting && isMoving && !isCurrentlyBreathing)
        {
            StartCoroutine(HandleSprintBreathing());
        }
        else if (!isSprinting || !isMoving)
        {
            isCurrentlyBreathing = false;
            breathingTimer = 0f;
        }

        // Exhausted sounds when stamina is depleted
        if (staminaController.currentStamina <= 0 && wasSprintingLastFrame)
        {
            if (exhaustedSounds != null && exhaustedSounds.Length > 0)
            {
                SoundFXManager.instance.PlayRandomSoundFXclip(exhaustedSounds, transform, 1f);
            }
        }

        wasSprintingLastFrame = isSprinting;
    }

    private void OnJump()
    {
        if (jumpSounds != null && jumpSounds.Length > 0)
        {
            SoundFXManager.instance.PlayRandomSoundFXclip(jumpSounds, transform, 1f);
        }
    }

    private void OnLand()
    {
        if (landingSounds != null && landingSounds.Length > 0)
        {
            SoundFXManager.instance.PlayRandomSoundFXclip(landingSounds, transform, 1f);
        }
    }

    private IEnumerator HandleSprintBreathing()
    {
        isCurrentlyBreathing = true;

        while (isCurrentlyBreathing)
        {
            if (breathingSounds != null && breathingSounds.Length > 0)
            {
                SoundFXManager.instance.PlayRandomSoundFXclip(breathingSounds, transform, 0.5f);
            }
            yield return new WaitForSeconds(breathingInterval);
        }
    }
}