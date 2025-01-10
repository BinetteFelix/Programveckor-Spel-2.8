using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CloudManager : MonoBehaviour
{
    public ParticleSystem cloudParticleSystem;
    public float cloudSpeed = 0.01f; // Speed of cloud movement

    private ParticleSystem.MainModule _mainModule;
    private ParticleSystem.VelocityOverLifetimeModule _velocityModule;

    void Start()
    {
        if (cloudParticleSystem == null)
        {
            cloudParticleSystem = GetComponent<ParticleSystem>();
        }

        // Configure particle system settings
        _mainModule = cloudParticleSystem.main;
        _mainModule.simulationSpace = ParticleSystemSimulationSpace.World;

        _velocityModule = cloudParticleSystem.velocityOverLifetime;
        _velocityModule.enabled = true;
        _velocityModule.space = ParticleSystemSimulationSpace.World;

        // Set cloud movement speed
        _velocityModule.x = cloudSpeed;
    }
}