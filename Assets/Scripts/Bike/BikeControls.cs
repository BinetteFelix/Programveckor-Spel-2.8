using System;
using UnityEngine;

public class BikeControls : MonoBehaviour
{
    [HideInInspector]
    public bool isRidden = false; // looking if bike is being used
    private bool isDrifting = false;
    public bool startTimer = false; // Starts a timer
    public Transform seatPosition; // The position for the player
    private GameObject currentPlayer;

    public float speed = 0f;
    public float maxSpeed = 20f;
    public float acceleration = 5f;
    public float deceleration = 10f;
    public float turnSpeed = 30f;
    public float tiltAmount = 5f;
    public float smoothTiltSpeed = 5f;
    public float smoothTurnSpeed = 10f;
    public float driftMultiplier = 3f;
    public float driftDeceleration = 8f;

    
    private float currentTilt = 0f;
    private float targetRotationY = 0f;
    
    private float timer = 0.1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on the bike.");
        }
    }

    void Update()
    {
        if (isRidden)
        {
            if (startTimer && timer > 0)
            {
                timer -= Time.deltaTime;
            }
            HandleBicycleControls();

            // Lets the player jump off the bike
            if (Input.GetKeyDown(KeyCode.E) && timer <= 0)
            {
                currentTilt = Mathf.Lerp(currentTilt, 0, Time.deltaTime * smoothTiltSpeed); // Resets the player's tilt
                Dismount();
            }
        }
    }

    void HandleBicycleControls()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Pedal();
        }

        if (Input.GetKey(KeyCode.S))
        {
            Brake();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            StartDrifting();
        }
        else
        {
            StopDrifting();
        }

        if (speed > 0.5f)
        {
            float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
            if (isDrifting)
            {
                // Turns faster when drifting
                turn *= driftMultiplier;

                // Adjusts the speed during drift
                speed -= driftDeceleration * Time.deltaTime;
                speed = Mathf.Max(speed, 0);
            }

            if (turn != 0)
            {
                Turn(turn);
            }
            else
            {
                ResetTilt();
            }
        }
        else
        {
            ResetTilt();
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            NaturalDeceleration();
        }

        ApplyMovement();
    }

    void ApplyMovement()
    {
        Vector3 forwardMovement = transform.forward * speed;
        forwardMovement.y = rb.linearVelocity.y; // Maintain vertical velocity for gravity
        rb.linearVelocity = forwardMovement;

        SmoothRotation();
    }

    public void Mount(GameObject player)
    {
        isRidden = true;
        currentPlayer = player;
        startTimer = true;

        // Puts the player on the bike
        player.transform.position = seatPosition.position;
        player.transform.rotation = transform.rotation;
        player.transform.parent = transform;
    }

    public void Dismount()
    {
        if (currentPlayer != null)
        {
            isRidden = false;
            startTimer = false;
            timer = 0.1f;
            speed = 0;

            // Puts the player besides the bike, rotates the player correctly, and removes the player as a child of the bike.
            currentPlayer.transform.position = transform.position + transform.right * 2f;
            Quaternion uprightRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            currentPlayer.transform.rotation = uprightRotation;
            currentPlayer.transform.parent = null;

            // Reset the camera
            if (CameraController.Instance != null)
            {
                CameraController.Instance.ResetCameraRotation(uprightRotation);
            }

            currentPlayer = null;
        }
    }

    void Pedal()
    {
        speed += acceleration * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);
    }

    void Brake()
    {
        speed -= deceleration * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);
    }

    void NaturalDeceleration()
    {
        if (speed > 0)
        {
            speed -= deceleration * 0.5f * Time.deltaTime;
            speed = Mathf.Max(speed, 0);
        }
    }

    void StartDrifting()
    {
        if (!isDrifting)
        {
            isDrifting = true;
        }
    }

    void StopDrifting()
    {
        if (isDrifting)
        {
            isDrifting = false;
        }
    }

    void Turn(float turnAmount)
    {
        // Adjust the target rotation for horizontal alignment
        targetRotationY += turnAmount;

        // Tilts the player while turning
        currentTilt = Mathf.Lerp(currentTilt, -Mathf.Sign(turnAmount) * tiltAmount, Time.deltaTime * smoothTiltSpeed);
    }

    void ResetTilt()
    {
        currentTilt = Mathf.Lerp(currentTilt, 0, Time.deltaTime * smoothTiltSpeed);
    }

    void SmoothRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, currentTilt);
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, Time.deltaTime * smoothTurnSpeed));
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Mount(other.gameObject);
        }
    }
}
