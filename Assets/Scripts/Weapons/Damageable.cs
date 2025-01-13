using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
       
        //is only here to chekc if this stuff works...
        //remove later
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        } 
        else 
        {
            //DMG SFX HERE!!

        }
    }

    private void Die()
    {
        //DEATH SFX HERE!!!

        Destroy(gameObject);
    }
}