using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] private Light muzzleFlashLight;
    private float flashDuration = 0.05f;
    private float flashIntensity = 2f;

    private float defaultIntensity;
    private bool isFlashing;
    private float flashTimer;

    private void Start()
    {
        if (muzzleFlashLight == null)
        {
            muzzleFlashLight = GetComponent<Light>();
        }
        
        defaultIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
    }

    private void Update()
    {
        if (isFlashing)
        {
            flashTimer += Time.deltaTime;
            if (flashTimer >= flashDuration)
            {
                EndFlash();
            }
        }
    }

    public void Flash()
    {
        isFlashing = true;
        flashTimer = 0;
        muzzleFlashLight.intensity = flashIntensity;
    }

    private void EndFlash()
    {
        isFlashing = false;
        muzzleFlashLight.intensity = 0;
    }
} 