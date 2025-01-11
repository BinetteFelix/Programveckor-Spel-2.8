using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
public class DayAndNightCycle : MonoBehaviour
{
    [Range(0f, 24f)] public float currentTime;
    public float timeSpeed = 1f;

    public string currentTimeString;
    public Light sunLight;
    public float sunPosition = 1f;
    public float sunIntensity = 1f;
    public AnimationCurve sunIntensityMultiplier;
    public AnimationCurve lightTempratureCurve;

    public bool isDay = true;

    public Light moonLight;

    private void Start()
    {
        UpdateTimeText();
        CheckShadowStatus();
    }

    private void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;

        if (currentTime >= 24f)
        {
            currentTime = 0f;
        }

        UpdateTimeText();
        UpdateLight();
        CheckShadowStatus();
    }

    private void OnValidate()
    {
        UpdateLight();
        CheckShadowStatus();
    }

    void UpdateTimeText()
    {
        currentTimeString = Mathf.Floor(currentTime).ToString("00") + ((currentTime % 1) * 60).ToString("00");
    }

    void UpdateLight()
    {
        float sunRotation = currentTime / 24f * 360f;
        sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90, sunPosition, 0f);
        moonLight.transform.rotation = Quaternion.Euler(sunRotation + 90, sunPosition, 0f);

        float normalizedTime = currentTime / 24f;
        float intensityCurve = sunIntensityMultiplier.Evaluate(normalizedTime);

        HDAdditionalLightData sunLightData = sunLight.GetComponent<HDAdditionalLightData>();

        if (sunLightData != null)
        {
            float i = intensityCurve * sunIntensity;
            sunLightData.intensity = i;
        }

        float tempratureMultiplier = lightTempratureCurve.Evaluate(normalizedTime);
        Light lightComponent = sunLight.GetComponent<Light>();

        if (lightComponent != null)
        {
            lightComponent.colorTemperature = tempratureMultiplier * 10000f;
        }
    }

    void CheckShadowStatus()
    {
        HDAdditionalLightData sunLightData = sunLight.GetComponent<HDAdditionalLightData>();
        HDAdditionalLightData moonLightData = moonLight.GetComponent<HDAdditionalLightData>();

        if (currentTime >= 6 && currentTime <= 18) 
        {
            sunLightData.EnableShadows(true);
            moonLightData.EnableShadows(false);

            isDay = true;
        }
        else
        {
            sunLightData.EnableShadows(false);
            moonLightData.EnableShadows(true);

            isDay = false;
        }
    }

}
