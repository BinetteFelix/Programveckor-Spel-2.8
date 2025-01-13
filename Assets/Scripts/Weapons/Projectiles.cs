using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f;
    private float damage;

    public void Initialize(float projectileDamage)
    {
        damage = projectileDamage;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Damageable target = collision.gameObject.GetComponent<Damageable>();
        if (target != null)
        {
            //COLLISSION SFX TARGET
            target.TakeDamage(damage);
        }
        else if (target == null)
        {
            //SFX BASED ON THING HIT
        }

        Destroy(gameObject);
    }
}