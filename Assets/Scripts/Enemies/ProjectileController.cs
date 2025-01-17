using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Transform target; // The player
    public float speed = 10f;
    public float lifetime = 5f; 
    public void Initialize(Transform playerTarget)
    {
        target = playerTarget;
        Destroy(gameObject, lifetime); // Destroy after a fixed lifetime
    }

    private void Update()
    {
        if (target != null)
        {
            // Move the projectile toward the player's current position
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Rotate to face the player
            Vector3 direction = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the projectile hits the player
        if (other.CompareTag("Player"))
        {
            
            Destroy(gameObject);

            
            Debug.Log("Player hit by projectile!");
        }

        // Check if it hits the ground or other objects
        if (other.CompareTag("Ground") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
