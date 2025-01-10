using UnityEngine;

public class BikeControls : MonoBehaviour
{
    public bool isRidden = false; // Om cykeln används
    private bool startTimer = false; // Startar en timer
    public Transform seatPosition; // Positionen där spelaren "sitter" på cykeln
    private GameObject currentPlayer; // Referens till spelaren som hoppar på

    public float speed = 0f;
    public float maxSpeed = 20f;
    public float acceleration = 5f;
    public float deceleration = 10f;
    public float turnSpeed = 50f;

    public float timer = 1f;

    void Update()
    {
        if (startTimer)
        {
            timer = timer - Time.deltaTime;
        }

        if (isRidden)
        {
            HandleBicycleControls();

            // Låt spelaren hoppa av cykeln
            if (Input.GetKeyDown(KeyCode.E) && timer <= 0)
            {
                Dismount();
            }
        }
    }

    void HandleBicycleControls()
    {
        // Hantera trampning
        if (Input.GetKey(KeyCode.W))
        {
            Pedal();
        }

        // Hantera bromsning
        if (Input.GetKey(KeyCode.S))
        {
            Brake();
        }

        // Hantera svängning
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        if (turn != 0)
        {
            Turn(turn);
        }

        // Naturlig inbromsning
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            NaturalDeceleration();
        }

        // Flytta cykeln framåt
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void Mount(GameObject player)
    {
        startTimer = true;
        isRidden = true;
        currentPlayer = player;

        // Placera spelaren på cykeln
        player.transform.position = seatPosition.position;
        player.transform.parent = transform;
    }

    public void Dismount()
    {
        if (currentPlayer != null)
        {
            isRidden = false;
            startTimer = false;
            timer = 1f;

            // Placera spelaren bredvid cykeln
            currentPlayer.transform.position = transform.position + transform.right * 2f;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player kan hoppa på cykeln. Tryck E.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Mount(other.gameObject);
        }
    }

}


