using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
public class DayAndNightCycle : MonoBehaviour
{
    [Range(0f, 20f)]
    public float currentTime;
    public float timeSpeed = 1f;

    public string currentTimeString;
    public Light sunLight;
    public float sunPosition = 1f;
    public float sunIntensity = 1f;
    public AnimationCurve sunIntensityMultiplier;
    private void Start()
    {
        UpdateTimeText();
    }

    private void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;

        if(currentTime >= 24f)
        {
            currentTime = 0f;
        }

        UpdateTimeText();
        UpdateLight();
    }

    private void OnValidate()
    {
        UpdateLight();
    }

    void UpdateTimeText()
    {
        currentTimeString = Mathf.Floor(currentTime).ToString("00") + ((currentTime % 1) * 60).ToString("00");
    }

    void UpdateLight()
    {
        float sunRotation = currentTime / 24f * 360f;
        sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90, sunPosition, 0f);

        float normalizedTime = currentTime / 24f;
        float intensityCurve = sunIntensityMultiplier.Evaluate(normalizedTime);

        HDAdditionalLightData sunLightData = sunLight.GetComponent<HDAdditionalLightData>();

        if (sunLightData != null )
        {
            sunLightData.intensity = intensityCurve * sunIntensity;
        }
    }
}
