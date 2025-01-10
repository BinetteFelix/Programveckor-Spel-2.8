using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight;
    public Light moonLight;
    public float dayLengthInMinutes = 1.0f; // Length of a full day-night cycle in minutes

    private float _timeOfDay;
    private float _dayDurationInSeconds;

    void Start()
    {
        _dayDurationInSeconds = dayLengthInMinutes * 60;
        _timeOfDay = 0;
    }

    void Update()
    {
        // Progress time
        _timeOfDay += Time.deltaTime / _dayDurationInSeconds;
        if (_timeOfDay > 1.0f) _timeOfDay -= 1.0f;

        // Update lights
        UpdateLighting(_timeOfDay);
    }

    void UpdateLighting(float time)
    {
        // Calculate sun and moon angles
        float sunAngle = time * 360.0f - 90.0f; // Sun starts at -90 degrees
        float moonAngle = sunAngle - 180.0f;    // Moon is always opposite the sun

        sunLight.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 170, 0));
        moonLight.transform.rotation = Quaternion.Euler(new Vector3(moonAngle, 170, 0));

        // Adjust intensities based on time
        float sunIntensity = Mathf.Clamp01(Mathf.Cos(time * Mathf.PI * 2.0f));
        float moonIntensity = Mathf.Clamp01(Mathf.Cos((time + 0.5f) * Mathf.PI * 2.0f));

        sunLight.intensity = sunIntensity;
        moonLight.intensity = moonIntensity;
    }
}