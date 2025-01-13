using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f; // How long the projectile exists before being destroyed
    private GunData gunData;

    public void Initialize(GunData data)
    {
        gunData = data;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Projectile hit: {collision.gameObject.name}");

        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(gunData.damage);
            Debug.Log($"Dealt {gunData.damage} damage to {collision.gameObject.name}");
        }

        Destroy(gameObject);
    }
}