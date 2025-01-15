using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f; // Time before the projectile is destroyed
    private float damage; // Damage value for the projectile
    private float headshotMultiplier;
    private bool isInitialized;

    public void Initialize(float projectileDamage, float headMultiplier)
    {
        damage = projectileDamage;
        headshotMultiplier = headMultiplier;
        isInitialized = true;
    }

    private void Start()
    {
        if (!isInitialized)
        {
            Debug.LogError("Projectile was not initialized before use!");
            Destroy(gameObject);
            return;
        }
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var target))
        {
            float finalDamage = damage;
            
            // Check for headshot (you'll need to tag the head collider or use a layer)
            if (collision.gameObject.CompareTag("Head"))
            {
                finalDamage *= headshotMultiplier;
                Debug.Log("Headshot!");
            }

            target.TakeDamage(finalDamage);
            Debug.Log($"Dealt {finalDamage} damage to {collision.gameObject.name}");
        }
        Destroy(gameObject);
    }
}
