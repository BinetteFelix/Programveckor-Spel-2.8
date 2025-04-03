using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamageable
{
    public static Health Instance;
    public int maxHealth = 100;
    public float currentHealth;
    public float healthRegenRate = 1.5f;
    public float timer = 0;

    public Slider healthBar;


    public void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
            Die();

        RegenHealth();
        UpdateUI();
    }

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadSceneAsync(2);
    }

    void RegenHealth()
    {
        currentHealth += healthRegenRate * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    private void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject Player = other.gameObject;
        EnemyProjectiles Projectile = Player.GetComponent<EnemyProjectiles>();

        if (Projectile != null)
        {
            TakeDamage(EnemyProjectiles.Instance.damage);
            Debug.Log("Player was Damaged!");
        }
        Destroy(FindAnyObjectByType<EnemyProjectiles>());
    }

    void OnTriggerStay(Collider other)
    {
        timer = timer - Time.deltaTime;

        if (timer <= 0)
        {
            if (other.CompareTag("Enemy"))
            {
                TakeDamage(5);
            }

            timer = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        timer = 0;
    }
}
