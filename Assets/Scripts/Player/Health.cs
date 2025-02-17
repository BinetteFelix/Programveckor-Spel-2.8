using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;
    public float healthRegenRate = 1.5f;
    public float timer = 0;

    public Slider healthBar;



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

    void TakeDamage()
    {
        currentHealth -= 5;
    }

    private void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }



    void OnTriggerStay(Collider other)
    {
        timer = timer - Time.deltaTime;

        if (timer <= 0)
        {
            if (other.CompareTag("Enemy"))
            {
                TakeDamage();
            }

            timer = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        timer = 0;
    }
}
