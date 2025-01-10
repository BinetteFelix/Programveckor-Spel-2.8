using UnityEngine;

public class FPSFOVController : MonoBehaviour
{
    public Camera playerCamera;
    public int defaultFOV = 70;
    public int sprintFOV = 90;
    public int crouchingFOV = 55;
    public float fovTransitionSpeed = 2f;

    private PlayerMovement playerMovement;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = GetComponent<Camera>();
        }

        playerMovement = FindObjectOfType<PlayerMovement>();

        playerCamera.fieldOfView = defaultFOV;
    }

    void Update()
    {
        //adjust for sprinting
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && playerMovement.currentSpeed == playerMovement.sprintSpeed;
        float targetSprintFOV = isSprinting ? sprintFOV : defaultFOV;

        //adjust for crouching
        bool isCrouching = Input.GetKey(KeyCode.LeftControl) && playerMovement.isCrouching;
        float targetCrouchFOV = isCrouching ? crouchingFOV : defaultFOV;

        //prio
        float targetFOV = isCrouching ? targetCrouchFOV : targetSprintFOV;

        //smoothly transition
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);

    }
}