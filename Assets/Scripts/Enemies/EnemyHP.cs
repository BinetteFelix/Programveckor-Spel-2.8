using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamageable
{
    public float health = 150f;
    private bool _IsDead = false;

    public void TakeDamage(float amount)
    {
        if (!_IsDead)
        {
            if (health > 0)
            {
                // <<DMG SFX HERE>>
                _IsDead = false;
                health -= amount;
            }
            else if (health <= 0)
            {
                health = 0;
                _IsDead = true;
                Die();
            }
            Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {health}");
        }
    }

    private void Die()
    {
        //die SFX HERE
        EnemyAnimations.Instance.Die();
        Debug.Log($"{gameObject.name} has died.");
        Invoke("RemoveEnemyObjectFromScene", 3.5f);
    }

    private void RemoveEnemyObjectFromScene()
    {
        Destroy(gameObject);
    }
}