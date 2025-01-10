using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight;
    public Light moonLight;
    public float dayLengthInMinutes = 1440f; // Full day cycle in minutes (24 hours)

    public float earthOrbitSpeed = 0.1f; // Speed at which Earth orbits the Sun
    public float moonOrbitSpeed = 0.2f; // Speed at which Moon orbits the Earth

    private float _timeOfDay;
    private float _dayDurationInSeconds;

    // Earth’s axial tilt for the seasonal changes
    private const float EarthTilt = 23.5f;

    void Start()
    {
        _timeOfDay = 0f;
    }

    void Update()
    {
        _timeOfDay += Time.deltaTime / (dayLengthInMinutes * 60);

        if (_timeOfDay > 1.0f)
        { 
            _timeOfDay -= 1.0f; 
        }

        UpdateSun(_timeOfDay);
        UpdateMoon(_timeOfDay);
    }

    void UpdateSun(float time)
    {
        // Earth’s orbit around the Sun affects the Sun’s position
        float earthAngle = time * 360f * earthOrbitSpeed; // Calculate Earth's position in orbit

        // Sun’s altitude (height above the horizon) based on Earth's position and tilt
        float sunAltitude = Mathf.Sin(Mathf.Deg2Rad * (earthAngle - 90f)) * EarthTilt;

        // Sun's azimuth (direction of movement along the horizon)
        float sunAzimuth = Mathf.Repeat(earthAngle + 180f, 360f);

        // Apply the sun's rotation based on altitude and azimuth
        sunLight.transform.rotation = Quaternion.Euler(new Vector3(sunAltitude, sunAzimuth, 0f));

        // Adjust sun light intensity (lower intensity during the day based on altitude)
        float sunIntensity = Mathf.Clamp01(Mathf.Cos(Mathf.Deg2Rad * sunAltitude)); // Dimming sun at low altitudes
        sunLight.intensity = Mathf.Lerp(0.1f, 1f, sunIntensity); // Minimum intensity at sunset/rise
    }

    void UpdateMoon(float time)
    {
        // The Moon orbits the Earth (12 hours offset from the Sun)
        float moonTime = time + 0.5f; // Moon's orbit is roughly 12 hours offset from the Sun
        float moonAngle = moonTime * 360f * moonOrbitSpeed;

        // Moon's altitude based on its orbit
        float moonAltitude = Mathf.Sin(Mathf.Deg2Rad * (moonAngle - 90f)) * EarthTilt;

        // Moon's azimuth (opposite direction to the Sun)
        float moonAzimuth = Mathf.Repeat(moonAngle + 180f, 360f);

        // Apply the moon's rotation in Unity
        moonLight.transform.rotation = Quaternion.Euler(new Vector3(moonAltitude, moonAzimuth, 0f));

        // Adjust moon light intensity (moon is dim, depending on its altitude)
        float moonIntensity = Mathf.Clamp01(Mathf.Cos(Mathf.Deg2Rad * moonAltitude)); // Dim moon at low altitudes
        moonLight.intensity = Mathf.Lerp(0.05f, 0.5f, moonIntensity); // Dimmer moon at lower altitudes
    }
}
