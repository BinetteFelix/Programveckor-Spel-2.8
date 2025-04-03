using UnityEngine;

public class EnemyProjectiles : MonoBehaviour, IProjectile
{
    private float lifeTime = 5.0f;
    public float LifeTime // Time before the projectile is destroyed
    {
        get { return lifeTime; }
        set { lifeTime = value; }
    }
    public float damage = 25.0f;
    public float Damage   // Projectile Damage towards object effected
    {
        get { return damage; }
        set { damage = value; }
    }
    private float headshotMultiplier = 1.35f;
    public float HeadshotMultiplier  // Projectile Damage towards object effected
    {
        get { return headshotMultiplier; }
        set { headshotMultiplier = value; }
    }
    public bool _IsInitialized { get; set; }

    public void Initialize(float projectileDamage, float headMultiplier)
    {
        Damage = projectileDamage;
        HeadshotMultiplier = headMultiplier;
        _IsInitialized = true;
    }
    public static EnemyProjectiles Instance;
    Animator animator;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (!_IsInitialized)
        {
            Debug.LogError("Projectile was not initialized before use!");
            Destroy(gameObject);
            return;
        }
        Destroy(gameObject, LifeTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject Projectile = collision.gameObject;
        Health Player = Projectile.GetComponent<Health>();

        if(Player != null)
        {
            Health.Instance.TakeDamage(damage);
        }
        
        if (collision.gameObject.TryGetComponent<IDamageable>(out var target))
        {
            float finalDamage = damage;

            // Check for headshot (you'll need to tag the head collider or use a layer)
            if (collision.gameObject.CompareTag("Player"))
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