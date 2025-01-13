using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
public class DayAndNightCycle : MonoBehaviour
{
    [Range(0f, 24f)] public float currentTime;
    public float timeSpeed = 1f;

    public string currentTimeString;
    public Light sunLight;
    //Luleå har en lattitud på ungefär 65.6 och ca 22.2 longitude
    //spelet utspelar sig där nån stans fast typ i skogen så vi tar bort lite
    public float sunLatitude = 65.6f;
    public float sunLongitude = 18.5f;
    public float sunIntensity = 1f;
    public AnimationCurve sunIntensityMultiplier;
    public AnimationCurve sunTempratureCurve;

    public float moonLatitude = 65.6f;
    public float moonLongitude = 18.5f;
    public Light moonLight;
    public float moonIntensity = 1f;
    public AnimationCurve moonIntensityMultiplier;
    public AnimationCurve moonTempratureCurve;


    public VolumeProfile volumeProfile;
    private PhysicallyBasedSky skySettings;
    public float starIntensity = 1f;
    public AnimationCurve starCurve;
    public float polarstarLatitude = 65.6f;
    public float polarstarLongitude = 18.5f;

    public bool isDay = true;

    public bool sunActive = true;
    public bool moonActive = true;

    private void Start()
    {
        UpdateTimeText();
        CheckShadowStatus();
        SkyStars();
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
        SkyStars();
    }

    private void OnValidate()
    {
        UpdateLight();
        CheckShadowStatus();
        SkyStars();
    }

    void UpdateTimeText()
    {
        currentTimeString = Mathf.Floor(currentTime).ToString("00") + ((currentTime % 1) * 60).ToString("00");
    }

    void UpdateLight()
    {
        float sunRotation = currentTime / 24f * 360f;
        sunLight.transform.localRotation = (Quaternion.Euler(sunLatitude - 90, sunLongitude, 0) * Quaternion.Euler(0, sunRotation, 0));
        moonLight.transform.localRotation = (Quaternion.Euler(90 - moonLatitude, moonLongitude - 170, 0) * Quaternion.Euler(0, sunRotation, 0));

        float normalizedTime = currentTime / 24f;
        float sunIntensityCurve = sunIntensityMultiplier.Evaluate(normalizedTime);
        float moonIntensityCurve = moonIntensityMultiplier.Evaluate(normalizedTime);

        HDAdditionalLightData sunLightData = sunLight.GetComponent<HDAdditionalLightData>();
        HDAdditionalLightData moonLightData = moonLight.GetComponent<HDAdditionalLightData>();


        if (sunLightData != null)
        {
            float i = sunIntensityCurve * sunIntensity;
            sunLightData.intensity = i;
        }
        if (moonLightData != null)
        {
            float i = moonIntensityCurve * moonIntensity;
            moonLightData.intensity = i;
        }

        float sunTempratureMultiplier = sunTempratureCurve.Evaluate(normalizedTime);
        float moonTempratureMultiplier = moonTempratureCurve.Evaluate(normalizedTime);


        Light sunLightComponent = sunLight.GetComponent<Light>();
        Light moonLightComponent = moonLight.GetComponent<Light>();

        if (sunLightComponent != null)
        {
            sunLightComponent.colorTemperature = sunTempratureMultiplier * 10000f;
        }
        if (moonLightComponent != null)
        {
            moonLightComponent.colorTemperature = moonTempratureMultiplier * 10000f;
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

        if (currentTime >= 5.7f && currentTime <= 18.3f)
        {
            sunLight.gameObject.SetActive(true);
            sunActive = false;
        }
        else
        {
            sunLight.gameObject.SetActive(false);
            sunActive = true;
        }

        if (currentTime >= 6.3f && currentTime <= 17.7f)
        {
            moonLight.gameObject.SetActive(false);
            moonActive = true;
        }
        else
        {
            moonLight.gameObject.SetActive(true);
            moonActive = false;
        }
    }

    void SkyStars()
    {
        volumeProfile.TryGet<PhysicallyBasedSky>(out skySettings);
        skySettings.spaceEmissionMultiplier.value = starCurve.Evaluate(currentTime / 24f) * starIntensity;

        skySettings.spaceRotation.value = (Quaternion.Euler(90 - polarstarLatitude, polarstarLongitude - 180, 0) * Quaternion.Euler(0, currentTime / 24 * 360, 0)).eulerAngles;
    }

}
