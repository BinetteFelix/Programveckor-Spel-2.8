using UnityEngine;
[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    //fpr movement
    protected float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    protected float crouchSpeed = 2.5f;
    protected float jumpHeight = 1.2f;
    protected float gravity = -20f;

    //Crouch
    protected float crouchHeight = 1f;
    protected float standingHeight = 2f;
    protected float crouchTransitionSpeed = 5f;

    //camera, Stamina script, Bikecontrols script and character controller referens
    public Transform cameraHolder;
    private StaminaController staminaController;
    private CharacterController controller;
    public BikeControls bikeControls;

    //other
    protected Vector3 velocity;
    public bool isGrounded;
    public float currentSpeed;
    public bool isCrouching { get; private set; }

    void Start()
    {
        //get ref for character cont and stamina
        controller = GetComponent<CharacterController>();
        staminaController = GetComponent<StaminaController>();
        
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        //player shouldn't be able to move when on bike...
        if (bikeControls.isRidden == false)
        {
            HandleMovement();
            HandleCrouch();
        }
    }

    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //getting input here
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        //prevent diagonall movement from being faster
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        //checks if the character should be sprinting and sets the speed to sprint speed
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && staminaController.canSprint)
        {
            currentSpeed = sprintSpeed;
        }
        else if (staminaController.currentStamina <= 5f)
        {
            currentSpeed = walkSpeed;
        }
        else
        {
            currentSpeed = isCrouching ? crouchSpeed : walkSpeed;
        }

        //Move
        controller.Move(move * currentSpeed * Time.deltaTime);

        //hmmm what does this one do? oh yeah it says JUMP for the get button down....... maybe jump?
        if (Input.GetButton("Jump") && isGrounded && staminaController.canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // gravi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        //set value for isCrouching
        isCrouching = Input.GetKey(KeyCode.LeftControl);

        // Adjust height and camera position
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        Vector3 targetPosition = new Vector3(cameraHolder.localPosition.x, targetHeight / 2, cameraHolder.localPosition.z);
        cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, targetPosition, Time.deltaTime * crouchTransitionSpeed);
    }
}