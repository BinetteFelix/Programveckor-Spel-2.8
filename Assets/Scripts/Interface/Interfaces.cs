using UnityEngine;

public class Interfaces : MonoBehaviour
{
}
interface IProjectile
{
    float LifeTime { get; set; }// Time before the projectile is destroyed
    float Damage { get; set; } // Damage value for the projectile
    float HeadshotMultiplier { get; set; }
     bool _IsInitialized { get; set; }

    public void Initialize(float projectileDamage, float headMultiplier) { }
    private void Start() { }
    private void OnCollisionEnter(Collision collision) { }
}
