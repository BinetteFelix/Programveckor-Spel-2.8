using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public CloudManager cloudManager;

    void Update()
    {
        dayNightCycle.UpdateCycle(Time.deltaTime);
        cloudManager.UpdateClouds(Time.deltaTime);
    }
}