using UnityEngine;

public class ThrowGranade : MonoBehaviour
{
    public GameObject SmokeBombPrefab; // Prefab for the smoke bomb
    public Transform ThrowPoint; // Point from which the smoke bomb is thrown
    public float BaseThrowForce = 10f; // Base force for throwing the smoke bomb
    public float JumpThrowMultiplier = 1.5f; // Multiplier for throw force when jumping
    public float VerticalForceMultiplier = 2f; // Additional force based on camera's vertical angle
    public float GroundCheckDistance = 0.1f; 
    public LayerMask GroundLayer;

    public Health Health;

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, GroundCheckDistance, GroundLayer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowSmokeBomb();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseMedkit();
        }
    }

    private void ThrowSmokeBomb()
    {
        // Look for a smoke bomb item in the inventory
        ItemProfile smokeBombItem = InventoryManager.Instance.Items.Find(item => item.Name == "Smoke Bomb");

        if (smokeBombItem != null)
        {
            // Instantiate the smoke bomb at the ThrowPoint
            GameObject smokeBomb = Instantiate(SmokeBombPrefab, ThrowPoint.position, ThrowPoint.rotation);

            // Calculate the throw direction based on the camera's angle
            Vector3 throwDirection = CalculateThrowDirection();

            // Get the Rigidbody component of the smoke bomb
            Rigidbody smokeBombRb = smokeBomb.GetComponent<Rigidbody>();

            if (smokeBombRb != null)
            {
      
                float throwForce = BaseThrowForce;

                if (!IsGrounded())
                {
                    throwForce *= JumpThrowMultiplier;
                }

                smokeBombRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            }

            InventoryManager.Instance.Items.Remove(smokeBombItem);

            // Update the inventory UI to reflect the change
            InventoryManager.Instance.ArrangeItems();
        }
        else
        {
            // Log a message if there are no smoke bombs in the inventory
            Debug.Log("No smoke bombs in inventory to throw!");
        }
    }

    private void UseMedkit()
    {
        // Find a medkit in the inventory
        var medkitItem = InventoryManager.Instance.Items.Find(item => item.Name == "Medkit");
        if (medkitItem == null)
        {
            Debug.Log("No medkits in inventory!");
            return;
        }

        // Restore health
        Health.currentHealth = Health.maxHealth;
        Debug.Log("Health fully restored!");

        // Remove the medkit from the inventory and update UI
        InventoryManager.Instance.Items.Remove(medkitItem);
        InventoryManager.Instance.ArrangeItems();
    }

    private Vector3 CalculateThrowDirection()
    {
        // Get the camera's forward direction
        Transform cameraTransform = Camera.main.transform;

        // Use the camera's forward vector, adding a vertical component
        Vector3 throwDirection = cameraTransform.forward;
        throwDirection.y += cameraTransform.forward.y * VerticalForceMultiplier;

        // Normalize the direction to ensure consistent throw behavior
        return throwDirection.normalized;
    }
}


