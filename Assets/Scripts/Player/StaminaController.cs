using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    private float maxStamina = 100f;
    private float sprintStaminaCost = 12f;
    private float jumpStaminaCost = 4f;
    private float staminaRegenRate;

    public Slider staminaBar;

    public float currentStamina;
    private PlayerMovement playerMovement;

    public bool canSprint;
    public bool canJump;

    void Start()
    {
        currentStamina = maxStamina;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        HandleStaminaRegen();
        HandleStamina();
        UpdateUI();
    }

    private void HandleStamina()
    {
        if (currentStamina >= jumpStaminaCost)
        {
            canJump = true;
        }
        else
            canJump = false;

        if (currentStamina > 10f)
        {
            canSprint = true;
        }
        else
            canSprint = false;

        if (Input.GetButton("Jump") && playerMovement.isGrounded && !playerMovement.isCrouching)
        {
            UseStaminaForJump();
        }
        else if (Input.GetKey(KeyCode.LeftShift) && playerMovement.currentSpeed == playerMovement.sprintSpeed && playerMovement.isGrounded)
        {
            UseStaminaForSprint();
        } 
        else if (playerMovement.isGrounded)
        {
            RegainStamina();
        }
    }


    private void UpdateUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina / maxStamina;
        }
    }

    public void UseStaminaForSprint()
    {
        if (canSprint)
        {
            //drain stam
            currentStamina -= sprintStaminaCost * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }
    private void UseStaminaForJump()
    {
        if (canJump)
        {
            //drain stam
            currentStamina -= jumpStaminaCost;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }
    private void RegainStamina()
    {
        //regen stam
        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    public void HandleStaminaRegen()
    {
        //insentivises playing the game more methodically and rewarding player for going slow.
        if(playerMovement.isCrouching) 
        {
            staminaRegenRate = 12f;
        }
        else
        {
            staminaRegenRate = 4f;
        }
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }
}