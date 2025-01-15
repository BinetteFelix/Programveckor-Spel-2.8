using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamageable
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        // DMG SFX HERE

        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //die SFX HERE

        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}