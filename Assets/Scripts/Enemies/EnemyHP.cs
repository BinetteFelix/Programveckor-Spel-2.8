using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamageable
{
    public float health = 100f;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
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
        Invoke("RemoveEnemyObjectFromScene", 3.5f);
        animator.SetBool("IsDead", true);
    }

    private void RemoveEnemyObjectFromScene()
    {
        Destroy(gameObject);
    }
}