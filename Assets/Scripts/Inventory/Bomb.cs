using Unity.VisualScripting;
using UnityEditor.Rendering.Analytics;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public GameObject ExplosionEffect; // The particle effect prefab for the explosion
    public float ExplosionRadius = 5f; // The radius of the explosion
    public float ExplosionForce = 500f; // The force applied to nearby objects
    public float ExplosionDuration = 2f; // How long the effect lasts before disappearing
    private float time = 5f;
    private float lifeTime = 8f;
    private bool timer = true;

    private void Update()
    {
        if(timer)
        time -= Time.deltaTime;
        lifeTime -= Time.deltaTime;


        if (time <= 0)
        {
            TriggerExplosion(transform.position);
            time = 1f;
            timer = false;
        }
    }

    public void TriggerExplosion(Vector3 position)
    {
            // Spawn the explosion particle effect
            if (ExplosionEffect != null)
            {
                GameObject explosion = Instantiate(ExplosionEffect, position, Quaternion.identity);
                Destroy(explosion, ExplosionDuration); // Destroy the effect after the duration
            }

            // Apply explosion force to nearby rigidbodies
            Collider[] colliders = Physics.OverlapSphere(position, ExplosionRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(ExplosionForce, position, ExplosionRadius);
                }
            }

            // Play an explosion sound
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                //audioSource.Play();  KABOM
            }
        if (time <= 0)
            Destroy(gameObject);
    }
}
