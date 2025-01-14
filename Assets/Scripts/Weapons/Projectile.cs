using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f; // Time before the projectile is destroyed
    private float damage; // Damage value for the projectile
    private bool isInitialized;

    public void Initialize(float projectileDamage)
    {
        damage = projectileDamage;
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
            target.TakeDamage(damage);
            Debug.Log($"Dealt {damage} damage to {collision.gameObject.name}");
        }
        Destroy(gameObject);
    }
}
