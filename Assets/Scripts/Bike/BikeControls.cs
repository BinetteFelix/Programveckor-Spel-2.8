using System.Threading;
using UnityEngine;

public class BikeControls : MonoBehaviour
{
    [HideInInspector]
    public bool isRidden = false; // looking if bike is being used
    public bool startTimer = false; // Starts a timer
    public Transform seatPosition; // The position for the player
    private GameObject currentPlayer;

    public float speed = 0f;
    public float maxSpeed = 20f;
    public float acceleration = 5f;
    public float deceleration = 10f;
    public float turnSpeed = 50f;

    private float timer = 0.1f;

    void Update()
    {
        if (isRidden)
        {
            if (startTimer && timer > 0)
            {
                timer -= Time.deltaTime;
            }
            HandleBicycleControls();

            // Lets the player jump of the bike
            if (Input.GetKeyDown(KeyCode.E) && timer <= 0)
            {
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

        if (speed > 0.5)
        {
            float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
            if (turn != 0)
            {
                Turn(turn);
            }
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            NaturalDeceleration();
        }

        // moves the bike forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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

            // Puts the player besides the bike, rotates the player corect and removes the player from being a child of the bike.
            currentPlayer.transform.position = transform.position + transform.right * 2f;
            currentPlayer.transform.rotation = transform.rotation;
            currentPlayer.transform.parent = null;


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
    void Turn(float turnAmount)
    {
        transform.Rotate(0, turnAmount, 0);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Mount(other.gameObject);
        }
    }

}


