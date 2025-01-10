using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight;
    public Light moonLight;
    //24h = 1440 min
    public float dayLengthInMinutes = 1440f;
    public float earthOrbitSpeed = 0.1f;
    public float moonOrbitSpeed = 0.2f;

    private float _timeOfDay;

    //had trouble with this for 3 hours, chatGPT fixed the equation by telling me it was supposed to be posetive... :_(
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
        //simple circular orbit
        float earthAngle = time * 360f * earthOrbitSpeed;

        //apply axial
        float sunAltitude = Mathf.Sin(Mathf.Deg2Rad * (earthAngle - 90f)) * EarthTilt;

        //direction of movement along the horizon
        float sunAzimuth = Mathf.Repeat(earthAngle + 180f, 360f);

        // Apply the sun's rotation based on altitude and azimuth
        sunLight.transform.rotation = Quaternion.Euler(new Vector3(sunAltitude, sunAzimuth, 0f));

        //lower intensity during the day based on altitude
        float sunIntensity = Mathf.Clamp01(Mathf.Cos(Mathf.Deg2Rad * sunAltitude));
        sunLight.intensity = Mathf.Lerp(0.1f, 1f, sunIntensity);
    }

    void UpdateMoon(float time)
    {
        //12h back
        float moonTime = time + 0.5f;
        float moonAngle = moonTime * 360f * moonOrbitSpeed;

        //Moon altitude based on its orbit
        float moonAltitude = Mathf.Sin(Mathf.Deg2Rad * (moonAngle - 90f)) * EarthTilt;

        //opposite direction to the Sun
        float moonAzimuth = Mathf.Repeat(moonAngle + 180f, 360f);

        //Apply moon rot
        moonLight.transform.rotation = Quaternion.Euler(new Vector3(moonAltitude, moonAzimuth, 0f));

        float moonIntensity = Mathf.Clamp01(Mathf.Cos(Mathf.Deg2Rad * moonAltitude));

        moonLight.intensity = Mathf.Lerp(0.01f, 0.1f, moonIntensity);
    }
}
