// 10/01/2025 AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public ParticleSystem cloudParticleSystem;
    public float cloudSpeed = 0.01f;
    public float particleSize = 1.0f;
    public float spawnRate = 10.0f;
    public float clumpSize = 2.0f;

    void Start()
    {
        var mainModule = cloudParticleSystem.main;
        mainModule.startSize = particleSize;
        var emissionModule = cloudParticleSystem.emission;
        emissionModule.rateOverTime = spawnRate;
    }

    void Update()
    {
        // Update particle speed
        var mainModule = cloudParticleSystem.main;
        mainModule.simulationSpeed = cloudSpeed;

        // Implement custom logic for clumping
        // This could involve modifying particle positions
    }
}